using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DeadPopup : MonoBehaviour
{
    [SerializeField] private TMP_Text zombieCountTxt;   //처치한 좀비 수
    [SerializeField] private TMP_Text levelTxt;    //최종 레벨
    [SerializeField] private TMP_Text timeTxt;  //플레이 시간

    private void Start()
    {
        //gameObject.SetActive(false);
    }

    public void SetUi(int zombieCnt, int level, System.TimeSpan timeSpan)
    {
        zombieCountTxt.text = $"잡은 좀비 수 : {zombieCnt}";
        levelTxt.text = $"최종 레벨 : {level}";

        string str = $"경과 시간 : ";
        if(timeSpan.Hours > 0)
            str += $"{timeSpan.Hours}시";

        if(timeSpan.Minutes > 0)
            str += $"{timeSpan.Minutes}분";

        str += $"{timeSpan.Seconds}초";

        timeTxt.text = str;
    }

    public void OnOk()
    {
        SceneManager.LoadScene("Main");
    }
}
