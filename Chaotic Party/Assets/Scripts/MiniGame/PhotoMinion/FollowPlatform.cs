using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class FollowPlatform : SerializedMonoBehaviour
{
    public Dictionary<Transform, Transform> _playerToParent = new();
    public Transform playerHolder;

    private void Awake()
    {
        playerHolder ??= transform.Find("PlayerHolder");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player") && !_playerToParent.ContainsKey(other.transform))
        {
            _playerToParent.Add(other.transform, other.transform.parent);
            other.transform.SetParent(playerHolder);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player") )
        {
            Transform playerTransform = other.transform;
            if (!_playerToParent.ContainsKey(playerTransform)) return;

            playerTransform.SetParent(_playerToParent[playerTransform]);
            _playerToParent.Remove(playerTransform);
        }
    }
}
