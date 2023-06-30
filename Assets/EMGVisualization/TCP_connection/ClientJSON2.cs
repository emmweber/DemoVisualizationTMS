//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
//using System.IO;
using System.Net.Sockets;
using UnityEngine.UI;
//using UnityEngine;
//using System.Collections.Generic;
using SimpleJSON;



namespace TCPeasy
{

    /// <summary>
    /// This script receives JSON data from TCP connection and store each element in a list. FOr now, add a list 
    /// for each element sent but could be optimized by automatically detecting the name of the value sent
    /// and creating an appropriate list for each element detected ?
    /// </summary>

    public class ClientJSON2 : MonoBehaviour
    {
        TcpClient client;
        string receivedMessage;
        byte[] buf = new byte[128];

        //public List<float> VppList;
        //public List<float> epochList;

        public int epochID;
        public float Vpp;

        // display the connection status
        //[SerializeField, Tooltip("Indicates the connection status")]
        //private Text connectText;
        private bool isConnected = false;
        [SerializeField]
        private  Image connectionStatusImg;


        // change to server address
        [SerializeField, Tooltip("The server IP address")]
        private string ipAddress = "10.35.120.230";//"10.35.125.24";// stanford //"10.10.10.3"; nano //"192.168.137.11" ; big computer IMMERS IP // rooter -- "10.10.10.3"; //"127.0.0.1";//"192.168.1.67"; // Mac windows IP -- "127.0.0.1"; // home 
        //public string ipAddress = "10.10.10.3";


        [SerializeField, Tooltip("The server port")]
        private int port = 5002;
        //public int port = 5002;

        // if disconnection
        private int lastEpoch;


        void Start()
        {
            //connectionStatusImg = GetComponentInChildren<Image>();
            //connectionStatusImg = GameObject.
            connectionStatusImg.color = Color.red;

            Debug.Log("Connecting to "+ ipAddress);
            
            
            connect2Server();

          

        }


        // Connect to server. For a faster timeout check the code in ReceiveClient.cs
        public void connect2Server()
        {
            // tries to connect for 0.5 seconds and then throws an error if it cannot connect
            // taken from https://social.msdn.microsoft.com/Forums/vstudio/en-us/2281199d-cd28-4b5c-95dc-5a888a6da30d/tcpclientconnect-timeout
            client = new TcpClient();
            IAsyncResult ar = client.BeginConnect(ipAddress, port, null, null);
            System.Threading.WaitHandle wh = ar.AsyncWaitHandle;
            try
            {
                if (!ar.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(0.5), false))
                {
                    client.Close();
                    throw new TimeoutException();
                }

                client.EndConnect(ar);

            }
            finally
            {
                wh.Close();
            }
            print("SOCKET READY");

            // Visualization on the Magic Leap ----------------------------
           // connectText.GetComponent<Text>().text = "Connected";
            //connectText.color = Color.green;
            connectionStatusImg.color = Color.green;

            // send a response/request to the server to inform that the connection has been -------------------
            // established and that the client is just waiting for sent msg --------------     
            Byte[] sendBytes = Encoding.UTF8.GetBytes("startStreaming\n");
            var stream = client.GetStream();
            stream.Write(sendBytes, 0, sendBytes.Length);

            isConnected = true;




        }

        void OnDestroy()
        {
            if (client.Connected)
            {
                client.Close();

                // inform the disconnection ont he Magic Leap
                connectionStatusImg.color = Color.red;
            }
        }



        void Update()
        {

            //if (!client.Connected)
            //{
            //  connectText.text = "Disconnected";
            //  connectText.color = Color.red;
            //  connectionStatusImg.color = Color.red;
            //  return;

            //}// early out to stop the function from running if client is disconnected

            

            // if loss of connection, tries to reconnect -----------------------
            try
            {
                // get the stream and start a callback function whent the message has been fully received
                var stream = client.GetStream();
                stream.BeginRead(buf, 0, buf.Length, Message_Received, null);

                

            }
            catch (InvalidOperationException)
            {

                isConnected = false;
                connectionStatusImg.color = Color.red;

                //Debug.Log("Connection lost");
                client.Close();
                connect2Server();

                if (isConnected == true )
                {
                    // have to keep track of the last epoch in case of reconnection
                    lastEpoch = epochID + 1;
                    connectionStatusImg.color = Color.green;

                    Debug.Log("Last epoch inside !!!!");
                }

            }

            

        }

        void Message_Received(IAsyncResult res)
        {

            if (res.IsCompleted && client.Connected)
            {
                // really optimized ? why do we khave to call that twice ?
                var stream = client.GetStream();
                int bytesIn = stream.EndRead(res);

                // reformat from bytes to ASCII strings
                receivedMessage = Encoding.ASCII.GetString(buf, 0, bytesIn);
                if (!receivedMessage.Contains("NaN") && receivedMessage.Contains("Vpp"))
                {
                    // extract values and store them in lists
                    JSON_Conversion(receivedMessage);
                }


            }
        }

        void JSON_Conversion(string data)
        {
            // defining local var
            //float Vpp = new float();
            //int epochID = new int();

            Debug.Log("msg format: " + data);

            // not usefull but I keep it as an example (as it was necessary  to remove the 
            // brackets for the previous python  code)
            //data = data.Trim('[', ']');

            // convert to JSON
            var N = JSON.Parse(data);

            // get the values from keys
            string _sVpp = N["Vpp"].Value;
            string _sepochID = N["epochID"].Value;

            // conversion from string to float or int
            Vpp = float.Parse(_sVpp);
            epochID = int.Parse(_sepochID) + lastEpoch; // add of last epoch value if there is a reconnection



        }



    }
}