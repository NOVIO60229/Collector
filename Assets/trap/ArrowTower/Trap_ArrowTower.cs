using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_ArrowTower : TrapHolder
{
    public TrapHolderStatus _status;
    public GameObject ArrowPrefab;
    public Transform firePoint;

    public override void ExecuteEnter()
    {
        Shoot();
    }
    public override void ExecuteAuto()
    {
        Shoot();
    }

    private void Shoot()
    {
        Vector2 fireDir = firePoint.position - transform.position;
        Instantiate(ArrowPrefab, firePoint.position, Quaternion.FromToRotation(ArrowPrefab.transform.up, fireDir));
    }
}