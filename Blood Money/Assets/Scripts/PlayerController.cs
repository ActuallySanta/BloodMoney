using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Physics Variables
    [SerializeField] float playerSpeed = 5f;
    [SerializeField] float jumpSpeed = 10f;

    //Reference to Input Manager (Can't name it that)
    [SerializeField] PlayerInputController playerInput;

    //Reference to the character sprite on the player
    [SerializeField] Sprite playerSprite;

    //Reference to animator
    [SerializeField] Animator anim;

    //Reference to all the animations that the player can have (will be set by character selector)
    public Animation idleAnim;
    public Animation runningAnim;
    public Animation dyingAnim;
    public Animation jumpingAnim;

    //Ground Check stuff
    [SerializeField] LayerMask groundLayers;
    [SerializeField] Transform floorPos;
    bool isGrounded;

    enum PlayerState
    {
        idle,
        moving,
        inAir,
        dead,
        dying,
    }

    PlayerState currState = PlayerState.idle;

    //Shows the input direction
    Vector2 inputVector;

    //The direction the sprite is facing
    float facingDir = 1;

    // Start is called before the first frame update
    void Start()
    {
        isGrounded = false;
        ChangeState(PlayerState.idle);
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the player is on the ground
        isGrounded = CheckForGround();

        //Make the sprite face the direction the character is moving
        if (inputVector.x != 0) facingDir = inputVector.x;

        transform.localScale = new Vector3(transform.localScale.x * facingDir, transform.localScale.y, transform.localScale.z);

        switch (currState)
        {
            case PlayerState.idle:
                //If the player inputs something while staying on the ground, switch to moving state
                if(inputVector != Vector2.zero && isGrounded)
                {

                }

                break;
        }
    }

    /// <summary>
    /// Check if the player is on the ground
    /// </summary>
    /// <returns>If the player is touching an object with a layer that is on the layermask, return true</returns>
    private bool CheckForGround()
    {
        return Physics2D.Raycast(floorPos.position, Vector2.down, 0.1f, groundLayers);
    }

    /// <summary>
    /// Switch the state and affect relevant systems
    /// </summary>
    /// <param name="_endingState">The state being switched to</param>
    private void ChangeState(PlayerState _endingState)
    {
        //Get the name of the starting animation from the state
        string startAnimName = currState.ToString(); //Convert the name to a string
        string[] subStrings = startAnimName.Split('.'); //Split the name at the .
        startAnimName = subStrings[1];

        anim.SetBool(startAnimName, false);
        
        //Get the name of the ending animation from the state
        string endingAnimName = _endingState.ToString(); //Convert the name to a string
        string[] endSubStrings = endingAnimName.Split('.'); //Split the name at the .
        endingAnimName = subStrings[1];

        anim.SetBool(endingAnimName, true);

        
        currState = _endingState;
    }
}
