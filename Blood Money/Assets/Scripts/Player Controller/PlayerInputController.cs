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

    private void Start()
    {
        Debug.Log(playerInd);
        player = ReInput.players.GetPlayer(playerInd);

        // Disable all individual Action Element Maps in all Controller Maps in the Player
        foreach (ControllerMap map in player.controllers.maps.GetAllMaps())
        {

            // Disable all Action Element Maps of all types
            foreach (ActionElementMap aem in map.AllMaps)
            {
                aem.enabled = false;
            }

            // Disable all Button Maps (these were already disabled above but this is just for illustration)
            foreach (ActionElementMap aem in map.ButtonMaps)
            {
                aem.enabled = false;
            }

            // Try disabling all Axis Maps if this is a Joystick Map (these were also disabled above)
            JoystickMap joystickMap = map as JoystickMap;
            if (joystickMap != null)
            {
                foreach (ActionElementMap aem in joystickMap.AxisMaps)
                {
                    aem.enabled = false;
                }
            }
        }

        foreach (Joystick joystick in player.controllers.Joysticks)
        {
            player.controllers.maps.LoadMap(ControllerType.Joystick, joystick.id, "GameInput", "Default", true);
        }
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
