using UnityEngine;

public class ObstacleTrap : MonoBehaviour
{
    public float fallSpeed = 5f;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = fallSpeed;

        // เผื่อ prefab ยังไม่กลับหัว
        transform.rotation = Quaternion.Euler(0, 0, 180);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerLives>()?.LoseLife();
            Destroy(gameObject);
        }
    }
}