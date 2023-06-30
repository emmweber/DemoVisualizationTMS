using UnityEngine;


namespace TCPeasy
{   
    [RequireComponent(typeof(MainListener))]
    public class EventManager2 : MonoBehaviour
    {
        // This script is meant to help with timing of the called functions. It defines an event 2 that is triggered when the listeners of the first event 
        // finished. (it is to wait for the Lists to be filled up). Note that the condition can be modified later, for now, it starts when _recordTrackingTransform._trackPosList is full

        // no argument passed to the event
        public delegate void ProcessedData();
        public static event ProcessedData OnProcessedData;

        RecordTrackingTransform _recordTrackingTransform;
        CreateColorCoilPosition _createColorCoilPosition;
        //public bool fireevent2;

        //public bool fireevent2;

        private void Start()
        {
            _recordTrackingTransform = GameObject.FindObjectOfType<RecordTrackingTransform>();
           
            
            _createColorCoilPosition = GameObject.FindObjectOfType<CreateColorCoilPosition>();
            //fireevent2 = false;
            
        }

        private void Update()
        {

            //fireevent2 = _createColorCoilPosition.fireevent2;

            if (_recordTrackingTransform._trackPosList.Count == EventManager.SizeVpp && _createColorCoilPosition._coilIndexList.Count== EventManager.SizeVpp && MainListener._dataReceived ==true)

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
