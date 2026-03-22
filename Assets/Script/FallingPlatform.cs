using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour
{
    [Header("Delay Before Fall")]
    public float fallDelay = 0.5f;

    [Header("Shake")]
    public float shakeDuration = 0.3f;
    public float shakeAmount = 0.1f;

    [Header("Fall")]
    public float fallGravity = 3f;

    [Header("Reset (optional)")]
    public bool resetPlatform = false;
    public float resetTime = 3f;
    Quaternion startRotation;

    Vector3 startPos;
    Rigidbody2D rb;
    bool isTriggered = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        startRotation = transform.rotation; //  เพิ่มบรรทัดนี้

        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isTriggered) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FallSequence());
        }
    }

    IEnumerator FallSequence()
    {
        isTriggered = true;

        //  สั่นก่อนตก
        yield return StartCoroutine(Shake());

        // รอเวลา
        yield return new WaitForSeconds(fallDelay);

        // เริ่มตก
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = fallGravity;

        // ถ้าต้องรีเซ็ต
        if (resetPlatform)
        {
            yield return new WaitForSeconds(resetTime);
            ResetPlatform();
        }
    }

    IEnumerator Shake()
    {
        float timer = 0f;

        while (timer < shakeDuration)
        {
            Vector3 offset = Random.insideUnitCircle * shakeAmount;
            transform.position = startPos + offset;

            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = startPos;
    }

    void ResetPlatform()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f; // หยุดการหมุน

        transform.position = startPos;
        transform.rotation = startRotation; // รีมุมกลับ

        isTriggered = false;
    }
}