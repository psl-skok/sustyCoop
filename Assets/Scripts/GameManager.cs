using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public GameObject chickenPrefab; // Assign this in the Inspector
    public GameObject cowPrefab; // Assign this in the Inspector

    // Use OnNetworkReady instead of OnNetworkStart
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            SpawnPlayers();
        }
    }

    private void SpawnPlayers()
    {
        // Get the client IDs of all players that have connected
        foreach (var clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            // Alternating player prefabs based on client ID (you can adjust this logic)
            GameObject playerPrefab = (clientId % 2 == 0) ? chickenPrefab : cowPrefab;

            GameObject playerInstance = Instantiate(playerPrefab);
            playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        }
    }
}
