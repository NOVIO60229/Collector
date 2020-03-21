using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    int HP { get; set; }
    void Hurt(int damage, Transform attacker = null);
}

public interface IBounce
{
    void Bounce();
}