﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Submit_1") || Input.GetButtonDown("Submit_2") || Input.GetButtonDown("Submit_3") || Input.GetButtonDown("Submit_4"))
        {
            SceneManager.LoadScene("PreGame");
        }
    }
}
