using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
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
    
    public List<byte> Receive()
    {
        //Get data from server
        myServerSocket.ReceiveTimeout = 30000;
        myServerSocket.SendTimeout = 30000;
        myServerSocket.Send(Encoding.UTF8.GetBytes("Send"));

        List<byte> tempReturnBytes = new List<byte>();
        int tempBytesReceived = 0;
        string tempStringReceived = "";
        bool tempShouldContinueReciveLoop = true;
        do
        {
            byte[] tempBufferList = new byte[1024];
            tempBytesReceived = myServerSocket.Receive(tempBufferList);

            if (tempStringReceived.Contains("Texture/"))
            {
                for (int i = 0; i < tempBytesReceived; i++)
                {
                    string tempString = Encoding.ASCII.GetString(tempBufferList, 0, tempBytesReceived);
                    if (tempString == "~")
                    {
                        tempShouldContinueReciveLoop = false;
                    }
                    else
                    {
                        tempReturnBytes.Add(tempBufferList[i]);
                    }
                }
            }
            else
            {
                tempStringReceived += Encoding.ASCII.GetString(tempBufferList, 0, tempBytesReceived);
            }

            for (int i = 0; i < tempStringReceived.Length; i++)
            {
                if (tempStringReceived[i] == '~')
                {
                    tempShouldContinueReciveLoop = false;
                }
            }
        } while (tempShouldContinueReciveLoop);
        Debug.Log("Receiving is done");


        List<string> tempSplitBufferList = tempStringReceived.Split('/').ToList();
        for (int i = 0; i < tempSplitBufferList.Count; i++)
        {
            if (tempSplitBufferList[i] == "SystemTime")
            {
                tempSplitBufferList.RemoveAt(i);
                GameObject tempServerText = GameObject.Find("LastRequestText");
                tempServerText.GetComponent<Text>().text = "Last receive from server: " + tempSplitBufferList[i];
                tempSplitBufferList.RemoveAt(i);
                Debug.Log("SystemTime Set");
            }
        }


        return tempReturnBytes;
    }

    public void Send(byte[] aBuffer)
    {
        myServerSocket.Send(aBuffer);
    }
}