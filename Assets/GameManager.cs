using UnityEngine;

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

    private ElixirBar elixirBar;

    void Start()
    {
        elixirBar = FindAnyObjectByType<ElixirBar>();
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
                ApplyPower(unitCollider);
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
        // Calculate the spawn position (spawn above the lane)
        Vector3 spawnPosition = lane.transform.position + new Vector3(0, spawnOffset, 0);
        
        // Instantiate the unitPrefab at the calculated position
        Instantiate(unitPrefab, spawnPosition, Quaternion.identity);
    }


    void ApplyPower(Collider2D unit)
    {
        unit.GetComponent<Units>().unitPower += 1;
        elixirBar.curElixir -= 2;
    }

    void ApplySpeed(Collider2D unit)
    {
        unit.GetComponent<Units>().originalSpeed += 1;
        elixirBar.curElixir -= 2;
    }

}
