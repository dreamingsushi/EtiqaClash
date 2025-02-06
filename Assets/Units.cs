using Unity.VisualScripting;
using UnityEngine;

public class Units : MonoBehaviour
{
    public TeamColor team;
    public int unitPower;
    public float unitSpeed;
    private Rigidbody2D rb;
    float originalSpeed;
    public enum TeamColor
    {
        Yellow, Black
    }    
    private void Start()
    {
        originalSpeed = unitSpeed;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.mass = unitPower;
        MoveUnit();
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
        unitSpeed = 1;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        unitSpeed = originalSpeed;
    }
}
