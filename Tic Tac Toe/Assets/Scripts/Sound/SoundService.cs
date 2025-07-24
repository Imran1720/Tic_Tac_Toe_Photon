using TicTacToe.Core;
using TicTacToe.Player;
using TicTacToe.Utility.Events;
using UnityEngine;

namespace TicTacToe.Audio
{

    public class SoundService
    {
        private EventService eventService;

        private AudioSource bgmAudioSource;
        private AudioSource sfxAudioSource;

        private AudioClipSO audioClipSO;

        public SoundService(EventService eventService, AudioClipSO audioClipSO, AudioSource bgm, AudioSource sfx)
        {
            this.audioClipSO = audioClipSO;

            this.eventService = eventService;
            bgmAudioSource = bgm;
            sfxAudioSource = sfx;

            StartBGM();
        }

        public void AddEventListeners()
        {
            eventService.OnBoardHighlightRequested.AddListener(OnBoardHighlighted);
            eventService.OnRequestTileClickSound.AddListener(OnClickDetected);
            eventService.OnGameWon.AddListener(OnGameWin);
            eventService.OnGameTied.AddListener(OnGameTie);
            eventService.OnButtonClickRequested.AddListener(PlayButtonClick);
            eventService.OnBGMVolumeChanged.AddListener(BGMVolumeChanged);
            eventService.OnSFXVolumeChanged.AddListener(SFXVolumeChanged);
        }

        public void RemoveEventListeners()
        {
            eventService.OnBoardHighlightRequested.RemoveListener(OnBoardHighlighted);
            eventService.OnRequestTileClickSound.RemoveListener(OnClickDetected);
            eventService.OnGameWon.RemoveListener(OnGameWin);
            eventService.OnGameTied.RemoveListener(OnGameTie);
            eventService.OnButtonClickRequested.RemoveListener(PlayButtonClick);
            eventService.OnBGMVolumeChanged.RemoveListener(BGMVolumeChanged);
            eventService.OnSFXVolumeChanged.RemoveListener(SFXVolumeChanged);
        }

        private void StartBGM()
        {
            AudioClip clip = GetAudioClip(SoundType.BGM);
            if (clip != null)
            {
                bgmAudioSource.clip = clip;
                bgmAudioSource.Play();
            }
        }

        private void PlaySfx(SoundType soundType)
        {
            AudioClip clip = GetAudioClip(soundType);
            if (clip != null)
            {
                sfxAudioSource.PlayOneShot(clip);
            }
        }

        private AudioClip GetAudioClip(SoundType soundType)
        {
            SoundData data = audioClipSO.soundDataList.Find(sound => sound.soundType == soundType);

            if (data != null)
            {
                return data.audioClip;
            }
            return null;
        }


        private void OnGameWin(PlayerType player)
        {
            if (player == GameService.Instance.GetLocalPlayerType())
            {
                PlaySfx(SoundType.WIN);
            }
            else
            {
                PlaySfx(SoundType.LOSE);
            }
        }

        public void PlayButtonClick()
        {
            PlaySfx(SoundType.BUTTON);
        }

        public Vector2 GetAudioVolumes()
        {
            return new Vector2(bgmAudioSource.volume, sfxAudioSource.volume);
        }

        private void BGMVolumeChanged(float volume)
        {
            bgmAudioSource.volume = volume;
        }

        private void SFXVolumeChanged(float volume)
        {
            sfxAudioSource.volume = volume;
        }

        private void OnClickDetected() => PlaySfx(SoundType.CLICK);
        private void OnGameTie() => PlaySfx(SoundType.TIE);
        private void OnBoardHighlighted(PlayerType player)
        {
            Debug.Log(player == GameService.Instance.GetLocalPlayerType());
            if (GameService.Instance.GetLocalPlayerType() == player)
            {
                PlaySfx(SoundType.TURN);
            }
        }
    }
}