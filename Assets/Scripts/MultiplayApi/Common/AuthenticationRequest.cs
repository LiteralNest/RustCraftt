using System;
using Newtonsoft.Json;

namespace Unity.Netcode.Samples
{
    [Serializable]
    public class AuthenticationRequest
    {
        [JsonProperty("scopes")] public string[] Scopes;
    }
}