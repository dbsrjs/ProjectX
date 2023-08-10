using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum GameState
{
    Play,
    Pause,
    Stop,
}

[System.Serializable]
public class UpgradeData    //업그레이드 목록
{
    public Sprite sprite;
    public string title;
    public string desc1;
    public string desc2;
}

[System.Serializable]
public class UpgradeUI
{
    public Image icon;
    public TMP_Text levelTxt;
    public TMP_Text title;
    public TMP_Text desc1;
    public TMP_Text desc2;
}

// 아이템 : 삽, 엽총, 음료수(피회복), 전투장화, 전투장갑
public class Ui : Singleton<Ui>
{
    [HideInInspector] public GameState gamestate = GameState.Stop;

    [SerializeField] private UpgradeData[] upData;
    [SerializeField] private UpgradeUI[] upUI;
    [SerializeField] private DeadPopup deadPopup;



    [SerializeField] private RectTransform canvas;
    [SerializeField] private Transform levelupPopup;
    
    [SerializeField] private BoxCollider2D[] boxColls;  // 0 위 1 아래 2 왼 3 오

    [SerializeField] private Slider sliderExp;
    [SerializeField] private Text txtTime;
    [SerializeField] private Text txtKillCount;
    [SerializeField] private Text txtLv;
    [SerializeField] private Image hpImg;

    private float timer = 0;
    private int killCount = 0;

    private List<UpgradeData> upgradeDatas = new List<UpgradeData>();
    private Player p;


    public void SetExp(ref float exp, ref float maxExp, ref int level)  //Level
    {
        sliderExp.value = exp / maxExp;

        if (exp >= maxExp)  //Level Up
        {
            AudioManager.instance.Play("levelup");
            SetUpgradeData();
            gamestate = GameState.Pause;    //게임 일시 정지
            levelupPopup.gameObject.SetActive(true);    //LevelUp 창 표시
            Level = (++level) + 1;  //레벨 증가
            if (GameManager.Insatnce.playerIndex == 0)  //총알 관통 횟수 증가
                p.BulletHitMaxCount++;
            else
                p.BulletFireDelayTime -= p.BulletFireDelayTime * 0.2f;  //총알 연사속도 증가
            maxExp += 150;  //maxExp 증가
            sliderExp.value = 0f;   //레벨 바 초기화
            exp = 0;    //경험치 초기화
        }
    }

    public int KillCount
    {
        get { return killCount; }
        set
        {
            killCount = value;
            txtKillCount.text = $"{killCount.ToString("000")}";
        }
    }

    public int Level
    {
        set
        {
            txtLv.text = $"Lv.{value}";    //Level 이미지 변경
        }
    }

    void Start()
    {
        OnGameStart();  //게임 시작
        sliderExp.value = 0f;
        
        for (int i = 0; i < boxColls.Length; i++)   // 몬스터 스폰 오브젝트 위치 고정
        {
            Vector2 v1 = canvas.sizeDelta;

            if (i < 2)
            {
                v1.x += 50;
                v1.y = 5;

            }
            else
            {
                v1.x = 5;
                v1.y =+ 50;
            }
            boxColls[i].size = v1;
        }

        levelupPopup.gameObject.SetActive(false);
    }

    void Update()
    {
        if(p == null && GameManager.Insatnce != null)
        {
            p = GameManager.Insatnce.p;
        }
        
        if (Input.GetKeyDown(KeyCode.F5))
        {
            gamestate = GameState.Play; //게임 재개
        }

        if (gamestate != GameState.Play)    //GameState가 Play가 아니라면
            return;

        timer += Time.deltaTime;
        System.TimeSpan ts = System.TimeSpan.FromSeconds(timer);
        txtTime.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);     //타이머 텍스트
    }
    
    public void SetHP(int HP, int maxHP)    //현재 HP 계산
    {
        hpImg.fillAmount = (float)HP / maxHP;
    }    

    public void OnGameStart()
    {
        gamestate = GameState.Play;
    }

    void SetUpgradeData()
    {
        List<UpgradeData> datas = new List<UpgradeData>();
        for (int i = 0; i < upData.Length; i++)
        {
            UpgradeData ud = new UpgradeData();
            ud.sprite = upData[i].sprite;
            ud.title = upData[i].title;
            ud.desc1 = upData[i].desc1;
            ud.desc2 = upData[i].desc2;
            datas.Add(ud);
        }

        upgradeDatas = new List<UpgradeData>();
        for (int i = 0; i < 3; i++)
        {
            int rand = Random.Range(0, datas.Count);
            UpgradeData ud = new UpgradeData();
            ud.sprite = datas[rand].sprite;
            ud.title = datas[rand].title;
            ud.desc1 = datas[rand].desc1;
            ud.desc2 = datas[rand].desc2;
            upgradeDatas.Add(ud);
            datas.RemoveAt(rand);
        }

        for (int i = 0; i < upgradeDatas.Count; i++)
        {
            upUI[i].icon.sprite = upgradeDatas[i].sprite;
            upUI[i].title.text = upgradeDatas[i].title;
            upUI[i].desc1.text = upgradeDatas[i].desc1;
            upUI[i].desc2.text = upgradeDatas[i].desc2;
        }
    }

    public void OnUpgrade(int index)    //업그레이드
    {
        Debug.Log(upgradeDatas[index].sprite.name);
        switch(upgradeDatas[index].sprite.name)
        {
            case "Select 0":
                p.AddShild();   //삽 추가
                break;
            case "Select 3":
                p.BulletHitMaxCount++;  //총알 관통 횟수 증가
                break;
            case "Select 6":
                p.BulletFireDelayTime -= p.BulletFireDelayTime * 0.1f;  //총알 연사속도 증가
                break;
            case "Select 7":
                p.Speed += 2f;  //플레이어 속도 증가
                break;
            case "Select 8":
                p.HP = p.MaxHP; //최대 HP 증가
                SetHP(p.HP, p.MaxHP);
                break;
        }
    }

    public void ShowDeadPopup(int level)
    {
        deadPopup.gameObject.SetActive(true);
        System.TimeSpan ts = System.TimeSpan.FromSeconds(timer);
        deadPopup.SetUi(killCount, level, ts);
    }
}
