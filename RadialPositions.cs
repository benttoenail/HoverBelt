using UnityEngine;
using System.Collections;

public class RadialPositions : MonoBehaviour {

	public int count;
    public float angle; //Radial distance between buttons
    public float space = 1.0f; //Distance from user
    public float angularSpace;

	public GameObject radialCube;
	GameObject[] nodes;
    Transform[] nodeIcon;
	Quaternion q;

    bool beltIsFixed;
    bool beltIsOpen = false;

	// Use this for initialization
	void Start () {

        angle = 0;

		nodes = new GameObject[count];
        nodeIcon = new Transform[count];
		GameObject temp = new GameObject();
		
        //Rotate and instantiate Object
		for(int i = 0; i < count; i++){

			q.eulerAngles = new Vector3(0, i * angle / count, 0); //set radial angle
			temp = Instantiate(radialCube, new Vector3(0,0,0), q) as GameObject;
            nodes[i] = temp;
            nodeIcon[i] = nodes[i].transform.GetChild(0);
            //nodes[i].transform.SetParent(gameObject.transform); //For some reason parenting the HoverBelt_02 is not working??
        }

	}

	// Update is called once per frame  
	void Update () {

        beltIsFixed = gameObject.GetComponent<HoverBelt>().beltPoisitionFixed;

        //Attach to the HoverBelt_02 Object
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].transform.position = gameObject.transform.position;
            nodes[i].transform.eulerAngles = new Vector3(0, i * angle / count + transform.eulerAngles.y, 0);
        }

        if (beltIsFixed == true)
        {
            StartCoroutine(OpenBelt());
        }
        if(beltIsFixed == false)
        {
            StartCoroutine(CloseBelt());
        }

    }


    //OPENING / CLOSING BELT ANIMATIONS
    IEnumerator OpenBelt()
    {
        
        if (beltIsFixed && !beltIsOpen)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                iTween.MoveBy(nodeIcon[i].gameObject, iTween.Hash("x", space, "time", 1.0f));
               
            }
            beltIsOpen = true;

            yield return new WaitForSeconds(0.6f);
            iTween.ValueTo(gameObject, iTween.Hash("from", angle, "to", angularSpace, "time", 1.0f, "onupdate", "TweenBeltAngle", "easetype", iTween.EaseType.easeInOutQuad));
        }

    }


    IEnumerator CloseBelt()
    {
        if (!beltIsFixed && beltIsOpen)
        {
            iTween.ValueTo(gameObject, iTween.Hash("from", angle, "to", 0, "time", 0.5f, "onupdate", "TweenBeltAngle", "easetype", iTween.EaseType.easeInOutQuad));
            beltIsOpen = false;
            yield return new WaitForSeconds(0.35f);

            for (int i = 0; i < nodes.Length; i++)
            {
                iTween.MoveBy(nodeIcon[i].gameObject, iTween.Hash("x", -space, "time", 0.5f, "easetype", iTween.EaseType.easeInQuad));
            }
            
        }
    }
   

    void TweenBeltAngle(float _angle)
    {
        angle = _angle;
    }

}
