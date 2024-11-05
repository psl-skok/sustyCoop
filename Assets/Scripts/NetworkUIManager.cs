using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUIManager : MonoBehaviour
{
    public Button hostButton;
    public Button joinButton;
    public GameObject canvas;

    private void Start()
    {
        // Set up button listeners
        hostButton.onClick.AddListener(HostGame);
        joinButton.onClick.AddListener(JoinGame);
    }

    private void HostGame()
    {
        // Start hosting
        NetworkManager.Singleton.StartHost();
        Debug.Log("Hosting Game");
        canvas.SetActive(false);
    }

    private void JoinGame()
    {
        // Start client
        NetworkManager.Singleton.StartClient();
        Debug.Log("Joining Game");
        canvas.SetActive(false);
    }
}
