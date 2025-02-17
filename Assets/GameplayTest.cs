using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

public class GameplayTest : MonoBehaviourPunCallbacks
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            this.transform.eulerAngles = new Vector3(0,0,180);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //when spawning units
        if(PhotonNetwork.IsMasterClient)
        {
            //team is yellow
        }
        else
            //team is black
            return;
    }
}
