using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] StompAttack _stompAttack;
    [SerializeField] PawnAttack _pawnAttack;
    [SerializeField] GorillaTrapPlanter _gorillaTrapPlanter;
    [SerializeField] ShadowTiger _shadowTiger;
    [SerializeField] bool _isThisShadowTiger = false;

    public void CallStompAttack(int i)
    {
        Debug.Log("CallStompAttackFromTheAnimation");
        _stompAttack.StartStomping();
    }

    public void FinishPawnAttack(int i)
    {
        Debug.Log("FinishPawnAttack");

        if (_isThisShadowTiger)
        {
            _shadowTiger.DisablePawnAttack();
        }
        else
        {
            _pawnAttack.DisablePawnAttack();
        }
    }

    public void PlantTheTrap(int i)
    {
        Debug.Log("TrapPlanted");
        _gorillaTrapPlanter.PlantTheTrap();
    }
}
