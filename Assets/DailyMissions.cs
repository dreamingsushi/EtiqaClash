
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyMissions : MonoBehaviour
{

    public Image checkImage;
    public GameObject tick;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        if(PlayerPrefs.GetInt("DailyGame") == 1)
        {
            checkImage.color = Color.yellow;
            tick.SetActive(true);
        }
        else
            checkImage.color = Color.black;


    }

    // Update is called once per frame

    public void OpenMission(GameObject panelUI)
    {
        panelUI.SetActive(true);
    }

    public void CloseMission(GameObject panelUI)
    {
        panelUI.SetActive(false);

    }


}
