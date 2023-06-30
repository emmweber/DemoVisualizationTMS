
namespace TCPeasy
{
    using PaintIn3D;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PaintEventDemo : MonoBehaviour
    {

        #region Initialization

        #region CoilPositions
        // TRACK COIL POSITION
        /// <summary>
        /// Fetch the mainlistener script to get the value sent by TCP. Not necessary for the demo.
        /// </summary>
        // MainListenerMax_demo s_mainListener;

        /// <summary>
        /// Controller transform
        /// </summary>
        private Transform controllerTransform;

        // List of all the coil pose transforms
        private List<Vector3> trackPosList;

        /// <summary>
        /// Disatnce between the coil and the target (used to change color)
        /// </summary>
        public float DistanceCoilTargetMeter;

        /// <summary>
        /// target where the stimulation should occur. (sphere GameObject at the moment manually placed)
        /// </summary>
        [SerializeField]
        private GameObject target;

        #endregion

        #region Create Color
        /// <summary>
        /// contains all colors for all simulations, will be constantly updated when new values are received
        /// </summary>
        public List<Color> ColorList = new List<Color>();

        /// <summary>
        /// Max amplitude for the color scale.
        /// </summary>
        public float maxAmplitude;

        /// <summary>
        /// Min maplitude for the color scale.
        /// </summary>
        public float minAmplitude;
        #endregion

        #region Painting process

        /// <summary>
        /// This GameObject contains the scripts from Paintin3D, it is moved to the coil position evrytime trigger is pressed
        /// </summary>
        public GameObject PaintGO;

        /// <summary>
        /// Patin3D class for brain texture
        /// </summary>
        P3dPaintableTexture s_p3DPaintableTexture;

        /// <summary>
        /// this sets the painting color
        /// </summary>
        P3dPaintSphere s_p3DPaintSphere;

        /// <summary>
        /// Distance between the hitpoint on the mesh (given by PaintIn3D) and the target location.
        /// </summary>
        public float DistanceHitPointTargetMeter;

        /// <summary>
        /// GameObject where the ray start (ideally coil location)
        /// </summary>
        private P3dHitBetween s_p3DHitBetween;

        #endregion

        #endregion

        // Start is called before the first frame update
        private void Start()
        {
            // Fetch main Listener to get the received value through TCP (not used in the demo, right ?)
            // s_mainListener = GameObject.FindObjectOfType<MainListenerMax_demo>();

            // Fetch the position of the controller used instead of coil position for the demo)
            controllerTransform = GameObject.Find("Controller").transform;

            // Fetch scripts necessary for awring with PaintIn3D
            s_p3DPaintableTexture = FindObjectOfType<P3dPaintableTexture>();
            s_p3DHitBetween = FindObjectOfType<P3dHitBetween>();
            s_p3DPaintSphere = FindObjectOfType<P3dPaintSphere>();

            // Subscribe to the Event.
            EventManager.OnReceivedData += ListenerPaintMesh;
        }

        #region Methods

        #region Record coil transform

        /// <summary>
        /// This method contains: 
        /// 1) the Storing of the coil transforms (here controller transform for the demo), 
        /// 2) calculation of a value (error) betweebn the coil position and the target,
        /// 3) the creation of a color associated to a value (and a colorlist),
        /// 4) the painting of the mesh
        /// 5) Calculation of the error between the hitpoint on the mesh and the placed target 
        /// (just for info)
        /// </summary>
        void ListenerPaintMesh() 
        {
            // Store the transform of the coil/controller into a list
            StoreTransformList();

            // Calculate distance coil-target (used for the demo)
            DistanceCoilTarget();


            // assign color to the distance to target
            ColorList = CreateColorList(DistanceCoilTargetMeter, ColorList, maxAmplitude, minAmplitude);

            // Starts the painting process (coroutine)
            PaintSurface();

            // Calculate distance HitPoint-target (not used, just for info)
            DistanceHitPointToTarget();
        }


        /// <summary>
        /// Stores the transform of the coil in a list.
        /// </summary>
        void StoreTransformList()
        {
            // size of the List of received values (=number of stimulations)
            // int triggerNumber = EventManager.SizeVpp;

            Vector3 pos = controllerTransform.position;
            trackPosList.Add(pos);
        }

        /// <summary>
        /// Calculate distance between the coil position and the target location.
        /// </summary>
        private void DistanceCoilTarget()
        {
            // take only the last element to only paint one point on the brain --------------------
            int lastelement = trackPosList.Count - 1;

            Vector3 coilPosition = trackPosList[lastelement];

            // Calculate distance between the 2 points
            DistanceCoilTargetMeter = Vector3.Distance(target.transform.position, coilPosition);
        }

        #endregion

        #region Create Color

        /// <summary>
        /// Associate a color to a value according to a defined scale. Add this color to a list of colors
        /// </summary>
        /// <param name="value">Float number to associate a color to.</param>
        /// <param name="_colorList">List of colors that have to be filled.</param>
        /// <param name="maxValue">Maximum value of the scale.</param>
        /// <param name="minValue">Minimum value of the scale.</param>
        /// <returns> List of colors with an additionnal element. </returns>
        private List<Color> CreateColorList(float value, List<Color> _colorList, float maxValue, float minValue)
        {
            // take max and min amplitudes of the list for scaling factor
            float maxAmplitude = maxValue; //Mathf.Max(List.ToArray());
            float minAmplitude = 0; //Mathf.Min(List.ToArray());

            // for debug -------------------
            float scale = value;

            // scaling factor
            float scaledvalue = (scale - minAmplitude) / (maxAmplitude - minAmplitude);

            // store the colors in a list (note, the smallest distance, the more towards the red)
            _colorList.Add(Color.Lerp(Color.red, Color.blue, scaledvalue));

            return _colorList;
        }

        /// <summary>
        /// Calculate the distance between the the hitpoint in PaintIn3D and the target on the mesh.
        /// </summary>
        private void DistanceHitPointToTarget()
        {
            // retrieve the hitpoint
            Transform hitPoint = s_p3DHitBetween.Point;

            // Calculate distance between the 2 points
            DistanceHitPointTargetMeter = Vector3.Distance(target.transform.position, hitPoint.position);
        }

        #endregion

        #region Painting Process
        /// <summary>
        /// Fetch the color change brush color
        /// </summary>
        private void PaintSurface()
        {
            // fetch coil positions
            // coilPosList = _trackPosList;

            // take only the last element to only paint one point on the brain --------------------
            int lastelement = ColorList.Count - 1;
            StartCoroutine(PaintingOneColor(ColorList[lastelement], trackPosList[lastelement]));
        }

        /// <summary>
        /// This coroutine allow to paint the Mesh. 
        /// 1) It moves the GameObject where the script 3dHitBetween is attached
        /// 2) it changes the starting point of the ray rtacing fo the drawing to the
        /// new location of the  gameobject.
        /// </summary>
        /// <param name="color">provide the color of the paint</param>
        /// <param name="pos">provide the position of the new object (i.e. coil or controller)</param>
        /// <returns></returns>
        IEnumerator PaintingOneColor(Color color, Vector3 pos)
        {
            s_p3DPaintSphere.Color = color;

            // move the gameobjects with Paintin3Dscripts to the coil position
            PaintGO.transform.position = pos;

            // Start the painting -------------------------
            s_p3DHitBetween.PointA = PaintGO.transform;

            // wait for the painting to succeed (here 1s, can be reduced)
            yield return new WaitForSeconds(1f);

            // stop the painting -----------------
            s_p3DHitBetween.PointA = null;
        }
        #endregion

        #endregion

    }
}
