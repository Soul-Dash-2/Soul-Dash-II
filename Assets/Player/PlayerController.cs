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

    // Private
    private bool isGrounded;            // is the player touching a ground object?
    private float movementFactor;       // a value between -1 and 1, which determines the direction the player moves
    private bool canDash;               // true if the player is allowed to dash
    private bool isDashing;             // true while the player is dashing
    private bool isJumping;             // true after the player has executed a jump
    private bool isCrouching;           // true when player is crouching
    private bool isFastFalling;         // true while player is fastfalling
    private bool isTouching;            // true while player is touching anything

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
    public float basicDashVelocity; // How quickly does the basic dash move
    public float slimeDashVelocity; // How quickly the slime dash moves
    public float dashTrajectoryModificationFactor;  /* How much does the effect of the players velocity before dashing affect the angle of the dash?
                                                        EXAMPLE: IF the factor is large, then if the player jumps before they dash horizontally, the
                                                        dash will actually move the player up as well. This can be set to 0 to disable this mechanic
                                                        altogether.*/

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
        // controls.Player.Slash.started += _ => Slash();
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
    bool CanDash()
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

    // Get the direction for a dash based on the mouse location
    Vector2 GetDirection()
    {
        Vector2 pos = rb.position;
        Vector2 mouse = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        return (mouse - pos).normalized;
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

    //Slime dash
    IEnumerator SlimeDash()
    {
        // Perform the movement
        Vector2 direction = GetDirection();
        Vector2 slimeDashDir = direction; //Copies the direction to the slime-dash/bounce to swap it after 
        slimeDashDir.x = -slimeDashDir.x; //Reflects the x direction for the bounce always
        if (direction.y < 0) //Checks if the initial dash is a dash going down
        {
            slimeDashDir.y = -slimeDashDir.y; //If going down, the bounce goes up
        }

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
            //add in explosion at end of first dash hitbox
            //Bounce dash
            rb.velocity = (slimeDashDir * slimeDashVelocity) + (rb.velocity * dashTrajectoryModificationFactor);
            // wait to complete dash
            dashTime = 0;
            while (dashTime < basicDashTime)
            {
                dashTime += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            //add in explosion at end of dash hitbox 
        }
        // finish dash
        rb.gravityScale = gravityScale;
        isDashing = false;
    }
}
