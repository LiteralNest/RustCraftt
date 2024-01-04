using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TechnologyManager : NetworkBehaviour
{
    // A network variable to store the technology id and user id
    [field: SerializeField] public NetworkVariable<CustomSendingTechnologyArrayData> TechArrayNetData { get; private set; } = new();
    
    // A method to add reaserched the technologies on the server and assign them ids
    [ServerRpc(RequireOwnership = false)]
    public void AddTechnologiesServerRpc(CustomSendingTechnologyData data)
    {
        if(!IsServer) return;
        
        // Loop through the tech ids in the data
        foreach (var techId in data.TechId)
        {
            // Add the technology to the network variable array
            var techArray = TechArrayNetData.Value.TechnologyArray;
            var newTech = new CustomSendingTechnologyData(new int[] {techId}, data.UserId);
            var newList = new List<CustomSendingTechnologyData>(techArray);
            newList.Add(newTech);
            TechArrayNetData.Value = new CustomSendingTechnologyArrayData(newList.ToArray());
        }
    }


    // A method to research a technology by user id
    [ServerRpc(RequireOwnership = false)]
    public void ResearchTechnologyServerRpc(int techId, ulong userId)
    {
        if(!IsServer) return;
        
        var techArray = TechArrayNetData.Value.TechnologyArray;
        var tech = Array.Find(techArray, t => t.TechId[0] == techId);
        
        // Check if the technology exists and belongs to the user
        if (tech.UserId == userId)
        {
            var networkObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[(ulong)techId];
            
            // Get the technology component from the network object
            var technology = networkObject.GetComponent<Technology>();
            
            technology.Research();
        }
    }
}
