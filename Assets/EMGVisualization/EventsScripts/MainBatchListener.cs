using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TCPeasy
{
    public class MainBatchListener : MonoBehaviour
    {
        public List<float> VppList;
        public List<float> epochList;
        public List<int> colorID;

        // test to check i it is a problem with the condition
        public List<float> VppListFull;

        ClientJSON2 _clientJSON2;
        public int epochID;
        public float Vpp;
        public int sizeVpp;
        public int batchNumber;


        // Start is called before the first frame update
        void Start()
        {
            _clientJSON2 = FindObjectOfType<ClientJSON2>();
            EventManager.OnReceivedData += FireEvent;

            // keep track of the batch of each Vpp
            eventTrackerPosition.OnTrackerPositionchange += ChangeBatchNumber;



        }

        // Update is called once per frame
        void Update()
        {
            epochID = _clientJSON2.epochID;
            Vpp = _clientJSON2.Vpp;

            if (!epochList.Contains(epochID) && Vpp != 0 && !VppList.Contains(Vpp) && !VppListFull.Contains(Vpp))
            {
                Debug.Log("size VppList" + sizeVpp.ToString());

                Debug.Log("size VppListPrevious" + VppListFull.Count.ToString());
                // compensate for discrepencies of  index when the client or the server connect first
                if (epochList.Count == 0 && epochID == 1)
                {
                    epochID = 0;
                }

                // fill up the lists
                VppList.Add(Vpp);
                epochList.Add(epochID);

                // sizeVpp is used to triggered the event so has to be updated
                sizeVpp = VppList.Count;
                //VppListFull.Add(Vpp);

                // colorID is used to change batch 
                colorID.Add(batchNumber);

                // this command triggers the Event
                EventManager.SizeVpp = sizeVpp;





            }

        }


        // Trigger listener ---------------------------------------
        public void FireEvent()
        {
            // to check that the vent is reallly started
            Debug.Log("KameHameHAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");

        }



        public void ChangeBatchNumber()
        {
            // increment if coil has moved
            if (epochList.Count > 0)
            {
                batchNumber += 1;
            }
            
        }

    }
}


