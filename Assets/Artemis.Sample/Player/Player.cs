using System;
using UnityEngine;

public class Player : MonoBehaviour
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
}