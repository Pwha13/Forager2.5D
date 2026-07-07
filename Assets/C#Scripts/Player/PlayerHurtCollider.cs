using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out MosterAi monster))
            PlayerEventCenter.Instance.RaiseOnPlayerHurt(monster.damage);
    }
}
