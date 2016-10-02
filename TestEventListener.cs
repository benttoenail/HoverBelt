using UnityEngine;
using System.Collections;

public class TestEventListener : MonoBehaviour {

	// Use this for initialization
	void Start () {

        ControllerEvents.ControllerEntered += ControllerEntered;
        ControllerEvents.ControllerExited += ControllerExited;
        ControllerEvents.ControllerTriggerPressed += ControllerTriggerPressed;
        ControllerEvents.ControllerCollideAndTrigger += ControllerCollideAndTrigger;

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ControllerEntered()
    {
        Debug.Log("Controller has ENTERED the building!");
    }

    public void ControllerExited()
    {
        Debug.Log("Controller has LEFT the building!");
    }

    public void ControllerTriggerPressed()
    {
        Debug.Log("Trigger Has been Pressed!!");
    }

    public void ControllerCollideAndTrigger()
    {
        Debug.Log("Trigger pressed Whilst collision is taking place!!!");
    }
}
