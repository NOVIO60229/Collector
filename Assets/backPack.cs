using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class backPack : MonoBehaviour, IDamagable
{
    public GameObject _player;
    public Transform _originalTransform;
    BoxCollider2D _collider;
    Rigidbody2D _rigi;
    public int HP { get; set; }

    private float _transitionTime = 0.5f;
    private float timeElapsed = 0;
    private void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _rigi = GetComponent<Rigidbody2D>();
        _rigi.simulated = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.position.y > transform.position.y)
        {
            IBounce bounce = collision.transform.GetComponent<IBounce>();
            bounce?.Bounce();

            if (collision.gameObject == _player)
                Retrun();
        }

    }
    public void Hurt(int damage, Transform attacker = null)
    {
    }

    public void Throw(Vector2 throwDir, float throwForce)
    {
        transform.SetParent(null);
        _rigi.simulated = true;
        _rigi.velocity += throwDir * throwForce;
        Physics2D.IgnoreCollision(_collider, _player.GetComponent<BoxCollider2D>());
        StartCoroutine(Timer(_transitionTime, () =>
        {
            Physics2D.IgnoreCollision(_collider, _player.GetComponent<BoxCollider2D>(), false);

        }));
    }

    public void Retrun()
    {
        _rigi.simulated = false;
        transform.position = _originalTransform.position;
        transform.rotation = _originalTransform.rotation;
        transform.SetParent(_player.transform);
    }

    public IEnumerator Timer(float time, Action EndOperation = null)
    {
        yield return new WaitForSeconds(time);
        EndOperation?.Invoke();
    }
}
