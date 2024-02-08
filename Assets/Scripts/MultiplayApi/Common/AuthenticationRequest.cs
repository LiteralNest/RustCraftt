using System;
using Newtonsoft.Json;

namespace MultiplayApi.Common
{
    [Serializable]
    public class AuthenticationRequest
    {
        [JsonProperty("scopes")] public string[] Scopes;
    }
}