using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 5f;
    public Vector3 offset;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    [Header("Zoom")]
    public float normalSize = 5f;
    public float zoomOutSize = 8f;
    public float zoomSpeed = 3f;

    float targetSize;
    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        targetSize = normalSize;
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;

        float clampX = Mathf.Clamp(desiredPosition.x, minX, maxX);
        float clampY = Mathf.Clamp(desiredPosition.y, minY, maxY);

        Vector3 clampedPosition = new Vector3(clampX, clampY, offset.z);

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, clampedPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;
        cam.orthographicSize = Mathf.Lerp(
        cam.orthographicSize,
        targetSize,
        zoomSpeed * Time.deltaTime
    );
    }
    public void SetZoom(bool isBossArea)
    {
        if (isBossArea)
            targetSize = zoomOutSize;
        else
            targetSize = normalSize;
    }
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
}