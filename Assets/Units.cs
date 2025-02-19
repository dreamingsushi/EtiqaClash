using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Tilemaps;
using Photon.Pun;

public class Units : MonoBehaviour
{
    public TeamColor team;
    public int unitPower;
    public float unitSpeed;
    private Rigidbody2D rb;
    public float originalSpeed;
    public bool colliding;
    [SerializeField] private TMP_Text unitPowerText;
    private ColorTilesManager tileManager;
    public enum TeamColor
    {
        Yellow, Black
    }

    void Awake()
    {
        //rotation of inverted (player two)
        if(!PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            transform.eulerAngles = new Vector3(0,0,180);
        }
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

        unitPowerText.text = unitPower.ToString();

        MoveUnit();

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

        if(PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (team == TeamColor.Yellow)
            {
                direction = Vector3.up;
            }
            else
            {
                direction = Vector3.down;
            }
            transform.position += direction * unitSpeed * Time.deltaTime;
        }
        else
        {
            if (team == TeamColor.Yellow)
            {
                direction = Vector3.down;
            }
            else
            {
                direction = Vector3.up;
            }
            transform.position += direction * unitSpeed * Time.deltaTime; 
        }
        

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        colliding = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        colliding = false;
    }
}
