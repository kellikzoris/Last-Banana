using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] StompAttack _stompAttack;

    public void CallStompAttack(int i)
    {
        Debug.Log("CallStompAttackFromTheAnimation");
        _stompAttack.StartStomping();
    }
}
