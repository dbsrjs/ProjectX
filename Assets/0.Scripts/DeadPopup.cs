using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DeadPopup : MonoBehaviour
{
    [SerializeField] private TMP_Text zombieCountTxt;   //óġ�� ���� ��
    [SerializeField] private TMP_Text levelTxt;    //���� ����
    [SerializeField] private TMP_Text timeTxt;  //�÷��� �ð�

    private void Start()
    {
        //gameObject.SetActive(false);
    }

    public void SetUi(int zombieCnt, int level, System.TimeSpan timeSpan)
    {
        zombieCountTxt.text = $"���� ���� �� : {zombieCnt}";
        levelTxt.text = $"���� ���� : {level}";

        string str = $"��� �ð� : ";
        if(timeSpan.Hours > 0)
            str += $"{timeSpan.Hours}��";

        if(timeSpan.Minutes > 0)
            str += $"{timeSpan.Minutes}��";

        str += $"{timeSpan.Seconds}��";

        timeTxt.text = str;
    }

    public void OnOk()
    {
        SceneManager.LoadScene("Main");
    }
}
