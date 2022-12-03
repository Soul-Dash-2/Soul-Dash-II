using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UIElements;



public class EyeballController : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 30.0f;
    [SerializeField] float maxShunModeTime = 2f;
    [SerializeField] float attackModeMovementSpeed = 8.0f;
    [SerializeField] float attackDelay = 1.0f;
    [SerializeField] float attackTime = 2.0f;
    [SerializeField] float laserMovementSpeed = 0.01f; //this value should be smaller than 0.3f;
    [SerializeField] float maxDistanceToPlayer = 30f;
    [SerializeField] float minDistanceToPlayer = 20f;
    [SerializeField] float laserBeamLength = 20f;
    [SerializeField] float laserDamagePerFrame = 0.3f;
    [SerializeField] LayerMask myLayer;

    bool attackMode = false;
    bool calMovementMode = false;
    bool shunMode = false;
    bool attackLeft = true;
    bool ableToDealDamage = false;
    float laserhitLeangth = 10000f;
    // Bit shift the index of the layer (8) to get a bit mask
    int layerMask = 1 << 8;

   
    float attackPrepareTimer = 0f;
    private SpriteRenderer _renderer;
    private LineRenderer _lineRenderer;
    Vector2 shunStartPoint;
    Vector2 shunMidpoint;
    Vector2 shunDestination;
    Vector2 attackCurrPos;
    IEnumerator laserController;

    float arcMovementCounter=0f;
    //assume there will be only one player in the scene

    [SerializeField] private GameObject player;

    public bool AttackMode
    {
        get { return attackMode; }
        set { attackMode = value; }
    }
    void Start()
    {

        // This would cast rays only against colliders in layer 8, so we just inverse the mask.
        layerMask = ~layerMask;
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        _renderer = GetComponent<SpriteRenderer>();
        _lineRenderer = transform.Find("LaserBeam").GetComponent<LineRenderer>();
        _lineRenderer.startWidth = 0.6f;
        _lineRenderer.endWidth = 0.3f;
        shunDestination = transform.position;
        shunMidpoint = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(attackMode + " " + calMovementMode + " " + shunMode);
        if (attackMode)
        {
            if (laserController != null) StopCoroutine(laserController);
            TurnTowardsPos(player.transform.position.x,player.transform.position.y);
            if (Vector2.Distance(player.transform.position, transform.position) > minDistanceToPlayer)
            {
                moveTowardsPlayer();
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
        else if (calMovementMode)
        {
            ShunModeController();

        }
        else if(shunMode)
        {
           StartCoroutine( arcMovementTotheOtherSideOfPlayer(shunStartPoint, shunMidpoint, shunDestination));
        }

    }

    //attack module

    //movement module
    IEnumerator arcMovementTotheOtherSideOfPlayer(Vector2 startPoint,Vector2 midPoint, Vector2 dest)
    {

        //calculate the destination of the movement
        //shunDestination = (,transform.position.y+2)

        float distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);
        if (
       ( distanceToPlayer < maxDistanceToPlayer &&
            arcMovementCounter < maxShunModeTime))
            {

                arcMovementCounter += Time.deltaTime;

            //ideally a bezier curve
                Vector3 m1 = Vector2.Lerp(startPoint, midPoint, arcMovementCounter);
                Vector3 m2 = Vector2.Lerp(midPoint, dest, arcMovementCounter);
                TurnTowardsPos(player.transform.position.x,player.transform.position.y);
                transform.position = Vector2.Lerp(m1, m2, arcMovementCounter);
                yield return null;

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
        _renderer.flipY = (transform.rotation.eulerAngles.z <= 100 ||transform.rotation.eulerAngles.z >= 260) ? false : true;
        Quaternion _lookRotation = Quaternion.Euler(new Vector3(0, 0, GetAngleToPos(posX, posY)));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _lookRotation, rotationSpeed * Time.deltaTime);
        

    }

    //move towards the player
    void moveTowardsPlayer()
    {
        //Vector2.Distance(transform.position, player.transform.position) - attackModeDistanceToPlayer
        
            transform.Translate(
               (player.transform.position.x < transform.position.x ?
               (- transform.position + player.transform.position ) :
               (- player.transform.position + transform.position)).normalized
               * attackModeMovementSpeed* Time.deltaTime);
        
    }

    //mode controllers
    //set the start point, mid point, and destination of the arc movement
    void ShunModeController() {
        shunStartPoint = transform.position;

        if ((int) Random.Range(0, 2) ==0){
            
            shunDestination = new Vector2(
                (transform.position.x - player.transform.position.x) > 0 ?
                player.transform.position.x + Random.Range(5f, 14f) :
                player.transform.position.x - Random.Range(5f, 14f),
                (player.transform.position.y + 13f < transform.position.y + 15f) ?
                player.transform.position.y + 13f :
                transform.position.y + 15f
                );

            shunMidpoint = shunStartPoint + (shunDestination - shunStartPoint) / 2;
        }else{
        attackMode = true;
        calMovementMode = false;
        shunMode = false; 
        }

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
            attackCurrPos = new Vector2(player.transform.position.x+5, player.transform.position.y);

        }
        else
        {
            attackLeft = false;
            attackCurrPos = new Vector2(player.transform.position.x-5, player.transform.position.y);
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

           
            StartCoroutine(LaserBeanController());

            attackTimer += Time.deltaTime;
            yield return null;
            

        }

        //reset attackTimer
        attackTimer = 0;
        attackMode = false;
        calMovementMode = true ;
        attackPrepareTimer = 0f;

    }



    IEnumerator LaserBeanController()
    {
        ableToDealDamage = true;
        Vector3 laserDest = new Vector3();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.right.normalized, laserBeamLength, layerMask: myLayer);
        if (hit.collider && this.gameObject.GetComponent<Enemy>().health > 0)
        {
            if (hit.transform.gameObject.CompareTag("Player") || hit.transform.gameObject.CompareTag("Ground"))
            {
                laserDest = hit.point;
                GameObject.Find("SFXManager").GetComponent<SFX_manager>().PlaySound("eyeballLaser");
                if (ableToDealDamage && hit.transform.gameObject.CompareTag("Player"))
                {
                   
                    GameObject.Find("Hero").transform.GetChild(1).GetComponent<EnemyCollider>().TakeDamage(laserDamagePerFrame);
                    ableToDealDamage = false;
                }
            }
        }
        _lineRenderer.SetPosition(0,new Vector3( transform.position.x-0.5f, transform.position.y - 0.5f,0));
        if (laserDest != new Vector3()) 
        _lineRenderer.SetPosition(1, laserDest);
        else
        {
            _lineRenderer.SetPosition(1, transform.position + -transform.right.normalized * laserBeamLength);
        }
        
        yield return new WaitForEndOfFrame();
        _lineRenderer.SetPosition(1, new Vector3(transform.position.x - 0.5f, transform.position.y - 0.5f, 0));

    }


}
