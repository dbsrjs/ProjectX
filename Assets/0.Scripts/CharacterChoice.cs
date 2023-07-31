using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterChoice : MonoBehaviour
{
    public void OnChoice(int index)
    {
        GameManager.Insatnce.playerIndex = index;

        SceneManager.LoadScene("Game");    //Game Scene으로 이동
    }
}
