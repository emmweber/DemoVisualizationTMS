using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if PLATFORM_LUMIN
using MagicLeap.Core;
using MagicLeapTools;
using UnityEngine.XR.MagicLeap;
#endif


namespace TCPeasy
{
    public class TrackHead : MonoBehaviour
    {

        public GameObject Mesh;
        ControlInput _controlInput;
        GameObject Headtracking;
        public float error;



        
        


        // Start is called before the first frame update
        void Start()
        {
            // Find control input ( the script where the controller button events are set)
            _controlInput = GameObject.FindObjectOfType<ControlInput>();
            Headtracking = GameObject.Find("HeadTracking").transform.GetComponentInChildren<MLImageTrackerBehavior>().gameObject;
            _controlInput.OnSwipe.AddListener(SetUpHeadTracking);//AddListener(SetUpHeadTracking);

            error = 0.05f;

        }






       void  SetUpHeadTracking(MLInput.Controller.TouchpadGesture.GestureDirection value)
        {


            Headtracking.GetComponent<MLImageTrackerBehavior>().OnTargetFound += StartTrack();
            Debug.Log("start TRCK");


           // return null;

        }


        MagicLeap.Core.MLImageTrackerBehavior.StatusUpdate StartTrack()
        {
            
           
            // this one works !!!! (used to ....)
            Mesh.transform.SetParent(Headtracking.GetComponentInChildren<MLImageTrackerBehavior>().transform, true);

         





            // Use spring ----------------------------------
            //SpringJoint _spring = GameObject.FindObjectOfType<SpringJoint>();
            //SlowRotation _rotation = GameObject.FindObjectOfType<SlowRotation>();


            //_spring.transform.position = Mesh.transform.position;
            //_spring.transform.rotation = Mesh.transform.rotation;
            // -------------------------------



            //_rotation.positionShift = Mesh.transform.position;
            //_spring.transform.rotation = Mesh.transform.rotation;


            //Mesh.AddComponent<SmoothTracking>();
            //Mesh.GetComponent<SmoothTracking>().target = _spring.gameObject.transform;//GameObject.Find("SmoothTracking").transform;
            //_spring.minDistance = Vector3.Distance(_spring.gameObject.transform.position , Headtracking.transform.position)+error;

            Debug.Log("submarine tracked");

            return null;

        }


    }
}
