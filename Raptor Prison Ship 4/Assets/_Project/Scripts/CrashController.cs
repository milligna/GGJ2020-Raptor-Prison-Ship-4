﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashController : MonoBehaviour
{
    public GameObject sparkEffect;
    public GameObject smokeEffect;
    public GameObject explosionEffect;

    void SetCrashScreen()
    {
        // Set crash screen for the computer object
    }

    void StartSmokingEffect()
    {
        // Activate the Smoke/spark object in the Prefab
        sparkEffect.SetActive(true);
        smokeEffect.SetActive(true);
    }

    void TriggerCountdownAudio()
    {
        // Activate the Countdown Audio Noise in the Prefab

    }

    void TriggerExplosionEffect()
    {
        // instantiate Explosion audio and prefab, kill it and end the game

    }

    void CancelCrashEffects()
    {
        // Deactivate all objects in the prefab
    }
}


