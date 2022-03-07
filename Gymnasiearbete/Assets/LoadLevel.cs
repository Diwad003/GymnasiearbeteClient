using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEditor;

public class LoadLevel : MonoBehaviour
{
    Networking myNetworking;

    public void LoadLevelNetworking()
    {
        float tempStartTime = Time.realtimeSinceStartup;
        myNetworking = GameObject.Find("UIController").GetComponent<Networking>();
        myNetworking.Send(Encoding.UTF8.GetBytes("LoadLevel1"));
        List<string> tempReceiveReturn = myNetworking.Receive();

        List<Texture2D> tempTextures = new List<Texture2D>();
        List<GameObject> tempGameObjects = new List<GameObject>();
        while (tempReceiveReturn.Count != 0)
        {
            if (tempReceiveReturn[0] == "Texture")
            {
                tempReceiveReturn.RemoveAt(0);
                tempTextures = LoadAllTextures(ref tempReceiveReturn);
            }
            else if (tempReceiveReturn[0] == "GameObjects")
            {
                tempReceiveReturn.RemoveAt(0);
                tempGameObjects = LoadAllGameObjects(ref tempReceiveReturn);
            }
        }

        for (int i = 0; i < tempGameObjects.Count; i++)
        {
            tempGameObjects[i].GetComponent<MeshRenderer>().material.mainTexture = tempTextures[i];
            Debug.Log("SET TEXTURE");
        }

        float tempDoneTime = Time.realtimeSinceStartup;
        float tempDifference = tempDoneTime - tempStartTime;
        Debug.Log("Time it takes to load from server " + tempDifference);
    }

    public void LoadLevelFile()
    {
        float tempStartTime = Time.realtimeSinceStartup;

        List<string> tempReceiveReturn = new List<string>();
        tempReceiveReturn.Add("Texture");
        tempReceiveReturn.Add("Wood");
        tempReceiveReturn.Add("GameObjects");
        tempReceiveReturn.Add("2:2:2");
        tempReceiveReturn.Add("Cube");

        List<Texture2D> tempTextures = new List<Texture2D>();
        List<GameObject> tempGameObjects = new List<GameObject>();
        while (tempReceiveReturn.Count != 0)
        {
            if (tempReceiveReturn[0] == "Texture")
            {
                tempReceiveReturn.RemoveAt(0);
                tempTextures = LoadAllTextures(ref tempReceiveReturn);
            }
            else if (tempReceiveReturn[0] == "GameObjects")
            {
                tempReceiveReturn.RemoveAt(0);
                tempGameObjects = LoadAllGameObjects(ref tempReceiveReturn);
            }
        }

        for (int i = 0; i < tempGameObjects.Count; i++)
        {
            tempGameObjects[i].GetComponent<MeshRenderer>().material.mainTexture = tempTextures[i];
            Debug.Log("SET TEXTURE");
        }


        float tempDoneTime = Time.realtimeSinceStartup;
        float tempDifference = tempDoneTime - tempStartTime;
        Debug.Log("Time it takes to load from file " + tempDifference);
    }

    List<GameObject> LoadAllGameObjects(ref List<string> tempGameObjectNames)
    {
        List<Vector3> tempGameObjectPositions = new List<Vector3>();
        List<GameObject> tempGameObjects = new List<GameObject>();
        for (int i = 0; i < tempGameObjectNames.Count; i++)
        {
            if (tempGameObjectNames[i].Contains(':'))
            {
                string[] tempGameObjectPositonString = tempGameObjectNames[i].Split(':');
                for (int j = 0; j < tempGameObjectPositonString.Length; j++)
                {
                    tempGameObjectPositions.Add(new Vector3(Convert.ToInt32(tempGameObjectPositonString[j]), Convert.ToInt32(tempGameObjectPositonString[j++]), Convert.ToInt32(tempGameObjectPositonString[j++])));
                }
                tempGameObjectNames.RemoveAt(i);
                i--;
            }
            else if (tempGameObjectNames[i] == "Cube" 
                || tempGameObjectNames[i] == "Capsule" 
                || tempGameObjectNames[i] == "Cylinder" 
                || tempGameObjectNames[i] == "Plane" 
                || tempGameObjectNames[i] == "Quad" 
                || tempGameObjectNames[i] == "Sphere")
            {
                if (tempGameObjectNames[i] == "Cube")
                {
                    tempGameObjects.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
                    Debug.Log("Spawning a Cube");
                }
                else if (tempGameObjectNames[i] == "Capsule")
                {
                    tempGameObjects.Add(GameObject.CreatePrimitive(PrimitiveType.Capsule));
                    Debug.Log("Spawning a Capsule");
                }
                else if (tempGameObjectNames[i] == "Cylinder")
                {
                    tempGameObjects.Add(GameObject.CreatePrimitive(PrimitiveType.Cylinder));
                    Debug.Log("Spawning a Cylinder");
                }
                else if (tempGameObjectNames[i] == "Plane")
                {
                    tempGameObjects.Add(GameObject.CreatePrimitive(PrimitiveType.Plane));
                    Debug.Log("Spawning a Plane");
                }
                else if (tempGameObjectNames[i] == "Quad")
                {
                    tempGameObjects.Add(GameObject.CreatePrimitive(PrimitiveType.Quad));
                    Debug.Log("Spawning a Quad");
                }
                else if (tempGameObjectNames[i] == "Sphere")
                {
                    tempGameObjects.Add(GameObject.CreatePrimitive(PrimitiveType.Sphere));
                    Debug.Log("Spawning a Sphere");
                }

                tempGameObjects[tempGameObjects.Count - 1].transform.position = tempGameObjectPositions[0];
                tempGameObjectNames.RemoveAt(i);
                i--;
            }
        }

        return tempGameObjects;
    }

    Texture2D FindTexture(string tempName)
    {
        Texture2D[] tempAllTextures = Resources.LoadAll<Texture2D>("Textures");
        foreach (Texture2D tempTexture in tempAllTextures)
        {
            if (tempTexture.name == tempName)
            {
                return tempTexture;
            }
        }
        return null;
    }

    List<Texture2D> LoadAllTextures(ref List<string> tempTextureNames)
    {
        List<Texture2D> tempTextures = new List<Texture2D>();
        while (tempTextureNames.Count != 0)
        {
            tempTextures.Add(FindTexture(tempTextureNames[0]));
            tempTextureNames.RemoveAt(0);
            if (tempTextureNames[0] == "GameObjects")
            {
                break;
            }
        }

        return tempTextures;
    }
}