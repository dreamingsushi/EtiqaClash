using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Tilemaps;
using Photon.Pun;
using Unity.Services.Matchmaker.Models;
using System.Collections;
using System.Collections.Generic;

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
    [SerializeField] private ParticleSystem collisonVFX;
    [SerializeField] private ParticleSystem powerUpVFX;
    [SerializeField] private ParticleSystem speedUpVFX;
    private ColorTilesManager tileManager;
    public bool canTile;
    private Animator anim;

    public enum TeamColor
    {
        Yellow, Black
    }
    
    private void Awake() {
        if(!photonView.IsMine)
        {
            GetComponent<Units>().team = TeamColor.Black;
            gameObject.tag = "UnitBlack";
            //GetComponent<SpriteRenderer>().color = Color.grey;
            GetComponent<SpriteRenderer>().sortingOrder = 3;
            GetComponentInChildren<Canvas>().sortingOrder = 10;
            GetComponentInChildren<Canvas>().transform.eulerAngles = new Vector3(0,0,180);
        }
        this.transform.parent = GameObject.FindAnyObjectByType<GameplayParentObject>().transform;
    }
    public override void OnEnable() {
        if(PhotonNetwork.LocalPlayer.ActorNumber == 1 && photonView.IsMine || PhotonNetwork.LocalPlayer.ActorNumber != 1 && !photonView.IsMine)
        {
            this.gameObject.layer = 6;
        }
        else if(PhotonNetwork.LocalPlayer.ActorNumber != 1 && photonView.IsMine || PhotonNetwork.LocalPlayer.ActorNumber == 1 && !photonView.IsMine)
            this.gameObject.layer = 7;

        if(photonView.IsMine)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else
            transform.eulerAngles = new Vector3(0,0,180);
     
    }

    private void Start()
    {
        
        anim = GetComponent<Animator>();
        originalSpeed = unitSpeed;
        rb = GetComponent<Rigidbody2D>();
        unitPowerText.text = unitPower.ToString();
        tileManager = FindAnyObjectByType<ColorTilesManager>();
    }

    void Update()
    {
        
        SwitchAnim();

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

        if(canTile)
        {
            StartCoroutine(TilePainting());
        }
        
    }

    private void LateUpdate() {
        // if(this.gameObject.transform.parent == FindAnyObjectByType<GameplayTest>().transform)
        //     tileManager.ChangeTile(transform.position, team == TeamColor.Yellow);
        
    }

    public IEnumerator TilePainting()
    {
        yield return new WaitForSeconds(0.1f);
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

    private void SwitchAnim()
    {
        if (team == TeamColor.Black && colliding)
        {
            anim.Play("BeeAngry");
            
        }
        else if (team == TeamColor.Black && !colliding)
        {
            anim.Play("BeeFront");
        }
        else if (team == TeamColor.Yellow)
        {
            anim.Play("BeeBack");
        }


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //need to check if colliding with enemy units or limit wall
        colliding = true;

        if (collision.collider.tag == "Unit")
        {
            collisonVFX.Play();
            AudioManager.Instance.PlaySFX("Clashed");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        colliding = false;
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.name == "StartTiling")
        {
            canTile = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.name == "StartTiling")
        {
            canTile = false;
        }
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


    [PunRPC]
    public void RPC_PlayPowerUpVFX()
    {
        powerUpVFX.Play();
    }

    [PunRPC]
    public void RPC_PlaySpeedUpVFX()
    {
        speedUpVFX.Play();
    }

    public void PlayPowerUpVFX()
    {
        photonView.RPC("RPC_PlayPowerUpVFX", RpcTarget.All);
    }

    public void PlaySpeedUpVFX()
    {
        photonView.RPC("RPC_PlaySpeedUpVFX", RpcTarget.All);
    }

}
