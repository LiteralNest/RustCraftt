using System.Collections;
using UnityEngine;

public class DelayItem : MonoBehaviour
{
    protected bool IsRecovering;
    
    protected IEnumerator RecoverRoutine(float recoveringTime)
    {
        IsRecovering = true;
        yield return new WaitForSeconds(recoveringTime);
        IsRecovering = false;
    }
}