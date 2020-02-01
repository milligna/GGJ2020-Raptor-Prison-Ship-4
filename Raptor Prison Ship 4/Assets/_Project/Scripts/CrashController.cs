using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CrashController : MonoBehaviour
{
   
    public GameObject smokeEffect;
    public GameObject explosionEffectPrefab;
    // public GameObject countdownStressSound;  // not sure if I need this - trying to set audioclip objects maybe?
  
   public AudioSource audioData;


public void SetCrashScreen()
    {
        // Set crash screen for the computer object

    }

   public void StartSmokingEffect()
    {
        // Activate the Smoke object in the Prefab
        smokeEffect.SetActive(true);
    }

    public void TriggerCountdownAudio()
    {
        // Activate the Countdown Audio Noise in the Prefab
        audioData = GetComponent<AudioSource>();
        audioData.Play(0);
    }

    public void TriggerExplosionEffect()
    {
        // instantiate Explosion audio and prefab, kill it and end the game
        Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
     }

   public void CancelCrashEffects()
    {
        // Deactivate all objects in the prefab
        smokeEffect.SetActive(false);
    }
}


