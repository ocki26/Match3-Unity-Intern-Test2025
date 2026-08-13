using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[Serializable]
public class Item
{
    // The current cell holding this item on the board
    public Cell Cell { get; private set; }

    // The original board cell where this item was spawned (used in Time Attack mode to return item)
    public Cell OriginalCell { get; set; }

    // Visual transform representing this item in the scene
    public Transform View { get; private set; }

    /// <summary>
    /// Instantiates the prefab for this item based on its type.
    /// </summary>
    public virtual void SetView()
    {
        string prefabname = GetPrefabName();

        if (!string.IsNullOrEmpty(prefabname))
        {
            GameObject prefab = Resources.Load<GameObject>(prefabname);
            if (prefab)
            {
                View = GameObject.Instantiate(prefab).transform;
            }
        }
    }

    protected virtual string GetPrefabName() { return string.Empty; }

    public virtual void SetCell(Cell cell)
    {
        Cell = cell;
        if (OriginalCell == null && cell != null)
        {
            OriginalCell = cell;
        }
    }

    internal void AnimationMoveToPosition()
    {
        if (View == null) return;

        View.DOMove(Cell.transform.position, 0.2f);
    }

    public void SetViewPosition(Vector3 pos)
    {
        if (View)
        {
            View.position = pos;
        }
    }

    public void SetViewRoot(Transform root)
    {
        if (View)
        {
            View.SetParent(root);
        }
    }

    public void SetSortingLayerHigher()
    {
        if (View == null) return;

        SpriteRenderer sp = View.GetComponent<SpriteRenderer>();
        if (sp)
        {
            sp.sortingOrder = 1;
        }
    }

    public void SetSortingLayerLower()
    {
        if (View == null) return;

        SpriteRenderer sp = View.GetComponent<SpriteRenderer>();
        if (sp)
        {
            sp.sortingOrder = 0;
        }
    }

    internal void ShowAppearAnimation()
    {
        if (View == null) return;

        Vector3 scale = View.localScale;
        View.localScale = Vector3.one * 0.1f;
        View.DOScale(scale, 0.15f).SetEase(Ease.OutBack);
    }

    internal virtual bool IsSameType(Item other)
    {
        return false;
    }

    /// <summary>
    /// Plays clear animation scaling down to 0 before destroying GameObject (Task 3 requirement).
    /// </summary>
    internal virtual void ExplodeView()
    {
        if (View)
        {
            Transform v = View;
            View = null; // Detach reference immediately

            // Smooth scale down to 0
            v.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack).OnComplete(() =>
            {
                if (v != null && v.gameObject != null)
                {
                    GameObject.Destroy(v.gameObject);
                }
            });
        }
    }

    internal void AnimateForHint()
    {
        if (View)
        {
            View.DOPunchScale(View.localScale * 0.1f, 0.1f).SetLoops(-1);
        }
    }

    internal void StopAnimateForHint()
    {
        if (View)
        {
            View.DOKill();
        }
    }

    internal void Clear()
    {
        Cell = null;
        OriginalCell = null;

        if (View)
        {
            GameObject.Destroy(View.gameObject);
            View = null;
        }
    }
}
