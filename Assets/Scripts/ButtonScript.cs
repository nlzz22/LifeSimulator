using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour {
    [SerializeField]
    private GameObject attributeGrid;
    [SerializeField]
    private GameObject attributeField;

    private WorldEditControllerScript gameScript;

    private void FindGameScript()
    {
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
        gameScript = gameController.GetComponent<WorldEditControllerScript>();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("PlayScene");
    }

    public void WorldEditor()
    {
        SceneManager.LoadScene("WorldEditor");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void AddAttribute()
    {
        if (attributeGrid != null)
        {
            GameObject field = Instantiate(attributeField);
            field.transform.SetParent(attributeGrid.transform);
        }
    }

    public void SaveAndExit()
    {
        if (gameScript == null)
        {
            FindGameScript();
        }
        gameScript.SaveGame();
        MainMenu();
    }

    public void RemoveItem()
    {
        GameObject cancelButton = EventSystem.current.currentSelectedGameObject;
        if (cancelButton != null)
        {
            GameObject.Destroy(cancelButton.transform.parent.gameObject);
        }
        else
        {
            Debug.Log("BUG: RemoveItem() at ButtonScript has currentSelectedGameObject as null");
        }
    }
}
