using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public Image fadeImage;
    public TMP_Text fadeText;

    public float fadeDuration = 1.5f;
    public float textDelay = 1f;
    public float sceneDelay = 2f;

    public string nextSceneName = "Level1";

    public void StartTransition()
    {
        StartCoroutine(FadeSequence());
    }

    IEnumerator FadeSequence()
    {
        // Fade to black
        float t = 0;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        // แสดงข้อความ
        fadeText.gameObject.SetActive(true);
        fadeText.text = "พักร้อนเหรอ... หึ บางทีการพักร้อนที่ยาวที่สุดอาจจะเป็นการหยุดหายใจไปเลยก็ได้นะ";

        yield return new WaitForSeconds(sceneDelay);

        // เปลี่ยนฉาก
        SceneManager.LoadScene(nextSceneName);
    }
}