using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    play,
    Pause,
    Stop,
}

[System.Serializable]
public class UpgradeData
{
    public Sprite sprite;
    public string title;
    public string desc1;
    public string desc2;
}

public class Ui : MonoBehaviour
{
    public static Ui instance;

    [SerializeField] private UpgradeData[] upData;

    [HideInInspector] public GameState gamestate = GameState.Stop;

    [SerializeField] private Player p;

    [SerializeField] private RectTransform canvas;
    [SerializeField] private Transform LevelUpPopup;

    //0 : 위 1 : 아래 : 2 왼쪽 : 3 오른쪽 : 4
    [SerializeField] private BoxCollider2D[] boxColls;

    [SerializeField] private Slider sliderExp;
    [SerializeField] private Text txtTime;
    [SerializeField] private Text txtKillConunt;
    [SerializeField] private Text txtLv;

    [SerializeField] private Image hpImg;

    private float timer = 0;

    private int killCount = 0;
    //Simple Code
    private float[] exps = { 100f, 200f, 300f, 400f, 500f };

    public void SetExp(ref float exp, ref float maxExp, ref int level)    //레벨
    {

        sliderExp.value = exp / maxExp;

        if (exp >= maxExp)
        {
            gamestate = GameState.Pause;    //게임 일시 정지
            LevelUpPopup.gameObject.SetActive(true);
            Level = (++level) + 1;  //레벨 증가
            maxExp = exps[level];
            sliderExp.value = 0f;
            exp = 0;
        }
    }

    public int KillCount
    {
        get { return killCount; }
        set
        {
            killCount = value;
            txtKillConunt.text = $"{killCount.ToString("000")}";
        }
    }

    public int Level
    {
        set
        {
            txtLv.text = $"Lv.{value}";    //Level UI

        }
    }


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        OnGameStart();

        instance = this;
        sliderExp.value = 0f;

        for (int i = 0; i < boxColls.Length; i++)   // 몬스터 스폰 오브젝트 위치 고정
        {
            Vector2 v1 = canvas.sizeDelta;
            if (i < 2)
                v1.y = 5;
            else
                v1.x = 5;
            boxColls[i].size = v1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F5))
        {
            gamestate = GameState.play; //게임 재개
        }

        if(gamestate != GameState.play)
            return;

        timer += Time.deltaTime;
        System.TimeSpan ts = System.TimeSpan.FromSeconds(timer);
        txtTime.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
    }
   
    public void SetHp(int HP, int maxHP)
    {
        hpImg.fillAmount = (float)HP / maxHP;
    }

    public void OnGameStart()
    {
        gamestate = GameState.play;
    }
}
