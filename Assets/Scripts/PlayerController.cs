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
    public float mouseSensitivity = 200f;
    public float moveSpeed = 5f;
    public float runSpeed = 9f;

    //Stamina & Health
    public float maxStamina = 100f;
    public float staminaRegenRate = 60f;
    public float staminaDepletionRate = 20f;
    private float currentStamina;
    private bool isExhausted = false;

    //Stamina & Health HUD
    public Slider staminaSlider;
    public TextMeshProUGUI staminaText;

    //Item Picking Up
    [Tooltip("Item pick up range")] public float pickUpRange = 2f;

    //Item Picking Up HUD
    [SerializeField] RawImage dotImage;
    [SerializeField] RawImage handImage;

    //Inventory
    [SerializeField] GameObject inventoryHUD;
    private InventoryManager inventoryManager;

    public GameObject flashlight;
    private MeshRenderer flaslightMeshRenderer;
    private MeshRenderer flashlightShineMeshRenderer;
    public GameObject playerRightHand;


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

        inventoryManager = GameObject.Find("Inventory Management").GetComponent<InventoryManager>();
        flaslightMeshRenderer = flashlight.GetComponent<MeshRenderer>();

        Transform flashlightShineTransform = flashlight.transform.GetChild(0);
        if (flashlightShineTransform != null)
        {
            flashlightShineMeshRenderer = flashlightShineTransform.GetComponent<MeshRenderer>();
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
        //Collecting Items
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interaction();
        }
        //Opening cloasing hand
        if (Input.GetKeyDown(KeyCode.F))
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

    void Interaction()
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
                        case PickableItem.ItemType.Flashlight:
                            Debug.Log("Got The FLASLIGHT");
                            flashlight.transform.SetParent(playerRightHand.transform);
                            flashlight.transform.localPosition = Vector3.zero; // El fenerini playerHand'in pozisyonuna s�f�rla
                            flashlight.transform.localRotation = Quaternion.identity; // El fenerini playerHand'in rotasyonuna s�f�rla
                            flaslightMeshRenderer.enabled = false;
                            flashlightShineMeshRenderer.enabled = false;
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
            else if (hit.transform.gameObject.tag == "Note")
            {
                // Access the custom component to determine the type
                NoteTypes notetypes = hit.transform.gameObject.GetComponent<NoteTypes>();
                //Debug.Log("Hit on NOTE");
                if (notetypes != null)
                {
                    // Now, you can use the item type to identify the type of object
                    switch (notetypes.noteType)
                    {
                        case NoteTypes.NoteType.Grave:
                            //Debug.Log("TOUCHED THE GRAVE");
                            inventoryManager.itemPickedInformation.SetActive(true);
                            inventoryManager.itemPickedInformationText.text = "An old grave, the writings on it is long gone.";
                            StartCoroutine(CloseNoteTime());
                            break;

                        case NoteTypes.NoteType.BlueDragon:
                            //Debug.Log("Touched THE DRAGON");
                            inventoryManager.itemPickedInformation.SetActive(true);
                            inventoryManager.itemPickedInformationText.text = "HORRRY SHIT IS THAT THE LEGENDARY BLUE DRAGONN!!?? RRRRAAAAAAAA";
                            StartCoroutine(CloseNoteTime());
                            break;
                    }
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
            else if (hit.transform.gameObject.tag == "Note")
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

    IEnumerator CloseNoteTime()
    {
        yield return new WaitForSeconds(3f);
        inventoryManager.itemPickedInformation.SetActive(false);
    }
}
