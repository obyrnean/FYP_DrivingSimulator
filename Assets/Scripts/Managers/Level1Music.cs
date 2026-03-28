using UnityEngine;

public class Level1Music : MonoBehaviour
{
    public AudioClip level1Music;

    void Start()
    {
        AudioManager.Instance.musicSource.clip = level1Music;
        AudioManager.Instance.musicSource.Play();
    }
}