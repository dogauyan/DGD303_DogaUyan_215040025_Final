using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booming : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().outputAudioMixerGroup = VolumeManager.SoundFx;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Finished()
    {
        Destroy(gameObject);
    }
}
