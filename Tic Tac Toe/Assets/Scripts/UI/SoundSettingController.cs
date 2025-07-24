using System;
using TicTacToe.Utility.Events;
using UnityEngine;
public class SoundSettingController
{
    private GameObject settingScreen;

    public SoundSettingController(GameObject settingScreen)
    {
        this.settingScreen = settingScreen;

    }

    public void ToggleSettingScreen()
    {
        if (settingScreen.activeSelf)
        {
            settingScreen.SetActive(false);
        }
        else
        {
            settingScreen.SetActive(true);
        }
    }

}
