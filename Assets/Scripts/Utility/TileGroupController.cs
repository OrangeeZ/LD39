using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Monads;
using UnityEngine;
using UnityEngine.Rendering;

//[RequireComponent(typeof(BoxCollider))]
[ExecuteInEditMode]
[SelectionBase]
public class TileGroupController : MonoBehaviour
{
    [SerializeField]
    private Sprite[] _sprites;

    [SerializeField]
    private int _spriteIndex;

    [SerializeField]
    private int _spriteCount;

    [SerializeField]
    private bool _hasCollisions = true;

    private bool _isDirty = false;

    void OnValidate()
    {
        _spriteIndex = _spriteIndex.Clamped(0, _sprites.Length);
        _spriteCount = _spriteCount.Clamped(0, 100);

        if (_sprites.Length == 0 || !gameObject.activeInHierarchy)
        {
            return;
        }

        _isDirty = true;
    }

    void Update()
    {
        if (!_isDirty)
        {
            return;
        }

        _isDirty = false;
        
        GenerateRenderers();
        GenerateCollider();
        GeneratePathfinding();
    }

    private void GenerateRenderers()
    {
        var removalList = new List<Transform>();
        foreach (var each in transform)
        {
            removalList.Add(each as Transform);
        }

        foreach (var each in removalList)
        {
            DestroyImmediate(each.gameObject);
        }
        
        var sprite = _sprites[_spriteIndex];

        for (var i = 0; i < _spriteCount; i++)
        {
            var spriteRenderer = new GameObject($"Tile {i}").AddComponent<SpriteRenderer>();
            spriteRenderer.transform.SetParent(transform);
            spriteRenderer.transform.localPosition = Vector3.right * sprite.bounds.size.x * i;
            spriteRenderer.sprite = sprite;
        }
    }

    private void GenerateCollider()
    {
//        var sprite = _sprites[_spriteIndex];
//        var spriteBounds = sprite.bounds;
//        var spriteBoundsSize = spriteBounds.size;
//        spriteBoundsSize.x *= _spriteCount;
//
//        GetComponent<BoxCollider>().size = spriteBoundsSize;
//        GetComponent<BoxCollider>().center = Vector3.right * spriteBoundsSize.x * 0.5f - Vector3.right * sprite.bounds.extents.x;
    }

    private void GeneratePathfinding()
    {
        var sprite = _sprites[_spriteIndex];
        var spriteBounds = sprite.bounds;
        var spriteBoundsSize = spriteBounds.size;
        spriteBoundsSize.x *= _spriteCount;
        spriteBoundsSize.z = 10f;

        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.ShadowsOnly;
        cube.transform.SetParent(transform);

        cube.GetComponent<BoxCollider>().enabled = _hasCollisions;
        
        cube.transform.localScale = spriteBoundsSize;
        cube.transform.localPosition = Vector3.right * spriteBoundsSize.x * 0.5f - Vector3.right * sprite.bounds.extents.x;
        cube.isStatic = true;
    }
}