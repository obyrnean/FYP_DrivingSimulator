using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VehicleSelector : MonoBehaviour
{
    public Image currentImage;            // Main vehicle image
    public Image nextImage;               // For sliding animation

    public Sprite[] vehicleSprites;       // Vehicle sprites
    public Vector2[] vehicleSizes;        // Custom sizes per vehicle

    public Image selectionHighlight;      // Highlight object
    public Sprite[] highlightSprites;     // Highlight sprite per vehicle
    public Vector2[] highlightSizes;      // Custom sizes for each highlight

    public float slideDistance = 600f;
    public float slideSpeed = 6f;

    private int currentIndex = 0;         // Currently displayed vehicle
    private int selectedIndex = -1;       // Vehicle chosen via "Choose"
    private bool isAnimating = false;

    void Start()
    {
        // Set first vehicle
        currentImage.sprite = vehicleSprites[currentIndex];

        if (vehicleSizes.Length > currentIndex)
            currentImage.rectTransform.sizeDelta = vehicleSizes[currentIndex];

        nextImage.gameObject.SetActive(false);

        if (selectionHighlight != null)
            selectionHighlight.gameObject.SetActive(false);
    }

    public void NextVehicle()
    {
        if (isAnimating) return;
        int newIndex = (currentIndex + 1) % vehicleSprites.Length;
        StartCoroutine(Slide(newIndex, 1));
    }

    public void PreviousVehicle()
    {
        if (isAnimating) return;
        int newIndex = (currentIndex - 1 + vehicleSprites.Length) % vehicleSprites.Length;
        StartCoroutine(Slide(newIndex, -1));
    }

    IEnumerator Slide(int newIndex, int direction)
    {
        isAnimating = true;

        nextImage.sprite = vehicleSprites[newIndex];

        // Apply size to incoming vehicle
        if (vehicleSizes.Length > newIndex)
            nextImage.rectTransform.sizeDelta = vehicleSizes[newIndex];

        nextImage.gameObject.SetActive(true);

        RectTransform currentRect = currentImage.rectTransform;
        RectTransform nextRect = nextImage.rectTransform;

        Vector2 startPos = currentRect.anchoredPosition;
        Vector2 nextStartPos = new Vector2(direction * slideDistance, startPos.y);
        nextRect.anchoredPosition = nextStartPos;

        // Store highlight initial position if current vehicle is selected
        Vector2 highlightStartPos = Vector2.zero;
        Vector2 highlightEndPos = Vector2.zero;
        bool highlightSliding = selectionHighlight != null && selectedIndex == currentIndex;

        if (highlightSliding)
        {
            highlightStartPos = selectionHighlight.rectTransform.anchoredPosition;
            highlightEndPos = new Vector2(-direction * slideDistance, 0);
        }

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * slideSpeed;

            currentRect.anchoredPosition =
                Vector2.Lerp(startPos, new Vector2(-direction * slideDistance, startPos.y), t);

            nextRect.anchoredPosition =
                Vector2.Lerp(nextStartPos, startPos, t);

            // Move highlight along if sliding
            if (highlightSliding)
            {
                selectionHighlight.rectTransform.anchoredPosition =
                    Vector2.Lerp(highlightStartPos, highlightEndPos, t);
            }

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

        // Update highlight after sliding to ensure it snaps to center of selected vehicle
        UpdateHighlight();
    }

    // Call when player chooses a vehicle
    public void ChooseVehicle()
    {
        selectedIndex = currentIndex;
        UpdateHighlight();
    }

    // Enable highlight only for chosen vehicle
    void UpdateHighlight()
    {
        if (selectionHighlight == null) return;

        if (selectedIndex == currentIndex)
        {
            selectionHighlight.gameObject.SetActive(true);

            // Set the highlight sprite for the current vehicle
            if (highlightSprites.Length > currentIndex)
                selectionHighlight.sprite = highlightSprites[currentIndex];

            // Set highlight size independently of vehicle
            if (highlightSizes.Length > currentIndex)
                selectionHighlight.rectTransform.sizeDelta = highlightSizes[currentIndex];

            // Center the highlight
            selectionHighlight.rectTransform.anchoredPosition = Vector2.zero;
        }
        else
        {
            selectionHighlight.gameObject.SetActive(false);
        }
    }
}
