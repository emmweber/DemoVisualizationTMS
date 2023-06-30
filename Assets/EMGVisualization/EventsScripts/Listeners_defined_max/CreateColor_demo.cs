using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PaintIn3D;
using System.IO;

namespace TCPeasy
{

    public class CreateColor_demo : MonoBehaviour

{

        // initialization

        /// <summary>
        /// This script receives the vpp value and store it in a list 
        /// </summary>
        //MainListenerMax_demo _mainListener;

        /// <summary>
        /// contains all colors for all simulations, will be constantly updated when new values are received
        /// </summary>
        public List<Color> ColorList = new List<Color>();

        /// <summary>
        /// List that contains indices for each different coil pose (same coil poses have same index)
        /// NOT IMPLEMENTED YET !!!!!!!
        /// </summary>
        // public List<int> _coilIndexList;

        // to simplify the logic and remove one event: fusion of two scripts ----------------------
        // coil positions -----------------------------------
        // this script contains list of coil positions (other event fired)
        private RecordTrackingTransformMax_demo s_coilTransform;

        // This list contains the coil poisitons
        private List<Vector3> coilPosList;

        // Painting process ----------------------
        /// <summary>
        /// This GameObject contains the scripts from Paintin3D, it is moved to the coil position evrytime trigger is pressed
        /// </summary>
        public GameObject PaintGO;

        /// <summary>
        /// Patin3D class for brain texture
        /// </summary>
        P3dPaintableTexture s_p3DPaintableTexture;

        /// <summary>
        /// this script does the actual painting
        /// </summary>
        P3dHitBetween s_p3DPaintObject;

        /// <summary>
        /// this sets the painting color
        /// </summary>
        P3dPaintSphere s_p3DPaintSphere;

        /// <summary>
        /// Max amplitude for the color scale.
        /// </summary>
        public float MaxAmplitude ; // 0.03F seems a good fit
        
        /// <summary>
        /// Min maplitude for the color scale.
        /// </summary>
        public float MinAmplitude=0F ;




        /// <summary>
        /// GameObject where the ray start (ideally coil location)
        /// </summary>
        private P3dHitBetween s_p3DHitBetween;

        /// <summary>
        /// Distance between the hitpoint on the mesh (given by PaintIn3D) and the target location.
        /// </summary>
        public float DistanceHitPointTargetMeter;

        /// <summary>
        /// target where the stimulation should occur. (sphere GameObject at the moment manually placed)
        /// </summary>
        [SerializeField]
        private GameObject target;

        /// <summary>
        /// Disatnce between the coil and the target in m (used to change color).
        /// </summary>
        public float DistanceCoilTargetMeter;




        // ---------------------------------------------------

        // --------------------------------------------------


        // Start is called before the first frame update
        void Start()
        {
            //_mainListener = GameObject.FindObjectOfType<MainListenerMax_demo>();

            // OnReceivedData Event subscription
             EventManager.OnReceivedData += ListenerCreateColorList;



            // for coil positions --------------------------------------------------
            s_coilTransform = this.gameObject.GetComponent<RecordTrackingTransformMax_demo>();

            //for painting ------------------------------------------------------
            s_p3DPaintableTexture = FindObjectOfType<P3dPaintableTexture>();
            s_p3DPaintObject = FindObjectOfType<P3dHitBetween>();
            s_p3DPaintSphere = FindObjectOfType<P3dPaintSphere>();
            s_p3DHitBetween = FindObjectOfType<P3dHitBetween>();



            
            

            

        }

        public void ListenerCreateColorList()
        {

            // Calculate distance bwtween target and hitpoint
            // DistanceHitPointTarget();

            // Calculate dictance coil-target
            DistanceCoilTarget();

            // assign color to the distance to target
            ColorList = CreateColorList(DistanceCoilTargetMeter, ColorList, MaxAmplitude);

            PaintSurface();


        }

        // -----------------------------------------------------------------------------------------------------------
        // Create color list from received values ----------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------------

        List<Color> CreateColorList(float vpp, List<Color> colorList, float maxVpp)
        {

            // take max and min amplitudes of the list for scaling factor
            float maxAmplitude = maxVpp; //Mathf.Max(List.ToArray());
            float minAmplitude = 0; //Mathf.Min(List.ToArray());
  
                    // for debug -------------------
                    float scale = vpp;

                    // scaling factor            
                    float scaledvalue = (scale - minAmplitude) / (maxAmplitude - minAmplitude);

                    // store the colors in a list (note, the smallest distance, the more towards the red)
                    colorList.Add(Color.Lerp(Color.red, Color.blue, scaledvalue));
                   
            return colorList;
        
        }

        /// <summary>
        /// Calculate distance between target and hitpoint
        /// </summary>
        /// <returns></float>
        private void DistanceHitPointTarget()
        {
            // retrieve the hitpoint
            Transform hitPoint = s_p3DHitBetween.Point;

            // Calculate distance between the 2 points
            DistanceHitPointTargetMeter = Vector3.Distance(target.transform.position, hitPoint.position);
        }

        void DistanceCoilTarget()
        {
            // fetch coil positions
            coilPosList = s_coilTransform._trackPosList;

            // take only the last element to only paint one point on the brain --------------------
            int lastelement = coilPosList.Count - 1;

            Vector3 coilPosition = coilPosList[lastelement];


            // Calculate distance between the 2 points
            DistanceCoilTargetMeter = Vector3.Distance(target.transform.position, coilPosition);
        }

        /// <summary>
        /// Create COlor list and coil position list. USed in case of repetitive stimulation at a single point.
        /// If the coil position change more than a threshold value, the index is incremented by an event and the float having the same index are avereaged.
        /// </summary>
        /// <param name="_vppList"></param>
        /// <param name="_colorList"></param>
        /// <param name="maxAmplitude"></param>
        /// <param name="minAmplitude"></param>
        /// <param name="_coilID"></param>
        /// <returns></returns>

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

        private float AverageVppCoilPosition(List<float> _vppList, List<int> _coilID)
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

        private float CalculateMean(List<float> ListFloat)
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

        // -----------------------------------------------------------------------------------------------------------
        // Painting process -----------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Fetch the color change brush color
        /// </summary>
        private void PaintSurface()
        {
            // fetch coil positions
            coilPosList = s_coilTransform._trackPosList;

            // take only the last element to only paint one point on the brain --------------------
            int lastelement = ColorList.Count - 1;
            StartCoroutine(PaintingOneColor(ColorList[lastelement], coilPosList[lastelement]));
        }

        IEnumerator PaintingOneColor(Color color, Vector3 pos)
        {

            s_p3DPaintSphere.Color = color;

            // move the gameobjects with Paintin3Dscripts to the coil position
            PaintGO.transform.position = pos;

            // Start the painting -------------------------
            s_p3DPaintObject.PointA = PaintGO.transform;

            // wait for the painting to succeed (here 1s, can be reduced)
            yield return new WaitForSeconds(1f);

            // stop the painting -----------------
            s_p3DPaintObject.PointA = null;
        }
    }
}
