using nickeltin.GameData.Events;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorParamatersChanger : MonoBehaviour
{
    private static readonly int animIndex_animator = Animator.StringToHash("AnimIndex");
    private static readonly int next_animator = Animator.StringToHash("Next");
    
    private Animator m_animator;

    private void Awake() => m_animator = GetComponent<Animator>();

    public void ChangeTo(int i)
    {
        m_animator.SetInteger(animIndex_animator, i);
        m_animator.SetTrigger(next_animator);
    }
}
