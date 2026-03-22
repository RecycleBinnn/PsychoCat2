using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform player;
    public float parallaxEffect; // 0 - 1 (ยิ่งน้อย = ยิ่งช้า)

    private float startX;
    private float spriteWidth;

    void Start()
    {
        startX = transform.position.x;
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float dist = player.position.x * parallaxEffect;
        transform.position = new Vector3(startX + dist, transform.position.y, transform.position.z);
    }
}