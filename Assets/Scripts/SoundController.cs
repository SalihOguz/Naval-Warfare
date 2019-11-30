using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioClip[] SoundsList;

    public AudioClip GetRandomSound()
    {
        return SoundsList[Random.Range(0, SoundsList.Length)];
    }
}
