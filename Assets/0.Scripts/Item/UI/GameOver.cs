using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TMP_Text txt_Count;
    [SerializeField] private TMP_Text txt_Level;
    [SerializeField] private TMP_Text txt_Timer;

    public void OnEnable()
    {
        UI.Instance.gamestate = GameState.Stop;
        txt_Count.text = $"잡은 좀비수 : {UI.option.Get("killcount")}";
        txt_Level.text = $"올린 레벨   : {GameManager.instance.player.level.Get("level")}";


        System.TimeSpan ts = System.TimeSpan.FromSeconds(UI.option.Get("time"));
        string time = $"경과 시간   : ";
        if (ts.Hours > 0)
            time += $"{ts.Hours}시 ";
        if (ts.Minutes > 0)
            time += $"{ts.Minutes}분 ";
        time += $"{ts.Seconds}초 ";

        txt_Timer.text = time;
    }

    public void OnOk()
    {
        SceneManager.LoadScene(0);
    }
}
