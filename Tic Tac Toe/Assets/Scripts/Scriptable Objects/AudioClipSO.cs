using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe.Audio
{
    [CreateAssetMenu(fileName = "AudioClipSO", menuName = "ScriptableObjects/AudioClipSO")]
    public class AudioClipSO : ScriptableObject
    {
        public List<SoundData> soundDataList;
    }
}
