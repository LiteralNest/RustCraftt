using System.Collections.Generic;
using System.Threading.Tasks;
using Building_System.Blocks;
using Unity.Netcode;
using UnityEngine;

namespace Building_System.Buildings_Connecting
{
    public class StructureConnector : BuildingConnector
    {
        [SerializeField] private BuildingBlock _buildingBlock;
        public override void ConnectStructures()
            => ConnectStructuresAsync();

        public void SetNewStructure(ConnectedStructure structure)
        {
            structure.Blocks.Add(_buildingBlock);
            _currentStructure = structure;
            _buildingBlock.CurrentStructure = structure;
            _buildingBlock.GetComponent<NetworkObject>().TrySetParent(_currentStructure.transform);
        }
    
        private ConnectedStructure GetInstantiatedStructure()
        {
            Vector3 position = transform.position;
            _currentStructure = Instantiate(_structurePrefab, position, Quaternion.identity);
            _currentStructure.GetComponent<NetworkObject>().Spawn();
            _buildingBlock.transform.GetComponent<NetworkObject>().TrySetParent(_currentStructure.transform);
            return _currentStructure;
        }
        
        private async void ConnectStructuresAsync()
        {
            await Task.Delay(100);
            ConnectedStructure currentStructure = null;
            List<ConnectedStructure> structures = new List<ConnectedStructure>();
            structures = GetAddedStructures(structures, GetRelativeStructuresList());
        
            int i = 0;
        
            if (structures.Count == 0)
                currentStructure = GetInstantiatedStructure();
            else
            {
                currentStructure = structures[0];
                while (structures.Count > 1)
                {
                    if (i == 0)
                    {
                        i++;
                        continue;
                    }
                    structures[i].MigrateBlocks(currentStructure);
                    structures.RemoveAt(i);
                }
            }
        
            SetNewStructure(currentStructure);
        }
    }
}
