using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject connectingText;

    void Start()
    {
        connectingText.SetActive(false);
        //PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void OnPlayGameButtonPressed()
    {
        connectingText.SetActive(true);
        if (!PhotonNetwork.IsConnected)
        {
            connectingText.GetComponent<TMP_Text>().text = ("Connecting...");
            PhotonNetwork.ConnectUsingSettings();
            
        }

    }

    public void GoToTutorial()
    {
        
        SceneManager.LoadScene("Tutorial");
    }

    public override void OnConnected()
    {
        Debug.Log("Connected to Internet");
    }

    public override void OnConnectedToMaster()
    {
        
        connectingText.GetComponent<TMP_Text>().text = ("Matchmaking...");
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to Photon");
        PhotonNetwork.JoinRandomRoom();
        
    }





    void CreateAndJoinRoom()
    {
        string randomRoomName = "Room" + Random.Range(0,10000);
 
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 2;
 
        PhotonNetwork.CreateRoom(randomRoomName, roomOptions); 
    }

    



    public override void OnJoinedRoom()
    {
        connectingText.GetComponent<TMP_Text>().text = ("Matchmaking... " + PhotonNetwork.PlayerList.Length + "/2");
        if(PhotonNetwork.PlayerList.Length == 2)
        {
            
            PhotonNetwork.LoadLevel("Network Test Scene");       
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        connectingText.GetComponent<TMP_Text>().text = ("Matchmaking... " + PhotonNetwork.PlayerList.Length + "/2");

        if(PhotonNetwork.PlayerList.Length == 2)
        {
            PhotonNetwork.LoadLevel("Network Test Scene");       
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log(message);
        CreateAndJoinRoom();
    }

}
