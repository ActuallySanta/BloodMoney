using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public int playerInd = 0;
    public KeyCode leftKey, rightKey, upKey, downKey, fireKey;

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
                break;

            case 1:
                leftKey = KeyCode.LeftArrow;
                rightKey = KeyCode.RightArrow;
                upKey = KeyCode.UpArrow;
                downKey = KeyCode.DownArrow;
                fireKey = KeyCode.RightShift;
                break;

            case 2:
                leftKey = KeyCode.J;
                rightKey = KeyCode.L;
                upKey = KeyCode.I;
                downKey = KeyCode.K;
                fireKey = KeyCode.Semicolon;
                break;
        }
    }
}
