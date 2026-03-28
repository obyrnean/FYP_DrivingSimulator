using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Scene14Controller : MonoBehaviour
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

    private int currentStepIndex = 0;
    private string typingTargetText = "";

    private HashSet<int> selectedCorrectOptions = new HashSet<int>();

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
        MultiSelectOptions
    }

    [System.Serializable]
    public class DialogueStep
    {
        public string line;
        public StepType stepType = StepType.DialogueOnly;

        public int correctOptionIndex = -1;
        public List<int> correctOptionIndexes = new List<int>();

        public string partialCorrectFeedback = "Good, there are more correct options.";
        public string finalCorrectFeedback = "Excellent! You found them all.";
        public string feedbackWrong = "That is not correct, try again.";

        public string[] buttonTexts;
    }

    [Header("Dialogue Steps")]
    public List<DialogueStep> dialogueSteps = new List<DialogueStep>();

    private void Start()
    {
        dialogueCanvasGroup = dialoguePanel.GetComponent<CanvasGroup>();
        if (dialogueCanvasGroup == null)
            dialogueCanvasGroup = dialoguePanel.gameObject.AddComponent<CanvasGroup>();

        dialogueCanvasGroup.alpha = 0f;
        textComponent.text = "";

        HideAllButtons();
        StartCoroutine(StartDialogue());
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
            if (stepState == StepState.TypingInstructor && typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;

                textComponent.text = typingTargetText;

                DialogueStep step = dialogueSteps[currentStepIndex];

                if (step.stepType != StepType.DialogueOnly)
                {
                    ShowOptionButtons();
                    stepState = StepState.WaitingOptionClick;
                }
                else
                {
                    stepState = StepState.Idle;
                }

                return;
            }

            if (stepState == StepState.TypingFeedback && typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;

                textComponent.text = typingTargetText;

                if (!AnyButtonsActive())
                    stepState = StepState.WaitingFeedbackClick;
                else
                    stepState = StepState.WaitingOptionClick;

                return;
            }

            // Continue after final feedback
            if (stepState == StepState.WaitingFeedbackClick)
            {
                stepState = StepState.Idle;
                currentStepIndex++;
                ShowCurrentStep();
                return;
            }

            // Continue dialogue-only
            if (stepState == StepState.Idle &&
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
        selectedCorrectOptions.Clear();

        stepState = StepState.TypingInstructor;
        typingTargetText = step.line;
        typingCoroutine = StartCoroutine(TypeText(step.line));
    }

    IEnumerator TypeText(string text)
    {
        textComponent.text = "";

        foreach (char c in text)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        typingCoroutine = null;

        DialogueStep step = dialogueSteps[currentStepIndex];

        if (step.stepType != StepType.DialogueOnly)
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

        stepState = StepState.WaitingOptionClick;
    }

    void OnOptionClicked(int index)
    {
        DialogueStep step = dialogueSteps[currentStepIndex];

        if (step.stepType == StepType.Options)
        {
            string feedback = index == step.correctOptionIndex
                ? step.finalCorrectFeedback
                : step.feedbackWrong;

            StartCoroutine(ShowFeedbackAndContinue(feedback));
        }
        else if (step.stepType == StepType.MultiSelectOptions)
        {
            if (step.correctOptionIndexes.Contains(index))
            {
                if (!selectedCorrectOptions.Contains(index))
                {
                    selectedCorrectOptions.Add(index);
                    optionButtons[index].interactable = false;

                    if (selectedCorrectOptions.Count == step.correctOptionIndexes.Count)
                    {
                        StartCoroutine(ShowFeedbackAndContinue(step.finalCorrectFeedback));
                    }
                    else
                    {
                        StartCoroutine(ShowTemporaryFeedback(step.partialCorrectFeedback));
                    }
                }
            }
            else
            {
                StartCoroutine(ShowTemporaryFeedback(step.feedbackWrong));
            }
        }
    }

    IEnumerator ShowTemporaryFeedback(string feedback)
    {
        stepState = StepState.TypingFeedback;

        typingTargetText = feedback;
        typingCoroutine = StartCoroutine(TypeText(feedback));

        yield return typingCoroutine;

        typingCoroutine = null;
        stepState = StepState.WaitingOptionClick;
    }

    IEnumerator ShowFeedbackAndContinue(string feedback)
    {
        HideAllButtons();

        stepState = StepState.TypingFeedback;

        typingTargetText = feedback;
        typingCoroutine = StartCoroutine(TypeText(feedback));

        yield return typingCoroutine;

        typingCoroutine = null;
        stepState = StepState.WaitingFeedbackClick;
    }

    void HideAllButtons()
    {
        foreach (Button b in optionButtons)
            b.gameObject.SetActive(false);
    }

    bool AnyButtonsActive()
    {
        foreach (Button b in optionButtons)
        {
            if (b.gameObject.activeSelf)
                return true;
        }
        return false;
    }

    void EndScene()
    {
        StartCoroutine(FadeOutAndGoToScene15());
    }

    IEnumerator FadeOutAndGoToScene15()
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
            panelManager.GoToScene15();
    }
}