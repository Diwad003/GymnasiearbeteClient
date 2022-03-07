using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;
using System.Net.Sockets;

public class UI : MonoBehaviour
{
    public string myPredoneIP;
    public GameObject myConnectionMenu;
    public GameObject myInputField;
    public GameObject myStartMainMenu;

    GameObject myIPAddress, myConnectButton, myLevelLoadController;
    List<GameObject> myStartButtonList = new List<GameObject>();

    void Start()
    {
        myInputField = GameObject.Find("InputField");
        myIPAddress = GameObject.Find("IPAddress");
        myConnectButton = GameObject.Find("ConnectButton");
        myConnectionMenu = GameObject.Find("ConnectionMenu");
        myStartMainMenu = GameObject.Find("StartMenu");

        myStartMainMenu.SetActive(false);
        myInputField.transform.GetChild(0).GetComponent<Text>().text = myPredoneIP;

    }

    public void StartButtonClick()
    {
        gameObject.AddComponent<Networking>();
        gameObject.GetComponent<Networking>().Connect();
        StartMenu();
    }

    public void RemakeConnectionToServerMenu()
    {
        Destroy(gameObject.GetComponent<Networking>());
        myConnectButton.SetActive(true);
        myIPAddress.SetActive(true);
        myInputField.SetActive(true);
    }

    void StartMenu()
    {
        myStartMainMenu.SetActive(true);
        myStartButtonList.Add(CreateButton(new Vector2(-350, 270)));
        for (int i = 0; i < 1; i++)
        {
            myStartButtonList.Add(CreateButton(new Vector2(myStartButtonList[i].transform.localPosition.x,
                myStartButtonList[i].transform.localPosition.y - (myStartButtonList[i].GetComponent<RectTransform>().rect.height + 149))));
        }

        myStartButtonList[0].GetComponentInChildren<Text>().text = "Load Level with Networking";
        myStartButtonList[1].GetComponentInChildren<Text>().text = "Load Level with File";

        for (int i = 0; i < myStartButtonList.Count; i++)
        {
            string tempName = myStartButtonList[i].GetComponentInChildren<Text>().text;
            myStartButtonList[i].GetComponent<Button>().onClick.AddListener(delegate { StartMenuClick(ref tempName); });
        }
    }

    void StartMenuClick(ref string aName)
    {
        myStartMainMenu.SetActive(true);
        switch (aName)
        {
            case "Load Level with Networking":
                myLevelLoadController = new GameObject();
                myLevelLoadController.name = "LevelLoadController";
                myLevelLoadController.AddComponent<LoadLevel>();
                myLevelLoadController.GetComponent<LoadLevel>().LoadLevelNetworking();
                break;

            case "Load Level with File":
                myLevelLoadController = new GameObject();
                myLevelLoadController.name = "LevelLoadController";
                myLevelLoadController.AddComponent<LoadLevel>();
                myLevelLoadController.GetComponent<LoadLevel>().LoadLevelFile();
                break;

            case "Load Specific GameObject":
                Debug.Log("Not Implemented");
                break;

            default:
                Debug.Log("ERROR WRONG NAME");
                return;
        }
    }

    GameObject CreateButton(Vector2 aPosition)
    {
        GameObject tempButton = Instantiate(Resources.Load("Button", typeof(GameObject))) as GameObject;
        GameObject tempPanel = GameObject.Find("Panel");
        tempButton.transform.SetParent(tempPanel.transform);
        tempButton.transform.localPosition = aPosition;
        return tempButton;
    }
}