using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Scene37Controller : MonoBehaviour
{
    [Header("UI Elements")]
    public Image dialoguePanel;
    public TextMeshProUGUI textComponent;

    [Header("Introduction Dialogue")]
    public string[] introLines;
    public float textSpeed = 0.05f;

    [Header("Minigame Settings")]
    public string minigameIntroText;

    public string feedback1;
    public string feedback2;
    public string feedback3;
    public string feedback4;
    public string feedback5;
    public string feedback6;
    public string feedback7;

    public string inspectionsRemainingText;
    public string outroText;
    public string skipText;

    [Header("Buttons")]
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;
    public Button button6;
    public Button button7;

    public Button okButton;
    public Button skipButton;

    [Header("Panel Manager")]
    public PanelManager panelManager;

    private int index = 0;
    private bool isTyping = false;
    private bool choicesShown = false;
    private bool minigameComplete = false;
    private bool outroStarted = false;

    private bool[] buttonDone = new bool[7];

    private Coroutine typingCoroutine;
    private string currentLine;

    private void Start()
    {
        Color c = dialoguePanel.color;
        dialoguePanel.color = new Color(c.r, c.g, c.b, 1f);

        textComponent.text = "";
        HideAllButtons();

        StartDialogue();
    }

    private void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // If typing -> finish instantly
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                textComponent.text = currentLine;
                isTyping = false;
                return;
            }

            // If outro finished -> go next scene
            if (minigameComplete && !isTyping)
            {
                panelManager.GoToScene38();
                return;
            }

            // Intro dialogue flow
            if (!choicesShown)
            {
                if (index >= introLines.Length - 1)
                    ShowMinigame();
                else
                    NextLine();
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
        currentLine = line;
        textComponent.text = "";

        foreach (char c in line)
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

        button1.gameObject.SetActive(true);
        button2.gameObject.SetActive(true);
        button3.gameObject.SetActive(true);
        button4.gameObject.SetActive(true);
        button5.gameObject.SetActive(true);
        button6.gameObject.SetActive(true);
        button7.gameObject.SetActive(true);

        okButton.gameObject.SetActive(true);
        skipButton.gameObject.SetActive(true);

        typingCoroutine = StartCoroutine(TypeLine(minigameIntroText));

        button1.onClick.AddListener(() => OnInspectionPressed(0, feedback1));
        button2.onClick.AddListener(() => OnInspectionPressed(1, feedback2));
        button3.onClick.AddListener(() => OnInspectionPressed(2, feedback3));
        button4.onClick.AddListener(() => OnInspectionPressed(3, feedback4));
        button5.onClick.AddListener(() => OnInspectionPressed(4, feedback5));
        button6.onClick.AddListener(() => OnInspectionPressed(5, feedback6));
        button7.onClick.AddListener(() => OnInspectionPressed(6, feedback7));

        okButton.onClick.AddListener(OnOkPressed);
        skipButton.onClick.AddListener(OnSkipPressed);
    }

    void OnInspectionPressed(int index, string feedback)
    {
        if (buttonDone[index]) return;

        buttonDone[index] = true;
        GetButtonByIndex(index).gameObject.SetActive(false);

        typingCoroutine = StartCoroutine(TypeLine(feedback));
    }

    void OnOkPressed()
    {
        bool allDone = true;

        for (int i = 0; i < buttonDone.Length; i++)
        {
            if (!buttonDone[i])
            {
                allDone = false;
                break;
            }
        }

        if (allDone)
        {
            StartOutro(outroText);
        }
        else
        {
            typingCoroutine = StartCoroutine(TypeLine(inspectionsRemainingText));
        }
    }

    void OnSkipPressed()
    {
        StartOutro(skipText);
    }

    void StartOutro(string line)
    {
        HideAllButtons();
        minigameComplete = true;
        outroStarted = true;

        typingCoroutine = StartCoroutine(TypeLine(line));
    }

    Button GetButtonByIndex(int i)
    {
        switch (i)
        {
            case 0: return button1;
            case 1: return button2;
            case 2: return button3;
            case 3: return button4;
            case 4: return button5;
            case 5: return button6;
            case 6: return button7;
        }
        return null;
    }

    void HideAllButtons()
    {
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(false);
        button4.gameObject.SetActive(false);
        button5.gameObject.SetActive(false);
        button6.gameObject.SetActive(false);
        button7.gameObject.SetActive(false);
        okButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
    }
}