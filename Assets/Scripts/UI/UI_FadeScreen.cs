using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FadeScreen : MonoBehaviour
{
    private const string FADE_IN = "FadeIn";
    private const string FADE_OUT = "FadeOut";

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void FadeIn() => animator.SetTrigger(FADE_IN);
    public void FadeOut() => animator.SetTrigger(FADE_OUT);
}
