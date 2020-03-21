using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayerChange : MonoBehaviour
{
    private int sortingLayerBase = 5000;
    private int offset = 50;
    public bool runOnlyOnce;
    private Renderer _renderer;

    private float timer;
    private float timerMax = 0.1f;
    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {

    }

    private void LateUpdate()
    {
        timer += Time.deltaTime;
        if (timer <= timerMax)
        {
            return;
        }
        timer -= timerMax;

        _renderer.sortingOrder = (int)(sortingLayerBase - transform.position.y * offset);

        if (runOnlyOnce)
        {
            Destroy(this);
        }
    }
}
