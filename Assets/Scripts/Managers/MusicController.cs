using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    public AudioSource musicSource;
    public Button musicButton;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;

    private bool isPlaying;

    private static MusicController instance;

    void Awake()
    {
        // Prevent duplicates if scene reloads
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Load saved music state (default = ON)
        isPlaying = PlayerPrefs.GetInt("MusicState", 1) == 1;

        if (musicSource != null)
        {
            if (isPlaying)
                musicSource.Play();
            else
                musicSource.Pause();
        }

        // Set correct button image
        if (musicButton != null)
        {
            musicButton.image.sprite = isPlaying ? musicOnSprite : musicOffSprite;
            musicButton.onClick.AddListener(ToggleMusic);
        }
    }

    public void ToggleMusic()
    {
        isPlaying = !isPlaying;

        if (musicSource != null)
        {
            if (isPlaying)
                musicSource.Play();
            else
                musicSource.Pause();
        }

        // Save setting
        PlayerPrefs.SetInt("MusicState", isPlaying ? 1 : 0);
        PlayerPrefs.Save();

        // Update button image
        if (musicButton != null)
        {
            musicButton.image.sprite = isPlaying ? musicOnSprite : musicOffSprite;
        }
    }
}