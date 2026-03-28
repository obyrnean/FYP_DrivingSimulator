using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InspectionMinigame : MonoBehaviour
{
    [Header("UI Elements")]
    public Image dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [Header("Buttons")]
    public Button licenseButton;
    public Button taxButton;
    public Button plateButton;
    public Button okButton;
    public Button skipButton;

    [Header("Images to Show")]
    public Image licenseImage;
    public Image taxImage;
    public Image plateImage;

    [Header("Instructor Text for Each Inspection (editable in Inspector)")]
    [TextArea(2, 5)]
    public string licenseText;
    [TextArea(2, 5)]
    public string taxText;
    [TextArea(2, 5)]
    public string plateText;

    [TextArea(2, 5)]
    public string introductionText; // initial line
    [TextArea(2, 5)]
    public string wellDoneText; // after finishing minigame
    [TextArea(2, 5)]
    public string inspectionsRemainingText; // if OK pressed early
    [TextArea(2, 5)]
    public string skipText; // if Skip pressed

    [Header("Settings")]
    public float textSpeed = 0.05f;
    public float fadeSpeed = 2f;

    private bool isTyping = false;
    private bool licenseDone = false;
    private bool taxDone = false;
    private bool plateDone = false;

    private void Start()
    {
        // Fade panel transparent at start
        if (dialoguePanel != null)
        {
            Color c = dialoguePanel.color;
            dialoguePanel.color = new Color(c.r, c.g, c.b, 0f);
        }

        dialogueText.text = "";

        // Hide all buttons and images at start
        HideAllButtons();
        HideAllImages();

        // Add button listeners
        licenseButton.onClick.AddListener(() => OnInspectionPressed("license"));
        taxButton.onClick.AddListener(() => OnInspectionPressed("tax"));
        plateButton.onClick.AddListener(() => OnInspectionPressed("plate"));
        okButton.onClick.AddListener(OnOkPressed);
        skipButton.onClick.AddListener(OnSkipPressed);

        // Start fade-in
        StartCoroutine(FadeInPanel());
    }

    IEnumerator FadeInPanel()
    {
        if (dialoguePanel == null) yield break;

        float alpha = 0f;
        Color c = dialoguePanel.color;

        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            dialoguePanel.color = new Color(c.r, c.g, c.b, Mathf.Clamp01(alpha));
            yield return null;
        }

        // Show buttons and introduction text
        ShowAllButtons();
        StartCoroutine(TypeText(introductionText));
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in text.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        isTyping = false;
    }

    void HideAllButtons()
    {
        licenseButton.gameObject.SetActive(false);
        taxButton.gameObject.SetActive(false);
        plateButton.gameObject.SetActive(false);
        okButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
    }

    void ShowAllButtons()
    {
        if (!licenseDone) licenseButton.gameObject.SetActive(true);
        if (!taxDone) taxButton.gameObject.SetActive(true);
        if (!plateDone) plateButton.gameObject.SetActive(true);
        okButton.gameObject.SetActive(true);
        skipButton.gameObject.SetActive(true);
    }

    void HideAllImages()
    {
        licenseImage.gameObject.SetActive(false);
        taxImage.gameObject.SetActive(false);
        plateImage.gameObject.SetActive(false);
    }

    void OnInspectionPressed(string type)
    {
        // Hide the clicked button so it can't be pressed again
        HideAllImages(); // clear previous inspection images

        switch (type)
        {
            case "license":
                licenseButton.gameObject.SetActive(false);
                licenseDone = true;
                licenseImage.gameObject.SetActive(true);
                StartCoroutine(TypeText(licenseText));
                break;
            case "tax":
                taxButton.gameObject.SetActive(false);
                taxDone = true;
                taxImage.gameObject.SetActive(true);
                StartCoroutine(TypeText(taxText));
                break;
            case "plate":
                plateButton.gameObject.SetActive(false);
                plateDone = true;
                plateImage.gameObject.SetActive(true);
                StartCoroutine(TypeText(plateText));
                break;
        }
    }

    void OnOkPressed()
    {
        HideAllImages(); // hide any inspection images

        if (licenseDone && taxDone && plateDone)
        {
            StartCoroutine(TypeText(wellDoneText));
            HideAllButtons(); // hide buttons after finishing
        }
        else
        {
            StartCoroutine(TypeText(inspectionsRemainingText));
            // Show remaining buttons again
            ShowAllButtons();
        }
    }

    void OnSkipPressed()
    {
        // Mark all inspections as done
        licenseDone = true;
        taxDone = true;
        plateDone = true;

        HideAllButtons();
        HideAllImages();

        StartCoroutine(TypeText(skipText));
    }
}