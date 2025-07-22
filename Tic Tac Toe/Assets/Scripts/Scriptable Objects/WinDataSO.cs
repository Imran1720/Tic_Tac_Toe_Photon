using System.Collections.Generic;
using TicTacToe.Utility;
using UnityEngine;

[CreateAssetMenu(fileName = "WinDataSO", menuName = "ScriptableObjects/WinDataSO")]
public class WinDataSO : ScriptableObject
{
    public List<WinData> winDataList;
}
