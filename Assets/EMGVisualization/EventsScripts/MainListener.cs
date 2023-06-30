using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TCPeasy
{
    public class MainListener : MonoBehaviour
    {
        // List of all received max amplitudes 
        public List<float> _vppList = new List<float>();
        // List of all IDs of received amplitudes
        private List<float> _epochList = new List<float>();
        
        // List of all the coil indeces
        public List<int> _coilIndexList = new List<int>();
        // Coil ID changes with each new coil pose
        public int _coilPositionID;

        // for the second event that starts after this one
        public static bool _dataReceived;

        // some stuff received from EPOCHGUI
        ClientJSON2 _clientJSON2;
        private int _epochID;
        private float _vpp;
        private int _sizeVpp;

        // Start is called before the first frame update
        void Start()
        {
            _clientJSON2 = FindObjectOfType<ClientJSON2>();

            // keep track of coil position
            eventTrackerPosition.OnTrackerPositionchange += ChangeCoilID;
            _dataReceived = false;
        }

        // check every frame whether a value is received, and once received, put value and associated IDs in respective lists
        void Update()
        {
            _epochID = _clientJSON2.epochID;
            _vpp = _clientJSON2.Vpp;

            if (!_epochList.Contains(_epochID) && _vpp>Mathf.Epsilon && !_vppList.Contains(_vpp)) 
            {
                //Debug.Log("size VppList" + sizeVpp.ToString());

                //Debug.Log("size VppListPrevious" +VppListFull.Count.ToString());
                

                // fill up the lists
                _vppList.Add(_vpp);
                _epochList.Add(_epochID);

                // associate at each Vpp a coil position
                _coilIndexList.Add(_coilPositionID);

                // sizeVpp is used to triggered the event so has to be updated
                _sizeVpp = _vppList.Count;

                // this command triggers the Event
                EventManager.SizeVpp = _sizeVpp;

                _dataReceived = true;
            }

        }

        /// <summary>
        /// Increments the coil index when the coil is moved (stays the same as long as coil doesn't move)
        /// </summary>
        public void ChangeCoilID()
        {
            // increment if coil has moved
            if (_epochList.Count > 0 && _coilIndexList.Contains(_coilPositionID))
            {
                _coilPositionID += 1;
            }
        }


    }
}
