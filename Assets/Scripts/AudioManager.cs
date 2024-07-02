using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour// selamın aleyküm
{
    public AudioSource audioSource;
    public List<AudioClip> audioClips;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(int index)
    {
        if (index >= 0 && index < audioClips.Count)
        {
            audioSource.clip = audioClips[index];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Invalid sound index: " + index);
        }
    }

    public void PlaySoundUnscaled(int index)
    {
        if (index >= 0 && index < audioClips.Count)
        {
            audioSource.PlayOneShot(audioClips[index]);
        }
        else
        {
            Debug.LogWarning("Invalid sound index: " + index);
        }
    }

    public void ToggleBackgroundMusic(AudioSource backgroundMusicSource)
    {
        if (backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Pause();
        }
        else
        {
            backgroundMusicSource.Play();
        }
    }
}
