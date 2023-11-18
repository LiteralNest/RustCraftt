using System;
using System.Text;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Netcode.Samples;
using UnityEngine;
using UnityEngine.Networking;

namespace MultiplayApi.Service
{
    public class MultiplayWebApi : IMultiplayWebApi
    {
        private readonly string _projectId;
        private readonly string _environmentId;

        private readonly string _fleetId;
        private readonly string _regionId;
        private readonly int _buildConfigId;

        private readonly string _keyBase64;
        private string _accessToken;
        
        /// <summary>
        /// Constructor for the MultiplayWebApi class, initializes configuration and authentication parameters.
        /// </summary>
        /// <param name="serviceAccountKeyId">Service account key ID.</param>
        /// <param name="serviceAccountSecretId">Service account secret ID.</param>
        /// <param name="projectId">Project ID.</param>
        /// <param name="environmentId">Environment ID.</param>
        /// <param name="fleetId">Fleet ID.</param>
        /// <param name="regionId">Region ID.</param>
        /// <param name="buildConfigId">Build configuration ID.</param>
        public MultiplayWebApi(
            string serviceAccountKeyId, string serviceAccountSecretId,
            string projectId, string environmentId,
            string fleetId, string regionId, int buildConfigId)
        {
            _projectId = projectId;
            _environmentId = environmentId;

            _fleetId = fleetId;
            _regionId = regionId;
            _buildConfigId = buildConfigId;
            
            var keyByteArray = Encoding.UTF8.GetBytes(serviceAccountKeyId + ":" + serviceAccountSecretId);
            _keyBase64 = Convert.ToBase64String(keyByteArray);
        }

        /// <summary>
        /// Initiates the authentication process by obtaining an access token required for API requests.
        /// </summary>
        public async Task Authenticate()
        {
            var authUrl = $"https://services.api.unity.com/auth/v1/token-exchange?projectId={_projectId}&environmentId={_environmentId}";
            var jsonPostBody = JsonConvert.SerializeObject(new AuthenticationRequest
            {
                Scopes = new[]
                {
                    "multiplay.allocations.create", "multiplay.allocations.list", 
                    "multiplay.allocations.get", "multiplay.allocations.delete"
                }
            });

            var authResultJson = await PostRequest(authUrl, jsonPostBody, request =>
            {
                request.SetRequestHeader("Authorization", "Basic " + _keyBase64);
            });

            var authenticationResponse = JsonConvert.DeserializeObject<AuthenticationResponse>(authResultJson);
            _accessToken = authenticationResponse.AccessToken;
        }

        /// <summary>
        /// Allocates a server for gameplay.
        /// </summary>
        /// <returns>Allocation ID of the allocated server.</returns>
        public async Task<string> AllocateServer()
        {
            var allocateServerUrl = $"https://multiplay.services.api.unity.com/v1/allocations/projects/{_projectId}/environments/{_environmentId}/fleets/{_fleetId}/allocations";

            var allocateServerBody = JsonConvert.SerializeObject(new ServerAllocationRequest
            {
                AllocationId = Guid.NewGuid().ToString(),
                BuildConfigurationId = _buildConfigId,
                RegionId = _regionId,
                Restart = true
            });

            var allocateServerResultJson = await PostRequest(allocateServerUrl, allocateServerBody, request =>
            {
                request.SetRequestHeader("Authorization", "Bearer " + _accessToken);
            });

            var allocationId = JsonConvert.DeserializeObject<ServerAllocationResponse>(allocateServerResultJson)?.AllocationId;
            return allocationId;
        }

        /// <summary>
        /// Retrieves a list of available servers.
        /// </summary>
        /// <returns>Array of server data.</returns>
        public async Task<Unity.Netcode.Samples.ServerData[]> GetServersList()
        {
            var getServersListUrl = $"https://services.api.unity.com/multiplay/servers/v1/projects/{_projectId}/environments/{_environmentId}/servers";

            var serversListJson = await GetRequest(getServersListUrl, request =>
            {
                request.SetRequestHeader("Authorization", "Basic " + _keyBase64);
            });

            var serversList = JsonConvert.DeserializeObject<Unity.Netcode.Samples.ServerData[]>(serversListJson);
            return serversList;
        }

        /// <summary>
        /// Retrieves server information based on its allocation ID.
        /// </summary>
        /// <param name="allocationId">Allocation ID of the server.</param>
        /// <returns>Data for the specified server.</returns>
        public async Task<ServerByIdData> GetServerById(string allocationId)
        {
            var getServerByIdUrl = $"https://multiplay.services.api.unity.com/v1/allocations/projects/{_projectId}/environments/{_environmentId}/fleets/{_fleetId}/allocations/{allocationId}";

            var serverByIdJson = await GetRequest(getServerByIdUrl, request =>
            {
                request.SetRequestHeader("Authorization", "Bearer " + _accessToken);
            });

            var serverById = JsonConvert.DeserializeObject<ServerByIdData>(serverByIdJson);
            return serverById;
        }

        /// <summary>
        /// Performs a GET request and returns the response as a string.
        /// </summary>
        /// <param name="url">The URL to send the GET request to.</param>
        /// <param name="requestSetup">Action to set up the UnityWebRequest before sending.</param>
        /// <returns>The response as a string or null on error.</returns>
        private async Task<string> GetRequest(string url, Action<UnityWebRequest> requestSetup)
        {
            var request = UnityWebRequest.Get(url);
            requestSetup?.Invoke(request);
            await request.SendWebRequest();

            if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                await UniTask.Yield(PlayerLoopTiming.LastUpdate);
                Debug.LogError(request.error);
                return null;
            }

            return request.downloadHandler.text;
        }

        /// <summary>
        /// Performs a POST request with a JSON body and returns the response as a string.
        /// </summary>
        /// <param name="url">The URL to send the POST request to.</param>
        /// <param name="bodyJsonString">The JSON body of the request.</param>
        /// <param name="requestSetup">Action to set up the UnityWebRequest before sending.</param>
        /// <returns>The response as a string or null on error.</returns>
        private async Task<string> PostRequest(string url, string bodyJsonString, Action<UnityWebRequest> requestSetup)
        {
            var request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);

            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            requestSetup?.Invoke(request);
            request.SetRequestHeader("Content-Type", "application/json");

            await request.SendWebRequest();

            if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                await UniTask.Yield(PlayerLoopTiming.LastUpdate);
                Debug.LogError(request.error);
                return null;
            }

            return request.downloadHandler.text;
        }
    }
}