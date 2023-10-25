using System;
using Newtonsoft.Json;

namespace Unity.Netcode.Samples
{
    [Serializable]
    public class AuthenticationResponse
    {
        [JsonProperty("accessToken")] public string AccessToken;
    }
}