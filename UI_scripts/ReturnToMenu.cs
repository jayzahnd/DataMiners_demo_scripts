using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour {

    public void ReturnToMenuNow()
    {
        SceneManager.LoadScene(0);

        Debug.Log("Returning To Menu!");
    }
}
