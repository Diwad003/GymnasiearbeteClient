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
    public TcpClient myServerSocket = new TcpClient();
    
    public TcpClient GetServerSocket()
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
            myServerSocket = new TcpClient(tempServerEndpoint);

            // Connect to Remote EndPoint  
            myServerSocket.Connect(tempServerEndpoint);
            Debug.Log("Socket connected to " + myServerSocket.Client.RemoteEndPoint.ToString());

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
        myServerSocket.ReceiveTimeout = 10000;
        byte[] tempBufferArray = new byte[1024];
        int tempReceivedBytes = myServerSocket.Client.Receive(tempBufferArray); //tempReceivedBytes gets size of the total send buffer for some reason
        Debug.Log("Send Size" + tempReceivedBytes);



        List<byte> tempBuffer = new List<byte>();
        tempBufferArray = new byte[1024];

        int tempAmountOfBits = myServerSocket.Client.Receive(tempBufferArray);
        string str = Encoding.ASCII.GetString(tempBufferArray, 0, tempAmountOfBits);
        Debug.Log("str " + str);

        Debug.Log("tempAmountOfBits " + tempAmountOfBits);
        for (int j = 0; j < tempAmountOfBits; j++)
        {
            tempBuffer.Add(tempBufferArray[j]);
            Debug.Log("Buffer Array " + tempBufferArray[j]);
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

        return tempStringList;
    }

    public void Send(byte[] aBuffer)
    {
        myServerSocket.Client.Send(aBuffer);
    }
}