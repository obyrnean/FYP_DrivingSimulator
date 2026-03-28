using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Toggle Button")]
    public Button soundToggleButton;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    [Header("Audio Source")]
    public AudioSource sfxSource;

    [Header("Sound Clips")]
    public AudioClip playButtonClip;
    public AudioClip carDoorClip;
    public AudioClip beepingClip;
    public AudioClip drivingClip;
    public AudioClip trainClip;
    public AudioClip angryPersonClip;
    public AudioClip tacklingClip;
    public AudioClip moneyClip;
    public AudioClip peopleTalkingClip;

    private bool soundEnabled;


    public bool soundsEnabled => soundEnabled;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        soundEnabled = PlayerPrefs.GetInt("SoundState", 1) == 1;

        if (soundToggleButton != null)
        {
            soundToggleButton.image.sprite = soundEnabled ? soundOnSprite : soundOffSprite;
            soundToggleButton.onClick.AddListener(ToggleSound);
        }
    }

    public void ToggleSound()
    {
        soundEnabled = !soundEnabled;

        PlayerPrefs.SetInt("SoundState", soundEnabled ? 1 : 0);
        PlayerPrefs.Save();

        if (soundToggleButton != null)
            soundToggleButton.image.sprite = soundEnabled ? soundOnSprite : soundOffSprite;
    }

    public void PlaySound(AudioClip clip)
    {
        if (soundEnabled && sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }


    public void PlayPlayButton() => PlaySound(playButtonClip);
    public void PlayCarDoor() => PlaySound(carDoorClip);
    public void PlayBeep() => PlaySound(beepingClip);
    public void PlayDriving() => PlaySound(drivingClip);
    public void PlayTrain() => PlaySound(trainClip);
    public void PlayAngryPerson() => PlaySound(angryPersonClip);
    public void PlayTackling() => PlaySound(tacklingClip);
    public void PlayMoney() => PlaySound(moneyClip);
    public void PlayPeopleTalking() => PlaySound(peopleTalkingClip);


    public void PlayPlayButtonOnClick()
    {
        PlayPlayButton();
    }
}