using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandsController : MonoBehaviour
{
   [SerializeField] private LongRangeWeaponObject _targetObject;
   private Vector3 _lookPos;
   
   public float Step => _step;
   [Header("Sway")] 
   [SerializeField] private float _step = 0.01f;
   public float MaxStepDistance => _maxStepDistance;
   [SerializeField] private float _maxStepDistance = 0.06f;
    
   public float RotationStep => _rotationStep;
   [Header("Sway Rotation")] [SerializeField]
   private float _rotationStep = 4f;
    
   public float MaxRotationStep => _maxRotationStep;
   [SerializeField] private float _maxRotationStep = 5f;
   
   private Vector3 _swayPos;
   private Vector3 _swayEulerRot;

   [SerializeField] private float _swayMultiplayer = 1;

   private float _smooth;
   private float _smoothRotation;
   
   private void Sway(Vector2 lookPos)
   {
      Vector3 invertLook = lookPos * -Step;
      invertLook.x = Mathf.Clamp(invertLook.x, -MaxStepDistance, MaxStepDistance);
      invertLook.y = Mathf.Clamp(invertLook.y, -MaxStepDistance, MaxStepDistance);
      _swayPos = invertLook;
   }
    
   private void SwayRotation(Vector2 lookPos)
   {
      Vector3 invertLook = lookPos * -Step;
      invertLook.x = Mathf.Clamp(invertLook.x, -MaxRotationStep, MaxRotationStep);
      invertLook.y = Mathf.Clamp(invertLook.y, -MaxRotationStep, MaxRotationStep);
      _swayEulerRot = new Vector3(invertLook.y, invertLook.x, invertLook.x);
   }
    
   private void CompositePositionRotation()
   {
      transform.localPosition = Vector3.Lerp(transform.localPosition, _swayPos, Time.deltaTime * _smooth);
      transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(_swayEulerRot),
         Time.deltaTime * _smoothRotation);
   }

   public void MoveHands()
   {
      Touch touch = Input.GetTouch(0);
      float x = Input.GetAxisRaw("Mouse X") * _swayMultiplayer;
      float y = Input.GetAxisRaw("Mouse Y") * _swayMultiplayer;
      Vector2 lookPos = new Vector2(x, y);
      Sway(lookPos);
      SwayRotation(lookPos);
      CompositePositionRotation();
   }
}
