using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TCPeasy

{   
    public class EventManager : MonoBehaviour
    {
        // for argument passed to the event
        // public delegate void ReceivedData(EventInfo eventInfo);
        
        // no argument passed to the event
        public delegate void ReceivedData();
        public static event ReceivedData OnReceivedData;
        public static int _sizeVpp;


        // condition for the event to be triggered
        public static int SizeVpp
        {


            get
            {
                // catch the value of _sizeVpp
                return _sizeVpp;
            }
            set
            {
                // if the value caught (SizeVpp) is different than the new one --> fire the event
                //if (_sizeVpp == value) return;
                if (_sizeVpp == value) return; // check if the new list size is inferior or equal to the previous value

                // PROBLEM : this make it stay at the number erased .... 
                if (_sizeVpp < value) // only fire if size Vpp > preious value
                {
                    // check that there is a listener inside the event
                    if (OnReceivedData != null)
                    {
                        OnReceivedData();
                    }
                }

                _sizeVpp = value;

            }
        }


        // to fire the event with an argument (class EventInfo)

       // public void FireEvent(EventInfo eventInfo)
        //{
        // 
            
                // check that there is a listener inside the event
        //        if (OnReceivedData != null)
         //       {
         //           OnReceivedData(eventInfo);
         //       }
            

       // }
    }


}

