using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour
{
    // Start is called before the first frame update
    public ItemType itemType;

    public enum ItemType
    {
        Medkit,
        Bullet,
        Flashlight
        // Add more item types as needed
    }
}
