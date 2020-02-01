using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashController : MonoBehaviour
{
   
    public GameObject smokeEffect;
    public GameObject explosionEffectPrefab;
    

    void SetCrashScreen()
    {
        // Set crash screen for the computer object
    }

    void StartSmokingEffect()
    {
        // Activate the Smoke object in the Prefab
    
        smokeEffect.SetActive(true);
    }

    void TriggerCountdownAudio()
    {
        // Activate the Countdown Audio Noise in the Prefab

    }

    void TriggerExplosionEffect()
    {
        // instantiate Explosion audio and prefab, kill it and end the game
        Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
    }

    void CancelCrashEffects()
    {
        // Deactivate all objects in the prefab
    }
}


