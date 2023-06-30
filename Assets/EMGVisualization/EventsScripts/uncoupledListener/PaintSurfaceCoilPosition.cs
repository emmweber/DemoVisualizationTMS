using System.Collections.Generic;
using UnityEngine;
using PaintIn3D;
using System.Threading.Tasks;
using System.Collections;



namespace TCPeasy
{ 
    [RequireComponent(typeof(RecordTrackingTransform))]
    [RequireComponent(typeof(CreateColorCoilPosition))]

    public class PaintSurfaceCoilPosition : MonoBehaviour
    {
        // Patin3D class for brain texture
        P3dPaintableTexture _p3DPaintableTexture;

        // this script does the actual painting
        P3dHitBetween _p3DPaintObject;
        // this sets the painting color
        P3dPaintSphere _p3DPaintSphere;

        MainListener _mainListener;



        // List that contains all the coil poses 
        private List<Transform> _coilTransformList;
        private List<Vector3> _coilPosList;
        // List that contains all the colors of the different coil poses
        private List<Color> _colorListColor;

        // List that contains all the indices for the different coil poses (same coil pose with same index)
        private List<int> _coilIndexList;

        CreateColorCoilPosition _color;
        RecordTrackingTransform _coilTransform;

        public GameObject _debugPLaceholder; 




        // Start is called before the first frame update
        void Start()
        {

            //for painting
            _p3DPaintableTexture = FindObjectOfType<P3dPaintableTexture>();
            _p3DPaintObject = FindObjectOfType<P3dHitBetween>();
            _p3DPaintSphere = FindObjectOfType<P3dPaintSphere>();
           //_Paint1=GameObject.Find("Paint1");

            // register to event 2
            EventManager2.OnProcessedData += ListenerPaintStuff;

            _coilTransform = this.gameObject.GetComponent<RecordTrackingTransform>();
            _color = this.gameObject.GetComponent<CreateColorCoilPosition>();
            

        }

        void ListenerPaintStuff()
        {
            PaintSurface();
        }

        /// <summary>
        /// Fetch the color and corresponding coil pose and change brush color
        /// </summary>
        void PaintSurface()
        {

            //_coilTransformList = _coilTransform._trackObjList;
            _coilPosList = _coilTransform._trackPosList;
            _colorListColor = _color._colorListColor;
            _coilIndexList = _color._coilIndexList;
            //Vector3 pos = _track.GetComponent<Transform>().position;
            //Debug.Log("image position = " + pos.ToString());
            // the paint needs time to appear, so we have to let it paint for a sufficient 
            // amount of time and not let it constantly ==> use of a coroutine

            // erase all brain paint
            _p3DPaintableTexture.Clear();

            // iterate through list of all coil transforms
            //for (int i = 0; i < _coilTransformList.Count; i++)
            //{   

            //  _p3DPaintSphere.Color = _colorListColor[_coilIndexList[i]];
            // Color color = _colorListColor[_coilIndexList[i]];


            // StartCoroutine(SimulataneousPainting(_coilTransformList[i]));


            //}

            StartCoroutine(PaintingOneAtTheTime(_colorListColor,_coilIndexList,_coilPosList));


        }




        IEnumerator PaintingOneAtTheTime(List<Color> colorList,List<int> coilIndexList, List<Vector3> coilPosList)
        {
            

           

            for (int i = 0; i < coilIndexList.Count; i++)
            {

                _p3DPaintSphere.Color = colorList[coilIndexList[i]];

                _debugPLaceholder.transform.position = coilPosList[i];

                _p3DPaintObject.PointA = _debugPLaceholder.transform;

                //yield return new WaitUntil(() => _p3DPaintObject.Point.transform.hasChanged == true);
                yield return new WaitForSeconds(2f);

                 _p3DPaintObject.PointA = null;
            }

            //        {
            //yield return new WaitUntil(()=> _p3DPaintObject.Point.transform.hasChanged == true);
            
            //       //   }
            //

            //_p3DPaintObject.Point.transform.hasChanged = false;
           




        }






    }
}
