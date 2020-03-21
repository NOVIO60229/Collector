using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTrapGenerator : MonoBehaviour
{
    public GameObject[] _traps;
    public int _amout = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GenerateRandomTrap();
        }
    }
    void GenerateRandomTrap()
    {
        int randomIndex = Random.Range(0, _traps.Length);
        Instantiate(
            _traps[randomIndex],
            new Vector2(Random.Range(-10, 10), Random.Range(0, 12)),
            Quaternion.Euler(0, 0, Random.Range(0, 360)));
    }
}
