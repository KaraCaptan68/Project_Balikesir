using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScaryRun : MonoBehaviour
{
    public GameObject scaryPic;
    public float runSpeed = 1.0f;
    public bool closeEnough = false;
    Vector3 targetposition = new Vector3();
    void Start()
    {
        targetposition.x = -137;
        targetposition.y = 3;
        targetposition.z = -260;
    }

    // Update is called once per frame
    void Update()
    {
        if (closeEnough)
        {
            scaryPic.transform.position = Vector3.MoveTowards(scaryPic.transform.position, targetposition, runSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            closeEnough = true;
        }
    }
}
