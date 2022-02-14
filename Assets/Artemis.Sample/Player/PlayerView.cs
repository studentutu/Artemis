using System;
using TMPro;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public Guid PlayerId;
    [field: SerializeField] public TMP_Text Nickname { get; private set; }
    [field: SerializeField] public SpriteRenderer Sprite { get; private set; }
}