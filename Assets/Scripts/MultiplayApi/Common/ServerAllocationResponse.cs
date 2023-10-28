using System;
using Newtonsoft.Json;

namespace Unity.Netcode.Samples
{
    [Serializable]
    public class ServerAllocationResponse
    {
        [JsonProperty("allocationId")] public string AllocationId;
        [JsonProperty("href")] public string Href;
    }
}