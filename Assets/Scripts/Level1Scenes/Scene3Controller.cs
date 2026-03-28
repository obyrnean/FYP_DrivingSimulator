using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Scene3Controller : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textComponent;
    public Image dialoguePanel;

    [Header("Intro Dialogue")]
    public string[] introLines;
    public float textSpeed = 0.05f;

    [Header("Dialogue Fade Settings")]
    public float delayBeforeDialogue = 2f;
    public float dialogueFadeDuration = 1.5f;

    [Header("Buttons Minigame")]
    public List<Button> stepButtons;
    public List<string> stepFeedback;

    [Header("Completion Dialogue")]
    public string wrongStepText = "You may have missed a step!";
    public string allDoneText = "All steps well followed, well done!";

    [Header("Panel Manager")]
    public PanelManager panelManager;

    private int introIndex = 0;
    private int currentStep = 0;

    private bool isTyping = false;
    private bool minigameStarted = false;
    private bool minigameComplete = false;

    private CanvasGroup dialogueCanvasGroup;
    private Coroutine typingCoroutine;
    private string currentLine = "";

    private void Start()
    {
        // Setup CanvasGroup like Scene 2
        dialogueCanvasGroup = dialoguePanel.GetComponent<CanvasGroup>();
        if (dialogueCanvasGroup == null)
            dialogueCanvasGroup = dialoguePanel.gameObject.AddComponent<CanvasGroup>();

        dialogueCanvasGroup.alpha = 0f;
        textComponent.text = "";

        HideAllButtons();

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

        // Start intro typing
        NextIntroLine();
    }

    private void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Finish typing instantly
            if (isTyping && typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                textComponent.text = currentLine;
                isTyping = false;
                return;
            }

            // If intro finished -> start minigame
            if (!minigameStarted && dialogueCanvasGroup.alpha >= 1f && introIndex >= introLines.Length)
            {
                StartMinigame();
                return;
            }

            // If minigame complete -> go to Scene 4
            if (minigameComplete && !isTyping)
            {
                panelManager.GoToScene4();
                return;
            }

            // Advance intro dialogue
            if (!minigameStarted && !isTyping && introIndex < introLines.Length)
            {
                NextIntroLine();
            }
        }
    }

    #region Intro Dialogue

    void NextIntroLine()
    {
        if (introIndex < introLines.Length)
        {
            currentLine = introLines[introIndex];
            typingCoroutine = StartCoroutine(TypeText(currentLine));
            introIndex++;
        }
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

    #endregion

    #region Minigame

    void StartMinigame()
    {
        minigameStarted = true;
        currentStep = 0;

        for (int i = 0; i < stepButtons.Count; i++)
        {
            int index = i;

            stepButtons[i].gameObject.SetActive(true);
            stepButtons[i].interactable = true;

            Image img = stepButtons[i].GetComponent<Image>();
            if (img != null)
                img.color = Color.white;

            stepButtons[i].onClick.RemoveAllListeners();
            stepButtons[i].onClick.AddListener(() => OnStepClicked(index));
        }
    }

    void OnStepClicked(int index)
    {
        if (minigameComplete || isTyping)
            return;

        if (index == currentStep)
        {
            currentLine = stepFeedback[index];
            typingCoroutine = StartCoroutine(TypeText(currentLine));

            Image img = stepButtons[index].GetComponent<Image>();
            if (img != null)
                img.color = new Color(0.3f, 0.3f, 0.3f, 1f);

            stepButtons[index].interactable = false;

            currentStep++;

            if (currentStep >= stepButtons.Count)
            {
                StartCoroutine(MinigameCompleteRoutine());
            }
        }
        else
        {
            currentLine = wrongStepText;
            typingCoroutine = StartCoroutine(TypeText(currentLine));
        }
    }

    IEnumerator MinigameCompleteRoutine()
    {
        minigameComplete = true;

        yield return new WaitForSeconds(0.5f);

        currentLine = allDoneText;
        typingCoroutine = StartCoroutine(TypeText(currentLine));
        yield return typingCoroutine;
    }

    void HideAllButtons()
    {
        foreach (Button btn in stepButtons)
            btn.gameObject.SetActive(false);
    }

    #endregion
}