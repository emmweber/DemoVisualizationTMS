using System.Collections.Generic;
using UnityEngine;
using MagicLeap.Core;

namespace TCPeasy
{
    public class RecordTrackingTransformMax : MonoBehaviour
    {

        MainListenerMax _mainListener;

        // different tracking options: --------------------------------------
        // with Vuforia
        //DefaultTrackableEventHandler _track;

        // with ML
        //MLImageTrackerBehavior _track;

        // to use a smoother tracking system
        //private GameObject _track;
        
        MLImageTrackerBehavior _track;
        // ---------------------------------------------------------------------

        // List of all the coil pose transforms
        //public List<Transform> _trackObjList;
        public List<Vector3> _trackPosList;
        public List<int> _coilPositions;
        private int p_coilID;

        public float _thresholdCoilPosition;

        // only used when used offline
        // public GameObject _debugPlaceholder;
        public bool _debugging;



        // Called prior to Start()
        private void Awake()
        {
            // the store transform list function must be called BEFORE the colorList filling. 
            // This means it has to be registered before it. That's why it is in the Awake() function while 
            // the registration to the event for the other scripts happen in the Start() function
            EventManager.OnReceivedData += ListenerStoreTransformList;

        }


        // Start is called before the first frame update
        void Start()
        { 

            _mainListener = GameObject.FindObjectOfType<MainListenerMax>();
            p_coilID = 0;


            // to change with the tracking method -----------------------------
            // Vuforia
            //_track = GameObject.FindObjectOfType<DefaultTrackableEventHandler>(); //Vuforia


            //ML
            if (_debugging == false)
            {
                //_track = GameObject.FindObjectOfType<MLImageTrackerBehavior>();
            }
            // Smoother tracking system  (to test)
            //_track = GameObject.Find("SmoothTracking"); 
            // -------------------------------------------------------------------
            //
            // Problem if using a spring if we want the rotation to be accurate ! 
            // it is good for the position and have a more stable tracking of it though
            //
            // ----------------------------------------------------------------------
        }

        /// <summary>
        /// Stores the transform of the coil in a list
        /// </summary>
        void ListenerStoreTransformList()
        {
            // size of the List of received values (=number of stimulations)
            int triggerNumber = EventManager.SizeVpp;

            if (_debugging == false)
            {
                // Store the transforms to reuse them at each trigger
                // get the transform of the tracked object (Smoothtracking cube, which is linked via spring joint to coil)

                Vector3 pos = _track.transform.position;
                _track.transform.position = pos;
                _trackPosList.Add(pos);

            }
            else
            {
                //  for debugging without webcam/Vuforia, generate random point in the 3 directions
                float randrange = 2.0f;
                Vector3 pos = new Vector3(Random.Range(-randrange, randrange), Random.Range(-randrange, randrange), Random.Range(-randrange, randrange));


                _trackPosList.Add(pos);



            }

            // compare coil positions and check if it moves compared to last received stream
            StoreCoilPositionList();

        }


        void StoreCoilPositionList()
        {
            int numberstreams = _trackPosList.Count;

            if (numberstreams >= 2)
            {
                Vector3 previousCoilPos = _trackPosList[numberstreams - 2];
                Vector3 currentCoilPos = _trackPosList[numberstreams - 1];


                float difference = Mathf.Abs(previousCoilPos.magnitude - currentCoilPos.magnitude);

                if (difference > _thresholdCoilPosition)
                {
                    p_coilID += 1;

                }
            }



            _coilPositions.Add(p_coilID);

        }


    }
}

