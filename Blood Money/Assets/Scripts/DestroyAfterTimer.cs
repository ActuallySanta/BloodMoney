using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTimer : MonoBehaviour
{
    [SerializeField] private float lifeTimer = 5f;

    private void Awake()
    {
        StartCoroutine(DestroyAfterTime());
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(lifeTimer);
        Destroy(gameObject);
    }
}
