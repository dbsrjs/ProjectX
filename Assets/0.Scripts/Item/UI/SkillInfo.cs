
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfo : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private Image level;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Transform levelLoc;

    public void SetUI(Sprite image, int lv, int max_lv)
    {
        img.sprite = image;
        SetLevel(lv, max_lv);
    }
    public void SetLevel(int lv, int max_lv)
    {
        text.text = $"{lv} / {max_lv}";
        while (levelLoc.childCount < max_lv)
        {
            Instantiate(level, levelLoc).gameObject.SetActive(true);
        }


        for (int i = 0; i < levelLoc.childCount; i++)
        {
            if (i < lv)
            {
                levelLoc.GetChild(i).GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                levelLoc.GetChild(i).GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}
