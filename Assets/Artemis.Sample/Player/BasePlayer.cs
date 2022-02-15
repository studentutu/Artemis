using System;
using Artemis.Sample.Core;
using UnityEngine;

public abstract class BasePlayer : MonoBehaviour
{
    public Guid Id;

    public void Initialize(Guid id, string nickname, Color color, Vector2 position)
    {
        var view = GetComponentInChildren<PlayerView>();
        Id = id;
        view.Nickname.text = nickname;
        view.Sprite.color = color;
        view.transform.position = position;
    }

    public abstract void OnSnapshotReceived(int tick, PlayerData snapshot);
}