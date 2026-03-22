using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndingTrigger : MonoBehaviour
{
    [Header("UI")]
    public GameObject interactHint; // Press E
    public GameObject endingUI;     // กล่อง The End
    public TMP_Text endText;
    public TMP_Text pressMText;
    public Image fadeScreen;

    [Header("Settings")]
    public float fadeSpeed = 2f;
    public string menuSceneName = "Scene_Menu";

    bool isPlayerNear = false;
    bool isEnding = false;
    bool canPressM = false;

    void Start()
    {
        interactHint.SetActive(false);
        endingUI.SetActive(false);

        endText.gameObject.SetActive(false);
        pressMText.gameObject.SetActive(false);

        fadeScreen.color = new Color(0, 0, 0, 0);
    }

    void Update()
    {
        // กด E เริ่ม Ending
        if (isPlayerNear && !isEnding && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(PlayEnding());
        }

        // กด M กลับเมนู
        if (isEnding && canPressM && Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene(menuSceneName);
        }
    }

    IEnumerator PlayEnding()
    {
        isEnding = true;
        interactHint.SetActive(false);

        Color c = fadeScreen.color;

        // fade ดำ
        while (c.a < 1)
        {
            c.a += Time.deltaTime * fadeSpeed;
            fadeScreen.color = c;
            yield return null;
        }

        // เปิด UI ตอนดำสนิท
        endingUI.SetActive(true);

        endText.gameObject.SetActive(true);
        pressMText.gameObject.SetActive(true);

        canPressM = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            isPlayerNear = true;

            if (!isEnding)
                interactHint.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            isPlayerNear = false;
            interactHint.SetActive(false);
        }
    }
}