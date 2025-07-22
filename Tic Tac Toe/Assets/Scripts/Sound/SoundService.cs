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

        public SoundService(GameService gameService, AudioClipSO audioClipSO)
        {
            this.audioClipSO = audioClipSO;

            eventService = gameService.GetEventService();
            bgmAudioSource = gameService.GetBGMAudioSource();
            sfxAudioSource = gameService.GetSFXAudioSource();

            AddEventListeners();
            StartBGM();
        }

        public void AddEventListeners()
        {
            eventService.OnRequestTileClickSound.AddListener(OnClickDetected);
            eventService.OnGameWon.AddListener(OnGameWin);
            eventService.OnGameTied.AddListener(OnGameTie);
        }

        public void RemoveEventListeners()
        {
            eventService.OnRequestTileClickSound.AddListener(OnClickDetected);
            eventService.OnGameWon.AddListener(OnGameWin);
            eventService.OnGameTied.AddListener(OnGameTie);
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

        private void OnClickDetected() => PlaySfx(SoundType.CLICK);
        private void OnGameTie() => PlaySfx(SoundType.LOSE);
    }
}