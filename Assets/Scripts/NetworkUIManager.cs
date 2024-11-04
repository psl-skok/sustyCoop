using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.UI;

public class NetworkUIManager : MonoBehaviour
{
    public InputField ipAddressInput; // Assign your IP address input field here in the Inspector.
    public Button hostButton;         // Assign your Host button here in the Inspector.
    public Button joinButton;         // Assign your Join button here in the Inspector.

    private void Start()
    {
        // Attach button click listeners
        hostButton.onClick.AddListener(HostGame);
        joinButton.onClick.AddListener(JoinGame);
    }

    public void HostGame()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void JoinGame()
    {
        // Set the IP address entered in the InputField for the client connection
        var unityTransport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        unityTransport.ConnectionData.Address = ipAddressInput.text; // Use the IP address entered by the user
        unityTransport.ConnectionData.Port = 7777; // Ensure this matches the port set in UnityTransport for the host

        NetworkManager.Singleton.StartClient();
    }
}
