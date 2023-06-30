using System.Collections.Generic;
using UnityEngine;

namespace TCPeasy
{
    public class CreateColorCoilPosition : MonoBehaviour
    {
        // initialization
        MainListener _mainListener;

        // contains all colors for all simulations, will be constantly updated when new values are recived
        public List<Color> _colorListColor = new List<Color>();

        // List that contains indices for each different coil pose (same coil poses have same index)
        public List<int> _coilIndexList;

        // List that contains all mean stimulation values for each coil pose
        private List<float> _meanVppList = new List<float>();

        // fire the second event whrn the list in full -----
        //public bool fireevent2;

        // Start is called before the first frame update
        void Start()
        {
            _mainListener = GameObject.FindObjectOfType<MainListener>();

            // OnReceivedData Event subscription
            EventManager.OnReceivedData += ListenerCreateColorList;
        }

        /// <summary>
        /// Creates a list with colors for each coil pose
        /// </summary>
        public void ListenerCreateColorList()
        {
            //_colorListColor.Clear();
            _colorListColor = new List<Color>();
            _meanVppList = new List<float>();

            List<float> vppList = _mainListener._vppList;

            _coilIndexList = _mainListener._coilIndexList;

            // loop over the different coil positions
            int numberidx = _coilIndexList.Count;
            int numberCoilPositions = _coilIndexList[numberidx - 1];
            
            for (int i = 0; i <= numberCoilPositions; i++)
            {
                // Create a sublist 
                List<float> subVppList = Gatherpositions(vppList, _coilIndexList, i);
                
                // Calculate mean for each coil position
                float mean = CalculateMean(subVppList);
                _meanVppList.Add(mean);

            }


            // final calculation of the Color associated with the 
            if (_meanVppList.Count > 1)
            {
                _colorListColor = CreateFinalColorList(_meanVppList);
            }
            else 
            {
                _colorListColor = CreateFinalColorListFirstElement(vppList, _meanVppList[0]);
            }


            //fireevent2 = true;

        }

        // end of Listener -----------------------------------------------------------------


        // functions -----------------------------------------------------------------------
        /// <summary>
        /// gathers all the stimulations in the same coil position and saves them in a list
        /// </summary>
        /// <returns>The gatherpositions.</returns>
        /// <param name="ListValues">List values.</param>
        /// <param name="ListIdx">List index.</param>
        /// <param name="idx">Index.</param>
        List<float> Gatherpositions(List<float> ListValues, List<int> ListIdx, int idx)
        {
            List<float> subList=new List<float>();

            for (int i=0; i<ListIdx.Count; i++)
            {
                if (ListIdx[i] == idx)
                {
                    subList.Add(ListValues[i]);
                }
            }
            return subList;
        }    


        /// <summary>
        /// Calculates the mean of all stimulations in the same coil pose
        /// </summary>
        /// <returns>The mean.</returns>
        /// <param name="ListFloat">List float.</param>
        float CalculateMean(List<float> ListFloat)
        {
            float sum = 0;
            float mean;
            foreach (float idx in ListFloat)
            {
                sum += idx;
            }

            mean = sum / ListFloat.Count;

            return mean;
        }

        /// <summary>
        /// Assigns a color to each coil pose up to now
        /// </summary>
        /// <returns>The final color list.</returns>
        /// <param name="List">List.</param>
        List<Color> CreateFinalColorList(List<float> List)
        {

            // take max and min amplitudes of the list for scaling factor
            float maxAmplitude = Mathf.Max(List.ToArray());
            float minAmplitude = Mathf.Min(List.ToArray());


            if (List.Count < 2)
            {
                _colorListColor.Add(Color.red);
            }
            else
            {
                // create color list that contains color of each gameobject
                foreach (float idx in List)
                {
                    // scaling factor            
                    float scaledvalue = (idx - minAmplitude) / (maxAmplitude - minAmplitude);

                    // store the colors in a list
                    _colorListColor.Add(Color.Lerp(Color.blue, Color.red, scaledvalue));

                }
            }


            return _colorListColor;
        }

        /// <summary>
        /// Creates the first element of the color list for all coil poses
        /// </summary>
        /// <returns>The final color list first element.</returns>
        /// <param name="List">List.</param>
        /// <param name="mean">Mean.</param>
        List<Color> CreateFinalColorListFirstElement(List<float> List, float mean)
        {

            // take max and min amplitudes of the list for scaling factor
            float maxAmplitude = Mathf.Max(List.ToArray());
            float minAmplitude = Mathf.Min(List.ToArray());

            _colorListColor = new List<Color>();

            if (List.Count < 2)
            {
                _colorListColor.Add(Color.red);
            }
            else
            {
                // create a unique color with mean of list
                
                
                    // scaling factor            
                    float scaledvalue = (mean - minAmplitude) / (maxAmplitude - minAmplitude);

                    // store the colors in a list
                    _colorListColor.Add(Color.Lerp(Color.blue, Color.red, scaledvalue));

                
            }


            return _colorListColor;
        }


    }
}

