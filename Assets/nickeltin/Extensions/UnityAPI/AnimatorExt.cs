using UnityEngine;

namespace nickeltin.Extensions
{
    public static class AnimatorExt
    {
        public static void PlayWithDuration(this Animator animator, string stateName, 
            string speedDependantName, float animationDuration)
        {
            animator.Play(stateName);
            animator.SetFloat(speedDependantName, 1 / animationDuration);
        }
        
        public static void PlayWithDuration(this Animator animator, int triggerId,
            string speedDependantName, float animationDuration)
        {
            animator.SetTrigger(triggerId);
            animator.SetFloat(speedDependantName, 1 / animationDuration);
        }
    }
}