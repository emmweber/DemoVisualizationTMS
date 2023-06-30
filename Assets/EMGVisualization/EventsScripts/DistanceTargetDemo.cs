using PaintIn3D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TCPeasy
{
    public class DistanceTargetDemo: MonoBehaviour
    {
        /// <summary>
        /// target where the stimulation should occur. (sphere GameObject at the moment manually placed)
        /// </summary>
        [SerializeField]
        private GameObject target;

        /// <summary>
        /// GameObject where the ray start (ideally coil location)
        /// </summary>
        private P3dHitBetween Paint;

        /// <summary>
        /// Distance between the hitpoint on the mesh (given by PaintIn3D) and the target location.
        /// </summary>
        public float p_distance;






        // Start is called before the first frame update
        void Start()
        {
            // find the appropriate script in the scene
            Paint = GameObject.FindObjectOfType<P3dHitBetween>();

            // OnReceivedData Event subscription
            EventManager.OnReceivedData += CalculateDistanceTarget;

        }

        void CalculateDistanceTarget()
        {
            // retrieve the hitpoint
           Transform hitPoint =  Paint.Point;

            // Calculate distance between the 2 points
            p_distance = Vector3.Distance(target.transform.position, hitPoint.position);




           
           
           
        }


    }
}
