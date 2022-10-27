using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{

    [SerializeField] float jumpHeight;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Vector2 boxSize;
    private Transform player;
    
    //private float horizontalMovement = 1;
    private bool facingRight = true;
    private bool touchingGround;
    private Rigidbody2D rb;

    [SerializeField] Vector2 lineOfSight;
    [SerializeField] LayerMask playerLayer;
    private bool playerInSight;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        touchingGround = Physics2D.OverlapBox(groundCheck.position, boxSize, 0, groundLayer);
        playerInSight = Physics2D.OverlapBox(transform.position, lineOfSight, 0, playerLayer);

        AnimationController();
    }

    void Jump()
    {
        float Xdistance = player.position.x - transform.position.x;
        if (touchingGround)
        {
            rb.AddForce(new Vector2(Xdistance, jumpHeight), ForceMode2D.Impulse);
        }
    }

    void Stop()
    {
        if (touchingGround)
        {
            rb.velocity = new Vector2(0.1f, rb.velocity.y);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(groundCheck.position, boxSize);
        Gizmos.DrawWireCube(transform.position, lineOfSight);
    }
    void Flip()
    {
        //horizontalMovement *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    void FlipTowardsPlayer()
    {
        float Xdistance = player.position.x - transform.position.x;
        if ((Xdistance < 0 && facingRight) || (Xdistance > 0 && !facingRight)) Flip();
    }

    void AnimationController()
    {
        animator.SetBool("playerInSight", playerInSight);
        animator.SetBool("touchingGround", touchingGround);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
