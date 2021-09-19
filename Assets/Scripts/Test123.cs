using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class Test123 : Singleton<Test123>
{
    public override void StartListenToEvents()
    {
        EventManager.AddListener(GameEvent.VIBRATE_HEAVY, VibrateHeavy);
    }

    public override void StopListenToEvents()
    {
        EventManager.RemoveListener(GameEvent.VIBRATE_HEAVY, VibrateHeavy);
    }

    public void VibrateHeavy()
    {
        if (PlayerPrefs.GetInt("Vibration") == 1)
        {
            MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
        }
    }
}
