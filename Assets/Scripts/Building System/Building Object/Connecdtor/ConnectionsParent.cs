using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionsParent : MonoBehaviour
{
   [SerializeField] private List<BuildingConnector> _connectedBuildings = new List<BuildingConnector>();
   
   public void AddConnectedBuilding(BuildingConnector building)
   {
      if(_connectedBuildings.Contains(building)) return;
      _connectedBuildings.Add(building);
   }

   public void RemoveConnectedBuilding(BuildingConnector building)
   {
      _connectedBuildings.Remove(building);
   }
}
