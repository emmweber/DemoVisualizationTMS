using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TCPeasy
{

    public class ListenerTrackerResetPosition : MonoBehaviour
    {

        public GameObject tracker;

        // Start is called before the first frame update
        void Start()
        {
            //_eventTrackerPosition = GameObject.FindObjectOfType<eventTrackerPosition>();
            eventTrackerPosition.OnTrackerPositionchange += resetCubePosition;
        }

        public void resetCubePosition()
        {
            gameObject.transform.position = tracker.transform.position;
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Debug.Log("High speed. resetting the position");
            // change randmloy the color to indicate a change of coil position
            //Color newColor = new Color(Random.value, Random.value, Random.value, 1.0f);
            //gameObject.GetComponent<Material>().color = newColor;
        }
    }
}
