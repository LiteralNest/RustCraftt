using System.Threading.Tasks;

namespace Unity.Netcode.Samples
{
    public interface IMultiplayWebApi
    {
        /// <summary>
        /// Authenticate with the multiplayer web API.
        /// </summary>
        Task Authenticate();

        /// <summary>
        /// Allocates a server for gameplay.
        /// </summary>
        /// <returns>Allocation ID of the allocated server.</returns>
        Task<string> AllocateServer();

        /// <summary>
        /// Retrieves a list of available servers.
        /// </summary>
        /// <returns>Array of server data.</returns>
        Task<ServerData[]> GetServersList();
    
        /// <summary>
        /// Retrieves server information based on its allocation ID.
        /// </summary>
        /// <param name="allocationId">Allocation ID of the server.</param>
        /// <returns>Data for the specified server.</returns>
        Task<ServerByIdData> GetServerById(string allocationId);
    }
}