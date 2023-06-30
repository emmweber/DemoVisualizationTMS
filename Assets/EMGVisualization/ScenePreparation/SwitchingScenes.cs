using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicLeapTools;


namespace TCPeasy
{
    public class SwitchingScenes : MonoBehaviour
    {
        // switch scenes at the end of registratrion by holding the touchpad

        public GameObject scene1, scene2;
        ControlInput _controlInput;

        GameObject currentScene;
        GameObject nextScene;


        // Start is called before the first frame update
        void Start()
        {

            // Find control input ( the script where the controller button events are set)
            _controlInput = GameObject.FindObjectOfType<ControlInput>();
            _controlInput.OnTouchHold.AddListener(Switching);




        }


        
        void Switching()
        {



            if (scene1.activeInHierarchy)
            {

                currentScene = scene1;
                nextScene = scene2;
            
            }

            if (scene2.activeInHierarchy)
            {

                currentScene = scene2;
                nextScene = scene1; // loop between the 2 scenes for now

            }


            currentScene.SetActive(false);
            nextScene.SetActive(true);



        }







    }
}

