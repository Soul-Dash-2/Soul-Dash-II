using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EyeballController : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 20.0f;
    [SerializeField] float shunSpeed = 0.3f;
    [SerializeField] float attackModeDistanceToPlayer = 1.0f;
    [SerializeField] float attackModeMovementSpeed = 5.0f;
    [SerializeField] float attackDelay = 2.0f;
    [SerializeField] float attackTime = 2.0f;
    [SerializeField] float laserMovementSpeed = 0.01f; //this value should be smaller than 0.3f;

    bool attackMode = true;
    bool shunMode = false;
    bool attackReady = false;


    Vector2 shunStartPoint;
    Vector2 shunMidpoint;
    Vector2 shunDestination;

    Vector2 laserCurrPos;
    Vector2 laserDest;
    float arcMovementCounter;
    //assume there will be only one player in the scene

    [SerializeField] private GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];


    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(attackMode +" "+ attackReady+ " " + shunMode);
        if (attackMode)
        {

            TurnTowardsPos(player.transform.position.x,player.transform.position.y);
            moveTowardsPlayer();

        }
        if (shunMode)
        {
            ShunModeController();
            arcMovementTotheOtherSideOfPlayer(shunStartPoint, shunMidpoint, shunDestination);
        }
       /* if (attackReady)
        {
            LaserController();
        } */

    }

    //attack module
    void LaserController()
    {
        Debug.Log(transform.position.x +" "+ transform.position.y);
        if (laserCurrPos.x - laserDest.x >0.5f)
        {
          //  Physics2D.Raycast(transform.position, new Vector2( transform.position.x, transform.position.y) - laserCurrPos);
          //  Debug.DrawRay(transform.position, new Vector2(- transform.position.x, - transform.position.y) + laserCurrPos, Color.red, 10.0f);
          //  laserCurrPos = new Vector2(laserCurrPos.x + laserMovementSpeed, laserCurrPos.y)*Time.deltaTime;

        }
        else
        {
            shunMode = true;
            attackReady = false;
        }
    }



    //movement module
    void arcMovementTotheOtherSideOfPlayer(Vector2 startPoint,Vector2 midPoint, Vector2 dest)
    {

        //calculate the destination of the movement
        //shunDestination = (,transform.position.y+2)
        if (arcMovementCounter < 1.0f)
        {
            arcMovementCounter += 0.3f * Time.deltaTime;

            Vector3 m1 = Vector3.Lerp(startPoint, midPoint, arcMovementCounter);
            Vector3 m2 = Vector3.Lerp(midPoint, dest, arcMovementCounter);
            transform.position = Vector3.Lerp(m1, m2, arcMovementCounter);
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
        Quaternion _lookRotation = Quaternion.Euler(new Vector3(0, 0, GetAngleToPos(posX, posY)));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _lookRotation, rotationSpeed * Time.deltaTime);
       

    }

    //move towards the player
    void moveTowardsPlayer()
    {
        //Vector2.Distance(transform.position, player.transform.position) - attackModeDistanceToPlayer
        if (Vector2.Distance(transform.position, player.transform.position) > attackModeDistanceToPlayer){
            transform.Translate((player.transform.position- transform.position).normalized* attackModeMovementSpeed* Time.deltaTime);
        }
        else
        {
            StartCoroutine( AttackDelayController());
        }
    }

    //mode controllers
    //set the start point, mid point, and destination of the arc movement
    void ShunModeController() {
        shunStartPoint = transform.position;
        shunMidpoint = new Vector2(player.transform.position.x, transform.position.y + 10.0f);
        shunDestination = new Vector2(player.transform.position.x < transform.position.x ?
            2 * player.transform.position.x - transform.position.x - 2.0f :
            2 * transform.position.x - player.transform.position.x + 3.0f,
            transform.position.y + 2.5f);
        attackMode = true;
        shunMode = false;
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
        yield return new WaitForSeconds(attackDelay);
        /* if (player.transform.position.x < transform.position.x)
         {
             laserCurrPos = new Vector2 (player.transform.position.x + attackDistaince, player.transform.position.y);
             laserDest = new Vector2(player.transform.position.x - attackDistaince, player.transform.position.y);
         }
         else
         {
             laserCurrPos = new Vector2(player.transform.position.x - attackDistaince, player.transform.position.y);
             laserDest = new Vector2(player.transform.position.y + attackDistaince, player.transform.position.y);
         }
        */


        float attackTimer = 0f;
        TurnTowardsPos(player.transform.position.x - 1, player.transform.position.y);
        while (attackTimer < attackTime)
        {
            TurnTowardsPos(player.transform.position.x, player.transform.position.y);
            Debug.DrawRay(transform.position, transform.right, Color.red, 10.0f);
            attackTimer += Time.deltaTime;
            yield return null;

        }
        attackMode = false;
        shunMode = true;
       

    }



}
