using UnityEngine;

public class BombPickup : MonoBehaviour
{
    public float throwForce = 10f;

    Rigidbody2D rb;
    Collider2D col;

    bool isHeld = false;
    Transform holder;

    public GameObject explosionEffect;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (isHeld && holder != null)
        {
            transform.position = holder.position;
        }
    }

    public void PickUp(Transform holdPoint)
    {
        isHeld = true;
        holder = holdPoint;

        rb.simulated = false;
        col.enabled = false;
    }

    public void Throw(Vector2 direction)
    {
        isHeld = false;
        holder = null;

        rb.simulated = true;
        col.enabled = true;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * throwForce, ForceMode2D.Impulse);
        direction.y += 0.3f;
        direction.Normalize();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!isHeld)
        {
            BossController boss = col.GetComponentInParent<BossController>();

            if (boss != null)
            {
                // สร้างเอฟเฟกต์ระเบิด
                if (explosionEffect != null)
                {
                    GameObject fx = Instantiate(explosionEffect, transform.position, Quaternion.identity);
                    Destroy(fx, 1f); // ลบทิ้งหลังเล่นจบ
                }

                boss.TakeDamage();
                Destroy(gameObject);
            }
        }
    }
}