using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource appStart;
    public AudioClip[] moos;
    public AudioClip cashIn;
    public AudioClip delete;

    // Play start car engine sound
    public void StartEngine()
    {
        appStart.Play();
    }

    // Play a random moo
    public void SayMoo()
    {
        var randomMoo = moos[Random.Range(0, moos.Length)];
        
        appStart.PlayOneShot(randomMoo);
    }

    // Play cash in sound
    public void CashIn()
    {
        appStart.PlayOneShot(cashIn);
    }

    // Play delete sound
    public void Delete()
    {
        appStart.PlayOneShot(delete);
    }
}
