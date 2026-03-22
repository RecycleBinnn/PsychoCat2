using UnityEngine;

public class VerticalPlatforms : MonoBehaviour
{
    private PlatformEffector2D effector;
    private float waitCount;
    public float waitTimeDefault = 0.5f;

    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
        waitCount = waitTimeDefault;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            if (waitCount <= 0)
            {
                effector.rotationalOffset = 180f;
            }
            else
            {
                waitCount -= Time.deltaTime;
            }
        }

        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            effector.rotationalOffset = 0f;
            waitCount = waitTimeDefault;
        }
    }
}