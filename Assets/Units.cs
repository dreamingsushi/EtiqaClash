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
    public int increasedSpeed;

    [SerializeField] private TMP_Text unitPowerText;
    private ColorTilesManager tileManager;

    public enum TeamColor
    {
        Yellow, Black
    }
    
    private void Awake() {
        if(!photonView.IsMine)
        {
            GetComponent<Units>().team = TeamColor.Black;
            GetComponent<SpriteRenderer>().color = Color.grey;
            
        }
        this.transform.parent = GameObject.FindAnyObjectByType<GameplayTest>().transform;
    }
    public override void OnEnable() {
        if(photonView.IsMine)
        {
            this.gameObject.layer = 6;
        }

        if(photonView.IsMine)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else
            transform.eulerAngles = new Vector3(0,0,180);
     
    }

    private void Start()
    {
        

        originalSpeed = unitSpeed;
        rb = GetComponent<Rigidbody2D>();
        unitPowerText.text = unitPower.ToString();
        tileManager = FindAnyObjectByType<ColorTilesManager>();
    }

    void Update()
    {
        
        
        rb.mass = unitPower;
        MoveUnit();

        

        if (colliding)
        {
            unitSpeed = 1;
        }
        else
        {
            unitSpeed = originalSpeed;
        }

        
    }

    private void LateUpdate() {
        if(this.gameObject.transform.parent == FindAnyObjectByType<GameplayTest>().transform)
            tileManager.ChangeTile(transform.position, team == TeamColor.Yellow);
    }

    private void MoveUnit()
    {
        Vector3 direction;

        // if(photonView.IsMine)
        // {
            if (team == TeamColor.Yellow)
            {
                
                direction = Vector3.up;
                
            }
            else
            {
                direction = Vector3.down;
                
            }
            transform.position += direction * unitSpeed * Time.deltaTime;
        // }
        // else if(!photonView.IsMine)
        // {
        //     if (team == TeamColor.Yellow)
        //     {
        //         //GetComponent<SpriteRenderer>().color = Color.yellow;
        //         direction = Vector3.up;
                
        //     }
        //     else
        //     {
        //         //GetComponent<SpriteRenderer>().color = Color.black;
        //         direction = Vector3.down;
                
        //     }
        //     transform.position += direction * unitSpeed * Time.deltaTime; 
        // }
        

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

    [PunRPC]
    public void SyncSpeed(int speed)
    {
        originalSpeed += speed;
        

    }
}
