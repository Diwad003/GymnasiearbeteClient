using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using OpenCvSharp;

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
        byte[] tempReceiveReturnArray = myNetworking.Receive().ToArray();

        for (int i = 0; i < tempReceiveReturnArray.Length; i++)
        {
            Debug.Log("tempReceiveReturnArray i:" + i + "VALUE:" + tempReceiveReturnArray[i]);
        }

        Mat tempImage = Mat.FromImageData(tempReceiveReturnArray, ImreadModes.AnyColor);
        Cv2.ImShow("Image", tempImage);
        Cv2.WaitKey(0);



        //bool tempImageLoaded = tempTexture.LoadImage((byte)tempStringList[0]);
        //Debug.Log("Image Loaded " + tempImageLoaded);

        //Sprite tempSprite = Sprite.Create(tempTexture, new Rect(0, 0, tempTexture.width, tempTexture.height), new Vector2(0,0));

        GameObject tempGameObject = GameObject.Find("Image");
        //tempGameObject.GetComponent<Image>().sprite = tempSprite;
    }
}