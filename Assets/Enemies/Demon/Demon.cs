using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : MonoBehaviour
{

    private bool facingRight = false;
    private Rigidbody2D rb;
    //BoxCollider2D collider;
    [SerializeField] Vector2 lineOfSight;
    [SerializeField] LayerMask playerLayer;
    private bool playerInSight;
    [SerializeField] Transform player;
    [SerializeField] float destinationOffset;
    [SerializeField] float attackRange;
    [SerializeField] float cd;
    private bool attackStarted;
    private BoxCollider2D boxcollider;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxcollider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        playerInSight = Physics2D.OverlapBox(transform.position, lineOfSight, 0, playerLayer);
        if (playerInSight && !attackStarted)
        {
            StartCoroutine(Attack());
            attackStarted = true;
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1);
        Teleport();
        yield return new WaitForSeconds(1);
        Vector2 original_size = boxcollider.size;
        Vector2 original_offset = boxcollider.offset;
        boxcollider.offset = new Vector2((boxcollider.offset.x + attackRange / 2.0f), boxcollider.offset.y);
        GetComponent<BoxCollider2D>().size = new Vector2((boxcollider.size.x + attackRange), boxcollider.size.y);
        yield return new WaitForSeconds(0.5f);
        boxcollider.offset = original_offset;
        boxcollider.size = original_size;
        yield return new WaitForSeconds(cd);
        attackStarted = false;
    }

    private void Teleport()
    {
        FlipTowardsPlayer();
        float Xdistance = player.position.x - transform.position.x;
        float newX;
        if (Xdistance < 0)
        {
            newX = player.position.x + destinationOffset;
        }
        else
        {
            newX = player.position.x - destinationOffset;
        }
        transform.position = new Vector2(newX,  player.position.y);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, lineOfSight);
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
    void FlipTowardsPlayer()
    {
        float Xdistance = player.position.x - transform.position.x;
        if ((Xdistance < 0 && facingRight) || (Xdistance > 0 && !facingRight)) Flip();
    }
}
