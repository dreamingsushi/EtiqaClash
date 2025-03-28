using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject connectingText;
    public GameObject cancelButton;
    public GameObject tutorialButton;
    public GameObject tutorialCanvas;

    void Start()
    {
        connectingText.SetActive(false);
        cancelButton.SetActive(false);
        //PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void OnPlayGameButtonPressed()
    {
        AudioManager.Instance.PlaySFX("MenuButton");
        connectingText.SetActive(true);
        if (!PhotonNetwork.IsConnected)
        {
            connectingText.GetComponent<TMP_Text>().text = ("Connecting...");
            PhotonNetwork.ConnectUsingSettings();
            
        }
        cancelButton.SetActive(true);
        //tutorialButton.GetComponent<Image>().color = Color.grey;
        tutorialButton.GetComponent<Button>().enabled = false;
    }

    public void OnCancelQueueButtonPressed()
    {
        connectingText.GetComponent<TMP_Text>().text = ("Cancelled Queue...");
        if (PhotonNetwork.IsConnected)
        { 
            PhotonNetwork.Disconnect();
            if(PhotonNetwork.InRoom)
            {
                PhotonNetwork.CurrentRoom.IsVisible = false;
                PhotonNetwork.LeaveRoom();
            }
        }

        cancelButton.SetActive(false);
        //tutorialButton.GetComponent<Image>().color = Color.yellow;
        tutorialButton.GetComponent<Button>().enabled = true;
    }

    public void OnTutorialCancel(bool opened)
    {
        tutorialCanvas.SetActive(opened);
    }

    public void GoToTutorial()
    {
        AudioManager.Instance.PlaySFX("MenuButton");
        AudioManager.Instance.musicSource.Stop();   
        SceneManager.LoadScene("Tutorial");   
        AudioManager.Instance.PlayBattleMusic();  
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
            AudioManager.Instance.musicSource.Stop();           
            PhotonNetwork.LoadLevel("Network Game Scene");
            AudioManager.Instance.PlayBattleMusic(); 
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        connectingText.GetComponent<TMP_Text>().text = ("Matchmaking... " + PhotonNetwork.PlayerList.Length + "/2");

        if(PhotonNetwork.PlayerList.Length == 2)
        {
            AudioManager.Instance.musicSource.Stop();
            PhotonNetwork.LoadLevel("Network Game Scene");
            AudioManager.Instance.PlayBattleMusic();       
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log(message);
        CreateAndJoinRoom();
    }

}
