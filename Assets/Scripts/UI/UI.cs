using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI Instance { get; private set; }

    public UI_MenuController MenuController { get; private set; }
    public UI_InGame InGame { get; private set; }
    public UI_EndScreen EndScreen { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
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
}
