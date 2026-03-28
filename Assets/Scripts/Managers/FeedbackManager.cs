using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class FeedbackManager : MonoBehaviour
{
    public TMP_InputField feedbackInput; 
    public Button sendButton;            

    void Start()
    {
        // Add listener to Send button
        if (sendButton != null)
            sendButton.onClick.AddListener(SendFeedback);
    }

    void SendFeedback()
    {
        if (feedbackInput != null)
        {
            string feedback = feedbackInput.text;
            Debug.Log("Feedback received: " + feedback); // just print for now
            feedbackInput.text = ""; // clear input field
        }
    }
}
