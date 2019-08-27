using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipLister : MonoBehaviour
{
    [Header("Audio source must be from the camera!")]
    public AudioSource AudioSource;

    [Header("Number of audio clips.")]
    int ClipsLoaded = 0;
    public AudioClip[] AudioClips;


    // Start is called before the first frame update
    void Start()
    {
    }

    public void PlaySound(int ClipNumber, float Pitch, int Priority)
    {
        AudioSource.clip = AudioClips[ClipNumber];
        AudioSource.pitch = Pitch;
        AudioSource.priority = Priority;

        AudioSource.Play();
    }
}
