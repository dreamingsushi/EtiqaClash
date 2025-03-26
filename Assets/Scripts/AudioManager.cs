using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;    
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("BGM");
    }
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        RPC_PlaySFX(name);
    }
    
    public void PlaySFXForBothPlayers(string name)
    {
        PhotonView photonView = GetComponent<PhotonView>();
        if (photonView != null && PhotonNetwork.InRoom)
        {
            photonView.RPC("RPC_PlaySFX", RpcTarget.All, name);
        }
        else
        {
            RPC_PlaySFX(name); // Fallback for offline mode
        }
    }

    [PunRPC]
    private void RPC_PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log($"Sound '{name}' not found!");
            return;
        }

        sfxSource.PlayOneShot(s.clip);
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    public void PlayBattleMusic()
    {
        PlaySFX("PreGame");
        StartCoroutine(DelayedPlayMusic(3f));
    }

    private IEnumerator DelayedPlayMusic(float delay)
    {
        yield return new WaitForSeconds(2f);
        PlaySFX("Game");
        yield return new WaitForSeconds(delay);
        PlayMusic("BattleBGM");
    }
}
