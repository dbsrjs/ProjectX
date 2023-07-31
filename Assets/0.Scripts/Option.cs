using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Option : MonoBehaviour
{
    public static Option instance;

    [SerializeField] private AudioSource bmgSource;
    [SerializeField] private AudioSource fxSource;
    [SerializeField] private TMP_Dropdown dropDown;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider fxSlider;
    [SerializeField] private TMP_Text bgmTxt;
    [SerializeField] private TMP_Text fxTxt;

    private void Awake()
    {
        instance = this;
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
        Ui.instance.gamestate = GameState.Pause;
    }
    
    public void OnDisable()    // ���� ������Ʈ�� ������ �۵�
    {
        Ui.instance.gamestate = GameState.Play;
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
}