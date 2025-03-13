using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ColorTilesManager : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase yellowTile;
    public TileBase blackTile;

    public TMP_Text tilesCount;
    int yellowAmount;
    int blackAmount;
    int totalAmount;
    public float percentageAmount;

    public Mission missions;

    void Start()
    {
        CountTiles(); // Count existing tiles at the start
    }

    void Update()
    {
        totalAmount = yellowAmount + blackAmount;
        if (totalAmount > 0)
        {
            percentageAmount = (float)yellowAmount / totalAmount * 100;
            percentageAmount = Mathf.Round(percentageAmount * 10.0f) * 0.1f;
        }
        else
        {
            percentageAmount = 0;
        }

        tilesCount.color = new Color(1, 0.9594803f, 0.7528302f);
        tilesCount.text = percentageAmount.ToString("F1") + "%";

        if(SceneManager.GetActiveScene().name == "Network Game Scene")
            missions.lastGamePercentage = percentageAmount;
    }

    void CountTiles()
    {
        yellowAmount = 0;
        blackAmount = 0;

        BoundsInt bounds = tilemap.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(pos);
            if (tile == yellowTile) yellowAmount++;
            if (tile == blackTile) blackAmount++;
        }
    }

    public void ChangeTile(Vector3 unitPosition, bool isYellowTeam)
    {
        Vector3Int tilePosition = tilemap.WorldToCell(unitPosition);
        TileBase currentTile = tilemap.GetTile(tilePosition);

        if (isYellowTeam)
        {
            if (currentTile == blackTile) blackAmount--; // Reduce black count if converting
            if (currentTile != yellowTile) yellowAmount++; // Increase yellow count only if different
            tilemap.SetTile(tilePosition, yellowTile);
        }
        else
        {
            if (currentTile == yellowTile) yellowAmount--; // Reduce yellow count if converting
            if (currentTile != blackTile) blackAmount++; // Increase black count only if different
            tilemap.SetTile(tilePosition, blackTile);
        }

        tilemap.RefreshTile(tilePosition);
    }
}
