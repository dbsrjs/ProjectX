using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterChoice : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnChoice()
    {
        SceneManager.LoadScene("Game");
    }
}
