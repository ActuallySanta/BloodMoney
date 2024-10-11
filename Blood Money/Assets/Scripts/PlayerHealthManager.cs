using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    public float currHealth;
    public float maxHealth;

    [SerializeField] private float invicibilityTimer = 1.5f;
    [HideInInspector] public bool canBeHit = true;

    private PlayerController pController;

    // Start is called before the first frame update
    void Start()
    {
        pController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currHealth <= 0)
        {
            PlayerDie();
        }
    }

    private void PlayerDie()
    {
        BattleSceneManager.instance.RemovePlayerFromActiveList(this.gameObject);
        pController.ChangeState(PlayerController.PlayerState.dying);

    }

    public void TakeDamage(float _dmg)
    {
        if (canBeHit)
        {
            currHealth -= _dmg;
            pController.ChangeState(PlayerController.PlayerState.hurt);
            StartCoroutine(IsInvincible());
        }

    }

    private IEnumerator IsInvincible()
    {
        yield return new WaitForSeconds(invicibilityTimer);
        canBeHit = true;
    }
}
