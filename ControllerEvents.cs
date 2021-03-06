﻿using UnityEngine;
using System.Collections;

public class ControllerEvents : MonoBehaviour {

    //Find the conontrollerrs device
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;
    
    //Variables for Creating Colliders and Rigidbodies at Runtime
    public GameObject customRigidbodyObject;
    private GameObject controllerCollisionDetector;
    private Object defaultColliderPrefab;
    private bool destroyColliderOnDisable;
    private bool controllerIsIntersecting;
    private Rigidbody touchRigidBody;

    //Events
    public delegate void ControllerEventHandler();
    public static event ControllerEventHandler ControllerEntered;
    public static event ControllerEventHandler ControllerExited;
    public static event ControllerEventHandler ControllerTriggerPressed;
    public static event ControllerEventHandler ControllerTriggerUp;
    public static event ControllerEventHandler ControllerCollideAndTrigger;


    void Awake()
    {
        destroyColliderOnDisable = false;
        defaultColliderPrefab = Resources.Load("ControllerColliders/HTCVive");
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }


    void OnEnable()
    {
        //Create the collider and rigidbody at runtime
        CreateTouchCollider();
        CreateTouchRigidBody();

    }

    void Update()
    {
        device = SteamVR_Controller.Input((int)trackedObj.index);

        if (device != null)
        {
            //Fire event when Trigger is Presssed - check if intersecting
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                if (controllerIsIntersecting == true)
                {
                    ControllerCollideAndTrigger();
                }
                else
                {
                    ControllerTriggerPressed();
                }
            }

            //When Trigger is Released
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger)) //This is not finding the controller object??
            {
                //ControllerTriggerUp();
            }
        }
    }


    //CREATE RIGID BODIES AND COLLIDERS
    private bool CustomRigidBodyIsChild()
    {
        foreach (var childTransform in GetComponentsInChildren<Transform>())
        {
            if (childTransform != transform && childTransform == customRigidbodyObject.transform)
            {
                return true;
            }
        }
        return false;
    }


    private void CreateTouchCollider()
    {
        if (customRigidbodyObject == null)
        {
            controllerCollisionDetector = Instantiate(defaultColliderPrefab, transform.position, transform.rotation) as GameObject;
            controllerCollisionDetector.transform.SetParent(transform);
            controllerCollisionDetector.transform.localScale = transform.localScale;
            controllerCollisionDetector.name = "ControllerColliders";
            destroyColliderOnDisable = true;
        }
        else
        {
            if (CustomRigidBodyIsChild())
            {
                controllerCollisionDetector = customRigidbodyObject;
                destroyColliderOnDisable = false;
            }
            else
            {
                controllerCollisionDetector = Instantiate(customRigidbodyObject, transform.position, transform.rotation) as GameObject;
                controllerCollisionDetector.transform.SetParent(transform);
                controllerCollisionDetector.transform.localScale = transform.localScale;
                destroyColliderOnDisable = true;
            }
        }
    }


    private void CreateTouchRigidBody()
    {
        touchRigidBody = gameObject.GetComponent<Rigidbody>();
        if (touchRigidBody == null)
        {
            touchRigidBody = gameObject.AddComponent<Rigidbody>();
            touchRigidBody.isKinematic = true;
            touchRigidBody.useGravity = false;
            touchRigidBody.constraints = RigidbodyConstraints.FreezeAll;
            touchRigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }
    }


    //Collision Detection
    void OnTriggerEnter(Collider ring)
    {
        if (controllerIsIntersecting == false && ring.gameObject.name == "CollisionCube")
        {
            ControllerEntered();
            controllerIsIntersecting = true;
            device.TriggerHapticPulse(700);
        }
    }

    void OnTriggerExit(Collider ring)
    {
        if (controllerIsIntersecting == true)
        {
            ControllerExited();
            controllerIsIntersecting = false;
        }
    }
}
