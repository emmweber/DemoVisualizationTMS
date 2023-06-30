using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowRotation : MonoBehaviour
{

    public Transform target;

    public float turnSpeed = .01f;
    public Vector3 positionShift=new Vector3(0,0,0);

    Quaternion rotGoal;

    Vector3 direction;








    void Update()

    {

        //direction = (target.position - transform.position ).normalized ;

        //rotGoal = Quaternion.LookRotation(direction);
        //rotGoal = rotGoal * target.rotation;
        // rotGoal = target.rotation;

        //transform.rotation = Quaternion.Slerp(transform.rotation, rotGoal, turnSpeed);

        // tesst ---------------


        


    }

}
        
    

