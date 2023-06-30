using System.Collections.Generic;
using UnityEngine;
using PaintIn3D;
using System.Threading.Tasks;
using System.Collections;



namespace TCPeasy
{
    [RequireComponent(typeof(RecordTrackingTransformMax))]
    [RequireComponent(typeof(CreateColor))]

    public class PaintBrain : MonoBehaviour
    {
        


        // Set the Color of the brush -----------------------------------------

        // List that contains all the colors 
        private List<Color> _colorList;

        // List that contains all the indices for the different coil poses (same coil pose with same index)
        private List<int> _coilIndexList; // not implemented yet --> assign each vpp to a coil position so that we can calculate the average.
                                          // has the size of _coilTransform

        // This script contains list of colors
        CreateColor s_color;

        // --------------------------------------------------------------------


        // coil positions -----------------------------------
        // this script contains list of coil positions
        RecordTrackingTransformMax _coilTransform;
        
        // This list contains the coil poisitons
        private List<Vector3> _coilPosList;

        // --------------------------------------------------

        
        
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

        // ---------------------------------------------------



        // Start is called before the first frame update
        void Start()
        {

            //for painting ------------------------------------------------------
            s_p3DPaintableTexture = FindObjectOfType<P3dPaintableTexture>();
            s_p3DPaintObject = FindObjectOfType<P3dHitBetween>();
            s_p3DPaintSphere = FindObjectOfType<P3dPaintSphere>();

            // register to event 2
            EventManagerPaintBrain.OnProcessedData += ListenerPaintStuff;


            // for coil positions --------------------------------------------------
            _coilTransform = this.gameObject.GetComponent<RecordTrackingTransformMax>();
            
            // for brush color ------------------------------------------
            s_color = this.gameObject.GetComponent<CreateColor>();


        }

        void ListenerPaintStuff()
        {
            PaintSurface();
        }

        /// <summary>
        /// Fetch the color change brush color 
        /// </summary>
        void PaintSurface()
        {
            // fetch coil positions
            _coilPosList = _coilTransform._trackPosList;

            // fetch the colors
            _colorList = s_color._colorList;


            // take only the last element to only paint one point on the brain --------------------
            int lastelement = _colorList.Count - 1;
            StartCoroutine(PaintingOneColor(_colorList[lastelement],  _coilPosList[lastelement]));


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

