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
    private Transform player;
    [SerializeField] float destinationOffset;
    [SerializeField] float attackRange;
    [SerializeField] float cd;
    private bool attackStarted;
    private BoxCollider2D boxcollider;
    private Animator animator;

    private AudioClip teleportSFX;


    // Start is called before the first frame update
    void Start()
    {
        teleportSFX = Resources.Load<AudioClip>("Audio/blink_sfx");
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        boxcollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(0, 0);
        playerInSight = Physics2D.OverlapBox(transform.position, lineOfSight, 0, playerLayer);
        if (playerInSight && !attackStarted)
        {
            StartCoroutine(Attack());
            attackStarted = true;
        }
    }

    IEnumerator Attack()
    {
        FlipTowardsPlayer();
        animator.SetTrigger("teleport");
        yield return new WaitForSeconds(1.2f);
        Teleport();
        yield return new WaitForSeconds(0.4f);
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
        if (this.gameObject.GetComponent<Enemy>().health > 0)
            this.gameObject.GetComponent<AudioSource>().PlayOneShot(teleportSFX, 0.2f);
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
