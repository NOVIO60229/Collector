using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation180 : MonoBehaviour
{
    private Animator _animator;
    private string _stateName = "Idle";
    private float _normalizedTime = 0;
    private float _playTimeMultiplier = 1;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        _animator.Play(_stateName, 0, _normalizedTime);
        _normalizedTime += Time.deltaTime * _playTimeMultiplier;
        Mathf.Repeat(_normalizedTime, 1);
    }
    public void Play_Idle(Vector2 dir)
    {
        string _name = "Idle";
        if (_name == _stateName) return;
        _stateName = _name;
        ChangeState(0.5f);
    }
    public void Play_Run(Vector2 dir)
    {
        string _name = "Run";
        if (_name == _stateName) return;
        _stateName = _name;
        ChangeState(0.8f);
    }
    public void Play_Roll(Vector2 dir)
    {
        string _name = "Roll";
        if (_name == _stateName) return;
        _stateName = _name;
        ChangeState(0.4f);
    }
    public void Play_JumpUp(Vector2 dir)
    {
        string _name = "JumpUp";
        if (_name == _stateName) return;
        _stateName = _name;
        ChangeState(0.4f);
    }
    public void Play_JumpDown(Vector2 dir)
    {
        string _name = "JumpDown";
        if (_name == _stateName) return;
        _stateName = _name;
        ChangeState(0.4f);
    }

    public void Play_Animation(string aniName, float playSecond)
    {
        if (aniName == _stateName) return;
        _stateName = aniName;
        ChangeState(playSecond);
    }


    public void ChangeState(float _playTime)
    {
        _playTimeMultiplier = 1 / _playTime;
        _normalizedTime = 0;
    }

}
