using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{

    private void Awake()
    {
        if(GameManager.instance == null)
        {
            SceneManager.LoadScene(0);
            return;
        }
    }


    void Start()
    {
        GameManager.instance.CreatePlayer(GameManager.instance.playerIdx);
    }

}
