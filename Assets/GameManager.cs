using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Realtime;

public class GameManager : MonoBehaviour
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
    public float spawnOffset = 1.0f;

    [Header("Powerups settings")]
    public int powerupValue1 = 1;
    public int speedupValue1 = 1;

    public GameObject gameplayParent;

    private ElixirBar elixirBar;

    void Awake()
    {
        Debug.Log("Player "+ PhotonNetwork.LocalPlayer.ActorNumber + " has joined the game.");
    }
    void Start()
    {
        elixirBar = FindAnyObjectByType<ElixirBar>();
        if(!PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            gameplayParent.transform.eulerAngles = new Vector3(0,0,180);
        }
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DetectClick();
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
    void DetectClick()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Collider2D unitCollider = Physics2D.OverlapPoint(mousePosition);

        if (unitCollider != null && unitCollider.CompareTag("Unit"))
        {
            if (elixirBar.curElixir >= 2 &&applyingPower)
            {
                
                ApplyPower(unitCollider, powerupValue1);
                unitCollider.gameObject.GetComponent<PhotonView>().RPC("SyncPower", RpcTarget.AllBuffered, unitCollider.gameObject.GetComponent<Units>().increasedPower);
                applyingPower = false;
            }
            if (elixirBar.curElixir >= 2 &&applyingSpeed)
            {
                ApplySpeed(unitCollider);
                applyingSpeed = false;
            }
        
        return;
        }

        if (lane1.OverlapPoint(mousePosition))
        {
            OnLaneClicked(1, lane1);
        }
        else if (lane2.OverlapPoint(mousePosition))
        {
            OnLaneClicked(2, lane2);
        }
        else if (lane3.OverlapPoint(mousePosition))
        {
            OnLaneClicked(3, lane3);
        }
        else if (lane4.OverlapPoint(mousePosition))
        {
            OnLaneClicked(4, lane4);
        }
        else if (lane5.OverlapPoint(mousePosition))
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
        if(PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            spawnOffset = 2;
        }
        else
            spawnOffset = 2;
        // Calculate the spawn position (spawn above the lane)
        Vector3 spawnPosition = lane.transform.position + new Vector3(0, spawnOffset, 0);
        
        // Instantiate the unitPrefab at the calculated position
        GameObject spawnedUnit = PhotonNetwork.Instantiate(unitPrefab.name, spawnPosition, Quaternion.identity);
        

        // if(spawnedUnit.GetPhotonView().IsMine)
        // {
            
        //     spawnedUnit.GetComponent<Units>().team = Units.TeamColor.Yellow;
        //     spawnedUnit.GetComponent<SpriteRenderer>().color = Color.yellow;
        // }
        // else 
        // {
            
        //     spawnedUnit.GetComponent<Units>().team = Units.TeamColor.Black;
        //     spawnedUnit.GetComponent<SpriteRenderer>().color = Color.black;
        // }
    }


    void ApplyPower(Collider2D unit, int powerValue)
    {
        unit.GetComponent<Units>().increasedPower = powerValue;
        elixirBar.curElixir -= 2;
    }

    void ApplySpeed(Collider2D unit)
    {
        unit.GetComponent<Units>().originalSpeed += 1;
        elixirBar.curElixir -= 2;
    }

}
