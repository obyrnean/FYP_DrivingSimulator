using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Scene21Controller : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textComponent;
    public Image dialoguePanel;

    [Header("Dialogue Settings")]
    public float textSpeed = 0.05f;
    public float delayBeforeDialogue = 1f;
    public float dialogueFadeDuration = 1.5f;

    [Header("Option Buttons")]
    public List<Button> optionButtons;
    public List<TextMeshProUGUI> optionButtonTexts;

    [Header("Panel Manager")]
    public PanelManager panelManager;

    private CanvasGroup dialogueCanvasGroup;
    private Coroutine typingCoroutine;
    private Coroutine startDialogueCoroutine;

    private int currentStepIndex = 0;
    private string typingTargetText = "";

    private enum StepState
    {
        TypingInstructor,
        WaitingOptionClick,
        Idle
    }

    private StepState stepState = StepState.Idle;

    public enum StepType
    {
        DialogueOnly,
        Options
    }

    [System.Serializable]
    public class DialogueStep
    {
        public string line;
        public StepType stepType = StepType.DialogueOnly;
        public int correctOptionIndex = -1;
        public string[] buttonTexts;
    }

    [Header("Dialogue Steps")]
    public List<DialogueStep> dialogueSteps = new List<DialogueStep>();


    private void OnEnable()
    {
        // Stop leftover coroutines (revisiting)
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        if (startDialogueCoroutine != null)
            StopCoroutine(startDialogueCoroutine);

        // Setup CanvasGroup
        dialogueCanvasGroup = dialoguePanel.GetComponent<CanvasGroup>();
        if (dialogueCanvasGroup == null)
            dialogueCanvasGroup = dialoguePanel.gameObject.AddComponent<CanvasGroup>();

        // FULL RESET
        currentStepIndex = 0;
        stepState = StepState.Idle;
        typingTargetText = "";
        textComponent.text = "";

        HideAllButtons();

        dialogueCanvasGroup.alpha = 0f;

        startDialogueCoroutine = StartCoroutine(StartDialogue());
    }

    IEnumerator StartDialogue()
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

        ShowCurrentStep();
    }

    private void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (stepState == StepState.TypingInstructor)
            {
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                    textComponent.text = typingTargetText;

                    DialogueStep step = dialogueSteps[currentStepIndex];

                    if (step.stepType == StepType.Options)
                    {
                        stepState = StepState.WaitingOptionClick;
                        ShowOptionButtons();
                    }
                    else
                    {
                        stepState = StepState.Idle;
                    }
                }
            }
            else if (stepState == StepState.Idle &&
                     currentStepIndex < dialogueSteps.Count &&
                     dialogueSteps[currentStepIndex].stepType == StepType.DialogueOnly)
            {
                currentStepIndex++;
                ShowCurrentStep();
            }
        }
    }

    void ShowCurrentStep()
    {
        if (currentStepIndex >= dialogueSteps.Count)
        {
            EndScene();
            return;
        }

        DialogueStep step = dialogueSteps[currentStepIndex];
        HideAllButtons();

        stepState = StepState.TypingInstructor;
        typingTargetText = step.line;

        typingCoroutine = StartCoroutine(TypeText(step.line, step.stepType));
    }

    IEnumerator TypeText(string text, StepType stepType)
    {
        textComponent.text = "";

        foreach (char c in text)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        if (stepType == StepType.Options)
        {
            ShowOptionButtons();
            stepState = StepState.WaitingOptionClick;
        }
        else
        {
            stepState = StepState.Idle;
        }
    }

    void ShowOptionButtons()
    {
        DialogueStep step = dialogueSteps[currentStepIndex];
        stepState = StepState.WaitingOptionClick;

        for (int i = 0; i < optionButtons.Count; i++)
        {
            int index = i;

            optionButtons[i].gameObject.SetActive(true);
            optionButtons[i].interactable = true;

            if (step.buttonTexts != null && i < step.buttonTexts.Length)
                optionButtonTexts[i].text = step.buttonTexts[i];
            else
                optionButtonTexts[i].text = "Option " + (i + 1);

            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => OnOptionClicked(index));
        }
    }

    void OnOptionClicked(int index)
    {
        DialogueStep step = dialogueSteps[currentStepIndex];
        HideAllButtons();

        if (index == step.correctOptionIndex)
        {
            StartCoroutine(FadeOutAndGoToScene22());
        }
        else
        {
            StartCoroutine(FadeOutAndGoToScene22_2());
        }
    }

    void HideAllButtons()
    {
        foreach (Button b in optionButtons)
            b.gameObject.SetActive(false);
    }

    void EndScene()
    {
        StartCoroutine(FadeOutAndGoToScene22());
    }

    IEnumerator FadeOutAndGoToScene22()
    {
        float t = 0f;

        while (t < dialogueFadeDuration)
        {
            t += Time.deltaTime;
            dialogueCanvasGroup.alpha = 1f - t / dialogueFadeDuration;
            yield return null;
        }

        dialogueCanvasGroup.alpha = 0f;

        if (panelManager != null)
            panelManager.GoToScene22();
    }

    IEnumerator FadeOutAndGoToScene22_2()
    {
        float t = 0f;

        while (t < dialogueFadeDuration)
        {
            t += Time.deltaTime;
            dialogueCanvasGroup.alpha = 1f - t / dialogueFadeDuration;
            yield return null;
        }

        dialogueCanvasGroup.alpha = 0f;

        if (panelManager != null)
            panelManager.GoToScene22_2();
    }
}