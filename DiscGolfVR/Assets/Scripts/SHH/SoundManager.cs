using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public GameObject[] backgroundIcon;
    public GameObject[] sfxIcon;
    public Slider bgSlider;
    public Slider sfxSlider;
    public AudioSource bgSound;
    public AudioSource[] sfxSound;


    public void SetbgSound(float volume)
    {
        bgSound.volume = volume;
    }

    public void SetsfxSound(float volume)
    {
        for(int i = 0; i < sfxSound.Length; i++)
        {
            sfxSound[i].volume = volume;
        }
    }

    public void SetbgSliderValue()
    {
        if (backgroundIcon[0].activeSelf)
        {
            backgroundIcon[0].SetActive(false);
            backgroundIcon[1].SetActive(true);
            bgSlider.value = 0;
        }
        else if (backgroundIcon[1].activeSelf) 
        {
            backgroundIcon[0].SetActive(true);
            backgroundIcon[1].SetActive(false);
            bgSlider.value = 1;
        }
    }

    public void SetsfxSliderValue()
    {
        if (sfxIcon[0].activeSelf)
        {
            sfxIcon[0].SetActive(false);
            sfxIcon[1].SetActive(true);
            sfxSlider.value = 0;
        }
        else if (sfxIcon[1].activeSelf)
        {
            sfxIcon[0].SetActive(true);
            sfxIcon[1].SetActive(false);
            sfxSlider.value = 1;
        }
    }
}
