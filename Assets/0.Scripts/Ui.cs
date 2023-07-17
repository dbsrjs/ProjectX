using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui : MonoBehaviour
{
    public static Ui instance;

    [SerializeField] private RectTransform canvas;
    //0 : 위 1 : 아래 : 2 왼쪽 : 3 오른쪽 : 4
    [SerializeField] private BoxCollider2D[] boxColls;


    [SerializeField] private Slider sliderExp;
    [SerializeField] private Text txtTime;
    [SerializeField] private Text txtKillConunt;
    [SerializeField] private Text txtLv;

    private float maxExp;
    private float exp;

    private int level = 0;
    private float timer = 0;
    //Simple Code
    private float[] exps = { 100f, 200f, 300f, 400f, 500f };

    public float Exp    //레벨
    {
        get { return exp; }
        set
        {
            exp = value;
            sliderExp.value = exp / maxExp;

            if (exp >= maxExp)
            {
                level++;    //레벨 증가
                maxExp = exps[level];
                sliderExp.value = 0f;
                exp = 0;

                txtLv.text = $"Lv.{level + 1}";
            }
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

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        maxExp = exps[level];
        sliderExp.value = 0f;
        exp = 0; sliderExp.value = 0f;
        txtLv.text = $"Lv.{level + 1}";    //Level UI

        

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
        if(Input.GetKey(KeyCode.F1))    //test
        {
            Exp += 1f;
        }

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
