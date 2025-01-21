using UnityEngine;

public class Flag : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ShowFlag()
    {
        if (animator != null)
        {
            animator.SetTrigger("Show");
        }
    }

    public void HideFlag()
    {
        if (animator != null)
        {
            animator.SetTrigger("Hide");
        }
    }
}
