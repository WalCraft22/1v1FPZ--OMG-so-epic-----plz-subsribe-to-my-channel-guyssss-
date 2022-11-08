using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class DebugScreen : NetworkBehaviour
{
    public bool IsShown = false;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            IsShown = !IsShown;
            if(IsShown)
            {
            }
        }
    }
}
