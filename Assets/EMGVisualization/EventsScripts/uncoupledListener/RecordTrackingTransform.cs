using System.Collections.Generic;
using UnityEngine;

namespace TCPeasy
{
    public class RecordTrackingTransform : MonoBehaviour
    {

        MainListener _mainListener;

        // different tracking options: --------------------------------------
        // with Vuforia
        //DefaultTrackableEventHandler _track;

        // with ML
        //MLImageTrackerBehavior _track;

        // to use a smoother tracking system
        private GameObject _track;
        // ---------------------------------------------------------------------

        // List of all the coil pose transforms
        public List<Transform> _trackObjList;
        public List<Vector3> _trackPosList;
        // only used when used offline
       // public GameObject _debugPlaceholder;
        public bool _debugging = false;


        // Start is called before the first frame update
        void Start()
        {
            _mainListener = GameObject.FindObjectOfType<MainListener>();

            // to change with the tracking method -----------------------------
            // Vuforia
            //_track = GameObject.FindObjectOfType<DefaultTrackableEventHandler>(); //Vuforia


            //ML
            //_track = GameObject.FindObjectOfType<MLImageTrackerBehavior>();

            // Smoother tracking system  (to test)
            _track = GameObject.Find("SmoothTracking");

            // ----------------------------------------------------------------------
            EventManager.OnReceivedData += ListenerStoreTransformList;
        }

        /// <summary>
        /// Stores the transform of the coil in a list
        /// </summary>
        void ListenerStoreTransformList()
        {
            // size of the List of received values (=number of stimulations)
            int triggerNumber = EventManager.SizeVpp;

            if (_debugging==false)
            {
                // Store the transforms to reuse them at each trigger
                // get the transform of the tracked object (Smoothtracking cube, which is linked via spring joint to coil)
                //Transform trackTransform = _track.GetComponent<Transform>(); // Vuforia and ML
                //_trackObjList.Add(trackTransform);

                Vector3 pos = _track.transform.position;
                _track.transform.position = pos;
                _trackPosList.Add(pos);
                //_trackObjList.Add(trackTransform);
            }
            else
            {
                // temporary : for debugging without webcam/Vuforia, generate random point in the 3 directions
                float randrange = 2.0f;
                Vector3 pos = new Vector3(Random.Range(-randrange, randrange), Random.Range(-randrange, randrange), Random.Range(-randrange, randrange));
                //_debugPlaceholder.transform.position = pos;
                //Transform randTransform = _debugPlaceholder.transform;
                //Transform randTransform = _track.transform;
               // _track.transform.position = pos;


                // looks like the transform is uopdated with the GO !!!
                //_trackObjList.Add(randTransform);
                _trackPosList.Add(pos);



            }

        }


    }
}
