using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class DrawerInteractible : XRGrabInteractable
{
    [SerializeField]
    XRSocketInteractor keySocket;

    [SerializeField]
    bool isLocked;

    [SerializeField]
    Transform drawerTransform;

    Transform parentTransform;
    const string defaultLayer = "Default";
    const string grabLayer = "Grab";
    bool isGrabbed;
    Vector3 limitPositions;
    [SerializeField]
    Vector3 limitDistances = new Vector3(0.02f, 0.02f, 0);

    private void Awake()
    {
        if (keySocket != null)
        {
            //Subscribe to the event "selectEntered" for the given xrSocketInteractor script
            keySocket.selectEntered.AddListener(OnDrawerUnlocked);
            keySocket.selectExited.AddListener(OnDrawerLocked);
        }

        parentTransform = transform.parent.transform;
        limitPositions = drawerTransform.localPosition;
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

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        if (!isLocked)
        {
            transform.SetParent(parentTransform);
            isGrabbed = true;
        }
        else
        {
            ChangeLayerMask(defaultLayer);
            isGrabbed = false;
        }
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        ChangeLayerMask(grabLayer);
        isGrabbed = false;
        transform.localPosition = drawerTransform.localPosition;
    }

    private void Update()
    {
        if (isGrabbed && drawerTransform != null)
        {
            drawerTransform.localPosition = new Vector3(
                drawerTransform.localPosition.x, 
                drawerTransform.localPosition.y, 
                transform.localPosition.z);

            CheckLimits();
        }
    }

    private void CheckLimits()
    {
        if (transform.localPosition.x >= limitPositions.x + limitDistances.x
            || transform.localPosition.x <= limitPositions.x - limitDistances.x)
        {
            ChangeLayerMask(defaultLayer);
        }
        else if (transform.localPosition.y >= limitPositions.y + limitDistances.y
            || transform.localPosition.y <= limitPositions.y - limitDistances.y)
        {
            ChangeLayerMask(defaultLayer);
        }
    }

    private void ChangeLayerMask(string mask)
    {
        interactionLayers = InteractionLayerMask.GetMask(mask);
    }
}
