using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class PermissionManager : MonoBehaviour
{
    // Ask user for permission to make calls
    public void AskForPermission()
    {
        if (!Permission.HasUserAuthorizedPermission("android.permission.CALL_PHONE"))
            Permission.RequestUserPermission("android.permission.CALL_PHONE");

        if(!Permission.HasUserAuthorizedPermission("android.permission.READ_PHONE_STATE"))
            Permission.RequestUserPermission("android.permission.READ_PHONE_STATE");
    }
    // Check if user has permitted calls
    public bool ReadyToCall()
    {
        return Permission.HasUserAuthorizedPermission("android.permission.CALL_PHONE")
            && Permission.HasUserAuthorizedPermission("android.permission.READ_PHONE_STATE");
    }
}
