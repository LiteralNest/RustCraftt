using System;
using Newtonsoft.Json;

namespace Unity.Netcode.Samples
{
    [Serializable]
    public class ServerByIdData
    {
        [JsonProperty("allocationId")] public string AllocationId;
        [JsonProperty("buildConfigurationId")] public int BuildConfigurationId;
        [JsonProperty("created")] public DateTime Created;
        [JsonProperty("fleetId")] public string FleetId;
        [JsonProperty("fulfilled")] public DateTime Fulfilled;
        [JsonProperty("gamePort")] public int GamePort;
        [JsonProperty("ipv4")] public string Ipv4;
        [JsonProperty("ipv6")] public string Ipv6;
        [JsonProperty("machineId")] public int MachineId;
        [JsonProperty("ready")] public DateTime Ready;
        [JsonProperty("regionId")] public string RegionId;
        [JsonProperty("requestId")] public string RequestId;
        [JsonProperty("requested")] public DateTime Requested;
        [JsonProperty("serverId")] public int ServerId;
    }
}