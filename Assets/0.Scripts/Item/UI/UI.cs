using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Play,
    Pause,
    Stop
}


public class UI : Singleton<UI>
{
    [HideInInspector] public GameState gamestate = GameState.Play;
    [SerializeField] private LevelUpPanel LevelUpPanel;

    public Transform bullets;

    [SerializeField] private Slider sliderExp;

    [SerializeField] private Text txt_Time;
    [SerializeField] private Text txt_KillCount;
    [SerializeField] private Text txt_Lv;
    [SerializeField] private GameObject gameOver;

    [SerializeField] private TextObject txt_obj;
    [SerializeField] private Transform textLoc;

    public static Stats option = new Stats(new string[] { "time:0", "killcount:0", "panal:3" });

    public void AddExp(float exp) {
        Stats stats = GameManager.instance.player.level;
        stats.Add("exp", exp);

        sliderExp.value = stats.Get("exp") / stats.Get("exp_Max");
        if (sliderExp.value >= 1)
        {
            stats.Add("exp", -stats.Get("exp_Max"));
            stats.Add("exp_Max", (int)(stats.Get("exp_Max") * 0.25f));
            stats.Add("lv", 1);

            txt_Lv.text = $"Lv.{stats.Get("lv")}";
            GameManager.instance.player.LevelUpOption();
            Instance.LevelUpPanel.Open();
            gamestate = GameState.Pause;

            sliderExp.value = stats.Get("exp") / stats.Get("exp_Max");
        }
    }

    public int KillCount
    {
        get { return (int)option.Get("killcount"); }
        set
        {
            option.Set("killcount", value);
            txt_KillCount.text = string.Format("{0:000}", option.Get("killcount"));
        }
    }

    void Awake()
    {
        gamestate = GameState.Play;
    }

    // Start is called before the first frame update
    void Start()
    {
        txt_Lv.text = $"Lv.1";
        option = new Stats(new string[] { "time:0", "killcount:0", "panal:3" });
    }

    // Update is called once per frame
    void Update()
    {
        if(gamestate != GameState.Play) return;

        if (Input.GetKey(KeyCode.F1))
        {
            AddExp(5);
        }

        if (sliderExp.value >= 1)
        {
            UI.Instance.AddExp(0);
        }

        option.Add("time", Time.deltaTime);
        System.TimeSpan ts = System.TimeSpan.FromSeconds(option.Get("time"));
        txt_Time.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
    }

    public void GameStart()
    {
        gamestate = GameState.Play;
    }

    public void GameStop()
    {
        gameOver.SetActive(true);
    }

    public static void SpawnText(Vector3 pos,string text,float time,Vector3 vecter)
    {
        pos.y += 1;
        TextObject to = Instantiate(UI.Instance.txt_obj, pos, Quaternion.identity);
        to.SetText(text,time,vecter);
        to.transform.SetParent(Instance.textLoc);
    }
}
