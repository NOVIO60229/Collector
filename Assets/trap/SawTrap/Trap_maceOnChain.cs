using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Trap_maceOnChain : TrapHolder
{
    public TrapHolderStatus _status;
    private float startAngle;
    public float speed;
    public GameObject rotater;

    private void Start()
    {
        //startAngle = rotater.transform.localRotation.eulerAngles.z;
        //float rotateAmount = startAngle > 180 ? -180 : 180;

        //Sequence seq = DOTween.Sequence();
        //seq.Append(rotater.transform.DOLocalRotate(new Vector3(0, 0, startAngle + rotateAmount), speed).SetEase(Ease.InOutQuad));
        //seq.Append(rotater.transform.DOLocalRotate(new Vector3(0, 0, startAngle), speed).SetEase(Ease.InOutQuad));
        //seq.SetLoops(-1);

    }
    private void Update()
    {
        rotater.transform.Rotate(0, 0, speed * Time.deltaTime);

    }
}