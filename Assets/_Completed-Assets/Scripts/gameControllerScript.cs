using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameControllerScript : MonoBehaviour
{

    // Changes the Scene to the 1st Level if the play button is clicked:
    public void OnPlayButtonClicked()
    {
        // Load the new scene:
        SceneManager.LoadScene("Level 1");
    }

    void Update(){
        // Press the Escape key in the build to end the game at any time:
        if (Input.GetKey("escape")) {
            Application.Quit();
        }
    }



}

