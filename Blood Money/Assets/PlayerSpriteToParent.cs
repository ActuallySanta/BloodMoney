using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteToParent : MonoBehaviour
{
    public void HurtStateEnd()
    {
        PlayerController playerController = GetComponentInParent<PlayerController>();

        playerController.ChangeState(PlayerController.PlayerState.idle);
    }
}
