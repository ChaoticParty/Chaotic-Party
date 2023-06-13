using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<SoundEvent> playEvent;
    [SerializeField] private List<SoundEvent> stopEvent;
    [SerializeField] private List<SoundEvent> loopEvent;

    public void EventPlay(string id)
    {
        foreach (var soundEvent in playEvent.Where(soundEvent => soundEvent.id.Equals(id)))
        {
            soundEvent.soundEvent.Invoke();
            Debug.Log("SoundEvent : Play. Id : "+id);
            return;
        }
        Debug.LogError("L'id : " + id + " des SoundEvent est inconnu.");
    }
    public void EventStop(string id)
    {
        foreach (var soundEvent in stopEvent.Where(soundEvent => soundEvent.id.Equals(id)))
        {
            soundEvent.soundEvent.Invoke();
            Debug.Log("SoundEvent : Stop. Id : "+id);
            return;
        }
        Debug.LogError("L'id : " + id + " des SoundEvent est inconnu.");
    }
    public void EventLoop(string id)
    {
        foreach (var soundEvent in loopEvent.Where(soundEvent => soundEvent.id.Equals(id)))
        {
            soundEvent.soundEvent.Invoke();
            Debug.Log("SoundEvent : Loop. Id : "+id);
            return;
        }
        Debug.LogError("L'id : " + id + " des SoundEvent est inconnu.");
    }

    public void PlaySelfSound(AudioSource cibleAudio)
    {
        Debug.Log("SoundEvent : Self. Id : "+cibleAudio.gameObject.name);
        if (cibleAudio.clip == null) return;
        
        cibleAudio.Play();
    }

    [Serializable]
    public struct SoundEvent
    {
        public string id;
        public UnityEvent soundEvent;
    }
}
