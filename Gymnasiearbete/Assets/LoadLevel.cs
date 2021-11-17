using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    Networking myNetworking;
    Socket myServerSocket;

    private void Start()
    {
        LoadLevel1();
    }

    private void Update()
    {
        myNetworking = GameObject.Find("UIController").GetComponent<Networking>();
        myServerSocket = myNetworking.myServerSocket;
    }

    void LoadLevel1()
    {
        myNetworking.Send(Encoding.UTF8.GetBytes("LoadLevel1"));
        byte[] tempBuffer = new byte[1000];
        myServerSocket.Receive(tempBuffer);
        string tempStringofData = Encoding.UTF8.GetString(tempBuffer);

        string[] tempDataArray = tempStringofData.Split('|');
    }
}