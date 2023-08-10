using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Option : MonoBehaviour
{
    public static Option instance;
    [SerializeField] private GameObject option; //본인

    [SerializeField] private AudioSource bmgSource; //배경음 설정
    [SerializeField] private AudioSource fxSource;  //효과음 설정
    [SerializeField] private TMP_Dropdown dropDown; //해상도
    [SerializeField] private Slider bgmSlider;  //배경음 Slider
    [SerializeField] private Slider fxSlider;   //효과음 Slider
    [SerializeField] private TMP_Text bgmTxt;   //배경음 텍스트
    [SerializeField] private TMP_Text fxTxt;    //효과음 텍스트
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

        string[] strSize = { "1920x1080", "1440x1080", "1280x720","1176x664", "720x576","720x480" };    //해상도

        List<TMP_Dropdown.OptionData> odList = new List<TMP_Dropdown.OptionData>();

        foreach (var item in strSize)
        {
            TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData();
            data.text = item;
            odList.Add(data);
        }
        dropDown.options = odList;
    }

    public void OnEnable()     // 게임 오브젝트가 켜질때 작동
    {
        Ui.Instance.gamestate = GameState.Pause;
        exit.SetActive(false);
    }
    
    public void OnDisable()    // 게임 오브젝트가 꺼질때 작동
    {
        Ui.Instance.gamestate = GameState.Play;
    }

    public void OnBGMValueChange(Slider slider)    //배경음 설정
    {
        bmgSource.volume = slider.value;
        bgmTxt.text = $"배경음:{(int)(slider.value * 100)}%";
    }

    public void OnFxValueChange(Slider slider)     //효과음 설정
    {
        fxSource.volume = slider.value;
        fxTxt.text = $"효과음:{(int)(slider.value * 100)}%";
    }

    public void OnDropdownChange(TMP_Dropdown dd)    //해상도 설정
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
