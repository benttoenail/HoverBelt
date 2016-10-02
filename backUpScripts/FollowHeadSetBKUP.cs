using UnityEngine;
using System.Collections;

public class FollowHeadSetBKUP : MonoBehaviour {

    public GameObject headSet;
    public Material beltMat;

    Vector3 beltPosition;
    Vector3 fixedPosition;
    Quaternion beltRotation;

    public bool beltIsFixed = false;

    public float height; //Height of user
    public float moveSpeed; //Speed of how fast the belt follows the user
    public float headTilt; //Angle which opens the belt

    Transform[] children;

    // Use this for initialization
    void Start()
    {

    }
	

	// Update is called once per frame
	void Update () {

        //CheckHeadSetAngle();

        //Children objects look at headset
        foreach (Transform child in transform)
        {
            child.transform.LookAt(headSet.transform.position);
        }

        //Check to see if belt is fixed in space
        if (!beltIsFixed)
        {
           //Debug.Log("Moving with headset");
           MoveWithHeadSet();
        }

	}


    //Look for the headset angle
    void CheckHeadSetAngle()
    {
        if (headSet.transform.eulerAngles.x > headTilt && headSet.transform.eulerAngles.x < 90.0f && beltIsFixed == false)
        {
            FixBeltPosition();
            StartCoroutine(FadeTo(1.0f, 1.0f));
            beltIsFixed = true;
        }
        else if (headSet.transform.eulerAngles.x < headTilt && beltIsFixed == true)
        {
            StartCoroutine(FadeTo(0.0f, 1.0f));
            beltIsFixed = false;
        }
    }


    //When Belt is not locked in place
    void MoveWithHeadSet()
    {
        beltPosition = new Vector3(headSet.transform.position.x, headSet.transform.position.y + height, headSet.transform.position.z);

        gameObject.transform.position = Vector3.Lerp(transform.position, beltPosition, Time.deltaTime * moveSpeed);

        beltRotation = Quaternion.Euler(new Vector3(0, headSet.transform.eulerAngles.y - 90, 0));

        transform.rotation = Quaternion.Slerp(transform.rotation, beltRotation, Time.deltaTime * moveSpeed);
    }


    //Once head is tilted down enough, show the belt and fix the position + rotation
    void FixBeltPosition()
    {
        Debug.Log("Fixing the belt position");

        beltPosition = new Vector3(headSet.transform.position.x, headSet.transform.position.y + height, headSet.transform.position.z);

        gameObject.transform.position = Vector3.Lerp(transform.position, beltPosition, Time.deltaTime * moveSpeed);

        beltRotation = Quaternion.Euler(new Vector3(0, headSet.transform.eulerAngles.y - 90, 0));

        transform.rotation = Quaternion.Slerp(transform.rotation, beltRotation, Time.deltaTime * moveSpeed);
    }


    //Fade the Alpha channel of the shader
    IEnumerator FadeTo(float aValue, float aTime)
    {
        float alpha = beltMat.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            beltMat.color = newColor;
            yield return null;
        }
    }

}
