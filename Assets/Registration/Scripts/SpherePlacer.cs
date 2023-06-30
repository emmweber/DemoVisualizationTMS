using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class that handles placing spheres for the registration object */
public class SpherePlacer : MonoBehaviour
{
    //[SerializeField]
    // Template GameObject for showing sphere placement preview
    //GameObject indicator;

    [SerializeField]
    // Point clouds of the target (cloud1) and destination object (cloud2)
    PointCloud cloud1, cloud2;

    // Reference to the current cloud to be edited
    PointCloud currentCloud;
    
    // Position of where to place the sphere
    Vector3 placePos;

    // Start is called before the first frame update
    void Start()
    {
        //indicator.GetComponent<Renderer>().enabled = false;
        currentCloud = cloud1;
    }

    //void Update()
    //{
        //indicator.transform.position = placePos;
    //}
    public void SetTarget(Vector3 pos)
    {
        placePos = pos;
    }

    //public void OnToggleIndicatorPressed(){
     //   indicator.GetComponent<Renderer>().enabled = !indicator.GetComponent<Renderer>().enabled;
    //}

    //private void ShowIndicator()
    //{
    //    indicator.GetComponent<Renderer>().enabled = true;
    //}

    //private void HideIndicator()
    //{
     //   indicator.GetComponent<Renderer>().enabled = false;
    //}

    public void OnSwapCloudButtonPressed() 
    {
        if (currentCloud == cloud1)
            currentCloud = cloud2;

        else if (currentCloud == cloud2)
            currentCloud = cloud1;
    }
    public void OnAddSphereButtonPressed()
    {

        if (cloud1.transform.childCount < cloud2.transform.childCount)
        {
            currentCloud.AddSphere(placePos);
        }
    }

    public void OnDeleteSphere()
    {
        currentCloud.DeleteClosestSphere(placePos);
    }

}
