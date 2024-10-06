using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private GameObject player;
    public float attackSpeed;
    public float idleSpeed;
    public float returnSpeed;
    public Vector2 offset;

    private SpriteRenderer swordRenderer;
    private SpriteRenderer playerRender;

    private bool canAttack;
    private bool isAttacking;
    private bool isReturning;
    private Vector2 currentOffset;
    private Vector2 attackLoc;


    // public setPlayer() {

    // }
    // Start is called before the first frame update
    void Start()
    {
        swordRenderer = GetComponent<SpriteRenderer>();
    }
    public void Setup(GameObject playerInput) {
        playerRender = playerInput.GetComponent<SpriteRenderer>();
        player = playerInput;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) {
            return;
        }

        UpdateOffset();

        if (isAttacking)
        {
            AttackAnim(attackLoc);
            return;
        }
        if (isReturning) {
            Return();
            return;
        }
        Idle();
    }

    public void Attack(Vector2 loc)
    {
        attackLoc = loc;
        isAttacking = true;
    }

    public void AttackAnim(Vector2 destination)
    {
        Vector2 direction = (destination - (Vector2)this.transform.position);
        Vector2 movement = direction * attackSpeed * Time.deltaTime;
        SetPosition(movement.x + this.transform.position.x, movement.y + this.transform.position.y);

        if(Vector2.Distance(this.transform.position, destination) < 0.1f) {
            isAttacking = false;
            isReturning = true;
        }
    }

    void Idle()
    {
        Vector2 destination = ((Vector2) player.transform.position) + currentOffset;
        Vector2 direction = (destination - (Vector2)this.transform.position);
        Vector2 movement = direction * idleSpeed * Time.deltaTime;
        SetPosition(movement.x + this.transform.position.x, movement.y + this.transform.position.y);
    }

    void Return()
    {
        Vector2 destination = ((Vector2) player.transform.position) + currentOffset;
        Vector2 direction = (destination - (Vector2)this.transform.position);
        Vector2 movement = direction * returnSpeed * Time.deltaTime;
        SetPosition(movement.x + this.transform.position.x, movement.y + this.transform.position.y);

        if(Vector2.Distance(this.transform.position, destination) < 0.1f) {
            isReturning = false;
        }
    }

    Vector2 SetPosition(float x, float y)
    {
        return this.transform.position = new Vector3(x, y, this.transform.position.z);
    }

    void UpdateOffset() {
        swordRenderer.flipX = false;
        currentOffset = offset;

        if (!playerRender.flipX)
        {
            swordRenderer.flipX = true;
            currentOffset = offset * new Vector2(-1, 1);
        }
    }
}
