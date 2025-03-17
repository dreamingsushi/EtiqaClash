using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Realtime;
using System.Collections;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
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

    public GameObject gameplayParent;

    public GameObject countdownTimerText;
    private float timeLeft = 60f;
    
    private ColorTilesManager colorManager;
    private CardManager cardManager;
    private ElixirBar elixirBar;

    void Awake()
    {
        print("Player "+ PhotonNetwork.LocalPlayer.ActorNumber + " has joined the game.");
    }
    void Start()
    {
        cardManager = FindAnyObjectByType<CardManager>();
        cardManager.enabled = false;
        elixirBar = FindAnyObjectByType<ElixirBar>();
        elixirBar.enabled = false;
        if(!PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            gameplayParent.transform.eulerAngles = new Vector3(0,0,180);
        }   

        colorManager = FindAnyObjectByType<ColorTilesManager>();
        
    }

    private void Update()
    {
        if(openingCutsceneText.GetComponent<UnityEngine.UI.Image>().enabled == false)
        {
            cardManager.enabled = true;
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
        if(elixirBar.enabled == true)
        {
            TimerCountdown();
        }
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
                unitCollider.gameObject.GetComponent<PhotonView>().RPC("SyncPower", RpcTarget.AllBuffered, unitCollider.gameObject.GetComponent<Units>().increasedPower);
                applyingPower = false;
                CardManager.Instance.DestroySelectedCard();
            }
            if (elixirBar.curElixir >= 2 && applyingSpeed)
            {
                ApplySpeed(unitCollider, speedupValue1);
                unitCollider.gameObject.GetComponent<PhotonView>().RPC("SyncSpeed", RpcTarget.AllBuffered, unitCollider.gameObject.GetComponent<Units>().increasedSpeed);
                applyingSpeed = false;
                CardManager.Instance.DestroySelectedCard();
            }
            if (elixirBar.curElixir >= 3 && applyingPower2)
            {
                ApplyPower(unitCollider, powerupValue1);
                unitCollider.gameObject.GetComponent<PhotonView>().RPC("SyncPower", RpcTarget.AllBuffered, unitCollider.gameObject.GetComponent<Units>().increasedPower);
                applyingPower2 = false;
                CardManager.Instance.DestroySelectedCard();
            }
            if (elixirBar.curElixir >= 3 && applyingSpeed2)
            {
                ApplySpeed(unitCollider, speedupValue1);
                unitCollider.gameObject.GetComponent<PhotonView>().RPC("SyncSpeed", RpcTarget.AllBuffered, unitCollider.gameObject.GetComponent<Units>().increasedSpeed);
                applyingSpeed2 = false;
                CardManager.Instance.DestroySelectedCard();
            }
            if (elixirBar.curElixir >= 3 && applyingMix)
            {
                ApplyPower(unitCollider, powerupValue1);
                ApplySpeed(unitCollider, speedupValue1);
                unitCollider.gameObject.GetComponent<PhotonView>().RPC("SyncSpeed", RpcTarget.AllBuffered, unitCollider.gameObject.GetComponent<Units>().increasedSpeed);
                unitCollider.gameObject.GetComponent<PhotonView>().RPC("SyncPower", RpcTarget.AllBuffered, unitCollider.gameObject.GetComponent<Units>().increasedPower);
                applyingMix = false;
                CardManager.Instance.DestroySelectedCard();
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
        AudioManager.Instance.PlaySFX("DeployBee");
        spawnOffset = 0.5f;
        

        // Calculate the spawn position (spawn above the lane)
        Vector3 spawnPosition = lane.transform.position + new Vector3(0, spawnOffset, 0);
        
        // Instantiate the unitPrefab at the calculated position
        PhotonNetwork.Instantiate(unitPrefab.name, spawnPosition, Quaternion.identity);
        

    }

    


    void ApplyPower(Collider2D unit, int powerValue)
    {
        AudioManager.Instance.PlaySFX("PowerUp");
        unit.GetComponent<Units>().increasedPower = powerValue;
        unit.GetComponent<Units>().PlayPowerUpVFX();
        elixirBar.curElixir -= 2;
        if (powerValue == 2)
        {
            elixirBar.curElixir -= 3;
        }
    }

    void ApplySpeed(Collider2D unit, int speedValue)
    {
        AudioManager.Instance.PlaySFX("SpeedUp");
        unit.GetComponent<Units>().increasedSpeed = speedValue;
        unit.GetComponent<Units>().PlaySpeedUpVFX();
        elixirBar.curElixir -= 2;
        if (speedValue == 2)
        {
            elixirBar.curElixir -= 3;
        }
    }

    public void TimerCountdown()
    {
        if(SceneManager.GetActiveScene().name == "Tutorial")
        {
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
                    AudioManager.Instance.PlaySFX("Victory");
                }
                else
                    SceneManager.LoadScene("Defeat");
                    AudioManager.Instance.PlaySFX("Lose");
            }
            countdownTimerText.GetComponent<TMP_Text>().text = timeLeft.ToString("F1");
        }   
    }
}
