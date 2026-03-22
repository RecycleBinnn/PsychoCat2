using UnityEngine;
using TMPro;

public class DoorLevel : MonoBehaviour
{
    public string nextScene = "Level2";
    public GameObject pressEText;

    bool playerInside = false;
    RespawnManager respawnManager;

    void Start()
    {
        respawnManager = FindObjectOfType<RespawnManager>();
        pressEText.SetActive(false);
    }

    void Update()
    {
        if (playerInside)
        {
            pressEText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                pressEText.SetActive(false);
                respawnManager.LevelComplete(nextScene);
            }
        }
        else
        {
            pressEText.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInside = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInside = false;
    }
}
