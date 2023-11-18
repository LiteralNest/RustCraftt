using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

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
      ConnectedStructure currentStructure = null;
      List<ConnectedStructure> structures = new List<ConnectedStructure>();
      structures = GetAddedStructures(structures, GetRelativeStructuresList());
        
      int i = 0;

      if (structures.Count == 0) return;
      SetNewClipBoard(structures[0]);
   }
}