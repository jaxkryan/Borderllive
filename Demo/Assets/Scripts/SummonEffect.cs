using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonEffect : MonoBehaviour
{
    private Animator animator;
    private bool isPlaying = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    public void ShowPillar(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
        isPlaying = true;
        animator.Play("SummonEffect");
    }

    public void HidePillar()
    {
        gameObject.SetActive(false);
        isPlaying = false;
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }

    void Update()
    {
        if (isPlaying && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            HidePillar();
        }
    }
}
