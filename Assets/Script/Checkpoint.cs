using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Vector3 lastCheckpoint;

    void Start()
    {
        lastCheckpoint = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            lastCheckpoint = transform.position;
            Debug.Log("Checkpoint Updated");
        }
    }
}