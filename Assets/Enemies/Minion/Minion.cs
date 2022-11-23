using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion: MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform wallCheck;
    [SerializeField] float checkingRadius;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask enemyLayer;
    private float horizontalMovement = 1;
    private bool facingRight = true;
    private bool touchingGround;
    private bool touchingWall;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        touchingGround = Physics2D.OverlapCircle(groundCheck.position, checkingRadius, groundLayer);
        touchingWall = Physics2D.OverlapCircle(wallCheck.position, checkingRadius, enemyLayer) || Physics2D.OverlapCircle(wallCheck.position, checkingRadius, groundLayer);
        Pertrolling();
    }

    void Pertrolling()
    {
        if (!touchingGround || touchingWall)
        {
            Flip();
        }
        rb.velocity = new Vector2(movementSpeed * horizontalMovement, rb.velocity.y);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(groundCheck.position, checkingRadius);
        Gizmos.DrawWireSphere(wallCheck.position, checkingRadius);
    }

    void Flip()
    {
        horizontalMovement *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
