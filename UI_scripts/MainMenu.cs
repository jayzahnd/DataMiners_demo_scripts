using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void PlayGameNow()
    {
        SceneManager.LoadScene(1);

        Debug.Log("Game Begins!");
    }

    public void QuitGameNow()
    {
        Application.Quit();

        Debug.Log("Game Quits!");
    }
	
}
