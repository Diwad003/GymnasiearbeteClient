using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    Networking myNetworking;

    private void Start()
    {
        myNetworking = GameObject.Find("UIController").GetComponent<Networking>();
        LoadLevel1();
    }

    void LoadLevel1()
    {
        myNetworking.Send(Encoding.UTF8.GetBytes("LoadLevel1"));
        byte[] tempBuffer = new byte[1000];
        myNetworking.GetServerSocket().Receive(tempBuffer);
        string tempStringofData = Encoding.UTF8.GetString(tempBuffer);

        List<string> tempDataList = tempStringofData.Split('|').ToList();
        myNetworking.LastReceivedFromServer(tempDataList[0]);
        tempDataList.RemoveAt(0);

        GameObject h = GameObject.CreatePrimitive(PrimitiveType.Cube);
    }
}