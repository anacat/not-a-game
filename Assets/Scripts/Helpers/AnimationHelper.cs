using System;
using System.Collections;
using UnityEngine;

public class AnimationHelper
{
    public static IEnumerator WaitForAnimation(Animator animator, int layer = 0)
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(layer).Length);
    }
}