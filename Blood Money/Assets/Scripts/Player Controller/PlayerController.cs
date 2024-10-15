using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    //Physics Variables
    [SerializeField] float groundSpeed = 5f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float inAirSpeed = 3.5f;
    [SerializeField] float downForce = 5f;

    //Reference to Input Manager (Can't name it that)
    [SerializeField] PlayerInputController playerInput;

    //Reference to the character sprite on the player
    [SerializeField] SpriteRenderer playerSprite;

    //Reference to animator
    [SerializeField] Animator anim;

    //Reference the rigidbody 2D attached to the player
    [SerializeField] Rigidbody2D rb;

    //Reference to all the animations that the player can have (will be set by character selector)
    public CharacterData charData;

    //Ground Check stuff
    [SerializeField] LayerMask groundLayers;
    [SerializeField] Transform floorPos;
    [SerializeField] float coyoteFrames;
    bool canJump = true;
    bool isGrounded;

    public enum PlayerState
    {
        idle,
        moving,
        inAir,
        dead,
        dying,
        hurt,
    }

    PlayerState currState = PlayerState.idle;

    //Shows the input direction
    Vector2 inputVector;

    //The direction the sprite is facing
    float facingDir = 1;

    float jumpTimer;

    void Start()
    {
        isGrounded = false;
        
        anim = playerSprite.GetComponent<Animator>();
        anim.runtimeAnimatorController = charData.charAnim;
        ChangeState(PlayerState.idle);
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the player is on the ground
        isGrounded = CheckForGround();

        if (!isGrounded)
        {
            jumpTimer = Time.time;
        }

        if (!isGrounded && jumpTimer + coyoteFrames < Time.time)
        {
            canJump = false;
        }

        if (isGrounded) canJump = true;

        //Get Input
        inputVector = playerInput.GetDirInput();

        if (BattleSceneManager.instance.isStarting) return;

        //Make the sprite face the direction the character is moving
        if (inputVector.x != 0) facingDir = inputVector.x;

        transform.localScale = new Vector3(facingDir, transform.localScale.y, transform.localScale.z);

        switch (currState)
        {
            case PlayerState.idle:
                //If the player inputs something while staying on the ground, switch to moving state
                if (inputVector != Vector2.zero && isGrounded)
                {
                    ChangeState(PlayerState.moving);
                }

                //Let the player jump
                if (inputVector.y > 0 && canJump)
                {
                    StartCoroutine(Jump());
                }
                break;

            case PlayerState.moving:

                //Allow the player to move
                MovePlayer(inputVector, groundSpeed);

                //Return to idle state if there is no horizontal input
                if (inputVector.x == 0 && isGrounded) ChangeState(PlayerState.idle);

                //Let the player jump if they press the up key
                if (inputVector.y > 0 && canJump)
                {
                    StartCoroutine(Jump());
                }

                break;

            case PlayerState.inAir:


                rb.AddForce(-transform.up * downForce, ForceMode2D.Force);

                //Let the player jump
                MovePlayer(inputVector, inAirSpeed);

                //Return to idle state if the player touches the ground
                if (isGrounded) ChangeState(PlayerState.idle);

                break;

            case PlayerState.dying:
                //TODO ADD TRANSITION FROM DYING TO DEAD
                break;

            case PlayerState.hurt:


                break;

            case PlayerState.dead:

                Destroy(gameObject);

                break;
        }
    }

    /// <summary>
    /// Let the player jump while on the ground
    /// </summary>
    private IEnumerator Jump()
    {
        //Remove any velocity it had beforehand (standardizes it)
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);

        //Actually add the force
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);

        //Wait a second to make sure the player is not on the ground
        yield return new WaitForSeconds(.01f);

        //Change the state
        ChangeState(PlayerState.inAir);
    }



    /// <summary>
    /// Affect the rigidbody's velocity
    /// </summary>
    /// <param name="_inputVector">The input vector from the keyboard</param>
    private void MovePlayer(Vector2 _inputVector, float _moveSpeed)
    {
        Vector2 _vel = new Vector2(_inputVector.x * _moveSpeed, rb.linearVelocity.y);

        rb.linearVelocity = _vel;
    }

    /// <summary>
    /// Check if the player is on the ground
    /// </summary>
    /// <returns>If the player is touching an object with a layer that is on the layermask, return true</returns>
    private bool CheckForGround()
    {
        return Physics2D.Raycast(floorPos.position, Vector2.down, 0.1f, groundLayers);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(floorPos.position, new Vector2(floorPos.position.x, floorPos.position.y - .1f), Color.green);
    }

    /// <summary>
    /// Switch the state and affect relevant systems
    /// </summary>
    /// <param name="_endingState">The state being switched to</param>
    public void ChangeState(PlayerState _endingState)
    {

        //Get the name of the starting animation from the state
        string startAnimName = currState.ToString(); //Convert the name to a string

        anim.SetBool(startAnimName, false);

        string endingAnimName = _endingState.ToString();
        anim.SetBool(endingAnimName, true);

        currState = _endingState;
    }

    public PlayerState GetCurrState()
    {
        return currState;
    }

    
}
