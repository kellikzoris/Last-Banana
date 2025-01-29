using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"OnTriggerEnter2DCalledWithinMeleeAttackto {collision.tag}");
        if (collision.tag.Contains("minion"))
        {
            collision.transform.GetComponent<Minion>().DisableThis();
        }

        if (collision.tag == "enemy")
        {
            collision.transform.GetComponent<Enemy>().DoEnemyGotHit();
        }

        if (collision.tag == "ShadowTiger")
        {
            collision.transform.GetComponent<ShadowTigerEnemy>().DoEnemyGotHit();
        }
    }
}