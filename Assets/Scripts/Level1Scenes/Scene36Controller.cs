using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Scene36Controller : MonoBehaviour
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

    [Header("Single Continue Button")]
    public Button singleContinueButton;
    public TextMeshProUGUI singleContinueButtonText;

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
        TypingFeedback,
        WaitingFeedbackClick,
        Idle
    }

    private StepState stepState = StepState.Idle;

    public enum StepType
    {
        DialogueOnly,
        Options,
        SingleButton   // NEW
    }

    [System.Serializable]
    public class DialogueStep
    {
        public string line;
        public StepType stepType = StepType.DialogueOnly;

        public int correctOptionIndex = -1;
        public string feedbackCorrect = "Well done!";
        public string feedbackWrong = "Wrong, try again!";

        public string[] buttonTexts;
    }

    [Header("Dialogue Steps")]
    public List<DialogueStep> dialogueSteps = new List<DialogueStep>();

    private void OnEnable()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        if (startDialogueCoroutine != null)
            StopCoroutine(startDialogueCoroutine);

        dialogueCanvasGroup = dialoguePanel.GetComponent<CanvasGroup>();
        if (dialogueCanvasGroup == null)
            dialogueCanvasGroup = dialoguePanel.gameObject.AddComponent<CanvasGroup>();

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
            if (stepState == StepState.TypingInstructor || stepState == StepState.TypingFeedback)
            {
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                    textComponent.text = typingTargetText;

                    DialogueStep step = dialogueSteps[currentStepIndex];

                    if (stepState == StepState.TypingInstructor)
                    {
                        if (step.stepType == StepType.Options)
                        {
                            ShowOptionButtons();
                            stepState = StepState.WaitingOptionClick;
                        }
                        else if (step.stepType == StepType.SingleButton)
                        {
                            ShowSingleButton();
                            stepState = StepState.WaitingOptionClick;
                        }
                        else
                        {
                            stepState = StepState.Idle;
                        }
                    }
                    else if (stepState == StepState.TypingFeedback)
                    {
                        stepState = StepState.WaitingFeedbackClick;
                    }
                }
            }
            else if (stepState == StepState.WaitingFeedbackClick)
            {
                stepState = StepState.Idle;
                currentStepIndex++;
                ShowCurrentStep();
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
        else if (stepType == StepType.SingleButton)
        {
            ShowSingleButton();
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

    void ShowSingleButton()
    {
        singleContinueButton.gameObject.SetActive(true);
        singleContinueButton.interactable = true;

        singleContinueButton.onClick.RemoveAllListeners();
        singleContinueButton.onClick.AddListener(OnSingleButtonClicked);
    }

    void OnSingleButtonClicked()
    {
        HideAllButtons();
        stepState = StepState.Idle;
        currentStepIndex++;
        ShowCurrentStep();
    }

    void OnOptionClicked(int index)
    {
        DialogueStep step = dialogueSteps[currentStepIndex];

        HideAllButtons();

        string feedback = index == step.correctOptionIndex
            ? step.feedbackCorrect
            : step.feedbackWrong;

        typingTargetText = feedback;
        stepState = StepState.TypingFeedback;

        typingCoroutine = StartCoroutine(TypeFeedback(feedback));
    }

    IEnumerator TypeFeedback(string feedback)
    {
        textComponent.text = "";

        foreach (char c in feedback)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        stepState = StepState.WaitingFeedbackClick;
    }

    void HideAllButtons()
    {
        foreach (Button b in optionButtons)
            b.gameObject.SetActive(false);

        if (singleContinueButton != null)
            singleContinueButton.gameObject.SetActive(false);
    }

    void EndScene()
    {
        StartCoroutine(FadeOutAndGoToScene37());
    }

    IEnumerator FadeOutAndGoToScene37()
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
            panelManager.GoToScene37();
    }
}