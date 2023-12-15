using ProceduralGeneration.Scripts.SavingSystem;
using ScriptableObjects;
using UnityEngine;

public class CharacterTerrainSoundPlayer : MonoBehaviour
{
    [SerializeField] private BlockDataBase _blocksData;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private LayerMask _terrainLayer;
    
    private Vector2Int _previousPlayerWorldPosition;
    
    private void Update()
    {
        var playerWorldPosition = Vector3Int.FloorToInt(transform.position);
        var fixedPlayerWorldPosition = new Vector2Int(playerWorldPosition.x, playerWorldPosition.z);


        if (fixedPlayerWorldPosition != _previousPlayerWorldPosition)
        {
            _previousPlayerWorldPosition = fixedPlayerWorldPosition;
            ChangePosition();
        }
    }

    public void ChangePosition()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1000, _terrainLayer))
        {
            var blocksHandler = hit.collider.GetComponent<ChunkBlocksHanlder>();
            if (blocksHandler == null) return;
            var pos = gameObject.transform.position;
            var block = blocksHandler.ChunkSavingData.GetBlock(new Vector2Int((int)pos.x, (int)pos.z));
            var info = _blocksData.GetInfo(block.T);
            if (info != null)
            {
                Debug.Log("Playing");
                _audioSource.PlayOneShot(info.AudioClip);
            }
        }
    }
}