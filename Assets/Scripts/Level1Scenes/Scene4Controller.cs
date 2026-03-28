using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Scene4Controller : MonoBehaviour
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
    private int currentImageIndex = -1;
    private HashSet<int> usedImageIndexes = new HashSet<int>();

    private enum StepState
    {
        TypingInstructor,
        WaitingOptionClick,
        TypingFeedback,
        WaitingFeedbackClick,
        WaitingImageFeedbackClick, 
        Idle
    }

    private StepState stepState = StepState.Idle;

    public enum StepType
    {
        DialogueOnly,
        Options,
        MultiSelectOptions,
        ImageOption
    }

    [System.Serializable]
    public class ImageOption
    {
        public Button imageButton;         // Button to select this image
        public GameObject imageObject;     // Hierarchy image to show
        public string[] optionTexts;       // Option texts for this image
        public int correctOptionIndex;     // Correct option index
        public string correctFeedback;     // Feedback if correct
        public string wrongFeedback;       // Feedback if wrong
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

        public List<ImageOption> imageOptions;
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
            // Finish typing instantly for instructor
            if (stepState == StepState.TypingInstructor && typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;

                textComponent.text = typingTargetText;

                DialogueStep step = dialogueSteps[currentStepIndex];

                if (step.stepType != StepType.DialogueOnly)
                {
                    if (step.stepType == StepType.ImageOption)
                        ShowImageButtons();
                    else
                        ShowOptionButtons();

                    stepState = StepState.WaitingOptionClick;
                }
                else
                {
                    stepState = StepState.Idle;
                }

                return;
            }

            // Finish typing instantly for feedback
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

            // Continue after single-image feedback, back to remaining images
            if (stepState == StepState.WaitingImageFeedbackClick)
            {
                stepState = StepState.WaitingOptionClick;
                ShowImageButtons();
                return;
            }

            // Continue after final feedback (all images answered)
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
        usedImageIndexes.Clear();
        currentImageIndex = -1;

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
            if (step.stepType == StepType.ImageOption)
                ShowImageButtons();
            else
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
                        StartCoroutine(ShowFeedbackAndContinue(step.finalCorrectFeedback));
                    else
                        StartCoroutine(ShowTemporaryFeedback(step.partialCorrectFeedback));
                }
            }
            else
            {
                StartCoroutine(ShowTemporaryFeedback(step.feedbackWrong));
            }
        }
    }

    void ShowImageButtons()
    {
        DialogueStep step = dialogueSteps[currentStepIndex];

        for (int i = 0; i < step.imageOptions.Count; i++)
        {
            int index = i;
            ImageOption imgOption = step.imageOptions[i];

            bool used = usedImageIndexes.Contains(i);

            imgOption.imageButton.gameObject.SetActive(!used);
            imgOption.imageButton.interactable = !used;

            imgOption.imageButton.onClick.RemoveAllListeners();
            imgOption.imageButton.onClick.AddListener(() => OnImageButtonClicked(index));
        }

        stepState = StepState.WaitingOptionClick;
    }

    void OnImageButtonClicked(int imageIndex)
    {
        currentImageIndex = imageIndex;
        DialogueStep step = dialogueSteps[currentStepIndex];
        ImageOption imgOption = step.imageOptions[imageIndex];

        // Hide all images first
        foreach (var io in step.imageOptions)
            io.imageObject.SetActive(false);

        // Show selected image
        imgOption.imageObject.SetActive(true);

        // Show option buttons with per-image texts
        ShowOptionButtonsForImage(imgOption, imageIndex);
    }

    void ShowOptionButtonsForImage(ImageOption imgOption, int imageIndex)
    {
        for (int i = 0; i < optionButtons.Count; i++)
        {
            int optionIndex = i;
            optionButtons[i].gameObject.SetActive(true);
            optionButtons[i].interactable = true;

            if (imgOption.optionTexts != null && i < imgOption.optionTexts.Length)
                optionButtonTexts[i].text = imgOption.optionTexts[i];
            else
                optionButtonTexts[i].text = "Option " + (i + 1);

            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => OnImageOptionClicked(imageIndex, optionIndex));
        }

        stepState = StepState.WaitingOptionClick;
    }

    void OnImageOptionClicked(int imageIndex, int optionIndex)
    {
        DialogueStep step = dialogueSteps[currentStepIndex];
        ImageOption imgOption = step.imageOptions[imageIndex];

        bool correct = optionIndex == imgOption.correctOptionIndex;

        string feedback = correct ? imgOption.correctFeedback : imgOption.wrongFeedback;

        StartCoroutine(ShowImageOptionFeedback(feedback, imageIndex));
    }

    IEnumerator ShowImageOptionFeedback(string feedback, int imageIndex)
    {
        HideAllButtons();

        stepState = StepState.TypingFeedback;
        typingTargetText = feedback;
        typingCoroutine = StartCoroutine(TypeText(feedback));

        yield return typingCoroutine;
        typingCoroutine = null;

        usedImageIndexes.Add(imageIndex);

        DialogueStep step = dialogueSteps[currentStepIndex];
        if (usedImageIndexes.Count == step.imageOptions.Count)
        {
            // All images answered, ready to move to next step
            stepState = StepState.WaitingFeedbackClick;
        }
        else
        {
            // Still have images left, wait for user click to return
            stepState = StepState.WaitingImageFeedbackClick;
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

        DialogueStep step = dialogueSteps[currentStepIndex];
        if (step.stepType == StepType.ImageOption && step.imageOptions != null)
        {
            foreach (ImageOption io in step.imageOptions)
            {
                io.imageButton.gameObject.SetActive(false);
                io.imageObject.SetActive(false);
            }
        }
    }

    bool AnyButtonsActive()
    {
        foreach (Button b in optionButtons)
            if (b.gameObject.activeSelf) return true;

        DialogueStep step = dialogueSteps[currentStepIndex];
        if (step.stepType == StepType.ImageOption && step.imageOptions != null)
            foreach (ImageOption io in step.imageOptions)
                if (io.imageButton.gameObject.activeSelf) return true;

        return false;
    }

    void EndScene()
    {
        StartCoroutine(FadeOutAndGoToScene5());
    }

    IEnumerator FadeOutAndGoToScene5()
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
            panelManager.GoToScene5();
    }
}