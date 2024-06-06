using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private int bulletCount = 0;
    private int medkitCount = 0;

    public TextMeshProUGUI itemPickedInformationText;
    public GameObject itemPickedInformation;

    public TextMeshProUGUI bulletCountText;
    public TextMeshProUGUI medkitCountText;

    public PlayerController playerController;


    public void BulletCollected()
    {
        // Logic for handling ammo collection
        bulletCount++;
        //Debug.Log("Bullet collected. Total bullet: " + bulletCount);
        itemPickedInformation.SetActive(true);
        itemPickedInformationText.text = "I got a bullet";
        bulletCountText.text = bulletCount.ToString();
        StartCoroutine(CloseInformationTime());
    }
    public void MedkitCollected()
    {
        // Logic for handling medkit collection
        medkitCount++;
        //Debug.Log("Medkit collected. Total medkits: " + medkitCount);
        itemPickedInformation.SetActive(true);
        itemPickedInformationText.text = "I got a medkit";
        medkitCountText.text = medkitCount.ToString();
        StartCoroutine(CloseInformationTime());
    }
    public void MedkitUsed()
    {
        if (medkitCount > 0 && playerController.currentHealth < playerController.maxHealth)
        {
            medkitCount--;
            Debug.Log("Medkit used. Total medkits: " + medkitCount);
            itemPickedInformationText.text = "I used a medkit";
            itemPickedInformation.SetActive(true);
            medkitCountText.text = medkitCount.ToString();

            if (playerController.currentHealth + playerController.medkitHealRate > playerController.maxHealth)
            {
                playerController.currentHealth = playerController.maxHealth;
                playerController.UpdateHealthUI();
            }
            else
            {
                playerController.currentHealth = playerController.currentHealth + playerController.medkitHealRate;
                playerController.UpdateHealthUI();
            }
            StartCoroutine(CloseInformationTime());
        }
        else if (medkitCount <= 0)
        {
            itemPickedInformationText.text = "I don't have a medkit";
            itemPickedInformation.SetActive(true);
            StartCoroutine(CloseInformationTime());
            Debug.Log("Don't have medkit");
        }
        else
        {
            itemPickedInformationText.text = "I am at full health";
            itemPickedInformation.SetActive(true);
            StartCoroutine(CloseInformationTime());
            Debug.Log("ý am at full health");
        }
    }
    IEnumerator CloseInformationTime()
    {
        yield return new WaitForSeconds(1.5f);
        itemPickedInformation.SetActive(false);
    }
}
