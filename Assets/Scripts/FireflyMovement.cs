using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyMovement : MonoBehaviour
{
    public float moveRadius = 2f; // Ateþ böceklerinin hareket edeceði alanýn yarýçapý
    public float moveSpeed = 2f;  // Hareket hýzý
    public Transform player;      // Player referansý
    public float playerDetectionRange = 10f; // Oyuncunun algýlanma mesafesi
    public float escapeSpeed = 3f; // Uzaklaþma hýzý
    private Vector3 initialPosition;
    private Vector3 targetPosition;

    void Start()
    {
        initialPosition = transform.position;
        SetRandomTarget();
    }

    void Update()
    {
        // Ateþ böceðinin oyuncuya olan mesafesini kontrol et
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < playerDetectionRange)
        {
            // Eðer oyuncu yaklaþýyorsa ateþ böceði yukarý doðru uzaklaþsýn
            Vector3 directionAwayFromPlayer = (transform.position - player.position).normalized;
            transform.position += (directionAwayFromPlayer + Vector3.up) * escapeSpeed * Time.deltaTime;
        }
        else
        {
            // Oyuncu uzakta, ateþ böceði rastgele hareket etsin
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                SetRandomTarget();
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    void SetRandomTarget()
    {
        // Ateþ böceklerinin rastgele hareket edeceði pozisyonu belirle
        Vector3 randomDirection = Random.insideUnitSphere * moveRadius;
        randomDirection += initialPosition;
        randomDirection.y = initialPosition.y; // Y ekseninde sabit kalmasý için
        targetPosition = randomDirection;
    }
}
