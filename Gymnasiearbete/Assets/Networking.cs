using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    
    public Socket GetServerSocket()
    {
        return myServerSocket;
    }

    public void Connect()
    {
        GameObject tempIPInput = GetComponent<UI>().myInputField;
        try
        {
            string tempIP;
            if (tempIPInput.transform.GetChild(0).GetComponent<Text>().IsActive())
            {
                tempIP = tempIPInput.transform.GetChild(0).GetComponent<Text>().text;
            }
            else
            {
                tempIP = tempIPInput.transform.GetChild(1).GetComponent<Text>().text;
            }

            IPAddress tempIPAddress = IPAddress.Parse(tempIP);
            IPEndPoint tempServerEndpoint = new IPEndPoint(tempIPAddress, 1000);

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
    }

    public void LastReceivedFromServer(string aSystemTime)
    {
        GameObject tempServerText = GameObject.Find("LastRequestText");
        tempServerText.GetComponent<Text>().text = "Last Sure Connection To Server: " + aSystemTime;
    }

    public List<string> Receive()
    {
        //Get data from server
        myServerSocket.ReceiveTimeout = 30000;
        byte[] tempBufferArray = new byte[4];
        myServerSocket.Send(Encoding.ASCII.GetBytes("Send"));
        int tempReceivedBytes = myServerSocket.Receive(tempBufferArray);

        int tempBufferInt = 0;
        for (int i = 0; i < tempReceivedBytes; i++)
        {
            if (tempBufferArray[i] != 0)
            {
                tempBufferInt = tempBufferArray[i];
            }
        }

        
        for (int i = 0; i < tempBufferInt; i++)
        {
            tempReceivedBytes = myServerSocket.Receive(tempBufferArray);
        }


        string tempString = Encoding.ASCII.GetString(tempBufferArray);
        List<string> tempBufferList = tempString.Split('/').ToList();

        //if (tempBufferList[0])
        {

        }


        List<string> tempStringList = new List<string>();
        GameObject.Find("LastRequestText").GetComponent<Text>().text = tempStringList[0];
        tempStringList.RemoveAt(0);

        return tempStringList;
    }

    public void Send(byte[] aBuffer)
    {
        myServerSocket.Send(aBuffer);
    }
}