using System;
using Newtonsoft.Json;

namespace MultiplayApi.Common
{
    [Serializable]
    public class AuthenticationResponse
    {
        [JsonProperty("accessToken")] public string AccessToken;
    }
}