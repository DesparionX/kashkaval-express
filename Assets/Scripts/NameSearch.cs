using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

public class NameSearch : MonoBehaviour
{
    public GameObject dropDown;
    public InputField dropDownMenu;
    void FixedUpdate()
    {
        /*
        if(dropDownMenu.isFocused)
        {
            dropDown.SetActive(true);
        }
        */
    }
    public void OpenDD()
    {
        dropDown.SetActive(true);
    }
    public void CloseDD()
    {
        dropDown.SetActive(false);
    }
}
