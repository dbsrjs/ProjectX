using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui : MonoBehaviour
{
    public static Ui instance;

    [SerializeField] private Player p;

    [SerializeField] private RectTransform canvas;
    //0 : 위 1 : 아래 : 2 왼쪽 : 3 오른쪽 : 4
    [SerializeField] private BoxCollider2D[] boxColls;


    [SerializeField] private Slider sliderExp;
    [SerializeField] private Text txtTime;
    [SerializeField] private Text txtKillConunt;
    [SerializeField] private Text txtLv;

    private float timer = 0;

    private int killCount = 0;
    //Simple Code
    private float[] exps = { 100f, 200f, 300f, 400f, 500f };

    public void SetExp(ref float exp, ref float maxExp, ref int level)    //레벨
    {

        sliderExp.value = exp / maxExp;

        if (exp >= maxExp)
        {
            Level = (++level) + 1;  //레벨 증가
            maxExp = exps[level];
            sliderExp.value = 0f;
            exp = 0;
        }
    }

    public int KillCount
    {
        get { return KillCount; }
        set
        {
            KillCount = value;
            txtKillConunt.text = $"{KillCount}";
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
        timer += Time.deltaTime;
        System.TimeSpan ts = System.TimeSpan.FromSeconds(timer);
        txtTime.text = string.Format("{0:0}:{1:00}", ts.Minutes, ts.Seconds);
    }

    [SerializeField] private Image hpImg;
    public void SetHp(int HP, int maxHP)
    {
        hpImg.fillAmount = (float)HP / maxHP;
    }
}
