using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;
    private string _stateName = "Player_Idle_Back";
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
        string _name = "Player_Idle";
        _name = DirectionCheck(_name, dir);
        if (_name == _stateName) return;
        _stateName = _name;
        ChangeState(0.5f);
    }
    public void Play_Run(Vector2 dir)
    {
        string _name = "Player_Run";
        _name = DirectionCheck(_name, dir);
        if (_name == _stateName) return;
        _stateName = _name;
        ChangeState(0.8f);
    }
    public void Play_Roll(Vector2 dir)
    {
        string _name = "Player_Roll";
        _name = DirectionCheck(_name, dir);
        if (_name == _stateName) return;
        _stateName = _name;
        ChangeState(0.4f);
    }

    public void ChangeState(float _playTime)
    {
        _playTimeMultiplier = 1 / _playTime;
        _normalizedTime = 0;
    }

    public string DirectionCheck(string name, Vector2 dir)
    {
        if (dir == Vector2.up)
            name += "_Back";
        else if (dir == Vector2.down)
            name += "_Front";
        else if (dir.x != 0)
            name += "_Right";

        return name;
    }
}
