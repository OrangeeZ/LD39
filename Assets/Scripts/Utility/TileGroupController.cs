using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGroupController : MonoBehaviour
{
    [SerializeField]
    private Sprite[] _sprites;

    [SerializeField]
    private int _spriteIndex;

    void OnValidate()
    {
        if (_sprites.Length == 0)
        {
            return;
        }

        _spriteIndex = _spriteIndex.Clamped(0, _sprites.Length);

        foreach (var each in GetComponentsInChildren<SpriteRenderer>())
        {
            each.sprite = _sprites[_spriteIndex];
        }
    }
}