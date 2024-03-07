using System;
using Newtonsoft.Json;

namespace MultiplayApi.Common
{
    [Serializable]
    public class ServerAllocationRequest
    {
        [JsonProperty("allocationId")] public string AllocationId;
        [JsonProperty("buildConfigurationId")] public int BuildConfigurationId;
        [JsonProperty("payload")] public string Payload;
        [JsonProperty("regionId")] public string RegionId;
        [JsonProperty("restart")] public bool Restart;
    }
}