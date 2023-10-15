using UnityEngine;

public class TechEdgeUI : MonoBehaviour
{

    public Vector2 start;
    public Vector2 end;

    void Start()
    {
        transform.position = (start + end) / 2;
        transform.rotation = Quaternion.Euler(0, 0, 90-Vector3.Angle(Vector3.up, start - end));
        //transform.rotation.SetLookRotation(Vector3.forward, start - end);
        transform.localScale = new Vector3( (start - end).magnitude, transform.localScale.y, transform.localScale.z);
    }
}