using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using UnityEngine.Tilemaps;

public class GameplayTest : MonoBehaviourPunCallbacks
{


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

        
    }

    // Update is called once per frame
    void Update()
    {
        

       

        if(PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            //team is yellow
            

        }
        else
        {
            //team is black
            

        } 
    }

}
