using UnityEngine;
using System.Collections;

public class ToggleOpenBelt : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;

    public bool controllerIsIntersecting = false;

	// Use this for initialization
	void Start () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void Update () {

        device = SteamVR_Controller.Input((int)trackedObj.index);

        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            device.TriggerHapticPulse(700);
        }
	
	}

    void OnTriggerEnter(Collider ring)
    {
        controllerIsIntersecting = true;
    }

    void OnTriggerExit(Collider ring)
    {
        controllerIsIntersecting = false;
    }
}
