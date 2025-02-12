using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Units : MonoBehaviour
{
    public TeamColor team;
    public int unitPower;
    public float unitSpeed;
    private Rigidbody2D rb;
    public float originalSpeed;
    public bool colliding;
    [SerializeField] private TMP_Text unitPowerText;
    public enum TeamColor
    {
        Yellow, Black
    }    
    private void Start()
    {
        originalSpeed = unitSpeed;
        rb = GetComponent<Rigidbody2D>();
        unitPowerText.text = unitPower.ToString();
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
    }
    private void MoveUnit()
    {
        Vector3 direction;
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        colliding = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        colliding = false;
    }
}
