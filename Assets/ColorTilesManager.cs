using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class ColorTilesManager : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase yellowTile;
    public TileBase blackTile;
    public void ChangeTile(Vector3 unitPosition, bool isYellowTeam)
    {
        Vector3Int tilePosition = tilemap.WorldToCell(unitPosition);
        TileBase currentTile = tilemap.GetTile(tilePosition);

        if (isYellowTeam && currentTile != yellowTile)
        {
            tilemap.SetTile(tilePosition, yellowTile);
            tilemap.RefreshTile(tilePosition);
        }
        else if (!isYellowTeam && currentTile != blackTile)
        {
            tilemap.SetTile(tilePosition, blackTile);
            tilemap.RefreshTile(tilePosition);
    
        }
    }
}
