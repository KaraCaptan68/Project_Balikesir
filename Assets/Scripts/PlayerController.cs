using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Camera playerCamera;
    private CharacterController controller;

    //Movement & Camera
    private float rotationX = 0f;
    public float mouseSensitivity = 100f;
    public float moveSpeed = 5f;
    public float runSpeed = 10f;

    //Stamina & Health
    public float maxStamina = 100f;
    public float staminaRegenRate = 10f;
    public float staminaDepletionRate = 20f;
    private float currentStamina;
    private bool isExhausted = false;

    public float maxHealth = 100f;
    public float medkitHealRate = 40f;
    public float currentHealth;

    //Stamina & Health HUD
    public Slider staminaSlider;
    public TextMeshProUGUI staminaText;
    public Slider healthSlider;
    public TextMeshProUGUI healthText;

    //Item Picking Up
    private float pickUpRange = 2f;

    //Item Picking Up HUD
    [SerializeField] RawImage dotImage;
    [SerializeField] RawImage handImage;

    //Inventory
    [SerializeField] GameObject inventoryHUD;
    private InventoryManager inventoryManager;
    bool isMedUsed = false;

    public GameObject flashlight;
    private MeshRenderer flaslightMeshRenderer;
    private MeshRenderer flashlightShineMeshRenderer;
    public GameObject playerRightHand;

    ////Pause Menu
    //public GameObject pauseMenu;            //PauseMenu script do this job

    //Damage Control
    //private float damageCooldown = 1.0f; // Set your desired cooldown time in seconds
    //private float lastDamageTime;         //NO ENEMY AT THE MOMENT

    /*CONTROLS:
     * W,A,S,D for movemnt
     * Shift for running
     * Mause for looking around
     * TAB for inventory
     * E for picking up an item
     * T for using medkit
     * F for opening closing hand
     * ESC for menu
    */

    private void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        currentStamina = maxStamina;
        UpdateStaminaUI();

        currentHealth = maxHealth;
        UpdateHealthUI();

        inventoryManager = GameObject.Find("Inventory Management").GetComponent<InventoryManager>();
        flaslightMeshRenderer= flashlight.GetComponent<MeshRenderer>();

        Transform flashlightShineTransform=flashlight.transform.GetChild(0);
        if(flashlightShineTransform != null )
        {
            flashlightShineMeshRenderer=flashlightShineTransform.GetComponent<MeshRenderer>();
        }
    }

    private void Update()
    {
        CameraControls();
        Movement();

        //To Open Inventory
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryHUD.SetActive(!(inventoryHUD.activeSelf));
        }
        ////To Open Pause Screen                   //PauseMenu script do this job
        //if(Input.GetKeyDown(KeyCode.Escape))
        //{
        //     pauseMenu.SetActive(!(pauseMenu.activeSelf));          
        //}

        //To use a MEDKÝT
        if (Input.GetKeyDown(KeyCode.T) && !(isMedUsed))
        {
            isMedUsed = true;
            inventoryManager.MedkitUsed();
            StartCoroutine(MedUseCooldown());
        }
        //Collecting Items
        if (Input.GetKeyDown(KeyCode.E))
        {
            PickUpItem();
        }
        //Opening cloasing hand
        if(Input.GetKeyDown(KeyCode.F))
        {
           playerRightHand.SetActive(!(playerRightHand.activeSelf));
        }
        //What are you looking at?
        LookingToWhat();
    }

    void Movement()
    {
        // Movement
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        Vector3 finalMoveDirection = moveDirection.normalized;
        if (Input.GetKey(KeyCode.LeftShift) && !(isExhausted) && finalMoveDirection.magnitude > 0)
        {
            controller.SimpleMove(finalMoveDirection * runSpeed);
            //Debug.Log("RUNNING");
            DepleteStamina();
            if (currentStamina <= 1)
            {
                isExhausted = true;
                StartCoroutine(StaminaRegenCooldown());
            }
        }
        else
        {
            controller.SimpleMove(finalMoveDirection * moveSpeed);
            //Debug.Log("WALKING");
            RegenerateStamina();
        }
    }
    void DepleteStamina()
    {
        currentStamina -= staminaDepletionRate * Time.deltaTime;

        if (currentStamina <= 0)
        {
            currentStamina = 0;
            //isRunning = false; // Stop running if stamina is depleted
        }

        UpdateStaminaUI();
    }
    void RegenerateStamina()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
            UpdateStaminaUI();
        }
    }
    void UpdateStaminaUI()
    {
        staminaSlider.value = currentStamina;
        staminaText.text = "Stamina: " + Mathf.Round(currentStamina).ToString();
    }
    IEnumerator StaminaRegenCooldown()
    {
        yield return new WaitForSeconds(3f);
        isExhausted = false;
    }

    public void UpdateHealthUI()
    {

        healthSlider.value = currentHealth;
        healthText.text = "Health: " + Mathf.Round(currentHealth).ToString();
    }

    void CameraControls()
    {
        // Rotation
        float lookX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float lookY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        rotationX -= lookY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.rotation *= Quaternion.Euler(0f, lookX, 0f);
    }

    void PickUpItem()
    {
        //perform raycast to check if player is looking at object within pickuprange
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
        {
            if (hit.transform.gameObject.tag == "Pickable")
            {
                // Access the custom component to determine the type
                PickableItem pickableObject = hit.collider.GetComponent<PickableItem>();

                if (pickableObject != null)
                {
                    // Now, you can use the item type to identify the type of object
                    switch (pickableObject.itemType)
                    {
                        case PickableItem.ItemType.Medkit:
                            Debug.Log("Destroyed Medkit");
                            // Perform Medkit-specific actions
                            inventoryManager.MedkitCollected();
                            break;

                        case PickableItem.ItemType.Bullet:
                            Debug.Log("Destroyed Bullet");
                            // Perform Bullet-specific actions
                            inventoryManager.BulletCollected();
                            break;

                        case PickableItem.ItemType.Flashlight:
                            Debug.Log("Got The FLASLIGHT");
                            flashlight.transform.SetParent(playerRightHand.transform);
                            flashlight.transform.localPosition = Vector3.zero; // El fenerini playerHand'in pozisyonuna sýfýrla
                            flashlight.transform.localRotation = Quaternion.identity; // El fenerini playerHand'in rotasyonuna sýfýrla
                            flaslightMeshRenderer.enabled = false;
                            flashlightShineMeshRenderer.enabled=false;
                            break;

                        // Add more cases for additional item types

                        default:
                            break;
                    }

                    if (PickableItem.ItemType.Flashlight != pickableObject.itemType)
                    {
                        // Destroy the object
                        Destroy(hit.collider.gameObject);
                    }

                    // Add it to the inventory or perform other actions
                }
            }
        }

    }

    //If player is looking to a pickable object center dot will turn to a hand
    private void LookingToWhat()
    {
        //perform raycast to learn what player is looking to
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
        {
            if (hit.transform.gameObject.tag == "Pickable")
            {
                ShowHandImage();
            }
            else
            {
                ShowDotImage();
            }
        }
        else
        {
            ShowDotImage();
        }
    }
    private void ShowDotImage()
    {
        dotImage.enabled = true;
        handImage.enabled = false;
    }
    private void ShowHandImage()
    {
        dotImage.enabled = false;
        handImage.enabled = true;
    }

    IEnumerator MedUseCooldown()
    {
        yield return new WaitForSeconds(1.6f);
        isMedUsed = false;
    }

    //Checking the colliding
    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    // Check if the collision involves an object with the "Enemy" tag.
    //    if (hit.gameObject.CompareTag("Enemy"))
    //    {
    //        if (Time.time - lastDamageTime > damageCooldown)
    //        {
    //            Debug.Log("Contact happened with ENEMY");

    //            if (currentHealth > 0)
    //            {
    //                Debug.Log("HEALTH DROPPED");
    //                currentHealth -= 20;
    //                UpdateHealthUI();

    //                if (currentHealth == 0)
    //                {
    //                    Debug.Log("YOU DEAD");
    //                }

    //                lastDamageTime = Time.time; // Update the last damage time
    //            }
    //        }
    //    }


    //}
}
