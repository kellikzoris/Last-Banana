using UnityEngine;

public class PlayerSpriteAnimatorListener : MonoBehaviour
{
    [SerializeField] private Collider2D _meleeAttackCollider;
    [SerializeField] Animator _animator;
    [SerializeField] ParticleSystem _waveAttack;

    public void DisableCollider2D(string s)
    {
        Debug.Log("EndOfAnimationIsCalled");
        _meleeAttackCollider.enabled = false;
    }

    public void EnableCollider2D(string s)
    {
        Debug.Log("EndOfAnimationIsCalled");
        _meleeAttackCollider.enabled = true;
        _waveAttack.Play();
    }

    public void DoMeleeAttack()
    {
        _animator.SetTrigger("meleeAttack");
    }


    private void Update()
    {
        if (this.transform.localScale.x < 0)
        {
            _meleeAttackCollider.transform.localPosition = new Vector3(-2.5f, 1, 0);
        }
        else
        {
            _meleeAttackCollider.transform.localPosition = new Vector3(2.5f, 1, 0);
        }
    }
}