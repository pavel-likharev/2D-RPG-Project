using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour, ISavePoint
{
    public static UI Instance { get; private set; }

    public UI_MenuController MenuController { get; private set; }
    public UI_InGame InGame { get; private set; }
    public UI_EndScreen EndScreen { get; private set; }
    [SerializeField] private UI_VolumeSlider[] volumeSliders;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        MenuController = GetComponentInChildren<UI_MenuController>();
        InGame = GetComponentInChildren<UI_InGame>();
        EndScreen = GetComponentInChildren<UI_EndScreen>();
    }

    public void LoadData(GameData data)
    {
        foreach (var slider in volumeSliders)
        {

            if (data.volumeSettings.TryGetValue(slider.parametr, out float value))
            {
                if (value > 0)
                    slider.slider.value = value;
            }

        }
    }

    public void SaveData(ref GameData data)
    {
        data.volumeSettings.Clear();
        
        foreach (var slider in volumeSliders)
        {
            data.volumeSettings.Add(slider.parametr, slider.slider.value);
        }
    }
}
