using System;
using Newtonsoft.Json;

namespace Unity.Netcode.Samples
{
    public class MachineSpec
    {
        [JsonProperty("contractEndDate")] public DateTime ContractEndDate;
        [JsonProperty("contractStartDate")] public DateTime ContractStartDate;
        [JsonProperty("cpuCores")] public int CPUCores;
        [JsonProperty("cpuDetail")] public string CPUDetail;
        [JsonProperty("cpuName")] public string CPUName;
        [JsonProperty("cpuShortname")] public string CPUShortname;
        [JsonProperty("cpuSpeed")] public int CPUSpeed;
        [JsonProperty("memory")] public long Memory;
    }
}