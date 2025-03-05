using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Realtime;
using System.Collections;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public bool selecting = false;
    public bool applyingPower = false;
    public bool applyingSpeed = false;
    public BoxCollider2D lane1;
    public BoxCollider2D lane2;
    public BoxCollider2D lane3;
    public BoxCollider2D lane4;
    public BoxCollider2D lane5;
    public GameObject unitPrefab;
    public float spawnOffset = 2.0f;
    public GameObject openingCutsceneText;

    [Header("Powerups settings")]
    public int powerupValue1 = 1;
    public int speedupValue1 = 1;

    public GameObject countdownTimerText;
    private float timeLeft = 66f;
    
    private ColorTilesManager colorManager;

    private ElixirBar elixirBar;

    void Awake()
    {
        
    }
    void Start()
    {
        
        elixirBar = FindAnyObjectByType<ElixirBar>();
        elixirBar.enabled = false;
         

        colorManager = FindAnyObjectByType<ColorTilesManager>();
        
    }

    private void Update()
    {
        if(openingCutsceneText.GetComponent<UnityEngine.UI.Image>().enabled == false)
        {
            elixirBar.enabled = true;
            openingCutsceneText.transform.parent.gameObject.SetActive(false);
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                DetectClick(touch.position);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            DetectClick(Input.mousePosition);
        }

        TimerCountdown();
    }

    public void SpawningTroop()
    {
        selecting = true;
    }
    public void ApplyingPower()
    {
        applyingPower = true;
    }
    public void ApplyingSpeed()
    {
        applyingSpeed = true;
    }
    void DetectClick(Vector2 inputPosition)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(inputPosition);
        Collider2D unitCollider = Physics2D.OverlapPoint(worldPosition);

        if (unitCollider != null && unitCollider.CompareTag("Unit"))
        {
            if (elixirBar.curElixir >= 2 && applyingPower)
            {
                ApplyPower(unitCollider, powerupValue1);

                applyingPower = false;
            }
            if (elixirBar.curElixir >= 2 && applyingSpeed)
            {
                ApplySpeed(unitCollider, speedupValue1);
                
                applyingSpeed = false;
            }
            return;
        }

        if (lane1.OverlapPoint(worldPosition))
        {
            OnLaneClicked(1, lane1);
        }
        else if (lane2.OverlapPoint(worldPosition))
        {
            OnLaneClicked(2, lane2);
        }
        else if (lane3.OverlapPoint(worldPosition))
        {
            OnLaneClicked(3, lane3);
        }
        else if (lane4.OverlapPoint(worldPosition))
        {
            OnLaneClicked(4, lane4);
        }
        else if (lane5.OverlapPoint(worldPosition))
        {
            OnLaneClicked(5, lane5);
        }
    }

    void OnLaneClicked(int laneNumber, BoxCollider2D lane)
    {
        if (elixirBar.curElixir >= 2 && selecting)
        {
            Debug.Log("Lane " + laneNumber + " clicked!");
            SpawnUnitAbove(lane);
            elixirBar.curElixir -= 2;
            selecting = false;
        }
    }

    void SpawnUnitAbove(BoxCollider2D lane)
    {
        
        spawnOffset = 0.5f;
        

        // Calculate the spawn position (spawn above the lane)
        Vector3 spawnPosition = lane.transform.position + new Vector3(0, spawnOffset, 0);
        
        // Instantiate the unitPrefab at the calculated position
        Instantiate(unitPrefab, spawnPosition, Quaternion.identity);
        

    }

    


    void ApplyPower(Collider2D unit, int powerValue)
    {
        unit.GetComponent<TutorialUnits>().unitPower ++;
        unit.GetComponent<TutorialUnits>().increasedPower ++;

        elixirBar.curElixir -= 2;
    }

    void ApplySpeed(Collider2D unit, int speedValue)
    {
        unit.GetComponent<TutorialUnits>().originalSpeed ++;

        elixirBar.curElixir -= 2;
    }

    public void TimerCountdown()
    {
        if(SceneManager.GetActiveScene().name == "Tutorial")
        {
            countdownTimerText.GetComponent<TMP_Text>().text = "∞";
            return;
        }
        else
        {
            timeLeft -= Time.deltaTime;
            if(timeLeft < 0)
            {
                if(colorManager.percentageAmount > 50f)
                {
                    SceneManager.LoadScene("Victory");
                }
                else
                    SceneManager.LoadScene("Defeat");
            }
            countdownTimerText.GetComponent<TMP_Text>().text = timeLeft.ToString("F1");
        }   
    }
}
