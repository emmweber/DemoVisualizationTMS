using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicLeapTools;

namespace TCPeasy
{

    public class ControllerInteractionRegistration : MonoBehaviour
    {

        public ControlInput _controlInput;
        public GameObject placePos;

        //[SerializeField]
        // Template GameObject for showing sphere placement preview
        //GameObject indicator;

        [SerializeField]
        // Point clouds of the target (cloud1) and destination object (cloud2)
        PointCloud cloud1, cloud2;

        // Reference to the current cloud to be edited
        PointCloud currentCloud;

        // Find script for registration
        calcTransform _calcTransform;

        // Position of where to place the sphere
        //Vector3 placePos;


        private void Start()
        {

            // Find control input ( the script where the controller button events are set)
            _controlInput = GameObject.FindObjectOfType<ControlInput>();
            _calcTransform = GameObject.FindObjectOfType<calcTransform>();
            currentCloud = cloud1;

            // add listeners to the buttons ------------------
            _controlInput.OnTriggerDown.AddListener(OnAddSphereButtonPressed);
            _controlInput.OnBumperDown.AddListener(OnDeleteSphere);
            //_controlInput.OnDoubleBumper.AddListener(StartOver);

            // Apply registration when spheres placed ------------------------------
            _controlInput.OnHomeButtonTap.AddListener(checkupbeforeregistration);
            _controlInput.OnHomeButtonTap.AddListener(_calcTransform.ComputeAndApplyTransform);
            _controlInput.OnDoubleBumper.AddListener(StartOver);

            _controlInput.OnTouchHold.AddListener(EndRegistration);
            
           

        }

        // function to be called and registered to buttons ------------------------------
        public void OnSwapCloudButtonPressed()
        {
            if (currentCloud == cloud1)
                currentCloud = cloud2;

            else if (currentCloud == cloud2)
                currentCloud = cloud1;
        }
        public void OnAddSphereButtonPressed()
        {
            if (currentCloud == cloud1) {
                if (cloud1.transform.childCount < cloud2.transform.childCount)
                {
                    currentCloud.AddSphere(placePos.transform.position);
                }
            }

            else if (currentCloud == cloud2)
            {
                if (cloud2.transform.childCount < cloud1.transform.childCount)
                {
                    currentCloud.AddSphere(placePos.transform.position);
                }
            }
        }

        public void OnDeleteSphere()
        {
            currentCloud.DeleteClosestSphere(placePos.transform.position);
        }



        public void StartOver()
        {
            currentCloud.ClearChildren();
            cloud2.gameObject.SetActive(false);
            

            //_calcTransform.srcContainer.transform.position =  new Vector3(-1,0,1);

        }


        public void checkupbeforeregistration()
        {
            if (cloud2.gameObject.activeInHierarchy == false)
            {
                cloud2.gameObject.SetActive(true);
            }


        }


        public void EndRegistration()
        {
            _controlInput.OnTriggerDown.RemoveAllListeners();
            _controlInput.OnBumperDown.RemoveAllListeners();
            _controlInput.OnHomeButtonTap.RemoveAllListeners();
            _controlInput.OnDoubleBumper.RemoveAllListeners();

            Debug.Log("registration over");

            //GameObject.Find("[EMG Visualization]").SetActive(true);
             


        }


        public void NextControllerButtons()
        { 
            
        }



    }
}
