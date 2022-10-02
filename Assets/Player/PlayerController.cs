using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public enum DashType
    {
        BASIC, SLIME, GOBLIN, EYE, EAGLE, SANDWORM, EYEBALL, DEMON
    }

    // Components
    private Rigidbody2D rb;             // the 2d rigid body
    private PlayerControls controls;    // the input system
    private SpriteRenderer render;      // the sprite renderer
    private Camera playerCamera;        // The camera following the player

    //Prefabs
    public GameObject slash;            //The slash prefab

    // Private
    private bool isGrounded;            // is the player touching a ground object?
    private float movementFactor;       // a value between -1 and 1, which determines the direction the player moves
    private bool canDash;               // true if the player is allowed to dash
    private bool isDashing;             // true while the player is dashing
    private bool isJumping;             // true after the player has executed a jump
    private bool isCrouching;           // true when player is crouching
    private bool isFastFalling;         // true while player is fastfalling
    private bool isTouching;            // true while player is touching anything

    //Damage things
    public float dashDamage;

    // Public Movement variables
    public DashType dashType;
    public float jumpVelocity;      // How much power the player's jump has
    public float gravityScale;      // How strong the effect of gravity on the player is: 1 = 100%, 0 = 0%
    public float shortHopEndVel;    // The velocity threshold at which a shorthop is said to be complete
    public float shortHopStrength;  // Higher numbers mean the short hop should be shorter

    public float accelerationX;     // the player's maximum acceleration on the x axis
    public float maxVelocityX;      // the player's maximum velocity on the x axis
    public float minimumVelocity;

    public float percentFrictionX;  // % friction: how quickly does the player come to a halt (in the x direction)
    public float percentFrictionY;

    public float basicDashTime;     // How long the dash lasts
    public float slimeDashTime;     // How long the slime dash lasts
    public float basicDashVelocity; // How quickly does the basic dash move
    public float slimeDashVelocity; // How quickly the slime dash moves
    public float dashTrajectoryModificationFactor;  /* How much does the effect of the players velocity before dashing affect the angle of the dash?
                                                        EXAMPLE: IF the factor is large, then if the player jumps before they dash horizontally, the
                                                        dash will actually move the player up as well. This can be set to 0 to disable this mechanic
                                                        altogether.*/
    public Vector2 slimeDashDirection;

    // Setup Code
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;

        render = GetComponent<SpriteRenderer>();
        playerCamera = GetComponentInChildren<Camera>();

        controls = new PlayerControls();
        controls.Enable();

        // See the PlayerControls object in the assets folder to view which keybinds activate the different movement events
        controls.Player.Movement.performed += ctx => movementFactor = ctx.ReadValue<float>(); // sets movement to the float value of the performed action
        controls.Player.Movement.canceled += _ => movementFactor = 0; // sets movement to the float value of the performed action
        controls.Player.Jump.performed += _ => Jump();
        controls.Player.Jump.canceled += _ => EndJump();
        controls.Player.Crouch.performed += _ => Crouch();
        controls.Player.Crouch.canceled += _ => EndCrouch();
        controls.Player.Dash.started += _ => Dash();
        controls.Player.Slash.started += _ => Slash();
    }

    // Fixed Update occurs whenever unity updates Physics objects, and does not necessarily occur at the same time as the frame update.
    // DeltaTime is not used, because the physics update occurs on a fixed interval, and is therefore not bound to the deltaTime
    void FixedUpdate()
    {
        float xVel = CalculateXVelocity();
        float yVel = CalculateYVelocity();
        rb.velocity = new Vector2(xVel, yVel);
        
        HandleShortHop(yVel);
        DoFlipIfNeeded(xVel);
    }

    // Enable and Disable events
    void onEnable() => controls.Enable();
    void onDisable() => controls.Disable();

    // Event which occurs when leaving a colliding object
    void OnCollisionExit2D(Collision2D collision)
    {
        isTouching = false;
        // if the previously collided object was the ground, set isGrounded false
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    // Event which occurs when entering a collision
    void OnCollisionEnter2D(Collision2D collision)
    {
        // if the previously collided object was the ground, set isGrounded false
        if (collision.gameObject.CompareTag("Ground") && collision.relativeVelocity.y > 0)
        {
            isGrounded = true;
            canDash = true;
            isJumping = false;
            isFastFalling = false;
            isCrouching = false;
            isTouching = true;
        }

        //Ignore the stuff below here, maybe make a trigger child to the player (or enemy)
        if (collision.gameObject.CompareTag("Enemy") && isDashing)
        {
            collision.gameObject.GetComponent<Enemy>().playerDamage(3);
        }
        else if (collision.gameObject.CompareTag("Enemy") && !isDashing)
        {
            //deal damage to player
        }
    }

    // Disables shorthop when the condition is met
    private void HandleShortHop(float yVel)
    {
        if (yVel < shortHopEndVel)
        {
            rb.gravityScale = gravityScale;
        }
    }

    // Flip the sprite renderer if necessary
    private void DoFlipIfNeeded(float xVel)
    {
        if (xVel > 0)
        {
            render.flipX = true;
            return;
        }
        if (xVel < 0)
        {
            render.flipX = false;
            return;
        }
    }

    // Gets the friction factor -- multiplied with the xVelocity 
    float GetFrictionFactorX()
    {
        // if accelerating in the same direction as velocity, don't count friction
        if ((Mathf.Ceil(Mathf.Clamp(movementFactor, -1, 1)) == Mathf.Ceil(Mathf.Clamp(rb.velocity.x, -1, 1))) || isDashing)
        {  // FIXME: this will likely NOT work on controller
            return 1;
        }
        return 1 - (percentFrictionX / 100);
    }

    float GetFrictionFactorY()
    {
        // if dashing, jumping, or falling, do not apply friction
        if (isDashing || isJumping || rb.velocity.y < 0)
        {
            if (isFastFalling && rb.velocity.y < 0) //Fastfalling in air increases y friction
            {
                return 1.5f;
            }
            return 1;
        }
        return 1 - (percentFrictionY / 100);
    }


    // Calculate X Velocity
    float CalculateXVelocity()
    {
        float newVel = (rb.velocity.x + (movementFactor * accelerationX)) * GetFrictionFactorX();        // FIXME: this does not account for controller deadzone
        // If the player is dashing, no need to clamp velocity
        if (isDashing)
        {
            return newVel;
        }
        // Set velocity to zero when velocity is small
        if (Mathf.Abs(newVel) < minimumVelocity)
        {
            return 0;
        }

        return Mathf.Clamp(newVel, -maxVelocityX, maxVelocityX);
    }

    float CalculateYVelocity()
    {
        return rb.velocity.y * GetFrictionFactorY();
    }

    // Jump event
    void Jump()
    {
        // Can only jump if on the ground
        if (isGrounded)// && !isDashing)
        {
            isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
        }
    }

    // Endjump --> allows for short hops
    void EndJump()
    {
        // if the shortHopEndvelocity has already been met, then no short hop will occur
        if (rb.velocity.y < shortHopEndVel)
        {
            return;
        }
        rb.gravityScale = gravityScale * shortHopStrength;
    }

    //Crouch (or fastfall)
    void Crouch()
    {
        if (isGrounded)
        {
            //Add in crouch hitbox
            //Maybe slow the player?
            isCrouching = true;
        }
        else
        {
            //Note isFastFalling increases y friction value to 1.5 
            isFastFalling = true;
        }
    }

    //EndCrouch (or endfastfall)
    void EndCrouch()
    {
        if (isCrouching)
        {
            //Add in crouch hitbox
            //Maybe speed back up the player?
            isCrouching = false;
        }
        else if (isFastFalling)
        {
            isFastFalling = false;
        }
    }

    // Whether or not the player is allowed to dash -- prefer this method over the boolean canDash
    public bool CanDash()
    {
        return canDash || isGrounded;
    }

    // Dash event --> executes the various dash type coroutines
    void Dash()
    {
        if (!CanDash())
        {
            return;
        }
        isJumping = false;
        if (dashType == DashType.BASIC)
        {
            StartCoroutine(BasicDash());
        }
        else if (dashType == DashType.SLIME)
        {
            StartCoroutine(SlimeDash());
        }
    }

    //Slashing
    void Slash()
    {
        if (!isDashing) //so player cannot slash while dashing
        {
            Vector2 relativeDirection = GetDirection();
            Vector2 pos = rb.position;
            Vector2 slashLocation = 2*relativeDirection + pos; //So its 3x further away from the player
            Instantiate(slash, slashLocation, Quaternion.identity); //TODO: Make the slash change rotation based on mouse
        }
    }

    // Get the direction for a dash based on the mouse location
    Vector2 GetDirection()
    {
        Vector2 pos = rb.position;
        Vector2 mouse = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        return (mouse - pos).normalized;
    }

    //DASHES BELOW HERE
    //

    public bool getDashing()
    {
        return isDashing;
    }

    public void letDash()
    {
        canDash = true;
    }

    public void setDashType(string dash)
    {
        Debug.Log("setdashtype called");
        if (dash.Equals("slime"))
        {
            dashType = DashType.SLIME;
            Debug.Log("gave the player slime dash");
        }
        else
        {
            //room for other dashes
        }
        return;
    }

    public DashType getDashType()
    {
        return dashType;
    }

    //Returns the dashType as an int - used in dashIconUpdates
    public int getDashTypeAsInt()
    {
        switch(dashType)
        {
            case DashType.BASIC:
                return 0;
                break;
            case DashType.SLIME:
                return 1;
                break;
            case DashType.GOBLIN:
                return 2;
                break;
            case DashType.EYE:
                return 3;
                break;
            case DashType.EAGLE:
                return 4;
                break;
            case DashType.SANDWORM:
                return 5;
                break;
            case DashType.EYEBALL:
                return 6;
                break;
            case DashType.DEMON:
                return 7;
                break;
            default:
                return -1;
                break;
        }
    }

    // Basic dash
    IEnumerator BasicDash()
    {
        // Perform the movement
        Vector2 direction = GetDirection();
        rb.velocity = (direction * basicDashVelocity) + (rb.velocity * dashTrajectoryModificationFactor);

        // set appropriate variables
        isDashing = true;
        canDash = false;
        rb.gravityScale = 0;

        // wait to complete dash
        float dashTime = 0;
        while (dashTime < basicDashTime)
        {
            dashTime += Time.deltaTime;
            yield return null;
        }
        // finish dash
        rb.gravityScale = gravityScale;
        isDashing = false;
    }

    //Slime dash methods
    IEnumerator SlimeDash()
    {
        // Perform the movement of basic dash
        Vector2 direction = GetDirection();
        setSlimeDashDir(direction); //Sets the slime dash in the opposite direction 

        rb.velocity = (direction * basicDashVelocity) + (rb.velocity * dashTrajectoryModificationFactor);

        // set appropriate variables
        isDashing = true;
        canDash = false;
        rb.gravityScale = 0;

        // wait to complete dash
        float dashTime = 0;
        while (dashTime < basicDashTime)
        {
            dashTime += Time.deltaTime;
            yield return null;
        }

        if (isTouching) //checks if the player hits something at the end of the dash 
        {
            slimeBounce();
        }
        else
        {
            
        }
        // finish dash
        rb.gravityScale = gravityScale;
        isDashing = false;
        dashType = DashType.BASIC;
    }

    //Helper method to set the slime dash direction
    public void setSlimeDashDir(Vector2 direction)
    {
        slimeDashDirection = direction; //Copies the direction to the slime-dash/bounce to swap it after 
        slimeDashDirection.x = -slimeDashDirection.x; //Reflects the x direction for the bounce always
        if (direction.y < 0) //Checks if the initial dash is a dash going down
        {
            slimeDashDirection.y = -slimeDashDirection.y; //If going down, the bounce goes up
        }
        return;
    }

    //The bounce from the slime dash
    public IEnumerator SlimeBounce()
    {
        //add in explosion at end of dash hitbox 
        rb.velocity = (slimeDashDirection * slimeDashVelocity);
        Debug.Log("Slime bounce velocity: " + rb.velocity);
        // wait to complete dash
        float slimeDashTime = 0;
        slimeDashTime = 0;
        while (slimeDashTime < basicDashTime)
        {
            slimeDashTime += Time.deltaTime;
            yield return null;
        }
        rb.gravityScale = gravityScale;
        isDashing = false;
        dashType = DashType.BASIC;
    }

    public void slimeBounce()
    {
        StartCoroutine(SlimeBounce());
    }
}
