using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour
{

    public enum DashType
    {
        BASIC, SLIME, GOBLIN, SANDWORM, EYEBALL, DEMON
    }

    // Components
    private Rigidbody2D rb;             // the 2d rigid body
    private PlayerControls controls;    // the input system
    private SpriteRenderer render;      // the sprite renderer
    private Camera playerCamera;        // The camera following the player
    private Animator _animator;
    private GameObject SFXManager;  //The manager for playing sound effect
    public PlayerOnGround groundDetector;
    private GameObject enemyCollider;  //enemyCollider object

    // Prefabs
    public GameObject slash;            //The slash prefab
    public GameObject swordPrefab;            // The Sword prefab
    //public GameObject sandwormExplosionn; //explosion prefab

    // Combat
    public float attackRange;
    private Sword sword;

    // Private
    private bool isGrounded;            // is the player touching a ground object?
    private float movementFactor;       // a value between -1 and 1, which determines the direction the player moves
    private bool canDash;               // true if the player is allowed to dash
    public bool isDashing;             // true while the player is dashing
    private bool isJumping;             // true after the player has executed a jump
    private bool isCrouching;           // true when player is crouching
    private bool isFastFalling;         // true while player is fastfalling
    private bool isTouching;            // true while player is touching anything
    private bool canGlide;              // true while the player has the ability to glide --> granted by eyeball dash
    private bool isTouchingWallGround;   // true while player is touching the wall or the ground
    private bool isInvisible;           // true while player is invisible (after goblin dash)
    private float coyoteCounter;        //Timer for coyote time
    private float timeSinceDash;
    private float timeSinceSlash;
    private float justTookDamgage;
    private float justDied;
    private float timeSinceKnockback;
    private float cameraSize;

    // Materials
    private Material norm;
    public Material white;


    //Damage things
    public float dashDamage;
    public float flashTime;

    // Public Movement variables
    public DashType dashType;
    public float jumpVelocity;      // How much power the player's jump has
    public float gravityScale;      // How strong the effect of gravity on the player is: 1 = 100%, 0 = 0%
    public float maxFallSpeed;      // Maximum (minimum) speed that the player can be falling
    private float fallSpeed;         // current max fall speed
    public float fastFallFac;
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
    public float demonDashDistance; // How far the player teleports when they have the demon dash
    public float glideFactor;       // How much weaker is gravity while gliding
    public float dashTrajectoryModificationFactor;  /* How much does the effect of the players velocity before dashing affect the angle of the dash?
                                                        EXAMPLE: IF the factor is large, then if the player jumps before they dash horizontally, the
                                                        dash will actually move the player up as well. This can be set to 0 to disable this mechanic
                                                        altogether.*/
    public Vector2 slimeDashDirection;
    public float coyoteTime;  //How long can the player be off of a platform and still be able to jump
    public float dashCooldown;
    public float slashCooldown;

    public bool movingLeft; //Tracks if player is moving left
    public bool movingRight; //Tracks if player is moving right



    //move the player to respawn point
    private void Awake()
    {
        GameObject LC = GameObject.Find("LevelController");
        if (LC.GetComponent<LevelController>().respawnPoint != new Vector3())
            gameObject.transform.position = LC.GetComponent<LevelController>().respawnPoint;
    }

    // Setup Code
    void Start()
    {
        enemyCollider = GameObject.Find("EnemyCollider");
        fallSpeed = maxFallSpeed;
        sword = CreateSword();

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;

        render = GetComponent<SpriteRenderer>();
        playerCamera = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
        cameraSize = playerCamera.orthographicSize;

        controls = new PlayerControls();
        controls.Enable();

        _animator = GetComponent<Animator>();
        norm = GetComponent<SpriteRenderer>().material;

        // See the PlayerControls object in the assets folder to view which keybinds activate the different movement events
        controls.Player.Movement.performed += ctx => movementFactor = ctx.ReadValue<float>(); // sets movement to the float value of the performed action
        controls.Player.Movement.canceled += _ => movementFactor = 0; // sets movement to the float value of the performed action
        controls.Player.Jump.performed += _ => Jump();
        controls.Player.Jump.canceled += _ => EndJump();
        controls.Player.Crouch.performed += _ => Crouch();
        controls.Player.Crouch.canceled += _ => EndCrouch();
        controls.Player.Dash.started += _ => Dash();
        controls.Player.Slash.started += _ => Slash();
        controls.Player.Glide.performed += _ => Glide();
        controls.Player.Glide.canceled += _ => EndGlide();
        controls.Player.ZoomOut.started += _ => ZoomOut();
        controls.Player.ZoomOut.canceled += _ => ZoomIn();

        //initilize SFX manager
        SFXManager = GameObject.Find("SFXManager");

    }


    Sword CreateSword()
    {
        GameObject swordInstance = Instantiate(swordPrefab);
        Sword sword = swordInstance.GetComponent<Sword>();
        sword.Setup(this.gameObject);
        return sword;
    }

    //Update method to take advantage of deltaTime, used for coyote time mechanic
    private void Update()
    {
        if(enemyCollider.GetComponent<EnemyCollider>().justDied == true)
        {
            _animator.SetBool("isDying", true);
            Debug.Log("player controller sees death");
            rb.velocity = new Vector2(0, 0); //stops movement after death
        }

        if (enemyCollider.GetComponent<EnemyCollider>().justTookDamage == true)
        {
            _animator.SetBool("isTakingDamage", true);
        }
        else if (enemyCollider.GetComponent<EnemyCollider>().justTookDamage == false)
        {
            _animator.SetBool("isTakingDamage", false);
        }

        timeSinceKnockback += Time.deltaTime;
        timeSinceDash += Time.deltaTime;
        timeSinceSlash += Time.deltaTime;
        if (onGround())
        {
            coyoteCounter = coyoteTime;
        }
        else {
            coyoteCounter -= Time.deltaTime;
        }

    }

    // Fixed Update occurs whenever unity updates Physics objects, and does not necessarily occur at the same time as the frame update.
    // DeltaTime is not used, because the physics update occurs on a fixed interval, and is therefore not bound to the deltaTime
    void FixedUpdate()
    {
        float xVel = CalculateXVelocity();
        float yVel = CalculateYVelocity();
        rb.velocity = new Vector2(xVel, yVel);
        if (enemyCollider.GetComponent<EnemyCollider>().justDied == true)
        {
            rb.velocity = new Vector2(0, 0); //stops movement after death
        }
        HandleShortHop(yVel);
        DoFlipIfNeeded(xVel);
        //CheckDashingAnimation();
        CheckWalkingAnimation();
        CheckFallingAnimation();
    }

    // Enable and Disable events
    public void onEnable() => controls.Enable();
    public void onDisable() => controls.Disable();

    // Event which occurs when leaving a colliding object
    void OnCollisionExit2D(Collision2D collision)
    {
        isTouching = false;
        isTouchingWallGround = false;
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
        if (groundDetector.OnGround())
        {
            isGrounded = true;
            canGlide = false;
            canDash = true;
            isJumping = false;
            isFastFalling = false;
            fallSpeed = maxFallSpeed;
            isCrouching = false;
            isTouching = true;
            _animator.SetBool("isFalling", false);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            isTouchingWallGround = true;
        }

        //Ignore the stuff below here, maybe make a trigger child to the player (or enemy)
        if (collision.gameObject.CompareTag("Enemy") && isDashing)
        {
           
            collision.gameObject.GetComponent<Enemy>().playerDamage(3, this.gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy") && !isDashing)
        {
            //deal damage to player
        }
    }

    // Disables shorthop when the condition is met
    private void HandleShortHop(float yVel)
    {
        if (yVel < shortHopEndVel && !canGlide)
        {
            rb.gravityScale = gravityScale;
        }
    }

    // Flip the sprite renderer if necessary
    private void DoFlipIfNeeded(float xVel)
    {
        if (xVel > 0)
        {
            movingLeft = false;
            movingRight = true;
            render.flipX = true;
            return;
        }
        if (xVel < 0)
        {
            movingRight = false;
            movingLeft = true;
            render.flipX = false;
            return;
        }
    }

    public PlayerCamera GetPlayerCamera() {
        return this.playerCamera.GetComponent<PlayerCamera>();
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
        float yVel = rb.velocity.y * GetFrictionFactorY();
        if (yVel < fallSpeed && !isDashing) {
            _animator.SetBool("isFalling", true);
            return fallSpeed;
        }
        return yVel;
    }

    // Jump event
    void Jump()
    {
        if (rb == null) {
            return;
        }
        // Can only jump if on the ground
        if (onGround() | coyoteCounter > 0f)// && !isDashing)
        {
            isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
        }
    }

    // Endjump --> allows for short hops
    void EndJump()
    {
        coyoteCounter = 0f;
        if (rb == null) {
            return;
        }
        // if the shortHopEndvelocity has already been met, then no short hop will occur
        if (rb.velocity.y < shortHopEndVel || canGlide)
        {
            return;
        }
        rb.gravityScale = gravityScale * shortHopStrength;
    }

    //Crouch (or fastfall)
    void Crouch()
    {
        if (rb == null) {
            return;
        }
        if (onGround())
        {
            //Add in crouch hitbox
            //Maybe slow the player?
            isCrouching = true;
        }
        else
        {
            //Note isFastFalling increases y friction value to 1.5 
            isFastFalling = true;
            fallSpeed = maxFallSpeed * fastFallFac;
        }
    }

    //EndCrouch (or endfastfall)
    void EndCrouch()
    {
        if (rb == null) {
            return;
        }
        if (isCrouching)
        {
            //Add in crouch hitbox
            //Maybe speed back up the player?
            isCrouching = false;
        }
        else if (isFastFalling)
        {
            fallSpeed = maxFallSpeed;
            isFastFalling = false;
        }
    }

    private bool isZoomingOut;
    void ZoomOut() {
        if(playerCamera == null) {
            return;
        }
        isZoomingOut = true;
        StartCoroutine(zoomOut());
    }

    void ZoomIn() {
        if(playerCamera == null) {
            return;
        }
        isZoomingOut = false;
        StartCoroutine(zoomIn());
    }

    private float zoomOutTime = 0.33f;
    private IEnumerator zoomOut() {
        float totalTime = 0;
        float startSize = playerCamera.orthographicSize;
        float goalSize = cameraSize * 2;
        float change = goalSize - startSize;

        while(totalTime < zoomOutTime && isZoomingOut) {
            totalTime += Time.deltaTime;
            if (totalTime > zoomOutTime) {
                totalTime = zoomOutTime;
            }
            playerCamera.orthographicSize = startSize + (change * ((-1 * Mathf.Pow((totalTime / zoomOutTime) - 1, 2)) + 1));
            yield return null;
        }
        if (isZoomingOut) {
            playerCamera.orthographicSize = cameraSize * 2;
        }
    }

    private float zoomInTime = 0.33f;
    private IEnumerator zoomIn() {
        float totalTime = 0;
        float startSize = playerCamera.orthographicSize;
        float goalSize = cameraSize;
        float change = goalSize - startSize;

        while(totalTime < zoomInTime && !isZoomingOut) {
            totalTime += Time.deltaTime;
            if (totalTime > zoomInTime) {
                totalTime = zoomInTime;
            }
            playerCamera.orthographicSize = startSize + (change * ((-1 * Mathf.Pow((totalTime / zoomInTime) - 1, 2)) + 1));
            yield return null;
        }
        if (!isZoomingOut) {
            playerCamera.orthographicSize = cameraSize;
        }
    }

    public void Knockback(GameObject source, float force) {
        if(timeSinceKnockback < 0.4f) {
            return;
        }
        timeSinceKnockback = 0;
        float knockbackAmp = force;
        Vector2 sourceLoc = source.transform.position;
        Vector2 playerLoc = this.transform.position;

        Vector2 knockbackDirection = (playerLoc - sourceLoc);
        knockbackDirection.Normalize();

        rb.velocity = (knockbackDirection * knockbackAmp);
    }

    // Whether or not the player is allowed to dash -- prefer this method over the boolean canDash
    public bool CanDash()
    {
        return (canDash || onGround()) && timeSinceDash >= dashCooldown;
    }

    // replaces uses of 'isGrounded' to check for feet on the floor.
    public bool onGround() {
        return groundDetector.OnGround();
    }

    // Dash event --> executes the various dash type coroutines
    void Dash()
    {
        if (rb == null) {
            return;
        }

        // check if dashing is possible
        if (!CanDash())
        {
            return;
        }

        // Execute correct dash type
        timeSinceDash = 0;
        isJumping = false;
        if (dashType == DashType.BASIC)
        {

            SFXManager.SendMessage("PlaySound", "basicDash");
            StartCoroutine(BasicDash());
            
        }
        else if (dashType == DashType.SLIME)
        {
            StartCoroutine(SlimeDash());
        }
        else if (dashType == DashType.EYEBALL)
        {
            StartCoroutine(EyeballDash());
        }
        else if (dashType == DashType.DEMON)
        {
            DemonDash();
        }
        else if (dashType == DashType.GOBLIN)
        {
            StartCoroutine(GoblinDash());
        }
        else if (dashType == DashType.SANDWORM)
        {
            StartCoroutine(SandwormDash());
        }
    }

    //Slashing
    void Slash()
    {
        if (rb == null || timeSinceSlash < slashCooldown) {
            return;
        }
        if (!isDashing) //so player cannot slash while dashing
        {
            Vector2 relativeDirection = GetDirection();
            Vector2 pos = rb.position;
            Vector2 slashLocation = attackRange * relativeDirection + pos; //So its 3x further away from the player

            SFXManager.SendMessage("PlaySound", "slash");
            float angle = Vector3.Angle(relativeDirection, Vector3.right);
            if (relativeDirection.y < 0) {
                angle = -angle;
            }

            Instantiate(slash, slashLocation, Quaternion.Euler(0, 0, angle)); //TODO: Make the slash change rotation based on mouse
            sword.Attack(slashLocation);
            Vector3.Angle(relativeDirection, Vector3.right);
            timeSinceSlash = 0;
        }
    }

    // Reduces the strength of gravity on the player --> if we wanted to get fancy we could also lock the movement factor to something?
    void Glide()
    {
        if (rb == null) {
            return;
        }
        if (!canGlide)
        {
            return;
        }
        rb.gravityScale = gravityScale * glideFactor;
        rb.velocity = new Vector2(rb.velocity.x, 0);
    }

    // Simply restores the gravity scale
    void EndGlide()
    {
        if (rb == null) {
            return;
        }
        if (!canGlide)
        {
            return;
        }
        canGlide = false;
        rb.gravityScale = gravityScale;
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
        timeSinceDash = dashCooldown;
    }

    public void setDashType(string dash)
    {
        Debug.Log("setdashtype called");
        if (dash.Equals("slime"))
        {
            dashType = DashType.SLIME;
            Debug.Log("gave the player slime dash");
        }
        else if (dash.Equals("eyeball"))
        {
            dashType = DashType.EYEBALL;
            Debug.Log("gave the player eyeball dash");
        }
        else if (dash.Equals("demon"))
        {
            dashType = DashType.DEMON;
            Debug.Log("gave the player demon dash");
        }else if (dash.Equals("goblin"))
        {
            dashType = DashType.GOBLIN;
            Debug.Log("gave the player goblin dash");
        }else if (dash.Equals("sandworm"))
        {
            dashType = DashType.SANDWORM;
            Debug.Log("gave the player sandworm dash");
        }
        return;
    }

    public DashType getDashType()
    {
        return dashType;
    }

    public DashType getDashTypeForHud()
    {
        if (canGlide)
        {
            return DashType.EYEBALL;
        }
        return dashType;
    }

    public bool getInvisible()
    {
        return isInvisible;
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
        CheckDashingAnimation();

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
        CheckDashingAnimation();
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
        CheckDashingAnimation();
        // wait to complete dash
        float dashTime = 0;
        while (dashTime < basicDashTime)
        {
            dashTime += Time.deltaTime;
            yield return null;
        }

        if (isTouchingWallGround) //checks if the player hits something at the end of the dash 
        {
            Debug.Log("player slime dashing into wall or floor");
            slimeBounce();
        }
        else
        {

        }
        // finish dash
        rb.gravityScale = gravityScale;
        isDashing = false;
        dashType = DashType.BASIC;
        CheckDashingAnimation();
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
        // finish dash
        rb.gravityScale = gravityScale;
        isDashing = false;
        dashType = DashType.BASIC;
    }

    public void slimeBounce()
    {
        StartCoroutine(SlimeBounce());
    }

    private float flashCurrTime;
    public void FlashTime() {
        flashCurrTime = 0;
        StartCoroutine(flashtime());
    }
    private IEnumerator flashtime() {
        while (flashCurrTime <= flashTime) {
            float x = flashCurrTime / flashTime;    // between 0 and 1
            flashCurrTime += Time.unscaledDeltaTime;
            Time.timeScale = (2 * Mathf.Pow((x - 0.5f), 2)) + 0.5f;
            yield return null;
        }
        Time.timeScale = 1;
    }

    public float GetFlashTime() {
        return this.flashTime;
    }

    public void Flash() {
        StartCoroutine(flash());
    }

    private IEnumerator flash() {
        GetComponent<SpriteRenderer>().material = white;
        yield return new WaitForSecondsRealtime(flashTime);
        GetComponent<SpriteRenderer>().material = norm;
        yield return null;
    }

    IEnumerator EyeballDash()
    {
        // Perform the movement
        Vector2 direction = GetDirection();
        rb.velocity = (direction * basicDashVelocity) + (rb.velocity * dashTrajectoryModificationFactor);

        // set appropriate variables
        isDashing = true;
        canDash = false;
        rb.gravityScale = 0;
        CheckDashingAnimation();
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

        // This section is all that is unique to the eyeball dash
        canDash = true;
        timeSinceDash = dashCooldown;
        dashType = DashType.BASIC;
        canGlide = true;
        CheckDashingAnimation();
    }

    void DemonDash()
    {
        Vector2 direction = GetDirection();
        Vector3 newPosition = new Vector3(
            rb.transform.position.x + direction.x * demonDashDistance,
            rb.transform.position.y + direction.y * demonDashDistance,
            rb.transform.position.z);

        canDash = false;
        rb.transform.position = newPosition;
        rb.velocity = (direction * basicDashVelocity) + (rb.velocity * dashTrajectoryModificationFactor);

        // TODO: AOE explosion

        dashType = DashType.BASIC;
    }
    IEnumerator GoblinDash()
    {
        // Perform the movement
        Vector2 direction = GetDirection();
        rb.velocity = (direction * basicDashVelocity) + (rb.velocity * dashTrajectoryModificationFactor);

        // set appropriate variables
        isDashing = true;
        canDash = false;
        rb.gravityScale = 0;
        CheckDashingAnimation();
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
        CheckDashingAnimation();
        //invisiblity part
        float invisbleTime = 0;
        isInvisible = true;
        dashType = DashType.BASIC;
        render.color = new Color(render.color.r, render.color.g, render.color.b, render.color.a == 0.01f ? 0.05f : 0.01f); //Color changes to be the same as the goblin ones
        while (invisbleTime < 1.5) //1.5 seconds as of now
        {  
            invisbleTime += Time.deltaTime;
            yield return null;
        }
        isInvisible = false;
        render.color = new Color(render.color.r, render.color.g, render.color.b, 1); //Color changes to be the same as the goblin ones new Color
    }

    IEnumerator SandwormDash()
    {
        // Perform the movement
        Vector2 direction = GetDirection();
        rb.velocity = (direction * basicDashVelocity) + (rb.velocity * dashTrajectoryModificationFactor);

        // set appropriate variables
        isDashing = true;
        canDash = false;
        rb.gravityScale = 0;
        CheckDashingAnimation();

        // wait to complete dash
        float dashTime = 0;
        while (dashTime < basicDashTime)
        {
            dashTime += Time.deltaTime;
            yield return null;
        }
        // finish dash
        dashType = DashType.BASIC;
        rb.gravityScale = gravityScale;
        isDashing = false;
        CheckDashingAnimation();
    }

    /*public void sandwormExplosion()
    {
        Vector2 pos = rb.position;
        Instantiate(sandwormExplosionn, pos, Quaternion.Euler(0, 0, 0));
    }*/

    public void CheckDashingAnimation()
    {
        if (isDashing)
        {
            if(rb.velocity.y< 0 && !onGround())
            {
               _animator.SetBool("isDashingDown", true);
            }
            else
            {
               _animator.SetBool("isDashingSide", true);
            }
        }else if (!isDashing)
        {
            StartCoroutine(dashAnimationCancelTimer());
        }
    }

    public void CheckWalkingAnimation()
    {
        float xVel = CalculateXVelocity();
        float yVel = CalculateYVelocity();
        if (!isDashing && onGround()) //checks if player is dashing first
        {
            if ((xVel != 0) && (Mathf.Abs (yVel) <= .1f))
            {
                _animator.SetBool("isWalking", true);
                SFXManager.SendMessage("isWalking", true);
            }
            else
            {
                _animator.SetBool("isWalking", false);
                SFXManager.SendMessage("notWalking", true);
            }
        }
        
    }

    public void CheckFallingAnimation()
    {
        if (isJumping) {
            _animator.SetBool("isFalling", true);
        }
        //float yVel = CalculateYVelocity();
        //if (rb.gravityScale != 0 && Mathf.Abs(yVel) >.1f)
        //{
        //    if (!onGround() && !isDashing) //so player isn't falling in place or while dashing
        //    {
        //        _animator.SetBool("isFalling", true);
        //    }
        //}
        //else
        //{
        //    _animator.SetBool("isFalling", false);
        //}

        //if (onGround()) //hard check on ground
        //{
        //    _animator.SetBool("isFalling", false);
        //}
    }

    IEnumerator dashAnimationCancelTimer()
    {
        float dashTime = 0;
        while (dashTime < 0.2)
        {
            dashTime += Time.deltaTime;
            yield return null;
        }
        _animator.SetBool("isDashingSide", false);
        _animator.SetBool("isDashingDown", false);
    }
}
