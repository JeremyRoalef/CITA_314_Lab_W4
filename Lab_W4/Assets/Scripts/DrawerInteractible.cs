using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class DrawerInteractible : MonoBehaviour
{
    [SerializeField]
    XRSocketInteractor keySocket;

    [SerializeField]
    bool isLocked;

    private void Awake()
    {
        if (keySocket != null)
        {

            //Subscribe to the event "selectEntered" for the given xrSocketInteractor script
            keySocket.selectEntered.AddListener(OnDrawerUnlocked);
            keySocket.selectExited.AddListener(OnDrawerLocked);
        }
    }

    private void OnDrawerLocked(SelectExitEventArgs arg0)
    {
        isLocked = true;
        Debug.Log("Drawer Locked");
    }

    void OnDrawerUnlocked(SelectEnterEventArgs arg0)
    {
        isLocked = false;
        Debug.Log("Drawer unlocked");
    }
}
