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
public class UpgradeData    //���׷��̵� ���
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

// ������ : ��, ����, �����(��ȸ��), ������ȭ, �����尩
public class Ui : Singleton<Ui>
{
    [HideInInspector] public GameState gamestate = GameState.Stop;

    [SerializeField] private UpgradeData[] upData;
    [SerializeField] private UpgradeUI[] upUI;
    [SerializeField] private DeadPopup deadPopup;



    [SerializeField] private RectTransform canvas;
    [SerializeField] private Transform levelupPopup;
    
    [SerializeField] private BoxCollider2D[] boxColls;  // 0 �� 1 �Ʒ� 2 �� 3 ��

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
            gamestate = GameState.Pause;    //���� �Ͻ� ����
            levelupPopup.gameObject.SetActive(true);    //LevelUp â ǥ��
            Level = (++level) + 1;  //���� ����
            if (GameManager.Insatnce.playerIndex == 0)  //�Ѿ� ���� Ƚ�� ����
                p.BulletHitMaxCount++;
            else
                p.BulletFireDelayTime -= p.BulletFireDelayTime * 0.2f;  //�Ѿ� ����ӵ� ����
            maxExp += 150;  //maxExp ����
            sliderExp.value = 0f;   //���� �� �ʱ�ȭ
            exp = 0;    //����ġ �ʱ�ȭ
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
            txtLv.text = $"Lv.{value}";    //Level �̹��� ����
        }
    }

    void Start()
    {
        OnGameStart();  //���� ����
        sliderExp.value = 0f;
        
        for (int i = 0; i < boxColls.Length; i++)   // ���� ���� ������Ʈ ��ġ ����
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
            gamestate = GameState.Play; //���� �簳
        }

        if (gamestate != GameState.Play)    //GameState�� Play�� �ƴ϶��
            return;

        timer += Time.deltaTime;
        System.TimeSpan ts = System.TimeSpan.FromSeconds(timer);
        txtTime.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);     //Ÿ�̸� �ؽ�Ʈ
    }
    
    public void SetHP(int HP, int maxHP)    //���� HP ���
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

    public void OnUpgrade(int index)    //���׷��̵�
    {
        Debug.Log(upgradeDatas[index].sprite.name);
        switch(upgradeDatas[index].sprite.name)
        {
            case "Select 0":
                p.AddShild();   //�� �߰�
                break;
            case "Select 3":
                p.BulletHitMaxCount++;  //�Ѿ� ���� Ƚ�� ����
                break;
            case "Select 6":
                p.BulletFireDelayTime -= p.BulletFireDelayTime * 0.1f;  //�Ѿ� ����ӵ� ����
                break;
            case "Select 7":
                p.Speed += 2f;  //�÷��̾� �ӵ� ����
                break;
            case "Select 8":
                p.HP = p.MaxHP; //�ִ� HP ����
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
