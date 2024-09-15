using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    public float bobSpeed = 5f;         // Headbob hýzýný belirler
    public float bobAmount = 0.05f;     // Sallanmanýn þiddetini belirler
    public float walkingBob = 5f;       //Yürürken sallanma
    public float runningBob = 9f;       //Koþarken sallanma
    public CharacterController controller; // Karakterin hareketini kontrol eden component
    private float defaultYPos = 0f;     // Kameranýn varsayýlan Y pozisyonu
    private float timer = 0f;           // Zamanlayýcý

    void Start()
    {
        // Kameranýn baþlangýçtaki Y pozisyonunu kaydeder
        defaultYPos = transform.localPosition.y;
    }

    void Update()
    {
        if (controller.velocity.magnitude > 0.1f && controller.velocity.magnitude < 7f) // Hareket ediyorsa 
        {
            //Debug.Log(controller.velocity.magnitude);
            bobSpeed = walkingBob;
            // Sinüs fonksiyonu ile sallanmayý hesapla
            timer += Time.deltaTime * bobSpeed;
            float newY = defaultYPos + Mathf.Sin(timer) * bobAmount;
            transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        }
        else if (controller.velocity.magnitude > 8f) //Koþuyorsa
        {
            bobSpeed = runningBob;
            // Sinüs fonksiyonu ile sallanmayý hesapla
            timer += Time.deltaTime * bobSpeed;
            float newY = defaultYPos + Mathf.Sin(timer) * bobAmount;
            transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        }
        else
        {
            // Hareket etmiyorsa kamerayý varsayýlan konuma döndür
            timer = 0f;
            transform.localPosition = new Vector3(transform.localPosition.x, defaultYPos, transform.localPosition.z);
        }
    }
}
