using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicLeap.Core;
using MagicLeapTools;
using UnityEngine.XR.MagicLeap;

namespace TCPeasy
{
    public class ChangeMaxColorAmplitude : MonoBehaviour

    {
        ControlInput _controlInput;
        MLInput.Controller _controller;  
        
        float a;
        float b;


        // Start is called before the first frame update
        void Start()
        {
            _controlInput = GameObject.FindObjectOfType<ControlInput>();
            
            _controlInput.OnTouchBeganMoving.AddListener(ChangeNumber);
            //_controller = MLInput.GetController(MLInput.Hand.Right);

           
            
          

            a = 0;
            b = 0;


        }

        // Update is called once per frame
        void Update()
        {

            

        }


        void ChangeNumber()
        {
            
            a=a+_controlInput.TouchValue.x * 10f;
            float speed = _controller.CurrentTouchpadGesture.Speed;

            b = b + speed;


            Debug.Log("a= " + a.ToString());
            Debug.Log("b =" + b.ToString());






        }

        void Swipe()
        {
            Debug.Log("ForceTouchDown");


        }
    }
}
