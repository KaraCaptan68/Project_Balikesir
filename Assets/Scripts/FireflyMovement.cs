using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyMovement : MonoBehaviour
{
    public float moveRadius = 2f; // Ate� b�ceklerinin hareket edece�i alan�n yar��ap�
    public float moveSpeed = 2f;  // Hareket h�z�
    public Transform player;      // Player referans�
    public float playerDetectionRange = 10f; // Oyuncunun alg�lanma mesafesi
    public float escapeSpeed = 3f; // Uzakla�ma h�z�
    private Vector3 initialPosition;
    private Vector3 targetPosition;

    void Start()
    {
        initialPosition = transform.position;
        SetRandomTarget();
    }

    void Update()
    {
        // Ate� b�ce�inin oyuncuya olan mesafesini kontrol et
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < playerDetectionRange)
        {
            // E�er oyuncu yakla��yorsa ate� b�ce�i yukar� do�ru uzakla�s�n
            Vector3 directionAwayFromPlayer = (transform.position - player.position).normalized;
            transform.position += (directionAwayFromPlayer + Vector3.up) * escapeSpeed * Time.deltaTime;
        }
        else
        {
            // Oyuncu uzakta, ate� b�ce�i rastgele hareket etsin
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                SetRandomTarget();
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    void SetRandomTarget()
    {
        // Ate� b�ceklerinin rastgele hareket edece�i pozisyonu belirle
        Vector3 randomDirection = Random.insideUnitSphere * moveRadius;
        randomDirection += initialPosition;
        randomDirection.y = initialPosition.y; // Y ekseninde sabit kalmas� i�in
        targetPosition = randomDirection;
    }
}
