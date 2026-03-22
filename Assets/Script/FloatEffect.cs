using UnityEngine;

public class FloatEffect : MonoBehaviour
{
    public float speed = 3f;
    public float height = 0.1f;

    Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        transform.localPosition = startPos + Vector3.up * Mathf.Sin(Time.time * speed) * height;
    }
}
