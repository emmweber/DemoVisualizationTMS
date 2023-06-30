using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicLeap.Core;
using UnityEngine.XR.MagicLeap;
using UnityEngine.UI;
using MagicLeapTools;

namespace TCPeasy
{

    public class ChangeMax : MonoBehaviour



    {
        public TextMesh text;// typeText, stateText, directionText;
        public double amplitude;
        public double amplitude_uV;

        private MLInput.Controller _controller;
        private CreateColor s_createColor;

        public int precisionChangeMax;

        //private ControlInput _controlInput; 

        // Start is called before the first frame update
        void Start()
        {
            _controller = MLInput.GetController(MLInput.Hand.Left);
            //_controlInput = FindObjectOfType<ControlInput>();
            
            s_createColor = FindObjectOfType<CreateColor>();
            //amplitude = 0;
            //amplitude_uV = 0;

            //_controlInput.OnTouchDown.AddListener(updateGesture);
        }

        // Update is called once per frame
        void Update()

        {

            
            updateGesture();

        }

        void updateGesture()
        {
            string gestureType = _controller.CurrentTouchpadGesture.Type.ToString();


            string gestureState = _controller.TouchpadGestureState.ToString();
            string gestureDirection = _controller.CurrentTouchpadGesture.Direction.ToString();
            float gestureSpeed = _controller.CurrentTouchpadGesture.Speed;



            float amplitude_uV_float;
            double sign;
            double scale = 5E-8;

            if (gestureState == "Continue" && gestureSpeed != 0 && gestureType == "RadialScroll")
            {
                amplitude =s_createColor.maxAmplitude;
                

                if (gestureDirection == "Clockwise")
                {
                    sign = 1;
                }
                else
                {
                    sign = -1;
                }

                // scale sets the sensibility/speed of the change 
                amplitude = amplitude + sign * gestureSpeed * scale;
                if (amplitude < 0)
                {
                    amplitude = 0;
                }


                amplitude_uV = amplitude*1E6; // conversion in uV           
                //amplitude_uV = (Mathf.Round((float)amplitude_uV/precisionChangeMax)*precisionChangeMax);//round to precision
                
                // reconversion for the create color script
                amplitude =  (amplitude_uV * 1E-6);

                
                s_createColor.maxAmplitude = (float) amplitude;



            }
            amplitude_uV_float = (float)amplitude_uV;
            text.text = "Type: " + gestureType + "\n State = " + gestureState + "\n Direction = " + gestureDirection + "\n Speed = " + gestureSpeed.ToString() + "\n amplitude = " + amplitude_uV_float.ToString();








        }


    }
}
