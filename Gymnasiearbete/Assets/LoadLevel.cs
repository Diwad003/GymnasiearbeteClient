using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

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
        myNetworking.Send(Encoding.UTF8.GetBytes("Texture"));
        List<string> tempStringList = myNetworking.Receive();
        for (int i = 0; i < tempStringList.Count; i++)
            Debug.Log(tempStringList[i]);

        Debug.Log(tempStringList[0]);
        int tempPictureSize = int.Parse(tempStringList[0]);
        tempStringList.RemoveAt(0);

        byte[] tempImageBytes = Encoding.Unicode.GetBytes(tempStringList[0]);
        tempStringList.RemoveAt(0);
        Texture2D tempTexture = new Texture2D(tempPictureSize/2, tempPictureSize/2);
        tempTexture.LoadImage(tempImageBytes);
    }
}