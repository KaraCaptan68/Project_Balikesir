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
        audioSource.clip = null;  // Baþlangýçta AudioSource boþ olacak
    }

    void Update()
    {
        if (playerController.velocity.magnitude > 0.1f && playerController.velocity.magnitude < 7f) // Walking
        {
            if (audioSource.clip != walkingAudio)  // Eðer þu an çalan ses yürüme sesi deðilse
            {
                audioSource.clip = walkingAudio;   // Yürüme sesini ayarla
            }
            if (!audioSource.isPlaying)  // Eðer ses çalmýyorsa, çalmaya baþla
            {
                audioSource.Play();
            }
        }
        else if (playerController.velocity.magnitude >= 7f) // Running
        {
            if (audioSource.clip != runningAudio)  // Eðer þu an çalan ses koþma sesi deðilse
            {
                audioSource.clip = runningAudio;   // Koþma sesini ayarla
            }
            if (!audioSource.isPlaying)  // Eðer ses çalmýyorsa, çalmaya baþla
            {
                audioSource.Play();
            }
        }
        else // Standing
        {
            if (audioSource.isPlaying)  // Eðer bir ses çalýyorsa
            {
                audioSource.Stop();  // Sesi durdur
            }
        }
    }
}
