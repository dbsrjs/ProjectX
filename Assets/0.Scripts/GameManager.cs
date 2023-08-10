using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Insatnce;

    [SerializeField] Player[] farmers;  //ĳ����
    [HideInInspector] public int playerIndex;   //ĳ���� ��ȣ(0�� 1)

    public Player p;

    private void Awake()
    {
        if (Insatnce == null)
        {
            Insatnce = this;
            DontDestroyOnLoad(gameObject);  //Scene�� ���� �ִ� gameObject ���� �ȵǰ� �ϱ�
        }
    }

    public void CreateFarmer()
    {
        p = Instantiate(farmers[playerIndex]);  //������ ĳ���� ����
    }
}
