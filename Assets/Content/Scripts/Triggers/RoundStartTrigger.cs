using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundStartTrigger : ShotTrigger
{
    [SerializeField]
    protected WaveManager waveManager;

    [Button]
    public override void TriggerMechanic()
    {
        waveManager.StartRound();
    }
}
