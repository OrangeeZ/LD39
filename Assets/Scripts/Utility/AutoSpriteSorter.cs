using UnityEngine;

public class AutoSpriteSorter : MonoBehaviour
{
    [SerializeField]
    private Vector3 _sortingAxisMask = new Vector3(0, 0, 1);

    [SerializeField]
    private float _sortingOrderScale = 100f;

    [ContextMenu("Sort")]
    private void Sort()
    {
#if UNITY_EDITOR
        var sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach (var each in sprites)
        {
            var sortingOrder = Vector3.Dot(each.transform.position, _sortingAxisMask);
            each.sortingOrder = Mathf.RoundToInt(sortingOrder * _sortingOrderScale);

            UnityEditor.EditorUtility.SetDirty(each);
        }

        foreach (var each in GetComponentsInChildren<TileMovement>())
        {
            DestroyImmediate(each);
        }

        foreach (var each in GetComponentsInChildren<IsometricMapGenerator>())
        {
            DestroyImmediate(each);
        }
#endif
    }
}