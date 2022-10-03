using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EyeballController : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 30.0f;
    [SerializeField] float shunSpeed = 0.3f;
    [SerializeField] float attackModeDistanceToPlayer = 1.0f;
    [SerializeField] float attackModeMovementSpeed = 8.0f;
    [SerializeField] float attackDelay = 1.0f;
    [SerializeField] float attackTime = 2.0f;
    [SerializeField] float laserMovementSpeed = 0.01f; //this value should be smaller than 0.3f;
    [SerializeField] float shunMaxDistanceToPlayer = 30f;
    [SerializeField] float shunMinDistanceToPlayer = 12f;

    bool attackMode = true;
    bool calMovementMode = false;
    bool shunMode = false;
    bool attackLeft = true;

    float attackPrepareTimer = 0f;
    private SpriteRenderer _renderer;
    Vector2 shunStartPoint;
    Vector2 shunMidpoint;
    Vector2 shunDestination;
    Vector2 attackCurrPos;
    IEnumerator laserController;

    float arcMovementCounter;
    //assume there will be only one player in the scene

    [SerializeField] private GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        arcMovementCounter = 0;
        _renderer = GetComponent<SpriteRenderer>();



    }

    // Update is called once per frame
    void Update()
    {
        if (attackMode)
        {
            if (laserController != null) StopCoroutine(laserController);
            TurnTowardsPos(player.transform.position.x,player.transform.position.y);
            moveTowardsPlayer();

        }
        else if (calMovementMode)
        {
            ShunModeController();

        }
        else if(shunMode)
        {
            arcMovementTotheOtherSideOfPlayer(shunStartPoint, shunMidpoint, shunDestination);
        }
       /* if (attackReady)
        {
            LaserController();
        } */

    }

    //attack module

    //movement module
    void arcMovementTotheOtherSideOfPlayer(Vector2 startPoint,Vector2 midPoint, Vector2 dest)
    {

        //calculate the destination of the movement
        //shunDestination = (,transform.position.y+2)


        if (
        new Vector2 (transform.position.x, transform.position.y) != dest &&
            (Vector2.Distance(player.transform.position, transform.position)< shunMinDistanceToPlayer||
            arcMovementCounter < 3.0f &&
                Vector2.Distance(player.transform.position, transform.position) < shunMaxDistanceToPlayer))
            {
                arcMovementCounter += shunSpeed * Time.deltaTime;

                Vector3 m1 = Vector3.Lerp(startPoint, midPoint, arcMovementCounter);
                Vector3 m2 = Vector3.Lerp(midPoint, dest, arcMovementCounter);
                TurnTowardsPos(Vector3.Lerp(m1, m2, arcMovementCounter).x, Vector3.Lerp(m1, m2, arcMovementCounter).y);
                transform.position = Vector3.Lerp(m1, m2, arcMovementCounter);

            }
            else
            {
                arcMovementCounter = 0;
                attackMode = true;
                calMovementMode = false;
                shunMode = false;
            }
        }

 /*   void avoidObstable()
    {
        RaycastHit2D hit = Physics2D.Raycast (transform.position, player.transform.position - transform.position);

            if (hit.collider != null)
            {
               if (hit.collider.gameObject.tag != "Player")
            {
                transform.Translate(new Vector3(0,1,0) * attackModeMovementSpeed * Time.deltaTime);
            }
            }
        
    }
 */
    //turn to the direction of the player

    void TurnTowardsPos(float posX, float posY)
    {
        Debug.Log(transform.rotation.eulerAngles.z);
        Debug.Log((transform.rotation.eulerAngles.z <= 100 || transform.rotation.eulerAngles.z >= -100));
        _renderer.flipY = (transform.rotation.eulerAngles.z <= 100 ||transform.rotation.eulerAngles.z >= 260) ? false : true;
        Quaternion _lookRotation = Quaternion.Euler(new Vector3(0, 0, GetAngleToPos(posX, posY)));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _lookRotation, rotationSpeed * Time.deltaTime);
        

    }

    //move towards the player
    void moveTowardsPlayer()
    {
        //Vector2.Distance(transform.position, player.transform.position) - attackModeDistanceToPlayer
        if (Vector2.Distance(transform.position, player.transform.position) > attackModeDistanceToPlayer){
            transform.Translate(
               (player.transform.position.x < transform.position.x ?
               (- transform.position + player.transform.position ) :
               (- player.transform.position + transform.position)).normalized
               * attackModeMovementSpeed* Time.deltaTime);
        }
        else
        {
            laserController = AttackDelayController();
            attackMode = false;
            calMovementMode = false;
            shunMode = false;
            StartCoroutine(laserController);

        }
    }

    //mode controllers
    //set the start point, mid point, and destination of the arc movement
    void ShunModeController() {
        shunStartPoint = transform.position;
        shunMidpoint = new Vector2(player.transform.position.x, transform.position.y + 3f);
        shunDestination = new Vector2(
            2 * player.transform.position.x - transform.position.x,
            transform.position.y + 8f);

        attackMode = false;
        calMovementMode = false;
        shunMode = true;
    }
    //utility functions
    float GetAngleToPos(float posX, float posY)
    {
        float angle = Mathf.Atan2( transform.position.y - posY, transform.position.x - posX) * Mathf.Rad2Deg;
        return angle;
    }


    //atack timer
    IEnumerator AttackDelayController()
    {



        float attackTimer = 0f;
        //pre-attack, turn the right or left of the plyer
        if (transform.position.x > player.transform.position.x)
        {
            attackLeft = true;
            attackCurrPos = new Vector2(player.transform.position.x + 1, player.transform.position.y);

        }
        else
        {
            attackLeft = false;
            attackCurrPos = new Vector2(player.transform.position.x - 1, player.transform.position.y);
        }
        while (attackPrepareTimer < attackDelay)
        {
            TurnTowardsPos(attackCurrPos.x, attackCurrPos.y);
            attackPrepareTimer += Time.deltaTime;
            yield return null;
        }

        //start the laser attack
        while (attackTimer < attackTime)
        {

            TurnTowardsPos(attackCurrPos.x, attackCurrPos.y);
            if (attackLeft) attackCurrPos.x -= laserMovementSpeed;
            else attackCurrPos.x += laserMovementSpeed;

            Debug.DrawRay(transform.position, -transform.right*20, Color.red, 2.0f, false);

            attackTimer += Time.deltaTime;
            yield return null;
            

        }
        //reset attackTimer
        attackTimer = 0;
        attackMode = false;
        calMovementMode = true ;
        attackPrepareTimer = 0f;

    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        if(shunDestination!=null)
        Gizmos.DrawSphere(shunDestination, 1);
    }



}
