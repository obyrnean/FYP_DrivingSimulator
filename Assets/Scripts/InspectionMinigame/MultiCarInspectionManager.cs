using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

[System.Serializable]
public class InspectionPart
{
    public Image arrow;               
    public TMP_InputField input;      
    public string correctName;        

    public Sprite blueSprite; // Default
    public Sprite greenSprite; // Correct
    public Sprite redSprite; // Wrong

    public int sideIndex;             
}

public class MultiCarInspectionManager : MonoBehaviour
{
    [Header("Car Sliding")]
    public Image currentImage;
    public Image nextImage;
    public Sprite[] vehicleSprites;
    public Vector2[] vehicleSizes;
    public float slideDistance = 600f;
    public float slideSpeed = 6f;

    [Header("Side Containers")]
    public GameObject[] sideContainers;

    [Header("Parts")]
    public InspectionPart[] carParts;

    private int currentIndex = 0;
    private bool isAnimating = false;

    void Start()
    {
        // Set initial car image
        currentImage.sprite = vehicleSprites[currentIndex];
        if (vehicleSizes.Length > currentIndex)
            currentImage.rectTransform.sizeDelta = vehicleSizes[currentIndex];

        nextImage.gameObject.SetActive(false);

        UpdateSideContainers();
    }

    // Call on arrow buttons
    public void NextSide()
    {
        if (isAnimating) return;
        int newIndex = (currentIndex + 1) % vehicleSprites.Length;
        StartCoroutine(Slide(newIndex, 1));
    }

    public void PreviousSide()
    {
        if (isAnimating) return;
        int newIndex = (currentIndex - 1 + vehicleSprites.Length) % vehicleSprites.Length;
        StartCoroutine(Slide(newIndex, -1));
    }

    IEnumerator Slide(int newIndex, int direction)
    {
        isAnimating = true;

        nextImage.sprite = vehicleSprites[newIndex];
        if (vehicleSizes.Length > newIndex)
            nextImage.rectTransform.sizeDelta = vehicleSizes[newIndex];
        nextImage.gameObject.SetActive(true);

        RectTransform currentRect = currentImage.rectTransform;
        RectTransform nextRect = nextImage.rectTransform;

        Vector2 startPos = currentRect.anchoredPosition;
        Vector2 nextStartPos = new Vector2(direction * slideDistance, startPos.y);
        nextRect.anchoredPosition = nextStartPos;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * slideSpeed;

            currentRect.anchoredPosition =
                Vector2.Lerp(startPos, new Vector2(-direction * slideDistance, startPos.y), t);
            nextRect.anchoredPosition =
                Vector2.Lerp(nextStartPos, startPos, t);

            yield return null;
        }

        currentRect.anchoredPosition = startPos;

        // Swap references
        Image temp = currentImage;
        currentImage = nextImage;
        nextImage = temp;
        nextImage.gameObject.SetActive(false);

        currentIndex = newIndex;
        isAnimating = false;

        // Show only the container for the current side
        UpdateSideContainers();
    }

    void UpdateSideContainers()
    {
        for (int i = 0; i < sideContainers.Length; i++)
        {
            sideContainers[i].SetActive(i == currentIndex);
        }

        // Reset all parts for the new side to blue
        foreach (InspectionPart part in carParts)
        {
            if (part.sideIndex == currentIndex)
            {
                part.arrow.sprite = part.blueSprite;
            }
        }
    }

    // Called by Check button
    public void CheckAnswers()
    {
        foreach (InspectionPart part in carParts)
        {
            if (part.sideIndex != currentIndex) continue;

            string answer = part.input.text.ToLower().Trim();
            string correct = part.correctName.ToLower();

            part.arrow.sprite = (answer == correct) ? part.greenSprite : part.redSprite;
        }
    }

    // Show correct answers
    public void ShowAnswers()
    {
        foreach (InspectionPart part in carParts)
        {
            if (part.sideIndex != currentIndex) continue;

            part.input.text = part.correctName;
            part.arrow.sprite = part.greenSprite;
        }
    }

    // Reset inputs for current side
    public void ResetAnswers()
    {
        foreach (InspectionPart part in carParts)
        {
            if (part.sideIndex != currentIndex) continue;

            part.input.text = "";
            part.arrow.sprite = part.blueSprite;
        }
    }
}
