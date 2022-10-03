using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : MonoBehaviour
{

    private bool facingRight = false;
    private Rigidbody2D rb;
    [SerializeField] Vector2 lineOfSight;
    [SerializeField] LayerMask playerLayer;
    private bool playerInSight;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    private void FixedUpdate()
    {
        playerInSight = Physics2D.OverlapBox(transform.position, lineOfSight, 0, playerLayer);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
}
