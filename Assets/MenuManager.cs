using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour//NetworkBehaviour
{
    public NetworkManager networkManager;
    public GameObject Connect;
    public GameObject IP_Input;
    public GameObject RunAsServer;
    void Start() 
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void OnConnectButtonPress()
    {
        networkManager.networkAddress = IP_Input.GetComponent<InputField>().text;
        if(RunAsServer.GetComponent<Toggle>().isOn)
        {
            HostLobby();
        }else
        {
            ConnectToLobby();
        }

    }
    private void HostLobby()
    {
        networkManager.StartHost();
    }
    private void ConnectToLobby()
    {
        networkManager.StartClient();
    }
    public virtual void OnServerDisconnect(NetworkConnection conn)
    {
        SceneManager.LoadScene("MainMenu");
        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetSceneByName("MainMenu"));
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
