using UnityEngine;
using System.Collections;

public class FallingTrap : MonoBehaviour
{
    public enum FallDirection
    {
        Down,
        Left,
        Right,
        Up
    }

    [Header("Trigger")]
    public float triggerWidth = 2f;
    public float triggerHeight = 1f;
    public LayerMask playerLayer;

    [Header("Fall")]
    public float fallDelay = 0.2f;
    public float fallForce = 10f;
    public FallDirection fallDirection = FallDirection.Down;

    [Header("Reset")]
    public bool canReset = false;
    public float resetTime = 3f;

    Vector3 startPos;
    Quaternion startRot;
    Rigidbody2D rb;

    bool isTriggered = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        startPos = transform.position;
        startRot = transform.rotation; // ®”¡ÿ¡‡√‘Ë¡µÈπ

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0;
    }

    void Update()
    {
        if (isTriggered) return;

        Collider2D hit = Physics2D.OverlapBox(
            transform.position + Vector3.down * 0.5f,
            new Vector2(triggerWidth, triggerHeight),
            0f,
            playerLayer
        );

        if (hit != null)
        {
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall()
    {
        isTriggered = true;
        yield return StartCoroutine(Shake());

        yield return new WaitForSeconds(fallDelay);

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0;

        Vector2 dir = GetDirection();

        rb.linearVelocity = dir * fallForce;

        if (canReset)
        {
            yield return new WaitForSeconds(resetTime);
            ResetTrap();
        }
    }

    Vector2 GetDirection()
    {
        switch (fallDirection)
        {
            case FallDirection.Left:
                return Vector2.left;
            case FallDirection.Right:
                return Vector2.right;
            case FallDirection.Up:
                return Vector2.up;
            default:
                return Vector2.down;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerLives>()?.LoseLife();
        }
    }
    void ResetTrap()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;

        transform.position = startPos;
        transform.rotation = startRot; //  √’‡´Áµ¡ÿ¡

        isTriggered = false;
    }

    IEnumerator Shake()
    {
        float t = 0f;
        float duration = 0.2f;
        float amount = 0.05f;

        Vector3 original = transform.position;

        while (t < duration)
        {
            transform.position = original + (Vector3)Random.insideUnitCircle * amount;
            t += Time.deltaTime;
            yield return null;
        }

        transform.position = original;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            transform.position + Vector3.down * 0.5f,
            new Vector3(triggerWidth, triggerHeight, 1)
        );
    }
}