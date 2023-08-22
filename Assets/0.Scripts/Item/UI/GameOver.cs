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
        txt_Count.text = $"���� ����� : {UI.option.Get("killcount")}";
        txt_Level.text = $"�ø� ����   : {GameManager.instance.player.level.Get("level")}";


        System.TimeSpan ts = System.TimeSpan.FromSeconds(UI.option.Get("time"));
        string time = $"��� �ð�   : ";
        if (ts.Hours > 0)
            time += $"{ts.Hours}�� ";
        if (ts.Minutes > 0)
            time += $"{ts.Minutes}�� ";
        time += $"{ts.Seconds}�� ";

        txt_Timer.text = time;
    }

    public void OnOk()
    {
        SceneManager.LoadScene(0);
    }
}
