using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

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


        int tempPixtureWidth = Convert.ToInt32(tempStringList[0]);
        tempStringList.RemoveAt(0);
        int tempPixtureHeight = Convert.ToInt32(tempStringList[0]);
        tempStringList.RemoveAt(0);

        Texture2D tempTexture = new Texture2D(tempPixtureWidth, tempPixtureHeight);


        //bool tempImageLoaded = tempTexture.LoadImage((byte)tempStringList[0]);
        //Debug.Log("Image Loaded " + tempImageLoaded);

        Sprite tempSprite = Sprite.Create(tempTexture, new Rect(0, 0, tempTexture.width, tempTexture.height), new Vector2(0,0));

        GameObject tempGameObject = GameObject.Find("Image");
        tempGameObject.GetComponent<Image>().sprite = tempSprite;

        tempTexture.Apply();
    }
}