using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundSlide : MonoBehaviour
{
    public AudioSource audioSource;
    public RawImage image;
    public float speed;
    Rect rect;

    // Start is called before the first frame update
    void Start()
    {
        rect = image.uvRect;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.state == GameManager.GameState.ESC) return;
        if (audioSource.outputAudioMixerGroup == null && VolumeManager.Music != null)
        {
            audioSource.outputAudioMixerGroup = VolumeManager.Music;
            audioSource.Play();
        }

        rect.y += Time.deltaTime * speed;
        if (rect.y > 1) rect.y--;
        image.uvRect = rect;
    }
}
