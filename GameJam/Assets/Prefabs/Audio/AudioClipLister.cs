using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipLister : MonoBehaviour
{

    public AudioSource AudioSource;
    public AudioClip[] AudioClips;
    int ClipsLoaded = 0;

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
