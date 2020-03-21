using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Trap_Spike : TrapHolder
{
    public TrapHolderStatus _status;
    public GameObject _spike;
    private bool _isSpikeUp = false;

    private void Start()
    {
        _spike.transform.localScale = new Vector3(1, 0, 1);
    }

    public override void ExecuteEnter()
    {
        SpikeUp();
    }

    public void SpikeUp()
    {
        if (_isSpikeUp) return;
        _isSpikeUp = true;
        _spike.transform.DOScaleY(1, 0.1f).onComplete += () => _spike.transform.DOScaleY(0, 0.5f).onComplete += () => _isSpikeUp = false;
    }
}