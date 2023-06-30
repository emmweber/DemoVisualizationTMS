namespace TCPeasy
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using MagicLeapTools;

    /// <summary>
    /// This class triggers the event ! THe veent is triggered when the size of the list changes.
    /// Note that we could just update a number, no need to create a list for the demo here.
    /// For the demo: the list is increased with a random number when the bumper is pressed.
    /// </summary>
    public class MainListenerMax_demo : MonoBehaviour
    {
        // List of all received max amplitudes 
        public List<float> VppList = new List<float>();

        ControlInput s_controllerInput;
        private float vpp;
        private int sizeVpp;

        // Start is called before the first frame update
        void Start()
        {
            s_controllerInput = GameObject.FindObjectOfType<ControlInput>();

            // add listener 
            s_controllerInput.OnBumperDown.RemoveAllListeners();
            s_controllerInput.OnBumperDown.AddListener(FireRandomValue);  
        }


        void FireRandomValue()
        {
            // Create a random value to trigger the event (the event is triggered when the size of the list changes)
            vpp = Random.Range(0f, 1f);

            // fill up the lists
            VppList.Add(vpp);

            // sizeVpp is used to triggered the event so has to be updated
            sizeVpp = VppList.Count;

            // this command triggers the Event
            EventManager.SizeVpp = sizeVpp;
        }
    }
}
