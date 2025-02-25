using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class GameplayTest : MonoBehaviourPunCallbacks
{   
    // public GameObject countdownTimerText;
    // private float timeLeft = 70f;
    
    // private ColorTilesManager colorManager;


    // // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
        
    //     colorManager = FindAnyObjectByType<ColorTilesManager>();
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     timeLeft -= Time.deltaTime;
    //     if(timeLeft < 0)
    //     {
    //         if(colorManager.percentageAmount > 50f)
    //         {
    //             SceneManager.LoadScene("Victory");
    //         }
    //         else
    //             SceneManager.LoadScene("Defeat");
    //     }
    //     countdownTimerText.GetComponent<TMP_Text>().text = timeLeft.ToString("F1");

       

    //     if(PhotonNetwork.LocalPlayer.IsMasterClient)
    //     {
    //         //team is yellow
            

    //     }
    //     else
    //     {
    //         //team is black
            

    //     } 
    // }

    

}
