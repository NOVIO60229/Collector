using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Trap_Saw : TrapHolder
{
    public TrapHolderStatus _status;
    public GameObject _saw;
    public Transform _startPoint;
    public Transform _endPoint;
    public float moveTime = 0.8f;

    private void Start()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_saw.transform.DOMove(_endPoint.position, moveTime).SetEase(Ease.InOutQuad));
        seq.Append(_saw.transform.DOMove(_startPoint.position, moveTime).SetEase(Ease.InOutQuad));
        seq.SetLoops(-1);
    }
}