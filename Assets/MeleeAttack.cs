using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Animator _animator;

   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("minion"))
        {
            collision.transform.GetComponent<Minion>().DisableThis();
        }

        if (collision.tag=="enemy")
        {
            collision.transform.GetComponent<Enemy>().DoEnemyGotHit();
        }
    }
}