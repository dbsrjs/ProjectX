using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[System.Serializable]
public class ItemInfo
{
    public string Name;
    public string Lore;
    public string stats;
    public Sprite img;
}

public class ItemPanal : Singleton<ItemPanal>
{
    [SerializeField] List<UpgradIUI> upgradIUIs;
    [SerializeField] public List<ItemInfo> items;
    List<ItemInfo> openUIs = new List<ItemInfo>();

    private void Awake()
    {
        var v = Instance;
        gameObject.SetActive(false);
    }

    public void Open()
    {
        UI.Instance.gamestate = GameState.Stop;
        gameObject.SetActive(true);


        foreach (var ui in upgradIUIs)
        {
            ui.gameObject.SetActive(false);
        }
        openUIs.Clear();

        int rd = 0;
        for(int i=0; i<3; i++)
        {
            rd = Random.Range(0, items.Count);
            if (!openUIs.Contains(items[rd]))
            {
                openUIs.Add(items[rd]);
            }
            else
            {
                i--;
            }
        }

        int n = 0;
        foreach (var txt in openUIs)
        {
            UpgradIUI ui = upgradIUIs[n++];
            ui.gameObject.SetActive(true);

            ui.txt_Name.text = txt.Name;
            ui.txt_Info.text = txt.Lore;
            ui.img.sprite = txt.img;
        }
    }

    public void OnClick(int number)
    {
        foreach (string stats in openUIs[number].stats.Split(","))
        {
            string[] str = stats.Split(":");
            GameManager.instance.player.stats.Add(str[0], float.Parse(str[1]));
        }
        UI.Instance.gamestate = GameState.Play;
        gameObject.SetActive(false);
    }
}
