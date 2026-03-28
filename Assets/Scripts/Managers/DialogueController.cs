using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DialogueController : MonoBehaviour
{
    [Header("UI Elements")]
    public Image dialoguePanel;
    public TextMeshProUGUI textComponent;

    [Header("Introduction Dialogue")]
    public string[] introLines;
    public float textSpeed = 0.05f;

    [Header("Minigame Settings")]
    public string minigameIntroText;
    public string licenseText;
    public string taxText;
    public string plateText;
    public string inspectionsRemainingText;
    public string wellDoneText;
    public string skipText;

    [Header("Buttons")]
    public Button licenseButton;
    public Button taxButton;
    public Button plateButton;
    public Button okButton;
    public Button skipButton;

    [Header("Images")]
    public Image licenseImage;
    public Image taxImage;
    public Image plateImage;

    [Header("Panel Manager")]
    public PanelManager panelManager;

    private int index = 0;
    private bool isTyping = false;
    private bool choicesShown = false;
    private bool licenseDone = false;
    private bool taxDone = false;
    private bool plateDone = false;
    private bool inspectionImageActive = false;
    private bool minigameComplete = false;

    private Coroutine typingCoroutine;

    private void Start()
    {
        if (dialoguePanel == null)
        {
            Debug.LogError("dialoguePanel is NOT assigned!");
            return;
        }

        // FORCE alpha to 1 (fully visible)
        Color c = dialoguePanel.color;
        dialoguePanel.color = new Color(c.r, c.g, c.b, 1f);

        textComponent.text = "";
        HideAllButtons();
        HideAllImages();

        StartDialogue();
    }

    private void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (inspectionImageActive)
            {
                HideAllImages();
                inspectionImageActive = false;
                return;
            }

            if (minigameComplete)
            {
                panelManager.GoToScene2();
                return;
            }

            if (isTyping)
            {
                if (typingCoroutine != null)
                    StopCoroutine(typingCoroutine);

                if (index < introLines.Length)
                    textComponent.text = introLines[index];

                isTyping = false;
            }
            else
            {
                if (!choicesShown)
                {
                    if (index >= introLines.Length - 1)
                        ShowMinigame();
                    else
                        NextLine();
                }
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        typingCoroutine = StartCoroutine(TypeLine(introLines[index]));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        textComponent.text = "";

        foreach (char c in line.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
    }

    void NextLine()
    {
        if (index < introLines.Length - 1)
        {
            index++;
            typingCoroutine = StartCoroutine(TypeLine(introLines[index]));
        }
    }

    void ShowMinigame()
    {
        choicesShown = true;

        licenseButton.gameObject.SetActive(true);
        taxButton.gameObject.SetActive(true);
        plateButton.gameObject.SetActive(true);
        okButton.gameObject.SetActive(true);
        skipButton.gameObject.SetActive(true);

        HideAllImages();
        typingCoroutine = StartCoroutine(TypeLine(minigameIntroText));

        licenseButton.onClick.RemoveAllListeners();
        taxButton.onClick.RemoveAllListeners();
        plateButton.onClick.RemoveAllListeners();
        okButton.onClick.RemoveAllListeners();
        skipButton.onClick.RemoveAllListeners();

        licenseButton.onClick.AddListener(() => OnInspectionPressed("license"));
        taxButton.onClick.AddListener(() => OnInspectionPressed("tax"));
        plateButton.onClick.AddListener(() => OnInspectionPressed("plate"));
        okButton.onClick.AddListener(OnOkPressed);
        skipButton.onClick.AddListener(OnSkipPressed);
    }

    void OnInspectionPressed(string type)
    {
        HideAllImages();

        switch (type)
        {
            case "license":
                licenseDone = true;
                licenseButton.gameObject.SetActive(false);
                licenseImage.gameObject.SetActive(true);
                typingCoroutine = StartCoroutine(TypeLine(licenseText));
                break;

            case "tax":
                taxDone = true;
                taxButton.gameObject.SetActive(false);
                taxImage.gameObject.SetActive(true);
                typingCoroutine = StartCoroutine(TypeLine(taxText));
                break;

            case "plate":
                plateDone = true;
                plateButton.gameObject.SetActive(false);
                plateImage.gameObject.SetActive(true);
                typingCoroutine = StartCoroutine(TypeLine(plateText));
                break;
        }

        inspectionImageActive = true;
    }

    void OnOkPressed()
    {
        HideAllImages();

        if (licenseDone && taxDone && plateDone)
        {
            typingCoroutine = StartCoroutine(TypeLine(wellDoneText));
            HideAllButtons();
            minigameComplete = true;
        }
        else
        {
            typingCoroutine = StartCoroutine(TypeLine(inspectionsRemainingText));
        }
    }

    void OnSkipPressed()
    {
        licenseDone = true;
        taxDone = true;
        plateDone = true;

        HideAllButtons();
        HideAllImages();

        typingCoroutine = StartCoroutine(TypeLine(skipText));
        minigameComplete = true;
    }

    void HideAllButtons()
    {
        licenseButton.gameObject.SetActive(false);
        taxButton.gameObject.SetActive(false);
        plateButton.gameObject.SetActive(false);
        okButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
    }

    void HideAllImages()
    {
        licenseImage.gameObject.SetActive(false);
        taxImage.gameObject.SetActive(false);
        plateImage.gameObject.SetActive(false);
        inspectionImageActive = false;
    }
}