using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCoil : MonoBehaviour
{ // script for demo that enable the coil visualization when EMG visualization scene is activated for the forst time
    // used in the demo to activate the coil GameObject attached to the controller.

    [SerializeField]
    private GameObject _virtualCoilGO;

    private void Awake()
    {
        

        ReplaceControllerCoil();
       

    }


    void ReplaceControllerCoil()
    {

        
        _virtualCoilGO.SetActive(true);





    }
}
