using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PaintIn3D;


namespace TCPeasy
{ 

public class CreateColor : MonoBehaviour

{

        // initialization

        // This script receives the vpp value and store it in a list 
        MainListenerMax _mainListener;

        // contains all colors for all simulations, will be constantly updated when new values are received
        public List<Color> _colorList = new List<Color>();

        // List that contains indices for each different coil pose (same coil poses have same index): NOT IMPLEMENTED
        public List<int> _coilIndexList;


        // to simplify the logic and remove one event: fusion of two scripts ----------------------
        // coil positions -----------------------------------
        // this script contains list of coil positions (other event fired)
        RecordTrackingTransformMax _coilTransform;

        // This list contains the coil poisitons
        private List<Vector3> _coilPosList;

        // Painting process ----------------------
        // This GameObject contains the scripts from Paintin3D,
        // it is moved to the coil position evrytime trigger is pressed
        public GameObject P_PaintGO;

        // Patin3D class for brain texture
        P3dPaintableTexture s_p3DPaintableTexture;

        // this script does the actual painting
        P3dHitBetween s_p3DPaintObject;

        // this sets the painting color
        P3dPaintSphere s_p3DPaintSphere;

        public float maxAmplitude ;
        public float minAmplitude ;


        // ---------------------------------------------------

        // --------------------------------------------------


        // Start is called before the first frame update
        void Start()
        {
            _mainListener = GameObject.FindObjectOfType<MainListenerMax>();

            // OnReceivedData Event subscription
             EventManager.OnReceivedData += ListenerCreateColorList;



            // for coil positions --------------------------------------------------
            _coilTransform = this.gameObject.GetComponent<RecordTrackingTransformMax>();

            //for painting ------------------------------------------------------
            s_p3DPaintableTexture = FindObjectOfType<P3dPaintableTexture>();
            s_p3DPaintObject = FindObjectOfType<P3dHitBetween>();
            s_p3DPaintSphere = FindObjectOfType<P3dPaintSphere>();

        }

        public void ListenerCreateColorList()
        {
          
            // assign a color to the last stream received and add it to the list of colors
            List<float> _vppList = _mainListener.VppList;
            int lastelement = _vppList.Count -1;

            float maxamplitude = 2e-5f;
            //float minAmplitude = 1e-6f;    
            _colorList = CreateColorList(_vppList[lastelement], _colorList, maxamplitude);
            
            
            
            // if track coil position: ---------------------------------------
            _coilIndexList = _coilTransform._coilPositions;
            //_colorList = CreateColorListCoil(_vppList, _colorList, maxAmplitude, minAmplitude, _coilIndexList);

            //else:
            //float maxamplitude = 2e-5f;
            //float minAmplitude = 1e-6f;    
            //_colorList = CreateColorList(_vppList[lastelement], _colorList, maxamplitude);
            // ---------------------------------------------------------------------------------------------------------------

            PaintSurface();


        }

        // -----------------------------------------------------------------------------------------------------------
        // Create color list from received values ----------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------------

        List<Color> CreateColorList(float vpp, List<Color> _colorList, float maxVpp)
        {

            // take max and min amplitudes of the list for scaling factor
            float maxAmplitude = maxVpp; //Mathf.Max(List.ToArray());
            float minAmplitude = 0; //Mathf.Min(List.ToArray());
  
                    // for debug -------------------
                    float  scale = vpp ;

                    // scaling factor            
                    float scaledvalue = (scale - minAmplitude) / (maxAmplitude - minAmplitude);

                    // store the colors in a list
                    _colorList.Add(Color.Lerp(Color.blue, Color.red, scaledvalue));
                   
            return _colorList;
        
        }


        List<Color> CreateColorListCoil(List<float> _vppList, List<Color> _colorList, float maxAmplitude, float minAmplitude, List<int> _coilID)
        {

            // take max and min amplitudes of the list for scaling factor
            //float maxAmplitude = maxVpp; //Mathf.Max(List.ToArray());
            //float minAmplitude = 0; //Mathf.Min(List.ToArray());

            float vpp = AverageVppCoilPosition(_vppList, _coilID);


            // for debug -------------------
            float scale = vpp ;

            // scaling factor
            //float scaled_value = InterpolateLog(scale,minAmplitude,maxAmplitude,1);
            float scaled_value = (scale - minAmplitude) / (maxAmplitude - minAmplitude);
            

            // store the colors in a list
            _colorList.Add(Color.Lerp(Color.blue, Color.red, scaled_value));

            return _colorList;

        }


        float AverageVppCoilPosition(List<float> _vppList, List<int> _coilID)
        {

            int lastElement = _vppList.Count - 1;
            int lastElementCoilID = _coilID[lastElement];

            List<float> vppListCurrentCoilPosition = new List<float>();


            for (int i = 0; i <= lastElement; i++)
            {
                if (_coilID[i] == lastElementCoilID)
                {
                   vppListCurrentCoilPosition.Add(_vppList[i]);
                }
            }


            float averagedVpp = CalculateMean(vppListCurrentCoilPosition);

            return averagedVpp;
        }

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

        
        //    test color styles --------------------------------------------------

        float InterpolateLog(float value,float start, float end, int weight)
        {
            float log_value = Mathf.Abs( start + (end - start) * Mathf.Log(value));
            return log_value;
        }
        Color LogRGB(float value,Color a, Color b)
        {
            Color interpColor = new Color() ;
            interpColor.r = a.r + (b.r - a.r) * Mathf.Log(value);


            return interpColor;
        }







        // -----------------------------------------------------------------------------------------------------------
        // Painting process -----------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Fetch the color change brush color 
        /// </summary>
        void PaintSurface()
        {
            // fetch coil positions
            _coilPosList = _coilTransform._trackPosList;

            // fetch the colors
            //_colorList = s_color._colorList;


            // take only the last element to only paint one point on the brain --------------------
            int lastelement = _colorList.Count - 1;
            StartCoroutine(PaintingOneColor(_colorList[lastelement], _coilPosList[lastelement]));


        }



        IEnumerator PaintingOneColor(Color color, Vector3 pos)
        {

            s_p3DPaintSphere.Color = color;

            // move the gameobjects with Paintin3Dscripts to the coil position
            P_PaintGO.transform.position = pos;


            // Start the painting -------------------------
            s_p3DPaintObject.PointA = P_PaintGO.transform;

            // wait for the painting to succeed (here 1s, can be reduced)
            yield return new WaitForSeconds(1f);

            // stop the painting -----------------
            s_p3DPaintObject.PointA = null;
        }




    }


}
