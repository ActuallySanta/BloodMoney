using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerInputController : MonoBehaviour
{
    public int playerInd = 0;

    [SerializeField] private InputAction playerMoveInputs;
    [SerializeField] private InputAction playerShootInputs;

    private Player player;

    public bool fireInput = false;
    public bool jumpInput = false;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(playerInd);

        player.controllers.maps.GetMap(1).enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        fireInput = player.GetButton("Fire");
        jumpInput = player.GetButton("Jump");
    }

    /// <summary>
    /// Get the input from the input manager
    /// </summary>
    /// <returns>A vector2 with the inputs of the player as the x and y values</returns>
    public Vector2 GetDirInput()
    {
        Vector2 moveDir;

        if (player.GetAxis("MoveHorizontal") <= 1f || player.GetAxis("MoveHorizontal") >= 1f)
        {
            moveDir.x = player.GetAxis("MoveHorizontal");
        }
        else
        {
            moveDir.x = 0;
        }

        moveDir.y = player.GetAxis("MoveVertical");

        return moveDir;
    }
}
