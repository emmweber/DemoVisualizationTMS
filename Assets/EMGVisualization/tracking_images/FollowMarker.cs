using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicLeap.Core;


public class FollowMarker : MonoBehaviour
{

    public float FilterDisplacement;
    //private float position_former;
    public Vector3 positionMarker;
    public Vector3 positionAttachedGO;

    // with trackinng 
    //private MLImageTrackerBehavior _tracker;

    // for testing
    public GameObject _tracker;
    private List<Vector3> buffer;
    public float displacement;

    

    // Start is called before the first frame update
    void Awake()
    {
        // with tracking 
        //MLImageTrackerBehavior _tracker = GameObject.FindObjectOfType<MLImageTrackerBehavior>();
        // _tracker.OnTargetLost += CubeAppears();

        // for testing without tracking
        //_tracker = GameObject.Find("Sphere1");

        // initialization 
        positionMarker = _tracker.transform.position;
        positionAttachedGO = gameObject.transform.position;
        
        buffer.Add(positionMarker);


    }

    // Update is called once per frame
    void Update()
    {

        MarkerPositionStabilization();

    }



    // MLImageTrackerBehavior.StatusUpdate CubeAppears()
    //{
    //   Debug.Log("tracker disappeared");

    //}


    public void MarkerPositionStabilization()
    {
        
        Vector3 meanMarkerPosition = CalculateAveragePosition(buffer);       
        positionAttachedGO = gameObject.transform.position;

        displacement = CalculateDisplacement(meanMarkerPosition,positionAttachedGO);
        
        if (displacement > FilterDisplacement)
        {
            gameObject.transform.position = _tracker.transform.position;
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

            // reinitialize buffer
            buffer = new List<Vector3>();

        }
        
        buffer.Add(positionMarker);
    }

    // calculate mean position 
    Vector3 CalculateAveragePosition(List<Vector3>  list)
    {
        float sumx=0f;
        float sumy = 0f;
        float sumz = 0f;
        

        foreach (Vector3 f in list)
        {

           sumx += f.x;
           sumy += f.y;
           sumz += f.z;
         }

        Vector3 mean = new Vector3(sumx / list.Count, sumy / list.Count, sumz/list.Count);

        return mean;
    }


    // Calculate mean displacement

    float CalculateDisplacement(Vector3 position1, Vector3 position2)
    {
        float displacement;

        float d_x = position1.x - position2.x;
        float d_y = position1.y - position2.y;
        float d_z = position1.z - position2.z;

        displacement = Mathf.Sqrt(d_x * d_x + d_y * d_y + d_z * d_z);

        return displacement;
    }





}

