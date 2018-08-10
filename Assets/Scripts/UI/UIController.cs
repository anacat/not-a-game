using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public CanvasGroup inventoryGroup;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void ShowInventoryGroup()
    {
        _animator.SetBool("ShowInventory", true);
    }

    public void HideInventoryGroup()
    {
        _animator.SetBool("ShowInventory", false);
    }
}
