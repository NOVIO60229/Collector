using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IDamagable
{
    public int HP { get; set; } = 50;

    public void Hurt(int damage, Transform attacker = null)
    {
    }
}
