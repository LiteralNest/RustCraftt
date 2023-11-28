using System.Collections.Generic;
using System.Threading.Tasks;
using Building_System.Placing_Objects;
using Unity.Netcode;
using UnityEngine;

namespace Building_System.Buildings_Connecting
{
   public class ClipBoardConnector : BuildingConnector
   {
      [SerializeField] private PlacingObject _placingObject;
      [SerializeField] private ToolClipboard _clipboard;
      public override void ConnectStructures()
         => ConnectStructuresAsync();

      private void SetNewClipBoard(ConnectedStructure structure)
      {
         structure.TargetClipBoards.Add(_clipboard);
         _currentStructure = structure;
         _placingObject.GetComponent<NetworkObject>().TrySetParent(_currentStructure.transform);
      }
   
      private async void ConnectStructuresAsync()
      {
         await Task.Delay(100);
         List<ConnectedStructure> structures = new List<ConnectedStructure>();
         structures = GetAddedStructures(structures, GetRelativeStructuresList());
         if (structures.Count == 0) return;
         SetNewClipBoard(structures[0]);
      }
   }
}