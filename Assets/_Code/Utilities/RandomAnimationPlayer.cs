using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RandomAnimationPlayer : MonoBehaviour
{
    public List<string> animationTriggersToPlay = new List<string>();
    public float minTimeBeforeAnimation = 1.0f;
    public float maxTimeBeforeAnimation = 5.0f;

    private Animator animator;
    private float elapsed;
    private float nextTarget;

    protected void Awake()
    {
        animator = GetComponent<Animator>();
        elapsed = 0;
        nextTarget = Random.Range(minTimeBeforeAnimation, maxTimeBeforeAnimation);
    }

    protected void Update()
    {
        elapsed += Time.deltaTime;
        if(elapsed > nextTarget)
        {
            string trigger = animationTriggersToPlay[Random.Range(0, animationTriggersToPlay.Count)];
            animator.SetTrigger(trigger);

            elapsed = 0;
            nextTarget = Random.Range(minTimeBeforeAnimation, maxTimeBeforeAnimation);
        }
    }
}
