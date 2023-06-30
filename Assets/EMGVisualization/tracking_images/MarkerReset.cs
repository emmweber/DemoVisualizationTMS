using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerReset : MonoBehaviour
{
    public Vector3 speed;
    public float ThresholdSpeed;
    public float magnitudeSpeed;
    public GameObject _tracker;

    
    //public Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {

       
    }

    // Update is called once per frame
    void Update()
    {


        speed = gameObject.GetComponent<Rigidbody>().velocity;
        magnitudeSpeed = Mathf.Sqrt(speed.sqrMagnitude);
        
        if (magnitudeSpeed > ThresholdSpeed)
        {
            resetPosition();
        }

    }


    void resetPosition()
    {
        gameObject.transform.position = _tracker.transform.position;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Debug.Log("High speed. resetting the position");
    }


}
