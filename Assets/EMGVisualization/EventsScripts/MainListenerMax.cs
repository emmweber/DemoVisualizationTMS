using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TCPeasy
{
    public class MainListenerMax : MonoBehaviour
    {
        // List of all received max amplitudes 
        public List<float> VppList = new List<float>();
        // List of all IDs of received amplitudes
        private List<float> epochList = new List<float>();

        // List of all the coil indeces
        public List<int> CoilIndexList = new List<int>();
        // Coil ID changes with each new coil pose
        public int CoilPositionID;

        // for the second event that starts after this one
       // public static bool _dataReceived;

        // some stuff received from EPOCHGUI
        ClientJSON2 s_clientJSON2;
        private int epochID;
        private float vpp;
        private int sizeVpp;

        // Start is called before the first frame update
        void Start()
        {
            // initialization
            s_clientJSON2 = FindObjectOfType<ClientJSON2>();

        }

        // check every frame whether a value is received, and once received, put value and associated IDs in respective lists
        void Update()
        {
            epochID = s_clientJSON2.epochID;
            vpp = s_clientJSON2.Vpp;

            // if statement to be sure that we receive a unique value and that this value is not empty
            if (!epochList.Contains(epochID) && vpp > Mathf.Epsilon && !VppList.Contains(vpp))
            {


                // fill up the lists
                VppList.Add(vpp);
                epochList.Add(epochID);

                // associate at each Vpp a coil position
                //CoilIndexList.Add(CoilPositionID);

                // sizeVpp is used to triggered the event so has to be updated
                sizeVpp = VppList.Count;

                // this command triggers the Event
                EventManager.SizeVpp = sizeVpp;

                //_dataReceived = true;
            }

        }

    }
}
