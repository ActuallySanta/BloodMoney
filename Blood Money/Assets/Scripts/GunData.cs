using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun Data", menuName = "Gun Data/Create New Gun Data")]
public class GunData : ScriptableObject
{
    public string gunName = "Gun";
    public string sfxName = "RifleShot";

    [Header("Firing Data")]
    public float ammo = 30f;
    public float reloadTime = .25f;
    public float fireSpeed = .1f;
    public float damage = 20f;
    public float bulletMoveSpeed = 20f;
    public float bulletCount = 1f;
    public float timeBetweenShots = 0f;
    public float burstCount = 1f;
    public float chargeUpTime = 0f;
    public float hitscanSize = 0f;
    public float damageFalloffMultiplier = 1f;

    [Header("Health Cost")]
    public float healthCost = 20f;

    [Header("Object References")]
    public GameObject gunObj;
    public GameObject projectileObj;

    [Header("Bullet Checks")]
    public bool isHitscan = false;
    public bool hasDamageFallOff = false;
}
