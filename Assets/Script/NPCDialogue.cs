using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text nameText;

    public Image darkOverlay;
    public SceneTransition sceneTransition;

    public DialogueLine[] lines;
    bool playerInRange = false;
    public GameObject interactHint;

    int index = 0;
    bool isTalking = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isTalking)
                StartDialogue();
            else
                NextLine();
        }
    }

    void StartDialogue()
    {
        isTalking = true;
        dialoguePanel.SetActive(true);
        darkOverlay.gameObject.SetActive(true);

        index = 0;
        ShowLine();
    }

    void NextLine()
    {
        index++;

        if (index < lines.Length)
            ShowLine();
        else
            EndDialogue();
    }

    void ShowLine()
    {
        var line = lines[index];

        dialogueText.text = line.text;
        nameText.text = line.speakerName;

        // ทำให้ทุกตัวมืดก่อน
        foreach (var l in lines)
        {
            SetDim(l.speakerImage, true);
        }

        // ตัวที่พูด  สว่าง  อยู่หน้าสุด
        SetDim(line.speakerImage, false);
        line.speakerImage.transform.SetAsLastSibling();
    }

    void SetDim(Image img, bool dim)
    {
        if (dim)
            img.color = new Color(1, 1, 1, 0.5f); // มืด
        else
            img.color = Color.white; // สว่าง
    }
    [System.Serializable]
    public class DialogueLine
    {
        public string speakerName;
        [TextArea] public string text;
        public Image speakerImage;
    }

    void EndDialogue()
    {
        isTalking = false;
        dialoguePanel.SetActive(false);
        darkOverlay.gameObject.SetActive(false);

        sceneTransition.StartTransition();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            playerInRange = true;
            interactHint.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            playerInRange = false;
            interactHint.SetActive(false);
        }
    }

}