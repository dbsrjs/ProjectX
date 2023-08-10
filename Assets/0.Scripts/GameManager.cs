using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Insatnce;

    [SerializeField] Player[] farmers;  //캐럭터
    [HideInInspector] public int playerIndex;   //캐릭터 번호(0과 1)

    public Player p;

    private void Awake()
    {
        if (Insatnce == null)
        {
            Insatnce = this;
            DontDestroyOnLoad(gameObject);  //Scene를 갖고 있는 gameObject 삭제 안되게 하기
        }
    }

    public void CreateFarmer()
    {
        p = Instantiate(farmers[playerIndex]);  //선택한 캐릭터 생성
    }
}
