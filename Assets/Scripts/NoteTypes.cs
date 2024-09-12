using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteTypes : MonoBehaviour
{
    // Start is called before the first frame update
    public NoteType noteType;

    public enum NoteType
    {
        Grave,
        BlueDragon
        // Add more item types as needed
    }
}
