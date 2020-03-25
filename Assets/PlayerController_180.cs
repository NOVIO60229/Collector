using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using Cinemachine;
public class PlayerController_180 : MonoBehaviour, IDamagable, IBounce
{
    public enum State
    {
        normal,
        slide,
    }
    public State state;

    //components
    Rigidbody2D _rb;
    SpriteRenderer _spr;
    BoxCollider2D _collider;
    PlayerAnimation180 _playerAnimation;
    CinemachineImpulseSource _impulse;

    //basic state
    public static PlayerController_180 _instance;
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
    private bool _isJumpButtonDown = false;
    private float _jumpForce = 13f;
    public LayerMask _groundLayers;
    private float fallMultiplier = 1.5f;
    private float lowJumpMultiplier = 5.5f;
    private float lastJumpMultiplier = 2.2f;
    private int _jumpTimes = 1;
    private int _jumpTimeLimit = 1;


    //dash variables
    private bool _isDashButtonDown = false;
    private float _dashForce = 3f;
    public LayerMask _dashLayerMask;
    private bool _isDashCD = false;
    private float _dashCDTime = 0.5f;

    //slide variables
    private float _slideSpeedMax = 30f;
    private float _slideSpeedMin = 15f;
    private float _slideCurrentSpeed = 0f;
    private float _slideTime = 0.25f;
    private float _slideCurrentTime = 0f;
    private float _slideInvincibleTime = 0.6f; // 0~1 for percentage
    private bool _isSlideCD = false;
    private float _slideCDTime = 0.3f;

    //crouch variables
    private readonly float _crouch_Ori_Offset = 0.1099848f;
    private readonly float _crouch_Ori_Size = 0.2199695f;
    private readonly float _crouch_Offset = 0.05244379f;
    private readonly float _crouch_Size = 0.1048876f;

    //backpack variables
    public GameObject _BP;
    backPack _BPbackpack;
    private float _throwForce = 16f;
    private float _bounceForce = 20f;


    private void Start()
    {
        _instance = this;
        _rb = GetComponent<Rigidbody2D>();
        _spr = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
        _playerAnimation = GetComponent<PlayerAnimation180>();
        _impulse = GetComponent<CinemachineImpulseSource>();
        _BPbackpack = _BP.GetComponent<backPack>();

    }
    private void Update()
    {
        switch (state)
        {
            case State.normal:

                // isGrounded check
                Collider2D hit = Physics2D.OverlapBox(
                    (Vector2)_collider.bounds.center - new Vector2(0, _collider.bounds.extents.y + 0.01f), new Vector2(_collider.bounds.size.x * 0.9f, 0.1f), 0, _groundLayers);
                _isGrounded = hit ? true : false;

                if (_isGrounded)
                {
                    _jumpTimes = _jumpTimeLimit;
                }

                _inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

                if (_inputDir != Vector2.zero)
                {
                    _facingDir = _inputDir.normalized;
                }

                if (_rb.velocity.y > 0.1f)
                {
                    _playerAnimation.Play_Animation("JumpUp", 0.8f);
                }
                else if (_rb.velocity.y < -0.1f)
                {
                    _playerAnimation.Play_Animation("JumpDown", 0.8f);
                }
                else if (_inputDir.x != 0)
                {
                    _playerAnimation.Play_Animation("Run", 0.8f);
                }
                else
                {
                    _playerAnimation.Play_Animation("Idle", 0.6f);
                }

                //Flip Player
                if (_inputDir.x < 0)
                    transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                else if (_inputDir.x > 0)
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);

                if (!_isGrounded)
                {
                    //make jump better(change gravity)
                    if (_rb.velocity.y < 0)
                    {
                        _rb.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.unscaledDeltaTime;
                    }
                    //add extra force on the last jump if holding button
                    else if (_rb.velocity.y > 0 && Input.GetKey(KeyCode.C))
                    {
                        _rb.velocity += Vector2.up * Physics2D.gravity.y * lastJumpMultiplier * Time.unscaledDeltaTime;
                    }
                    else if (_rb.velocity.y > 0)
                    {
                        _rb.velocity += Vector2.up * Physics2D.gravity.y * lowJumpMultiplier * Time.unscaledDeltaTime;
                    }
                }
                if (Input.GetKeyDown(KeyCode.V))
                {
                    ThrowBP();
                }
                if (Input.GetKeyDown(KeyCode.C))
                {
                    _isJumpButtonDown = true;
                }
                if (Input.GetKeyDown(KeyCode.X))
                {
                    _isDashButtonDown = true;
                    _impulse.GenerateImpulse();
                }
                if (Input.GetKeyDown(KeyCode.LeftControl) && _inputDir.x != 0 && !_isSlideCD)
                {
                    state = State.slide;
                    _slideCurrentSpeed = _slideSpeedMax;
                    _slideCurrentTime = 0;
                    _isSlideCD = true;
                    DOTween.To(() => _slideCurrentSpeed, x => _slideCurrentSpeed = x, _slideSpeedMin, _slideTime).onComplete += () =>
                    {
                        if (state != State.normal)
                        {
                            state = State.normal;
                            _collider.offset = new Vector2(_collider.offset.x, _crouch_Ori_Offset);
                            _collider.size = new Vector2(_collider.size.x, _crouch_Ori_Size);
                            StartCoroutine(Timer(_slideCDTime, () => _isSlideCD = false));
                        }
                    };
                }
                break;
            case State.slide:
                _playerAnimation.Play_Animation("Slide", _slideTime);
                _collider.offset = new Vector2(_collider.offset.x, _crouch_Offset);
                _collider.size = new Vector2(_collider.size.x, _crouch_Size);

                //invincible time check
                _slideCurrentTime += Time.deltaTime;
                if (_slideCurrentTime < _slideTime * _slideInvincibleTime)
                {
                    _isInvincible = true;
                }
                else
                {
                    _isInvincible = false;
                }

                // jump to interupt slide
                if (Input.GetKeyDown(KeyCode.C))
                {
                    _isJumpButtonDown = true;
                    state = State.normal;
                    StartCoroutine(Timer(_slideCDTime, () => _isSlideCD = false));
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
                _rb.velocity = new Vector2(_inputDir.x * _walkSpeed, _rb.velocity.y);

                if (_isDashButtonDown)
                {
                    if (!_isDashCD)
                        Dash();
                    _isDashButtonDown = false;

                }

                if (_isJumpButtonDown)
                {
                    Jump();
                    _isJumpButtonDown = false;
                }
                break;

            case State.slide:
                _rb.velocity = _facingDir * _slideCurrentSpeed;

                if (_isJumpButtonDown)
                {
                    Jump();
                    _isJumpButtonDown = false;
                }
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

    void Jump()
    {
        if (_jumpTimes == 0) return;
        _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
        _jumpTimes -= 1;
    }
    void Crouch()
    {

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

    public void Bounce()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, _bounceForce);
    }

    public void ThrowBP()
    {
        _BPbackpack.Throw(_facingDir, _throwForce);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        TrapHolder trap = collision.GetComponent<TrapHolder>();
        if (trap != null)
            trap.ExecuteEnter();
    }
}
