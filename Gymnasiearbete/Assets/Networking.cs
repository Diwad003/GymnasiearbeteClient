using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Networking : MonoBehaviour
{
    public Socket myServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    
    public void Connect()
    {
        GameObject tempIPInput = GetComponent<UI>().myInputField;
        try
        {
            string tempIP;
            if (tempIPInput.transform.GetChild(1).GetComponent<Text>().IsActive())
            {
                tempIP = tempIPInput.transform.GetChild(1).GetComponent<Text>().text;
            }
            else
            {
                tempIP = tempIPInput.transform.GetChild(2).GetComponent<Text>().text;
            }

            //myServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress tempIPAddress = IPAddress.Parse(tempIP);
            IPEndPoint tempServerEndpoint = new IPEndPoint(tempIPAddress, 54000);

            // Connect to Remote EndPoint  
            myServerSocket.Connect(tempServerEndpoint);
            Debug.Log("Socket connected to " + myServerSocket.RemoteEndPoint.ToString());

            GetComponent<UI>().myConnectionMenu.SetActive(false);
        }
        catch (Exception aException)
        {
            Debug.Log("Unexpected exception : " + aException.ToString());
            GetComponent<UI>().RemakeConnectionToServerMenu();
        }

        Send(Encoding.UTF8.GetBytes("GETSYSTEMTIME"));
        ReceiveLoop(1);
    }

    public void ReceiveLoop(int aTimesToLoop)
    {
        for (int i = 0; i < aTimesToLoop; i++)
        {
            byte[] tempBuffer = new byte[1000];
            myServerSocket.Receive(tempBuffer);
            if (!myServerSocket.Connected)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            GetComponent<UI>().myLastReceivedFromServer = Encoding.UTF8.GetString(tempBuffer);
        }
    }

    public void Send(byte[] aBuffer)
    {
        myServerSocket.Send(aBuffer);
    }
}