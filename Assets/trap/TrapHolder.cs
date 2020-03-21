using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapHolder : MonoBehaviour
{
    public enum Mode
    {
        automatic,
        trigger
    }
    public Mode mode = Mode.trigger;
    private float _cycleTime = 1;
    private float _currentTime = 0;
    public virtual void ExecuteEnter()
    {
    }

    public virtual void ExecuteStay()
    {
    }

    public virtual void ExecuteExit()
    {
    }
    public virtual void ExecuteAuto()
    {
    }
    private void OnEnable()
    {
        if (mode == Mode.automatic)
        {
            Collider2D collider = GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = false;
            }
        }
    }
    void Update()
    {
        if (mode == Mode.automatic)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime > _cycleTime)
            {
                _currentTime -= _cycleTime;
                ExecuteAuto();
            }
        }

    }
}