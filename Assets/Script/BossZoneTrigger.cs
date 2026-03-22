using UnityEngine;

public class BossZoneTrigger : MonoBehaviour
{
    public CameraFollow cam;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            cam.SetZoom(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            cam.SetZoom(false);
        }
    }
}