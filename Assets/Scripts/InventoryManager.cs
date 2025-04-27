using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public TextMeshProUGUI itemPickedInformationText;
    public GameObject itemPickedInformation;

    public PlayerController playerController;


    IEnumerator CloseInformationTime()
    {
        yield return new WaitForSeconds(1.5f);
        itemPickedInformation.SetActive(false);
    }
}
