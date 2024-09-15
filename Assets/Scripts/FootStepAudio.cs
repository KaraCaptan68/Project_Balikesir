using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepAudio : MonoBehaviour
{
    private CharacterController playerController;
    private AudioSource audioSource;
    public AudioClip walkingAudio;
    public AudioClip runningAudio;

    void Start()
    {
        playerController = GetComponentInParent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = null;  // Ba�lang��ta AudioSource bo� olacak
    }

    void Update()
    {
        if (playerController.velocity.magnitude > 0.1f && playerController.velocity.magnitude < 7f) // Walking
        {
            if (audioSource.clip != walkingAudio)  // E�er �u an �alan ses y�r�me sesi de�ilse
            {
                audioSource.clip = walkingAudio;   // Y�r�me sesini ayarla
            }
            if (!audioSource.isPlaying)  // E�er ses �alm�yorsa, �almaya ba�la
            {
                audioSource.Play();
            }
        }
        else if (playerController.velocity.magnitude >= 7f) // Running
        {
            if (audioSource.clip != runningAudio)  // E�er �u an �alan ses ko�ma sesi de�ilse
            {
                audioSource.clip = runningAudio;   // Ko�ma sesini ayarla
            }
            if (!audioSource.isPlaying)  // E�er ses �alm�yorsa, �almaya ba�la
            {
                audioSource.Play();
            }
        }
        else // Standing
        {
            if (audioSource.isPlaying)  // E�er bir ses �al�yorsa
            {
                audioSource.Stop();  // Sesi durdur
            }
        }
    }
}
