using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.UI;
using TMPro;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Net.NetworkInformation;

public class NetworkUIManager : MonoBehaviour
{
    public Button hostButton;
    public Button joinButton;
    public GameObject canvas;

    private void Start()
    {
        // Attach button click listeners
        hostButton.onClick.AddListener(HostGame);
        joinButton.onClick.AddListener(JoinGame);
    }

    public void HostGame()
    {
        NetworkManager.Singleton.StartHost();
        Debug.Log("Hosting Game");
        canvas.SetActive(false);
    }

    public void JoinGame()
    {
        // Set the IP address entered in the InputField for the client connection
        var unityTransport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        unityTransport.ConnectionData.Address = "127.0.0.1";
        unityTransport.ConnectionData.Port = 7777; // Ensure this matches the port set in UnityTransport for the host

        NetworkManager.Singleton.StartClient();
        Debug.Log("Joining Game");
        canvas.SetActive(false);
    }


}
