using System;
using TMPro;
using UnityEngine;

public class ShowResults : MonoBehaviour
{
    public Mission missions;
    public TMP_Text percentage;

    void Start()
    {
       
        percentage.text = missions.lastGamePercentage.ToString() + "%";
        if(missions.lastGamePercentage > PlayerPrefs.GetFloat("HighScore"))
        {
            PlayerPrefs.SetFloat("HighScore", missions.lastGamePercentage);
        }
    }
}
