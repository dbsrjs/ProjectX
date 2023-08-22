using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextObject : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    float time = 0;
    Vector3 vector = new Vector3(0, 0.2f,0);

    public void SetText(string txt, float time, Vector3 vtr)
    {
        text.text = txt;
        this.time = time;
        vector = vtr;
    }


    // Update is called once per frame
    void Update()
    {
        if (UI.Instance.gamestate != GameState.Play) return;
        gameObject.transform.position += vector * Time.deltaTime;


        time -= Time.deltaTime;
        if(time <= 0)
        {
            Destroy(gameObject);
        }

    }
}
