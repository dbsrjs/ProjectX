using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Option : MonoBehaviour
{
    public static Option instance;
    [SerializeField] private GameObject option; //����

    [SerializeField] private AudioSource bmgSource; //����� ����
    [SerializeField] private AudioSource fxSource;  //ȿ���� ����
    [SerializeField] private TMP_Dropdown dropDown; //�ػ�
    [SerializeField] private Slider bgmSlider;  //����� Slider
    [SerializeField] private Slider fxSlider;   //ȿ���� Slider
    [SerializeField] private TMP_Text bgmTxt;   //����� �ؽ�Ʈ
    [SerializeField] private TMP_Text fxTxt;    //ȿ���� �ؽ�Ʈ
    [SerializeField] private GameObject exit;   //exit panel

    private void Awake()
    {        
        instance = this;
        option.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        bmgSource.volume = bgmSlider.value;
        fxSource.volume = fxSlider.value;

        string[] strSize = { "1920x1080", "1440x1080", "1280x720","1176x664", "720x576","720x480" };    //�ػ�

        List<TMP_Dropdown.OptionData> odList = new List<TMP_Dropdown.OptionData>();

        foreach (var item in strSize)
        {
            TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData();
            data.text = item;
            odList.Add(data);
        }
        dropDown.options = odList;
    }

    public void OnEnable()     // ���� ������Ʈ�� ������ �۵�
    {
        Ui.Instance.gamestate = GameState.Pause;
        exit.SetActive(false);
    }
    
    public void OnDisable()    // ���� ������Ʈ�� ������ �۵�
    {
        Ui.Instance.gamestate = GameState.Play;
    }

    public void OnBGMValueChange(Slider slider)    //����� ����
    {
        bmgSource.volume = slider.value;
        bgmTxt.text = $"�����:{(int)(slider.value * 100)}%";
    }

    public void OnFxValueChange(Slider slider)     //ȿ���� ����
    {
        fxSource.volume = slider.value;
        fxTxt.text = $"ȿ����:{(int)(slider.value * 100)}%";
    }

    public void OnDropdownChange(TMP_Dropdown dd)    //�ػ� ����
    {
        string sizeTxt = dropDown.options[dd.value].text;
        string[] size = sizeTxt.Split('x');
        Screen.SetResolution(int.Parse(size[0]), int.Parse(size[1]), false);
    }

    public void OnPanel()
    {
        exit.SetActive(true);
    }

    public void OnExit()
    {
        SceneManager.LoadScene("Main");
    }

    public void OnContinue()
    {
        exit.SetActive(false);
    }
}
