using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using TMPro;
using Photon.Pun;

public class ColorTilesManager : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase yellowTile;
    public TileBase blackTile;

    public TMP_Text tilesCount;
    int yellowAmount;
    int blackAmount;
    int totalAmount;
    float percentageAmount;

    void Start()
    {
        yellowAmount = 0;
        blackAmount = 0;   
    }

    void Update()
    {
        Debug.Log(percentageAmount);
        totalAmount = yellowAmount + blackAmount;
        if(PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            percentageAmount = (float)yellowAmount/totalAmount*100;
            percentageAmount = Mathf.Round(percentageAmount*10.0f)*0.1f;
            tilesCount.color = new Color(1,0.9594803f,0.7528302f);
        }
        else
        {
            percentageAmount = (float)blackAmount/totalAmount*100;
            percentageAmount = Mathf.Round(percentageAmount*10.0f)*0.1f;
            tilesCount.color = Color.black;
        }
        
        tilesCount.text = percentageAmount.ToString() + "%";
    }

    public void ChangeTile(Vector3 unitPosition, bool isYellowTeam)
    {
        Vector3Int tilePosition = tilemap.WorldToCell(unitPosition);
        TileBase currentTile = tilemap.GetTile(tilePosition);

        if (isYellowTeam && currentTile != yellowTile)
        {
            tilemap.SetTile(tilePosition, yellowTile);
            tilemap.RefreshTile(tilePosition);
            yellowAmount++;

        }
        else if (!isYellowTeam && currentTile != blackTile)
        {
            tilemap.SetTile(tilePosition, blackTile);
            tilemap.RefreshTile(tilePosition);
            blackAmount++;
        }
    }

    
}
