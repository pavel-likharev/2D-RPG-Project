using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MenuController : MonoBehaviour
{
    public void SwitchMenu(GameObject menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (menu != null)
        {
            menu.SetActive(true);
        }
    }
}
