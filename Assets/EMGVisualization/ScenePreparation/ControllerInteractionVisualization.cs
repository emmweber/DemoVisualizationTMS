using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicLeap.Core;
using MagicLeapTools;
#if PLATFORM_LUMIN
using UnityEngine.XR.MagicLeap;
#endif


namespace TCPeasy
{
    public class ControllerInteractionVisualization : MonoBehaviour
    {
        // This script is attached to the gameobject (brain mesh) at the begining of the app.
        // It is aim at moving the GameObject when the trigger is pressed. 
        // trigger pressed--> gameobject is child of the controller
        // trigger released --> GameoBject is no longer the child of the controller

        public ControlInput _controlInput;
        public GameObject controller;
        public GameObject _mesh;

        private void Start()
        {


            // Find controller 
            controller = GameObject.Find("Controller");

            // Find control input ( the script where the controller button events are set)
            _controlInput = GameObject.FindObjectOfType<ControlInput>();

            // find mesh 
            _mesh = GameObject.FindObjectOfType<ScenePreparationMax>().BrainMeshGO;

            // add listeners to the buttons ------------------
            _controlInput.OnTriggerDown.AddListener(MoveMesh);
            _controlInput.OnTriggerUp.AddListener(StabilizeMesh);


        }


        // function to be called and registered to buttons ------------------------------
        void MoveMesh()
        {

            _mesh.transform.SetParent(controller.transform);

        }

        void StabilizeMesh()
        {
            _mesh.transform.parent = null;
        }



        // This is a work in progress --> aim at only register the listeners to the button when the controller collides with the mesh...
        // not working well for the moment


        //private void OnCollisionEnter(Collision collision)
        //{   
        // when sphere collide , register the element
        //_controlInput.OnTriggerDown.AddListener(MoveMesh);
        //_controlInput.OnTriggerUp.AddListener(StabilizeMesh);

        //gameObject.GetComponent<Material>().color = Color.blue;


        //}

        //private void OnCollisionExit(Collision collision)
        //{
        //gameObject.GetComponent<Material>().color = currentGOColor;
        // unsubscribe the event
        //_controlInput.OnTriggerDown.RemoveListener(MoveMesh);
        //_controlInput.OnTriggerDown.RemoveListener(StabilizeMesh);
        //}

    }
}
