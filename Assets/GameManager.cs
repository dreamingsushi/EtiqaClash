using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool selecting = false;
    public BoxCollider2D lane1;
    public BoxCollider2D lane2;
    public BoxCollider2D lane3;
    public BoxCollider2D lane4;
    public BoxCollider2D lane5;
    public GameObject unitPrefab;
    public float spawnOffset = 1.0f;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && selecting)
        {
            DetectLaneClick();
        }
    }

    public void SpawningTroop()
    {
        selecting = true;
    }

    void DetectLaneClick()
    {
        // Get the mouse position in world space
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Check if the mouse position is within the bounds of each lane's collider
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
        Debug.Log("Lane " + laneNumber + " clicked!");
        SpawnUnitAbove(lane);
        selecting = false;
    }

    void SpawnUnitAbove(BoxCollider2D lane)
    {
        // Calculate the spawn position (spawn above the lane)
        Vector3 spawnPosition = lane.transform.position + new Vector3(0, spawnOffset, 0);
        
        // Instantiate the unitPrefab at the calculated position
        Instantiate(unitPrefab, spawnPosition, Quaternion.identity);
    }

}
