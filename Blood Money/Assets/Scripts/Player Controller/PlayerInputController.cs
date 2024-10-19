using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public int playerInd = 0;

    [SerializeField] private InputAction playerMoveInputs;
    [SerializeField] private InputAction playerShootInputs;

    [SerializeField] private Player1InputActions playerControls;

    public bool didFire = false;
    public InputDevice playerInputDevice;

    private void Awake()
    {
        playerControls = new Player1InputActions();
    }

    void OnEnable()
    {
        playerMoveInputs = playerControls.Player.Move;
        playerMoveInputs.Enable();

        playerShootInputs = playerControls.Player.Fire;
        playerMoveInputs.Enable();

    }

    private void OnDisable()
    {
        playerMoveInputs.Disable();
        playerShootInputs.Disable();

    }

    // Update is called once per frame
    void Update()
    {
        didFire = playerShootInputs.IsPressed();
    }

    /// <summary>
    /// Get the input from the input manager
    /// </summary>
    /// <returns>A vector2 with the inputs of the player as the x and y values</returns>
    public Vector2 GetDirInput()
    {
        Vector2 moveDir;


        if (playerMoveInputs.ReadValue<Vector2>().x != 0)
        {
            moveDir = playerMoveInputs.ReadValue<Vector2>();
        }
        else
        {
            moveDir = new Vector2(0, playerMoveInputs.ReadValue<Vector2>().y);
        }
        return moveDir;
    }
}
