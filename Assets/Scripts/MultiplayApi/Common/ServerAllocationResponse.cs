using System;
using Newtonsoft.Json;

namespace MultiplayApi.Common
{
    [Serializable]
    public class ServerAllocationResponse
    {
        [JsonProperty("allocationId")] public string AllocationId;
        [JsonProperty("href")] public string Href;
    }
}