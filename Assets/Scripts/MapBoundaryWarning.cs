using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapBoundaryWarning : MonoBehaviour
{
    public TextMeshProUGUI itemPickedInformationText;
    public GameObject itemPickedInformation;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger is happened");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Trigger is with PLAYER");
            if (gameObject.name == "RoadForwardWall")
            {
                Debug.Log("FORWARD WALL");
                itemPickedInformation.SetActive(true);
                itemPickedInformationText.text = "I need some fuel for the car. I can't walk all the way";
            }
            else if (gameObject.name == "RoadBackwardWall")
            {
                Debug.Log("BACK WALL");
                itemPickedInformation.SetActive(true);
                itemPickedInformationText.text = "There is nothing for me to go back to";
            }
            StartCoroutine(CloseInformationTime());
        }
    }

    IEnumerator CloseInformationTime()
    {
        yield return new WaitForSeconds(2.5f);
        itemPickedInformation.SetActive(false);
    }
}
