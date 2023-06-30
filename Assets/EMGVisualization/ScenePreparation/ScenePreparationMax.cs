using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaintIn3D;
using MagicLeapTools;
using MagicLeap.Core;
using MagicLeap;

namespace TCPeasy
{
    public class ScenePreparationMax : MonoBehaviour
    {

        public GameObject BrainMeshGO;
        public GameObject PaintProperties;
        public GameObject TargetRayCast;
        

 
        //public GameObject HeadTracking;
        public string nameGO;
        public bool NoCoilTracking;
        
        
 
        

        // Start is called before the first frame update
        void Awake()
        {
            // Find the gameObjects to update
            BrainMeshGO = GameObject.Find(nameGO);
            PaintProperties = GameObject.Find("Paint");
            //HeadTracking = GameObject.Find("HeadTracking").transform.GetComponentInChildren<MLImageTrackerBehavior>().gameObject;
            // Scale and rotate mesh
            //ScaleMesh(BrainMeshGO, 0.001f, new Vector3(0, 0, 0.5f));
            //RotateMesh(BrainMeshGO, new Vector3(-90, 90, 0));

            // Make GO paintable with 3DPaint
            PreparingMeshForPaint(BrainMeshGO);
            TargetPaint(PaintProperties, BrainMeshGO);

 

            //SetUpHeadTracking(BrainMeshGO, HeadTracking);

            // test for interaction with pointer
            //PreparationForControllerInteraction(BrainMeshGO);

            // Debug
            //SetDebug(NoCoilTracking);

        }



        void SetUpHeadTracking(GameObject MeshGO, GameObject headTracking)
        {

            
            headTracking.GetComponent<MLImageTrackerBehavior>().OnTargetFound += TrackHead() ;




        
        }


        MagicLeap.Core.MLImageTrackerBehavior.StatusUpdate TrackHead()
        {
            //GameObject headTracker = HeadTracking.GetComponentInChildren<MLImageTrackerBehavior>().gameObject;

            //BrainMeshGO.transform.SetParent(HeadTracking.transform, true);
            //BrainMeshGO.transform.parent = HeadTracking.transform;



            Debug.Log("submarine tracked");

            return null;
            
        }



 



    void PreparingMeshForPaint(GameObject MeshGO)
        {

            
             
            
            Mesh _testMesh = MeshGO.GetComponent<MeshFilter>().sharedMesh;

            if (_testMesh==false) 
            {   
                Debug.Log("Mesh should be readable for the paint to work !!");
                Debug.Log(" READABLE HEAD MESH :  " + _testMesh.isReadable);
            }
            
            

            MeshGO.AddComponent<MeshCollider>();
            MeshGO.AddComponent<P3dPaintable>();
            MeshGO.AddComponent<P3dPaintableTexture>();
            MeshGO.AddComponent<P3dMaterialCloner>();

            // manually activate both PaintableTexture and MaterialCloner
            MeshGO.GetComponent<P3dPaintable>().Activate();

            //Mesh _testMesh = MeshGO.GetComponent<MeshFilter>().sharedMesh;

           // print(" READABLE HEAD MESH :  " + _testMesh.isReadable);




        }

        void ScaleMesh(GameObject MeshGO, float factor, Vector3 pos)
        {

            MeshGO.transform.position = pos;
            MeshGO.transform.localScale = new Vector3(factor, factor, factor);

        }

        void RotateMesh(GameObject MeshGO, Vector3 rot)
        {

            MeshGO.transform.Rotate(rot, Space.Self);

        }

        void TargetPaint(GameObject Paint, GameObject MeshGo)
        {
            // Fetch the script requiring the mesh GO (brain)
            P3dHitBetween hitBetween = Paint.GetComponent<P3dHitBetween>();

            // define target (here, center of mesh but can be refined)
            // Transform target = MeshGo.transform;
            Transform target = TargetRayCast.transform;
            // assign target to the Paint GO 
            hitBetween.PointB = target;


        }


        void PreparationForControllerInteraction(GameObject MeshGo)
        {
            //MeshGo.AddComponent<ControllerInteractionVisualization>();

            ControllerInteractionVisualization _interactController = GameObject.FindObjectOfType<ControllerInteractionVisualization>();
            //ControllerInteractionVisualization _interactController = MeshGo.GetComponent<ControllerInteractionVisualization>();
            _interactController._controlInput = GameObject.Find("ControlInput").GetComponent<ControlInput>();
            _interactController.controller = GameObject.Find("Controller");
            _interactController._mesh = MeshGo;

            // wait for registration to happen --------------------
            //_interactController.gameObject.SetActive(false);


        }


        




        void SetDebug(bool NoTracking)
        {

            RecordTrackingTransformMax RTT = GameObject.FindObjectOfType<RecordTrackingTransformMax>();
            
            RTT._debugging = NoTracking;

            //eventTrackerPosition ETP = GameObject.FindObjectOfType<eventTrackerPosition>();
            //ETP._debugging = debugging;
        }


    }
}
