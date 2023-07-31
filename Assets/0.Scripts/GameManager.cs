using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Insatnce;

    [SerializeField] Player[] farmers;
    [HideInInspector] public int playerIndex;

    public Player p;

    private void Awake()
    {
        if (Insatnce == null)
        {
            Insatnce = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void CreateFarmer()
    {
        p = Instantiate(farmers[playerIndex]);  //캐릭터 생성
    }
}
