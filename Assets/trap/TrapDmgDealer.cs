using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDmgDealer : MonoBehaviour
{
    public TrapDmgDealerStatus _status;

    private void OnTriggerStay2D(Collider2D other)
    {
        IDamagable target = other.GetComponent<IDamagable>();
        target?.Hurt(_status.damage, transform);
    }
}