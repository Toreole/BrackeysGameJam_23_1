using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AmbientSoundPlayer : MonoBehaviour
{
    [SerializeField]
    private Vector2 intervalRange; 
    [SerializeField]
    private AudioClip[] clips;
    [SerializeField]
    private Vector2 pitchRange;
    [SerializeField]
    private Vector2 volumeRange;

    private float nextPlay;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextPlay)
        {
            audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
            audioSource.volume = Random.Range(volumeRange.x, volumeRange.y);
            audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
            nextPlay = Time.time + Random.Range(intervalRange.x, intervalRange.y);
        }
    }
}
