using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Scene2MinigameController : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textComponent;
    public Image dialoguePanel;

    [Header("Intro")]
    public string introLine;
    public float textSpeed = 0.05f;

    [Header("Dialogue Fade Settings")]
    public float delayBeforeDialogue = 2f;
    public float dialogueFadeDuration = 1.5f;

    [Header("Buttons")]
    public List<Button> partButtons;
    public Button skipButton;

    [Header("Instructor Prompts")]
    public List<string> prompts;

    [Header("Feedback Text")]
    public string correctText = "Well done!";
    public string wrongText = "Wrong, try again!";
    public string allDoneText =
        "All covered, well done! Knowing your controls means you can react quickly and safely when driving.\nLet’s move on.";

    [Header("Panel Manager")]
    public PanelManager panelManager;

    private int currentIndex = 0;
    private bool isTyping = false;
    private bool minigameStarted = false;
    private bool minigameComplete = false;

    private CanvasGroup dialogueCanvasGroup;
    private Coroutine typingCoroutine;
    private string currentLine = "";

    private void Start()
    {
        // Setup CanvasGroup for fading
        dialogueCanvasGroup = dialoguePanel.GetComponent<CanvasGroup>();
        if (dialogueCanvasGroup == null)
            dialogueCanvasGroup = dialoguePanel.gameObject.AddComponent<CanvasGroup>();

        dialogueCanvasGroup.alpha = 0f;
        textComponent.text = "";

        // Hide buttons at start
        foreach (Button btn in partButtons)
            btn.gameObject.SetActive(false);

        skipButton.gameObject.SetActive(false);

        // Start fade-in + delayed dialogue
        StartCoroutine(DelayedDialogueStart());
    }

    IEnumerator DelayedDialogueStart()
    {
        yield return new WaitForSeconds(delayBeforeDialogue);

        float t = 0f;
        while (t < dialogueFadeDuration)
        {
            t += Time.deltaTime;
            dialogueCanvasGroup.alpha = t / dialogueFadeDuration;
            yield return null;
        }
        dialogueCanvasGroup.alpha = 1f;

        // Start typing intro
        currentLine = introLine;
        typingCoroutine = StartCoroutine(TypeText(currentLine));
    }

    private void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Finish line instantly if typing
            if (isTyping && typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                textComponent.text = currentLine;
                isTyping = false;
                return;
            }

            // If minigame is done, proceed to next scene
            if (minigameComplete && !isTyping)
            {
                panelManager.GoToScene3();
                return;
            }

            // Start minigame only after intro is fully displayed
            if (!minigameStarted && dialogueCanvasGroup.alpha >= 1f && !isTyping)
            {
                StartMinigame();
            }
        }
    }

    void StartMinigame()
    {
        minigameStarted = true;

        for (int i = 0; i < partButtons.Count; i++)
        {
            int index = i;
            partButtons[i].gameObject.SetActive(true);
            partButtons[i].onClick.RemoveAllListeners();
            partButtons[i].onClick.AddListener(() => OnPartClicked(index));
        }

        skipButton.gameObject.SetActive(true);
        skipButton.onClick.RemoveAllListeners();
        skipButton.onClick.AddListener(OnSkipPressed);

        AskNextQuestion();
    }

    void AskNextQuestion()
    {
        if (currentIndex >= prompts.Count)
        {
            currentLine = allDoneText;
            typingCoroutine = StartCoroutine(TypeText(currentLine));
            minigameComplete = true;
            return;
        }

        currentLine = prompts[currentIndex];
        typingCoroutine = StartCoroutine(TypeText(currentLine));
    }

    void OnPartClicked(int clickedIndex)
    {
        if (minigameComplete || isTyping) return;

        if (clickedIndex == currentIndex)
        {
            partButtons[clickedIndex].gameObject.SetActive(false);
            StartCoroutine(CorrectRoutine());
        }
        else
        {
            currentLine = wrongText;
            typingCoroutine = StartCoroutine(TypeText(currentLine));
        }
    }

    IEnumerator CorrectRoutine()
    {
        currentLine = correctText;
        typingCoroutine = StartCoroutine(TypeText(currentLine));
        yield return typingCoroutine;
        yield return new WaitForSeconds(0.5f);

        currentIndex++;
        AskNextQuestion();
    }

    void OnSkipPressed()
    {
        minigameComplete = true;

        foreach (Button btn in partButtons)
            btn.gameObject.SetActive(false);

        skipButton.gameObject.SetActive(false);

        currentLine = allDoneText;
        typingCoroutine = StartCoroutine(TypeText(currentLine));
    }

    IEnumerator TypeText(string line)
    {
        isTyping = true;
        textComponent.text = "";

        foreach (char c in line)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
    }
}