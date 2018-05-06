using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour {
    [SerializeField]
    private GameObject attributeGrid;
    [SerializeField]
    private GameObject attributeField;

	public void PlayGame()
    {

    }

    public void WorldEditor()
    {
        SceneManager.LoadScene("WorldEditor");
    }

    public void AddAttribute()
    {
        if (attributeGrid != null)
        {
            GameObject field = Instantiate(attributeField);
            field.transform.SetParent(attributeGrid.transform);
        }
    }
}
