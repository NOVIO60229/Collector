using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using Cinemachine;
public class PlayerController_45 : MonoBehaviour, IDamagable
{
    public enum State
    {
        normal,
        roll,
    }
    public State state;

    //components
    Rigidbody2D _rb;
    SpriteRenderer _spr;
    BoxCollider2D _collider;
    PlayerAnimation _playerAnimation;
    CinemachineImpulseSource _impulse;

    //basic state
    public static PlayerController_45 _instance;
    public int HP { get; set; } = 100;

    //player state
    public bool _isGrounded = false;
    public bool _isHurt = false;
    public bool _canNotMove = false;
    private bool _isInvincible = false;
    private Vector2 _facingDir = Vector2.right;

    //walk variables
    private Vector2 _inputDir;
    private float _walkSpeed = 4.5f;
    private Vector2 _knockBackForce;

    //jump variable
    //private bool _isJumpButtonDown = false;
    //private float _jumpForce = 9f;
    //public LayerMask _groundLayers;

    //dash variables
    private bool _isDashButtonDown = false;
    private float _dashForce = 1.5f;
    public LayerMask _dashLayerMask;
    private bool _isDashCD = false;
    private float _dashCDTime = 0.5f;

    //roll variables
    private float _rollSpeedMax = 25f;
    private float _rollSpeedMin = 5f;
    private float _rollCurrentSpeed = 0f;
    private float _rollTime = 0.4f;
    private float _rollCurrentTime = 0f;
    private float _rollInvincibleTime = 0.3f; // 0~1 for percentage

    private void Start()
    {
        _instance = this;
        _rb = GetComponent<Rigidbody2D>();
        _spr = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
        _playerAnimation = GetComponent<PlayerAnimation>();
        _impulse = GetComponent<CinemachineImpulseSource>();
    }
    private void Update()
    {
        switch (state)
        {
            case State.normal:

                //// isGrounded check
                //Collider2D hit = Physics2D.OverlapBox(
                //    (Vector2)_collider.bounds.center - new Vector2(0, _collider.bounds.extents.y + 0.01f), new Vector2(_collider.bounds.size.x, 0.1f), 0, _groundLayers);
                //_isGrounded = hit ? true : false;

                _inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

                if (_inputDir != Vector2.zero)
                {
                    _facingDir = _inputDir;
                    _playerAnimation.Play_Run(_facingDir);
                }
                else
                {
                    _playerAnimation.Play_Idle(_facingDir);
                }

                //Flip Player
                if (_inputDir.x < 0)
                    transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                else if (_inputDir.x > 0)
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);

                if (Input.GetKeyDown(KeyCode.X))
                {
                    _isDashButtonDown = true;
                    _impulse.GenerateImpulse();
                }
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    state = State.roll;
                    _rollCurrentSpeed = _rollSpeedMax;
                    _rollCurrentTime = 0;
                    DOTween.To(() => _rollCurrentSpeed, x => _rollCurrentSpeed = x, _rollSpeedMin, _rollTime).onComplete += () =>
                    {
                        state = State.normal;
                    };
                }
                break;
            case State.roll:
                _playerAnimation.Play_Roll(_facingDir);
                _rollCurrentTime += Time.deltaTime;
                if (_rollCurrentTime < _rollTime * _rollInvincibleTime)
                {
                    _isInvincible = true;
                }
                else
                {
                    _isInvincible = false;
                }
                break;
            default:
                Debug.LogError("Not in any state");
                break;

        }
    }
    private void FixedUpdate()
    {
        switch (state)
        {
            case State.normal:
                _rb.velocity = _inputDir * _walkSpeed;
                if (_isDashButtonDown)
                {
                    if (!_isDashCD)
                        Dash();
                    _isDashButtonDown = false;
                }
                break;
            case State.roll:
                _rb.velocity = _facingDir * _rollCurrentSpeed;
                break;
            default:
                Debug.LogError("Not in any state");
                break;
        }
    }
    void Dash()
    {
        Vector2 DesirePosition = (Vector2)transform.position + (_facingDir * _dashForce);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _facingDir, _dashForce, _dashLayerMask);
        if (hit)
        {
            DesirePosition = hit.point;
        }

        //dash effect

        _rb.MovePosition(DesirePosition);
        _isDashButtonDown = false;
        _isDashCD = true;
        StartCoroutine(Timer(_dashCDTime, () => _isDashCD = false));
    }

    public void Hurt(int damage, Transform attacker = null)
    {
        if (_isHurt || _isInvincible) return;
        _isHurt = true;
        HP -= damage;
        if (attacker != null)
        {
            Vector2 knockDir = transform.position - attacker.position;
            _rb.velocity += knockDir * _knockBackForce;
        }
        StartCoroutine(IvincibleTime(0.4f));
    }

    public IEnumerator Timer(float time, Action EndOperation = null)
    {
        yield return new WaitForSeconds(time);
        EndOperation?.Invoke();
    }

    public IEnumerator IvincibleTime(float duration)
    {
        float timeLeft = duration;

        //shine red light effect variables
        float switchColorTime = 0.08f;
        float colorTime = switchColorTime;
        bool isColorRed = true;

        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;

            //shine red light effect
            colorTime -= Time.deltaTime;
            if (colorTime <= 0)
            {
                colorTime += switchColorTime;
                isColorRed = !isColorRed;

                if (isColorRed)
                    _spr.color = Color.red;
                else
                    _spr.color = Color.white;
            }
            yield return null;
        }

        _spr.color = Color.white;
        _isHurt = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TrapHolder trap = collision.GetComponent<TrapHolder>();
        if (trap != null)
            trap.ExecuteEnter();
    }
}
