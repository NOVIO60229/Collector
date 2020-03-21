using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDmanager : MonoBehaviour
{
    public Text _text;
    public GameObject player;
    private void Update()
    {
        _text.text = player.GetComponent<IDamagable>().HP.ToString();
    }
}
