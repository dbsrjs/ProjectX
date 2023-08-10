using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private void Awake()
    {
        if(GameManager.Insatnce == null)
        {
            SceneManager.LoadScene("Main"); //Main Scene∑Œ ¿Ãµø
            return;
        }

        GameManager.Insatnce.CreateFarmer();
    }
}
