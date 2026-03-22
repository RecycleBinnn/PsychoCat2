using UnityEngine;
using UnityEngine.UI;

public class PlayerLives : MonoBehaviour
{
    public int lives = 3;

    public Image livesImage;

    public Sprite life3;
    public Sprite life2;
    public Sprite life1;
    public Sprite life0;

    public bool isDead = false;

    public Transform spawnPoint;
    public float fallLimit = -10f;

    void Start()
    {
        UpdateLifeUI();
    }

    void Update()
    {
        // µ°·¡æ
        if (transform.position.y < fallLimit)
        {
            LoseLife();
        }
    }

    public void LoseLife()
    {
        if (isDead) return;

        isDead = true;

        lives--;

        if (lives < 0)
            lives = 0;

        UpdateLifeUI();

        FindFirstObjectByType<RespawnManager>().PlayerDied();
    }
    void Respawn()
    {
        transform.position = spawnPoint.position;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void UpdateLifeUI()
    {
        switch (lives)
        {
            case 3:
                livesImage.sprite = life3;
                break;
            case 2:
                livesImage.sprite = life2;
                break;
            case 1:
                livesImage.sprite = life1;
                break;
            default:
                livesImage.sprite = life0;
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Obstacle"))
        {
            LoseLife();
        }
    }
}