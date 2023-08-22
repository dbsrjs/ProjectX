using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [SerializeField] Player[] farmers;
    [HideInInspector] public int playerIdx;

    public Player player;

    private void Awake()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }

    public void CreatePlayer(int number)
    {
        playerIdx = number;
        player = Instantiate(farmers[playerIdx]);
    }

}
