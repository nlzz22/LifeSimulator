using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameScript : MonoBehaviour {
    [SerializeField]
    private GameObject ball;

    private static string saveFileName = "/worldeditor.data";

    private Save CreateSaveGameObject()
    {
        Save save = new Save();
        Vector3 pos = ball.transform.localPosition;
        save.SavePosition(pos.x, pos.y);

        return save;
    }

    public void SaveGame()
    {
        // Create a Save instance with all the data for the current session saved into it.
        Save save = CreateSaveGameObject();

        // Create a BinaryFormatter and a FileStream by passing a path for the Save instance to be saved to. 
        // It serializes the data (into bytes) and writes it to disk and closes the FileStream. 
        // There will now be a file named worldeditor.data on your computer.
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + saveFileName);
        bf.Serialize(file, save);
        file.Close();
        
        Debug.Log("Game Saved");
    }

    public void LoadGame()
    {
        // Checks to see that the save file exists.
        if (File.Exists(Application.persistentDataPath + saveFileName))
        {
            // Similar to what you did when saving the game, you again create a BinaryFormatter, 
            // only this time you are providing it with a stream of bytes to read instead of write.
            // So you simply pass it the path to the save file. It creates the Save object and closes the FileStream.
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + saveFileName, FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            // Load save information into the game state.
            ball.transform.localPosition = save.GetPosition();

            Debug.Log("Game Loaded");
        }
        else
        {
            Debug.Log("No game saved!");
        }
    }
}
