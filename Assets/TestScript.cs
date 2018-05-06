using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {
    private GameScript gameScript;

    // Use this for initialization
    void Start () {
        gameScript = FindObjectOfType<GameScript>();
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.localPosition += new Vector3(0, -0.3f, 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.localPosition += new Vector3(0, 0.3f, 0);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameScript.SaveGame();

        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            gameScript.LoadGame();

        }

    }
} 
