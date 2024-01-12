using System.Collections;
using UnityEngine;

public class DelayItem : MonoBehaviour
{
    protected bool _isRecovering;
    
    protected IEnumerator RecoverRoutine(float recoveringTime)
    {
        _isRecovering = true;
        yield return new WaitForSeconds(recoveringTime);
        _isRecovering = false;
    }
}