using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement")]
    public float distance = 3f;
    public float speed = 2f;
    public float angle = 0f;

    [Header("Wait Time")]
    public float waitTime = 1f;

    Vector3 startPos;
    Vector3 targetPos;
    Vector3 lastPos;

    bool movingToTarget = true;
    bool isWaiting = false;

    Transform playerOnPlatform;

    void Start()
    {
        startPos = transform.position;

        Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.right;
        targetPos = startPos + (Vector3)(dir * distance);

        lastPos = transform.position;
    }

    void Update()
    {
        if (isWaiting) return;

        if (movingToTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
                StartCoroutine(WaitAndSwitch());
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, startPos) < 0.01f)
                StartCoroutine(WaitAndSwitch());
        }
    }

    void LateUpdate()
    {
        Vector3 delta = transform.position - lastPos;

        if (playerOnPlatform != null)
        {
            playerOnPlatform.position += delta;
        }

        lastPos = transform.position;
    }

    IEnumerator WaitAndSwitch()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        movingToTarget = !movingToTarget;
        isWaiting = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.contacts[0].normal.y < -0.5f)
            {
                playerOnPlatform = collision.transform;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = null;
        }
    }
}