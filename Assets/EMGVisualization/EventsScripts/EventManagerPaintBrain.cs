using UnityEngine;


namespace TCPeasy
{
    [RequireComponent(typeof(MainListener))]
    public class EventManagerPaintBrain : MonoBehaviour
    {
        // This script is meant to help with timing of the called functions. It defines an event 2 that is triggered when the listeners of the first event 
        // finished. (it is to wait for the Lists to be filled up). Note that the condition can be modified later, for now, it starts when _recordTrackingTransform._trackPosList is full

        // no argument passed to the event
        public delegate void ProcessedData();
        public static event ProcessedData OnProcessedData;

        RecordTrackingTransformMax _recordTrackingTransform;
        CreateColor _createColor;
        //public bool fireevent2;

        //public bool fireevent2;

        private void Start()
        {
            _recordTrackingTransform = GameObject.FindObjectOfType<RecordTrackingTransformMax>();


            _createColor = GameObject.FindObjectOfType<CreateColor>();
            //fireevent2 = false;

        }

        private void Update()
        {

            //fireevent2 = _createColorCoilPosition.fireevent2;

            if (_recordTrackingTransform._trackPosList.Count == EventManager.SizeVpp && _createColor._colorList.Count == EventManager.SizeVpp && MainListener._dataReceived == true)

            {
                if (OnProcessedData != null)
                {


                    // fire the drawing event once the data preprocessing has been done
                    OnProcessedData();

                    // reset the boleans 
                    MainListener._dataReceived = false;
                    //_createColorCoilPosition.fireevent2 = false;


                }
            }

        }

    }
}

