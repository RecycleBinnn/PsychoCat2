using UnityEngine;
using UnityEngine.UI; // สำหรับ Image Component

public class CreditsManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject creditsPanel;    // ตัว Pop-up หลัก
    public Image creditsImageDisplay; // Component Image ที่ใช้วาดรูป

    [Header("Navigation Buttons")]
    public GameObject prevButton;      // ปุ่มย้อนกลับ
    public GameObject nextButton;      // ปุ่มหน้าถัดไป

    [Header("Credits Sprites (Put 11 Sprites here)")]
    public Sprite[] creditSprites;    // เก็บไฟล์รูปภาพทั้ง 10 รูป

    private int currentIndex = 0;

    void Start()
    {
        // ปิด Pop-up ไว้ตอนเริ่มเกม
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false);
        }
    }

    // ฟังก์ชันเปิด Pop-up (ใช้ผูกกับปุ่ม "Credits" หน้าเมนู)
    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
        currentIndex = 0; // เริ่มหน้าที่ 1 เสมอ
        UpdateDisplay();
    }

    // ฟังก์ชันปิด Pop-up
    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }

    // ฟังก์ชันไปหน้าถัดไป
    public void ShowNextCredit()
    {
        if (currentIndex < creditSprites.Length - 1)
        {
            currentIndex++;
            UpdateDisplay();
        }
    }

    // ฟังก์ชันย้อนกลับไปหน้าก่อนหน้า
    public void ShowPrevCredit()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateDisplay();
        }
    }

    // ฟังก์ชันอัปเดตการแสดงผล (รูปและปุ่ม)
    private void UpdateDisplay()
    {
        // 1. เปลี่ยนรูปภาพตาม Index ปัจจุบัน
        if (creditsImageDisplay != null && creditSprites.Length > 0)
        {
            creditsImageDisplay.sprite = creditSprites[currentIndex];
        }

        // 2. ซ่อนปุ่ม "ย้อนกลับ" ถ้าอยู่หน้าแรก
        if (currentIndex == 0)
        {
            prevButton.SetActive(false);
        }
        else
        {
            prevButton.SetActive(true);
        }

        // 3. ซ่อนปุ่ม "หน้าถัดไป" ถ้าอยู่หน้าสุดท้าย
        if (currentIndex >= creditSprites.Length - 1)
        {
            nextButton.SetActive(false);
        }
        else
        {
            nextButton.SetActive(true);
        }
    }
}