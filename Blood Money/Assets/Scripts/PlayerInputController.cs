using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public int playerInd = 0;
    [HideInInspector] public KeyCode leftKey, rightKey, upKey, downKey, fireKey, equipmentKey;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Set which keys are the given to each player based on playerInd
        switch (playerInd)
        {
            case 0:
                leftKey = KeyCode.A;
                rightKey = KeyCode.D;
                upKey = KeyCode.W;
                downKey = KeyCode.S;
                fireKey = KeyCode.E;
                equipmentKey = KeyCode.Q;
                break;

            case 1:
                leftKey = KeyCode.LeftArrow;
                rightKey = KeyCode.RightArrow;
                upKey = KeyCode.UpArrow;
                downKey = KeyCode.DownArrow;
                fireKey = KeyCode.RightShift;
                equipmentKey = KeyCode.Slash;
                break;

            case 2:
                leftKey = KeyCode.J;
                rightKey = KeyCode.L;
                upKey = KeyCode.I;
                downKey = KeyCode.K;
                fireKey = KeyCode.Semicolon;
                equipmentKey = KeyCode.Quote;
                break;
        }
    }

    /// <summary>
    /// Get the input from the input manager
    /// </summary>
    /// <returns>A vector2 with the inputs of the player as the x and y values</returns>
    public Vector2 GetDirInput()
    {
        //Define temp variables
        float _xInput = 0;
        float _yInput = 0;

        //If the left or right key are pressed, affect the float 
        if (Input.GetKey(leftKey)) _xInput--;
        if (Input.GetKey(rightKey)) _xInput++;

        //If the left or right key are pressed, affect the float 
        if (Input.GetKey(downKey)) _yInput--;
        if (Input.GetKey(upKey)) _yInput++;

        return new Vector2(_xInput, _yInput);
    }
}
