using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{

    [System.Serializable]
    public class MyClip
    {
        public string name;

        public AudioClip audioClip;
    }
    [SerializeField] private List<MyClip> m_Clip;
    [SerializeField] private AudioSource audioSource;

    public void Play(string name)
    {
        foreach (MyClip clip in m_Clip)
        {
            if(clip.name == name)
            {
                audioSource.clip = clip.audioClip;
                audioSource.Play();
                break;
            }
        }
    }


}
