using UnityEngine;

public class WarningFlash : MonoBehaviour
{
    public float flashSpeed = 10f;
    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float a = Mathf.Abs(Mathf.Sin(Time.time * flashSpeed));
        sr.color = new Color(1, 0, 0, a);
    }
}