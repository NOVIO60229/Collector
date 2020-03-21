using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : TrapDmgDealer
{
    float _startSpeed = 600;
    Rigidbody2D _rb;
    private float _destroyTime = 2;
    private float _currentTime = 0;
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity += (Vector2)transform.up * _startSpeed * Time.deltaTime;
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime > _destroyTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamagable target = other.GetComponent<IDamagable>();
        if (target != null)
        {
            target?.Hurt(_status.damage, transform);
            Destroy(gameObject);
        }
        else if (other.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
    }
}