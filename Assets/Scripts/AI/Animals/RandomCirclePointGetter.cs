using UnityEngine;

namespace AI.Animals
{
    public class RandomCirclePointGetter : MonoBehaviour
    {
        public float circleRadius = 5f;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, circleRadius);
        }

        public Vector3 GetRandomPointInCircle()
        {
            float angle = Random.Range(0f, 2f * Mathf.PI);
            float distance = Mathf.Sqrt(Random.Range(0f, 1f)) * circleRadius;

            float x = transform.position.x + distance * Mathf.Cos(angle);
            float z = transform.position.z + distance * Mathf.Sin(angle);

            return new Vector3(x, transform.position.y, z);
        }
    }
}