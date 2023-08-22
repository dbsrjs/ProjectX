using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Option : Singleton<Option>
{
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource fxSource;
    [SerializeField] private TMP_Dropdown dropDown;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider fxSlider;

    [SerializeField] private TMP_Text bgmTxt;
    [SerializeField] private TMP_Text fxTxt;

    public void OnEnable()
    {
        UI.Instance.gamestate = GameState.Pause;
    }

    public void OnDisable()
    {
        UI.Instance.gamestate = GameState.Play;
    }

    private void Start()
    {
        bgmSource.volume = bgmSlider.value;
        fxSource.volume = fxSlider.value;

        string[] strSize = { "1920x1080", "1440x1080","1280x720", "1176x664", "720x576","720x480"};

        List<TMP_Dropdown.OptionData> odList = new List<TMP_Dropdown.OptionData>();
        foreach(var item in strSize)
        {
            TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData();
            data.text = item;
            odList.Add(data);
        }
        dropDown.options = odList;
    }

    public void OnBGValueChange(Slider slider)
    {
        bgmSource.volume = slider.value;
        bgmTxt.text = $"    배경음: {Mathf.Round(slider.value * 1000)/10.0f}%";
    }

    public void OnFXValueChange(Slider slider)
    {
        fxSource.volume = slider.value;
        fxTxt.text = $"    효과음: {Mathf.Round(slider.value * 1000) / 10.0f}%";
    }

    public void OnDropdownChange(TMP_Dropdown value)
    {
        string sizeTxt = dropDown.options[value.value].text;
        string[] size = sizeTxt.Split("x");
        Screen.SetResolution(int.Parse(size[0]), int.Parse(size[1]), false);
    }
}
