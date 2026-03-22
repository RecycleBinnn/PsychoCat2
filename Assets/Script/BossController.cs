using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    [Header("Boss Stats")]
    public int maxHealth = 3;
    int currentHealth;

    [Header("UI")]
    public Image[] healthImages;

    [Header("Animation")]
    public Animator animator;

    [Header("Attack Timing")]
    public float jumpInterval = 3f;

    [Header("Obstacle")]
    public GameObject obstaclePrefab;
    public GameObject warningPrefab; //  เส้นเตือน
    public int obstacleCount = 3;
    public float spawnRangeX = 8f;
    public float spawnHeight = 6f;

    [Header("Safe Zone (Respawn Safe Area)")]
    public Transform safeCenter; // เช่น spawnPoint player
    public float safeWidth = 2f;

    [Header("Warning")]
    public float warningTime = 0.5f;

    [Header("Bomb")]
    public GameObject bombPrefab;
    public float bombInterval = 20f;

    [Header("Jump Speed by HP")]
    public float maxJumpInterval = 4f; // ตอนเลือดเต็ม (ช้า)
    public float minJumpInterval = 1.5f; // ตอนเลือดใกล้หมด (เร็ว)

    [Header("Scene Transition")]
    public Image fadeScreen;
    public float fadeSpeed = 2f;
    public string nextSceneName = "Level5";

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();

        if (fadeScreen != null)
            fadeScreen.color = new Color(0, 0, 0, 0);

        StartCoroutine(JumpLoop());
        StartCoroutine(BombLoop());
    }

    IEnumerator JumpLoop()
    {
        while (currentHealth > 0)
        {
            yield return new WaitForSeconds(GetJumpInterval());

            if (animator != null)
                animator.SetTrigger("Jump");

            StartCoroutine(SpawnWithWarning());
        }
    }

    IEnumerator SpawnWithWarning()
    {
        for (int i = 0; i < obstacleCount; i++)
        {
            float randX = GetSafeRandomX();

            Vector3 spawnPos = new Vector3(randX, spawnHeight, 0);

            // สร้างเส้นเตือน
            if (warningPrefab != null)
            {
                GameObject warn = Instantiate(warningPrefab, new Vector3(randX, 0, 0), Quaternion.identity);
                Destroy(warn, warningTime);
            }

            yield return new WaitForSeconds(warningTime);

            Instantiate(obstaclePrefab, spawnPos, Quaternion.Euler(0, 0, 180)); //  กลับหัว
        }
    }

    float GetSafeRandomX()
    {
        float randX;

        do
        {
            randX = Random.Range(-spawnRangeX, spawnRangeX);
        }
        while (Mathf.Abs(randX - safeCenter.position.x) < safeWidth);

        return randX;
    }

    IEnumerator BombLoop()
    {
        while (currentHealth > 0)
        {
            yield return new WaitForSeconds(bombInterval);

            float randX = GetSafeRandomX(); // ใช้อันเดียวกันเลย

            Vector3 spawnPos = new Vector3(randX, spawnHeight, 0);

            Instantiate(bombPrefab, spawnPos, Quaternion.identity);
        }
    }

    public void TakeDamage()
    {
        currentHealth--;
        if (currentHealth < 0) currentHealth = 0;

        UpdateUI();

        if (currentHealth <= 0)
        {
            StopAllCoroutines(); // หยุด spawn ทุกอย่าง

            if (animator != null)
                animator.SetTrigger("Dead");

            StartCoroutine(FadeAndLoadScene()); // เพิ่มตรงนี้
        }
    }
    float GetJumpInterval()
    {
        float hpPercent = (float)currentHealth / maxHealth;

        return Mathf.Lerp(minJumpInterval, maxJumpInterval, hpPercent);
    }
    IEnumerator FadeAndLoadScene()
    {
        yield return new WaitForSeconds(1f); // รอ anim boss ตายนิดนึง

        Color c = fadeScreen.color;

        while (c.a < 1)
        {
            c.a += Time.deltaTime * fadeSpeed;
            fadeScreen.color = c;
            yield return null;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
    }

    void UpdateUI()
    {
        for (int i = 0; i < healthImages.Length; i++)
        {
            healthImages[i].gameObject.SetActive(i < currentHealth);
        }
    }
}