using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Realtime;
using System.Collections;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{
    public bool selecting = false;
    public bool applyingPower = false;
    public bool applyingSpeed = false;
    public bool applyingPower2 = false;
    public bool applyingSpeed2 = false;
    public bool applyingMix = false;
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

    [Header("Tutorial")]
    public GameObject tutorialCanvas;
    public List<GameObject> tutorialObjects;
    public GameObject tutorialMsg;
    public GameObject enemyUnitTutorial;
    public GameObject blocker;
    public GameObject endTutorial;
    private int currentTutorial = 0;
    private bool tutorialCutscene;
    private bool showLastTutorial = true;

    private float timeLeft = 66f;
    
    private ColorTilesManager colorManager;

    private ElixirBar elixirBar;
    private bool tutorialEnded;

    void Start()
    {

        currentTutorial = 0;
        elixirBar = FindAnyObjectByType<ElixirBar>();
        elixirBar.enabled = false;
         

        colorManager = FindAnyObjectByType<ColorTilesManager>();

        foreach(GameObject obj in tutorialObjects)
        {
            obj.SetActive(false);
        }

        tutorialObjects[0].SetActive(true);
        tutorialCanvas.SetActive(false);
        enemyUnitTutorial.SetActive(false);
        endTutorial.SetActive(false);
        tutorialCutscene = true;
    
    }

    private void Update()
    {
        Debug.Log(selecting);
        if(openingCutsceneText.GetComponent<UnityEngine.UI.Image>().enabled == false)
        {
            tutorialCanvas.SetActive(true);
            openingCutsceneText.transform.parent.gameObject.SetActive(false);
            
        }

        
        // if(openingCutsceneText.GetComponent<UnityEngine.UI.Image>().enabled == false && tutorialEnded)
        // {
        //     elixirBar.enabled = true;
            
        // }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if(!tutorialEnded)
                {
                    Time.timeScale = 1f;
                    StartTutorialPhase();
                }
                
                DetectClick(touch.position);
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            if(!tutorialEnded)
            {
                Time.timeScale = 1f;
                StartTutorialPhase();
            }
            DetectClick(Input.mousePosition);
        }

        TimerCountdown();

        if(enemyUnitTutorial.activeInHierarchy && showLastTutorial)
        {
            showLastTutorial = false;
            StartTutorialPhase();
        }
    }

    public void SpawningTroop()
    {
        selecting = true;
        if(tutorialEnded == false)
        {
            tutorialCutscene = true;
            StartTutorialPhase();
        }
    }
    public void ApplyingPower()
    {
        applyingPower = true;
    }
    public void ApplyingSpeed()
    {
        applyingSpeed = true;
    }
    public void ApplyingPower2()
    {
        applyingPower2 = true;
    }
    public void ApplyingSpeed2()
    {
        applyingSpeed2 = true;
    }
    public void ApplyingMix()
    {
        applyingMix = true;
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
                TutorialCardManager.Instance.DestroySelectedCard();
            }
            if (elixirBar.curElixir >= 2 && applyingSpeed)
            {
                ApplySpeed(unitCollider, speedupValue1);
                
                applyingSpeed = false;
                TutorialCardManager.Instance.DestroySelectedCard();
            }

            if (elixirBar.curElixir >= 3 && applyingPower2)
            {
                ApplyPower(unitCollider, powerupValue1);
                applyingPower2 = false;
                TutorialCardManager.Instance.DestroySelectedCard();
            }
            if (elixirBar.curElixir >= 3 && applyingSpeed2)
            {
                ApplySpeed(unitCollider, speedupValue1);
                applyingSpeed2 = false;
                TutorialCardManager.Instance.DestroySelectedCard();
            }
            if (elixirBar.curElixir >= 3 && applyingMix)
            {
                ApplyPower(unitCollider, powerupValue1);
                ApplySpeed(unitCollider, speedupValue1);
                applyingMix = false;
                TutorialCardManager.Instance.DestroySelectedCard();
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
            if(!tutorialEnded)
            {
                tutorialCutscene = true;
                StartTutorialPhase();
            }
            
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
    void StartTutorialPhase()
    {   

        if(tutorialCutscene)
        {
            foreach(GameObject obj in tutorialObjects)
            {
                obj.SetActive(false);
            }
            
            if(tutorialObjects[currentTutorial].name == "enemy")
            {
                Time.timeScale = 1f;
                tutorialCutscene = false;
                tutorialEnded = true;
                StartCoroutine(EndTutorial());
                return;
            }

            if(tutorialEnded)
            {
                return;
            }
            else
            {
                currentTutorial++;
                tutorialObjects[currentTutorial].SetActive(true);
            }
            
            if(currentTutorial > 2)
            {
                tutorialMsg.SetActive(false);
                
            }

            if(tutorialObjects[currentTutorial].name == "Arrow")
            {
                elixirBar.enabled = true;
                elixirBar.maxElixir = 2;
                elixirBar.curElixir = 2;
                tutorialCutscene = false;
                blocker.SetActive(false);
            }

            if(tutorialObjects[currentTutorial- 1].name == "Arrow")
            {
                tutorialCutscene = false;
            }

            if(tutorialObjects[currentTutorial].name == "timestop")
            {
                Time.timeScale = 0f;
                
            }
            

            if(tutorialObjects[currentTutorial].name == "silhouette 5")
            {
                
                elixirBar.maxElixir = 10;
                
                StartCoroutine(EnemyTutorial());
                
            }
            if(tutorialObjects[currentTutorial].name == "placeholder")
            {
                
                tutorialCutscene = false;
                
            }
            
            
        }

        if(enemyUnitTutorial.activeInHierarchy)
        {
            Time.timeScale = 0f;
            tutorialObjects[tutorialObjects.Count-1].SetActive(true);
            tutorialCutscene = true;
            
        }

        
    }

    public IEnumerator EnemyTutorial()
    {
        yield return new WaitForSeconds(3.8f);
        enemyUnitTutorial.SetActive(true);

    }

    public IEnumerator EndTutorial()
    {
        yield return new WaitForSeconds(5f);
        
        endTutorial.SetActive(true);
        yield return new WaitForSeconds(6f);

        endTutorial.SetActive(false);
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
