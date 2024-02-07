using UnityEngine;
using Cinemachine;

public class AmbientZone : MonoBehaviour
{
    [Tooltip("Cinemachine Path to follow")]
    public CinemachinePathBase m_Path;
    [Tooltip("Tag of the player")]
    public string playerTag = "Player";
    [Tooltip("Reference to the AudioSource object")]
    public AudioSource audioSource;

    private Transform playerTransform;
    private float m_Position;
    private CinemachinePathBase.PositionUnits m_PositionUnits = CinemachinePathBase.PositionUnits.PathUnits;

    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag(playerTag);
        
        playerTransform = playerObject.transform;
        audioSource.volume = 0.3f;
        audioSource.Play();
    }

    void Update()
    {
        if (playerTransform != null && m_Path != null)
        {
            SetCartPosition(m_Path.FindClosestPoint(playerTransform.position, 0, -1, 10));
            
            Vector3 Sub = transform.position - playerTransform.position;
            Vector3 Spline = transform.right;
            
            if (Vector3.Dot(Sub, Spline) > 0)
            {
             
                audioSource.transform.position = playerTransform.position;
                audioSource.transform.rotation = playerTransform.rotation;
            }
        }
    }

    void SetCartPosition(float distanceAlongPath)
    {
        m_Position = m_Path.StandardizeUnit(distanceAlongPath, m_PositionUnits);
        transform.position = m_Path.EvaluatePositionAtUnit(m_Position, m_PositionUnits);
        transform.rotation = m_Path.EvaluateOrientationAtUnit(m_Position, m_PositionUnits);
    }
}