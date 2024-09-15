using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestAmbianceTrigger : MonoBehaviour
{
    public AudioSource ambianceAudioSource;
    public float fadeDuration = 1.0f; // Saniye cinsinden fade süresi
    public float maxVolume = 0.6f; // Maksimum ses seviyesi
    private Coroutine fadeCoroutine; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = StartCoroutine(FadeIn(ambianceAudioSource, fadeDuration));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = StartCoroutine(FadeOut(ambianceAudioSource, fadeDuration));
        }
    }

    private IEnumerator FadeIn(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;
        audioSource.volume = 0f;
        audioSource.Play();

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            audioSource.volume = Mathf.Lerp(0f, maxVolume, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = maxVolume;
    }

    private IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
    }
}
