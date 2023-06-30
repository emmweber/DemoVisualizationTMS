using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TCPeasy
{
    public class DisplayCanvasDemo : MonoBehaviour
    {
        [SerializeField]
        TextMesh _Text;

        CreateColor_demo s_CreateColor;

        [SerializeField]
        private Image _colorPaintImage;

        /// <summary>
        /// 
        /// </summary>
        private List<string> _listStringText=new List<string>();

        /// <summary>
        /// 
        /// </summary>
        private int _lineCounter=0;

        /// <summary>
        /// 
        /// </summary>
        private int _maxLines = 5;

        /// <summary>
        /// 
        /// </summary>
        private string _newText;


        // Start is called before the first frame update
        void Start()
        {
           s_CreateColor=GameObject.FindObjectOfType<CreateColor_demo>();

            // register to event
            EventManager.OnReceivedData += WriteNewDistance;
            EventManager.OnReceivedData += ChangeColorSprite;

            // initialize TMP
            _Text.text = "Distance Coil - target:";

            _lineCounter = 0;



        }

        /// <summary>
        ///  Write info about stimulation next to the coil. Fetch the distance to the target point and display the error.
        /// </summary>
        void WriteNewDistance()
        {

            if (_lineCounter <= _maxLines)
            {
                
                
                _lineCounter += 1;
                

            }
            else 
            {
                // remove first element to make space
                _listStringText.RemoveAt(0);

                
                
            }
            
            // fetch new value and add it to the list of string
            _listStringText.Add(s_CreateColor.DistanceCoilTargetMeter.ToString());

            
            // write text header
            _newText = "Distance Coil - target:"; // _Text.text + "\n" + "Distance Coil - target:";

            // loop over the list to write all items
            foreach (string _str in _listStringText)
            {
                _newText = _newText + "\n" + _str;
            }

            _Text.text = _newText;
            

        }


        /// <summary>
        /// Change Color of the sprit next to the coil for better visualization
        /// </summary>
        private void ChangeColorSprite()
        {
             _colorPaintImage.color = s_CreateColor.ColorList[s_CreateColor.ColorList.Count - 1];
        }
    }

}
