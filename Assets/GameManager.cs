using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject obj45;
    public GameObject obj180;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            obj45.SetActive(!obj45.activeInHierarchy);
            obj180.SetActive(!obj180.activeInHierarchy);
        }
    }
}
