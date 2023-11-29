using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSound : MonoBehaviour
{
    [SerializeField] private int audioIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            AudioManager.Instance.PlaySFX(audioIndex);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null) 
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.StopSfxWith(audioIndex);
            }
        }
            
    }
}
