using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Tilemaps;
using Photon.Pun;
using Unity.Services.Matchmaker.Models;

public class Units : MonoBehaviourPunCallbacks
{
    public TeamColor team;
    public int unitPower;
    public float unitSpeed;
    private Rigidbody2D rb;
    public float originalSpeed;
    public bool colliding;

    public int increasedPower;

    [SerializeField] private TMP_Text unitPowerText;
    private ColorTilesManager tileManager;

    public enum TeamColor
    {
        Yellow, Black
    }
    
    public override void OnEnable() {
        this.transform.parent = GameObject.FindAnyObjectByType<GameplayTest>().transform;
    }

    private void Start()
    {
        if(!photonView.IsMine)
        {
            GetComponent<Units>().team = Units.TeamColor.Black;
            GetComponent<SpriteRenderer>().color = Color.black;
        }

        originalSpeed = unitSpeed;
        rb = GetComponent<Rigidbody2D>();
        unitPowerText.text = unitPower.ToString();
        tileManager = FindAnyObjectByType<ColorTilesManager>();
    }

    void Update()
    {
        if(photonView.IsMine)
        {
            this.gameObject.layer = 6;
        }
        
        rb.mass = unitPower;
        MoveUnit();

        //Show power to all
        //photonView.RPC("SyncPower", RpcTarget.AllBuffered, increasedPower);

        if (colliding)
        {
            unitSpeed = 1;
        }
        else
        {
            unitSpeed = originalSpeed;
        }

        tileManager.ChangeTile(transform.position, team == TeamColor.Yellow);
    }
    private void MoveUnit()
    {
        Vector3 direction;

        if(photonView.IsMine)
        {
            if (team == TeamColor.Yellow)
            {
                
                direction = Vector3.up;
                
            }
            else
            {
                //GetComponent<SpriteRenderer>().color = Color.black;
                direction = Vector3.down;
                
            }
            transform.position += direction * unitSpeed * Time.deltaTime;
        }
        else if(!photonView.IsMine)
        {
            if (team == TeamColor.Yellow)
            {
                //GetComponent<SpriteRenderer>().color = Color.yellow;
                direction = Vector3.down;
                
            }
            else
            {
                //GetComponent<SpriteRenderer>().color = Color.black;
                direction = Vector3.up;
                
            }
            transform.position += direction * unitSpeed * Time.deltaTime; 
        }
        

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //need to check if colliding with enemy units or limit wall
        colliding = true;    
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        colliding = false;
    }

    [PunRPC]
    public void SyncPower(int power)
    {
        unitPower += power;
        unitPowerText.text = unitPower.ToString();

    }
}
