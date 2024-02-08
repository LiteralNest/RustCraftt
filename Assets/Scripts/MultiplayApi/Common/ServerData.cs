using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MultiplayApi.Common
{
    [Serializable]
    public class ServerData
    {
        [JsonProperty("buildConfigurationID")] public int BuildConfigurationID;
        [JsonProperty("buildConfigurationName")] public string BuildConfigurationName;
        [JsonProperty("buildName")] public string BuildName;
        [JsonProperty("cpuLimit")] public int CPULimit;
        [JsonProperty("deleted")] public bool Deleted;
        [JsonProperty("fleetID")] public string FleetID;
        [JsonProperty("fleetName")] public string FleetName;
        [JsonProperty("hardwareType")] public string HardwareType;
        [JsonProperty("holdExpiresAt")] public int HoldExpiresAt;
        [JsonProperty("id")] public int ID;
        [JsonProperty("ip")] public string IP;
        [JsonProperty("locationID")] public int LocationID;
        [JsonProperty("locationName")] public string LocationName;
        [JsonProperty("machineID")] public int MachineID;
        [JsonProperty("machineName")] public string MachineName;
        [JsonProperty("machineSpec")] public MachineSpec MachineSpec;
        [JsonProperty("memoryLimit")] public int MemoryLimit;
        [JsonProperty("port")] public int Port;
        [JsonProperty("status"), JsonConverter(typeof(StringEnumConverter))] public ServerStatus Status;
    }
}