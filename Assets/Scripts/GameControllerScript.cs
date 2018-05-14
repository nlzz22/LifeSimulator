using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public abstract class GameControllerScript : MonoBehaviour {
    [SerializeField]
    protected GameObject attributesGrid;
    [SerializeField]
    protected GameObject eventFunctionsGrid;

    protected static string saveFileName = "/worldeditor.data";
    protected string saveFileLocation;

    public abstract void LoadGame();

    private void Awake()
    {
        saveFileLocation = Application.dataPath + saveFileName;
        // Application.persistentDataPath will store in AppData, making it hard to be
        // transferrable across computers.
        Debug.Log("Save file location is at : " + saveFileLocation);
    }

}
