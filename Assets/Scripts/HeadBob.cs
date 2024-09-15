using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    public float bobSpeed = 5f;         // Headbob h�z�n� belirler
    public float bobAmount = 0.05f;     // Sallanman�n �iddetini belirler
    public float walkingBob = 5f;       //Y�r�rken sallanma
    public float runningBob = 9f;       //Ko�arken sallanma
    public CharacterController controller; // Karakterin hareketini kontrol eden component
    private float defaultYPos = 0f;     // Kameran�n varsay�lan Y pozisyonu
    private float timer = 0f;           // Zamanlay�c�

    void Start()
    {
        // Kameran�n ba�lang��taki Y pozisyonunu kaydeder
        defaultYPos = transform.localPosition.y;
    }

    void Update()
    {
        if (controller.velocity.magnitude > 0.1f && controller.velocity.magnitude < 7f) // Hareket ediyorsa 
        {
            //Debug.Log(controller.velocity.magnitude);
            bobSpeed = walkingBob;
            // Sin�s fonksiyonu ile sallanmay� hesapla
            timer += Time.deltaTime * bobSpeed;
            float newY = defaultYPos + Mathf.Sin(timer) * bobAmount;
            transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        }
        else if (controller.velocity.magnitude > 8f) //Ko�uyorsa
        {
            bobSpeed = runningBob;
            // Sin�s fonksiyonu ile sallanmay� hesapla
            timer += Time.deltaTime * bobSpeed;
            float newY = defaultYPos + Mathf.Sin(timer) * bobAmount;
            transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        }
        else
        {
            // Hareket etmiyorsa kameray� varsay�lan konuma d�nd�r
            timer = 0f;
            transform.localPosition = new Vector3(transform.localPosition.x, defaultYPos, transform.localPosition.z);
        }
    }
}
