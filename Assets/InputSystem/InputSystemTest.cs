using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemTest : MonoBehaviour
{

    private void Update()
    {
        InputSystem.onDeviceChange +=
       (device, change) =>
       {
           switch (change)
           {
               case InputDeviceChange.Added:
                   Debug.Log("add");
                   // New Device.
                   break;
               case InputDeviceChange.Disconnected:
                   // Device got unplugged.
                   break;
               case InputDeviceChange.Reconnected:
                   // Plugged back in.
                   break;
               case InputDeviceChange.Removed:
                   // Remove from Input System entirely; by default, Devices stay in the system once discovered.
                   break;
               default:
                   // See InputDeviceChange reference for other event types.
                   break;
           }
       };
    }
}
