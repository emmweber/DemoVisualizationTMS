using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicLeap.Core;


namespace TCPeasy
{

    public class eventTrackerPosition : MonoBehaviour
    {
        public delegate void changeTrackerPosition();
        public static event changeTrackerPosition OnTrackerPositionchange;

        //public Vector3 speed;
        //public float ThresholdSpeed;
        //public float magnitudeSpeed;
        public float MeanDisplacement;
        public Vector3 displacement;
        public float maxDisplacement;

        public MLImageTrackerBehavior _imageTracker;

        public bool _debugging;
        //public GameObject smoothTracking;

        //public GameObject _tracker;

        private void Start()
        {
            _imageTracker = GameObject.FindObjectOfType<MLImageTrackerBehavior>();
            //_imageTracker.OnTargetFound += gameObject.SetActive(true) ;
            //smoothTracking = GameObject.Find("SmoothTracking");
            //_debugging = false;

        }

        // Update is called once per frame
        void Update()
        {

            if (_debugging == true)
            {


                // determine the mean displacement
                MeanDisplacement = CalculateDisplacement_debug();



                // Note: as this is possibly considereing the displacement between one frame and another, this is not representative of the total motion.
                // this means that the option to only work with the velocity made sense as well
                if (MeanDisplacement > maxDisplacement)
                {
                    OnTrackerPositionchange();
                    Debug.Log("too large displacement, change of position");
                }
            }


            else
            {
                //speed = gameObject.GetComponent<Rigidbody>().velocity;
                // check whether the image is being tracked
                bool isTracking = _imageTracker.IsTracking;

                // if the image is found
                if (isTracking == true)
                {
                    //if (gameObject.activeInHierarchy == false)
                    //{
                    //   gameObject.SetActive(true);
                    //}


                    // determine the mean displacement
                    MeanDisplacement = CalculateDisplacement();



                    // Note: as this is possibly considereing the displacement between one frame and another, this is not representative of the total motion.
                    // this means that the option to only work with the velocity made sense as well
                    if (MeanDisplacement > maxDisplacement)
                    {
                        OnTrackerPositionchange();
                        Debug.Log("too large displacement, change of position");
                    }
                }
            }



        }



        float CalculateDisplacement()
        {
            // calculate the displacement according to F=k*dx
            float spring = gameObject.GetComponent<SpringJoint>().spring;
            Vector3 SpringForce = gameObject.GetComponent<SpringJoint>().currentForce;
            Vector3 velocity = gameObject.GetComponent<Rigidbody>().velocity;
            float damping = gameObject.GetComponent<SpringJoint>().damper;

            displacement = (SpringForce + damping * velocity) / spring;
            //magnitudeSpeed = Mathf.Sqrt(speed.sqrMagnitude);
            MeanDisplacement = Mathf.Abs(displacement.magnitude);

            return MeanDisplacement;

        }

        float CalculateDisplacement_debug()
        {
            // calculate the displacement according to F=k*dx
            //float spring = gameObject.GetComponent<SpringJoint>().spring;
            //Vector3 SpringForce = gameObject.GetComponent<SpringJoint>().currentForce;
            //Vector3 velocity = gameObject.GetComponent<Rigidbody>().velocity;
            //float damping = gameObject.GetComponent<SpringJoint>().damper;

            //displacement = (SpringForce + damping * velocity) / spring;
            //magnitudeSpeed = Mathf.Sqrt(speed.sqrMagnitude);

            RecordTrackingTransform posGO = GameObject.FindObjectOfType<RecordTrackingTransform>();
            List<Vector3> posLIST = posGO._trackPosList;

            if (posLIST.Count > 1)
            {
                Vector3 displacement = posLIST[posLIST.Count] - posLIST[posLIST.Count - 1];
            }
            else if (posLIST.Count == 1)
            {

                Vector3 displacement = posLIST[0];
            }
            else
            {
                Vector3 displacement = Vector3.zero;
            }

            MeanDisplacement = Mathf.Abs(displacement.magnitude);

            return MeanDisplacement;

        }



    }



}

