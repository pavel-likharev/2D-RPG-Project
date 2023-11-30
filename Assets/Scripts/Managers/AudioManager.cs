using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private float sfxDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    public bool isPlayingBgm;
    private int bgmIndex;


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

    private void Update()
    {
        if (!isPlayingBgm)
        {
            StopAllBgm();
        }
        else
        {
            if (!bgm[bgmIndex].isPlaying)
            {
                PlayRandomBgm();
            }
        }
    }

    public void PlaySFX(int sfxIndex, Transform sourse = null)
    {
        //if (sfx[sfxIndex].isPlaying)
        //    return;

        if (sourse != null && Vector2.Distance(PlayerManager.Instance.Player.transform.position, sourse.position) > sfxDistance)
            return;

        if (sfxIndex < sfx.Length)
        {
            sfx[sfxIndex].pitch = Random.Range(0.85f, 1.15f);
            sfx[sfxIndex].Play();
        }
    }

    public void PlayRandomBgm()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    public void StopSFX(int sfxIndex)
    {
        if (sfxIndex < sfx.Length)
        {
            sfx[sfxIndex].Stop();
        }
    }
    public void StopSfxWith(int index)
    {
        StartCoroutine(DecreaseVolume(sfx[index]));
    }

    IEnumerator DecreaseVolume(AudioSource audio)
    {
        float defaultVolume = audio.volume;

        while (audio.volume > 0.1f)
        {
            audio.volume -= audio.volume * 0.2f;
            yield return new WaitForSeconds(0.25f);

            if (audio.volume <= 0.1f)
            {
                audio.Stop();
                audio.volume = defaultVolume;
                break;
            }
        }
    }

    public void PlayBGM(int bgmIndex)
    {
        this.bgmIndex = bgmIndex;

        StopAllBgm();

        bgm[bgmIndex].Play();
    }

    public void StopAllBgm()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }
}
