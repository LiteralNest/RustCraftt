using UnityEngine;

public class PlacingObject : MonoBehaviour
{
    [field:SerializeField] public string RequiredTag { get; private set; }
    
    public bool CanBePlaced { get; set; }
}