using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveStartTrigger : ShotTrigger
{
    [SerializeField]
    protected WaveManager waveManager;

    [Button]
    public override void TriggerMechanic()
    {
        waveManager.StartNextWave();
    }
}
