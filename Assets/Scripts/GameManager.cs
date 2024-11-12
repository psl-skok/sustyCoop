using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public GameObject chickenPrefab; // Assign this in the Inspector
    public GameObject cowPrefab; // Assign this in the Inspector

    // Define spawn points in the scene
    public Transform[] spawnPoints; // Assign spawn points in the Inspector

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            SpawnPlayers();
        }
    }

    private void SpawnPlayers()
    {
        int spawnIndex = 0;

        foreach (var clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            // Select the player prefab (alternating between chicken and cow for this example)
            GameObject playerPrefab = (clientId % 2 == 0) ? chickenPrefab : cowPrefab;

            // Choose a spawn point based on spawnIndex (loops if there are fewer spawn points than players)
            Transform spawnPoint = spawnPoints[spawnIndex % spawnPoints.Length];
            spawnIndex++;

            // Instantiate and set position
            GameObject playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        }
    }
}
