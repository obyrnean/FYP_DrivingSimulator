using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CarInspection : MonoBehaviour
{
    public Image currentImage;
    public Image nextImage;

    public Sprite[] vehicleSprites;
    public Vector2[] vehicleSizes; // custom sizes

    public float slideDistance = 600f;
    public float slideSpeed = 6f;

    private int currentIndex = 0;
    private bool isAnimating = false;

    void Start()
    {
        currentImage.sprite = vehicleSprites[currentIndex];

        // Apply starting size
        if (vehicleSizes.Length > currentIndex)
            currentImage.rectTransform.sizeDelta = vehicleSizes[currentIndex];

        nextImage.gameObject.SetActive(false);
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
    }
}
