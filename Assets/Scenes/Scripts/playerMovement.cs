using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Specifies that in order to use this component, a CharacterController must be attached to it (i.e., whatever component is attached to this one must be a type of chartacter controller)
[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]

public class playerMovement : MonoBehaviour
{
    //Movement variables
    private float maxCrouchSpeed = 5.0f;
    public float currentHorzSpeed = 0.0f;
    private float maxHorzSpeed = 10f;
    public float fallSpeed = 0f;
    private float maxFallSpeed = 5f;
    private float fastFallSpeed = 10f;
    private float momentum = 0.05f;
    private bool isMovingHorz = false;
    private bool isMovingVert = false;
    public bool isCrouching = false;
    public bool isFastFalling = false;
    //Jumping+dashing to be added below

    //This is a local referecne to the object, and multiple scripts can have references to this SINGLUAR character controller instance
    private CharacterController charController;

    // Start is called before the first frame update
    void Start()
    {
        //At the start of the scene, assign the character controller to the component which is assigned the role of the player (or the character controller)
        //Since this is defined as such at the beginning of the scene, mulitple scripts will be able to reference this one character controller
        //Types are defined in C# by diamond (<>) operators, enclosing the type in that (it does not appear that the 'new' reference is used for creating a new object here)
        charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Variable to check if the player is holding a horizontal direction button 
        float horzInput = Input.GetAxis("Horizontal");
        if (horzInput != 0)
        {
            isMovingHorz = true;
            //Gives the player a set momentum that caps off at the maxHorzSpeed
            currentHorzSpeed = Math.Min(currentHorzSpeed + momentum, maxHorzSpeed);
        }
        else
        {
            isMovingHorz = false;
            //If the player is not holding a horizontal direction button, set the currentHorzSpeed to 0 
            currentHorzSpeed = 0;
        }

        //Sets how far the player moves in the horizontal position
        float deltaX = Input.GetAxis("Horizontal") * currentHorzSpeed;

        //Checking to see if the player is falling
        if (!charController.isGrounded) // NOTE: May need to change isGrounded or fix it for the platforms
        {
            //Having to do with jump things (Phineas)
            //Ex: if player is jumping, give a certain time for positive fall (going up) then negative fall (going down)
        }
        else
        {
            //Player on ground
            fallSpeed = 0f;
        }

        //Crouching and fastfalling
        if (Input.GetKey(KeyCode.S) /*&& (controller input) */)
        {
            Debug.Log("player holding s");
            if (charController.isGrounded) //Player crouching on the ground
            {
                //Crouching
                isCrouching = true;
                //Change the collider here
                //Slowing the player by half if they are holding crouch
                currentHorzSpeed = Math.Min(currentHorzSpeed + momentum, maxCrouchSpeed);
            }
            else //Player fastfalling
            {
                //Note fallSpeed while falling is positive
                isFastFalling = true;
                fallSpeed = Math.Min(fallSpeed + momentum, fastFallSpeed);
            }
        }
        else
        {
            isCrouching = false;
            //Fix the collider here
            isFastFalling = false;
        }

        //Sets how far the player moves in the vertical position (note: w and s will be the veritcal axis)
        float deltaY = -fallSpeed;
        //Create a vectore for the players change in movement in space
        Vector2 movement = new Vector2(deltaX, deltaY); //Note: needs to be combined with the dash and Jump to have comeplete movement
        
        //Space to change movement based on jumping and dashing
        
        //Moves the player by movement
        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);

        //Adds collision detection to the movement
        charController.Move(movement);

    }
}
