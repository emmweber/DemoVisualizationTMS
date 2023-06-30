using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaintIn3D;
//using MagicLeapTools;



namespace TCPeasy
{
    public class ScenePreparation : MonoBehaviour
    {

        public GameObject BrainMeshGO;
        public GameObject PaintProperties;
        public string nameGO;
        public bool debugging;

        // Start is called before the first frame update
        void Awake()
        {
            // Find the gameObjects to update
            BrainMeshGO = GameObject.Find(nameGO);
            PaintProperties = GameObject.Find("Paint");

            // Scale and rotate mesh
            ScaleMesh(BrainMeshGO, 0.001f, new Vector3(0, 0, 0.5f));
            RotateMesh(BrainMeshGO, new Vector3(-90, 90, 0));

            // Make GO paintable with 3DPaint
            PreparingMeshForPaint(BrainMeshGO);
            targetPaint(PaintProperties, BrainMeshGO);

            // test for interaction with pointer
            //PreparationForControllerInteraction(BrainMeshGO);

            // Debug
            SetDebug(debugging);

        }


        void PreparingMeshForPaint(GameObject MeshGO)
        {


            MeshGO.AddComponent<MeshCollider>();
            MeshGO.AddComponent<P3dPaintable>();
            MeshGO.AddComponent<P3dPaintableTexture>();
            MeshGO.AddComponent<P3dMaterialCloner>();

            // manually activate both PaintableTexture and MaterialCloner
            MeshGO.GetComponent<P3dPaintable>().Activate();



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

        void targetPaint(GameObject Paint, GameObject MeshGo)
        {
            // Fetch the script requiring the mesh GO (brain)
            P3dHitBetween hitBetween = Paint.GetComponent<P3dHitBetween>();

            // define target (here, center of mesh but can be refined)
            Transform target = MeshGo.transform;

            // assign target to the Paint GO 
            hitBetween.PointB = target;


        }


        void PreparationForControllerInteraction(GameObject MeshGo)
        {
            //MeshGo.AddComponent<InteractController>();

            //InteractController _interactController = GameObject.FindObjectOfType<InteractController>();
            //InteractController _interactController = MeshGo.GetComponent<InteractController>();
            //_interactController._controlInput = GameObject.Find("ControlInput").GetComponent<ControlInput>();
            //_interactController.controller = GameObject.Find("Controller");

        }




        void SetDebug(bool debugging)
        {

            RecordTrackingTransform RTT = GameObject.FindObjectOfType<RecordTrackingTransform>();
            RTT._debugging = debugging;
            
            eventTrackerPosition ETP = GameObject.FindObjectOfType<eventTrackerPosition>();
            ETP._debugging = debugging;
        }


    }
}
