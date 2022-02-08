using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

            myServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
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
    }

    public void LastReceivedFromServer(string aSystemTime)
    {
        GameObject tempServerText = GameObject.Find("LastRequestText");
        tempServerText.GetComponent<Text>().text = "Last Sure Connection To Server: " + aSystemTime;
    }

    public List<string> Receive()
    {
        //Get data from server
        List<byte> tempBuffer = new List<byte>();
        myServerSocket.ReceiveTimeout = 1000;
        while (myServerSocket.Connected)
        {
            try
            {
                byte[] tempBufferArray = new byte[1000000];
                myServerSocket.Receive(tempBufferArray);
                for (int i = 0; i < tempBufferArray.Length; i++)
                    tempBuffer.Add(tempBufferArray[i]);
            }
            catch (Exception tempException)
            {
                throw;
            }
        }

        //Convert to string and return
        string tempString = Encoding.UTF8.GetString(tempBuffer.ToArray());
        for (int i = 0; i < tempString.Length; i++)
        {
            tempString = tempString.Replace("\0", string.Empty);
        }

        string[] tempStringArray = tempString.Split('/');
        List<string> tempStringList = new List<string>();
        for (int i = 0; i < tempStringArray.Length; i++)
            tempStringList.Add(tempStringArray[i]);


        GameObject.Find("LastRequestText").GetComponent<Text>().text = tempStringList[0];
        tempStringList.RemoveAt(0);
        tempStringList.RemoveAt(tempStringList.Count - 1);

        return tempStringList;
    }

    public void Send(byte[] aBuffer)
    {
        myServerSocket.Send(aBuffer);
    }
}