using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSoundSO", menuName = "ScriptableObjects/PlayerSound")]
public class PlayerSoundSo : ScriptableObject
{
    public AudioClip idle;
    public AudioClip triche;
    public AudioClip happy;
    public AudioClip sad;
    public AudioClip hit;
}
