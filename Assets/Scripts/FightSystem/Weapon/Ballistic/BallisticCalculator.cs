using UnityEngine;

namespace FightSystem.Weapon.Ballistic
{
    public class BallisticCalculator
    {
        public float GetCalculatedAngle(Vector3 forwardDirection, float force)
        {
            var objectTiltAngle = Vector3.Angle(Vector3.up, forwardDirection) - 90f;
            return -objectTiltAngle;
        }
        
        public Vector3 GetCalculatedVelocity(Vector3 direction, float angle, float speed)
        {
            var angleInRadians = angle * Mathf.Deg2Rad;
            var horizontalSpeed = Mathf.Cos(angleInRadians) * speed;
            var verticalSpeed = Mathf.Sin(angleInRadians) * speed;

            var velocity = direction.normalized * horizontalSpeed;
            velocity.y = verticalSpeed;

            return velocity;
        }
    }
}