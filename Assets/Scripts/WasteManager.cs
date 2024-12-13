using UnityEngine;


public class WasteManager : MonoBehaviour
{
    public GameObject wastePrefab;
    public Transform spawnPoint1;
    public Transform spawnPoint2;

    void Start()
    {
        // Spawn waste objects with the rotation of each spawn point
        Instantiate(wastePrefab, spawnPoint1.position, spawnPoint1.rotation);
        Instantiate(wastePrefab, spawnPoint2.position, spawnPoint2.rotation);
    }

}
