
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothTracking : MonoBehaviour

{
    public Transform target;
    //Quaternion constant_shift;

    private void Start()
    {
        //constant_shift = transform.rotation * Quaternion.Inverse(target.rotation);
    }



    // Update is called once per frame
    void Update()
    {
        transform.position = target.position;
        transform.rotation = target.rotation ;
    }
}
