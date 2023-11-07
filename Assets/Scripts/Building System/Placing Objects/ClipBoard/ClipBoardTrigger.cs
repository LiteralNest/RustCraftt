using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipBoardTrigger : MonoBehaviour
{
    public bool IsInsideOtherClipBoard { get; private set; }
    
    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("ShelfZone")) return;
        IsInsideOtherClipBoard = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("ShelfZone")) return;
        IsInsideOtherClipBoard = false;
    }
}
