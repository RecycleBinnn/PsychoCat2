using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    public Image fadeScreen;
    public Image lifeImage;
    public Sprite life3;
    public Sprite life2;
    public Sprite life1;
    public Sprite life0;

    public Transform player;
    public Transform spawnPoint;

    public float fadeSpeed = 2f;
    public GameObject deathUI;

    bool isDead = false;
    public PlayerLives playerLives;
    public TMP_Text pressRText;
    public TMP_Text pressMText;
    bool isGameOver = false;

    bool canPressKey = false;
    bool isLevelComplete = false;
    string nextSceneName = "";
    public TMP_Text levelCompleteText;

    void Start()
    {
        if (GameState.shouldFadeIn)
        {
            fadeScreen.color = new Color(0, 0, 0, 1); // เริ่มดำ
            StartCoroutine(FadeIn());
            GameState.shouldFadeIn = false;
        }
        else
        {
            fadeScreen.color = new Color(0, 0, 0, 0);
        }

        deathUI.SetActive(false);
        pressRText.gameObject.SetActive(false);
        pressMText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isDead) return;
        if (!canPressKey) return;

        if (isLevelComplete && Input.GetKeyDown(KeyCode.N))
        {
            GameState.shouldFadeIn = true; //  บอก scene ใหม่ให้ fade
            SceneManager.LoadScene(nextSceneName);
        }

        if (!isGameOver && !isLevelComplete && Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }

        if (isGameOver && Input.GetKeyDown(KeyCode.M))
        {
            GameState.shouldFadeIn = true;
            SceneManager.LoadScene("Scene_Menu");
        }
    }

    public void PlayerDied()
    {
        if (playerLives.lives <= 0)
            isGameOver = true;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        StartCoroutine(FadeDeath());
    }

    IEnumerator FadeDeath()
    {
        isDead = true;
        canPressKey = false;

        Color c = fadeScreen.color;

        while (c.a < 1)
        {
            c.a += Time.deltaTime * fadeSpeed;
            fadeScreen.color = c;
            yield return null;
        }

        deathUI.SetActive(true);
        UpdateLifeUI();

        if (isGameOver)
        {
            pressRText.gameObject.SetActive(false);
            pressMText.gameObject.SetActive(true);
        }
        else
        {
            pressRText.gameObject.SetActive(true);
            pressMText.gameObject.SetActive(false);
        }

        canPressKey = true;
    }

    void Respawn()
    {
        pressRText.gameObject.SetActive(false);
        pressMText.gameObject.SetActive(false);
        levelCompleteText.gameObject.SetActive(false);
        isGameOver = false;

        player.position = spawnPoint.position;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = true;
        }

        player.GetComponent<PlayerLives>().isDead = false;

        StartCoroutine(FadeBack());
    }
    void UpdateLifeUI()
    {
        switch (playerLives.lives)
        {
            case 3:
                lifeImage.sprite = life3;
                break;

            case 2:
                lifeImage.sprite = life2;
                break;

            case 1:
                lifeImage.sprite = life1;
                break;

            default:
                lifeImage.sprite = life0;
                break;
        }
    }
    IEnumerator FadeBack()
    {
        GameState.isDeadScreen = false;
        deathUI.SetActive(false);

        Color c = fadeScreen.color;

        while (c.a > 0)
        {
            c.a -= Time.deltaTime * fadeSpeed;
            fadeScreen.color = c;
            yield return null;
        }

        isDead = false;
    }
    public void LevelComplete(string sceneName)
    {
        nextSceneName = sceneName;
        StartCoroutine(FadeLevelComplete());
    }
    IEnumerator FadeLevelComplete()
    {
        isDead = true;
        isLevelComplete = true;
        canPressKey = false;

        Color c = fadeScreen.color;

        while (c.a < 1)
        {
            c.a += Time.deltaTime * fadeSpeed;
            fadeScreen.color = c;
            yield return null;
        }

        deathUI.SetActive(true);

        UpdateLifeUI();

        // เปิดข้อความใหญ่
        levelCompleteText.gameObject.SetActive(true);


        // ข้อความกด N
        pressRText.text = "Press N to go to Next Level";
        pressRText.gameObject.SetActive(true);
        pressMText.gameObject.SetActive(false);

        canPressKey = true;
    }
    IEnumerator FadeIn()
    {
        Color c = fadeScreen.color;

        while (c.a > 0)
        {
            c.a -= Time.deltaTime * fadeSpeed;
            fadeScreen.color = c;
            yield return null;
        }
    }
}