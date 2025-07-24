using System;
using TicTacToe.Audio;
using UnityEngine;
using UnityEngine.UI;

public class MenuSoundManager : MonoBehaviour
{

    [SerializeField] private AudioSource bgm_AudioSource;
    [SerializeField] private AudioSource sfx_AudioSource;
    [SerializeField] private AudioClipSO audioClipSO;

    [SerializeField] private GameObject settingScreen;
    [SerializeField] private Button settingButton;

    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private SoundService soundService;
    private SoundSettingController soundSettingController;
    private void Awake()
    {
        soundService = new SoundService(null, audioClipSO, bgm_AudioSource, sfx_AudioSource);
        soundSettingController = new SoundSettingController(settingScreen);
        settingButton.onClick.AddListener(ToggleSettingScreen);

        bgmSlider.onValueChanged.AddListener(BGMVolumeChanged);
        sfxSlider.onValueChanged.AddListener(SFXVolumeChanged);

        SetAudioSourceVolume();
    }

    private void SetAudioSourceVolume()
    {
        bgmSlider.value = bgm_AudioSource.volume;
        sfxSlider.value = sfx_AudioSource.volume;
    }

    private void BGMVolumeChanged(float volume)
    {
        bgm_AudioSource.volume = volume;
    }

    private void SFXVolumeChanged(float volume)
    {
        sfx_AudioSource.volume = volume;
    }

    public void PlayButtonClick()
    {
        soundService.PlayButtonClick();
    }



    private void ToggleSettingScreen()
    {
        soundService.PlayButtonClick();
        soundSettingController.ToggleSettingScreen();
    }
}
