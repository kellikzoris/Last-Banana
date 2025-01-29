using System.Collections.Generic;
using UnityEngine;

public class ShadowTigerManager : MonoBehaviour
{
    [SerializeField] private List<Transform> _placesWhereTheShadowTigersCanBeInstantiated;
    [SerializeField] private List<Transform> _positionsAlreadySelected;
    [SerializeField] private List<GameObject> _instantiatedShadowTigers;
    [SerializeField] private Player _player;
    [SerializeField] private Transform _shadowTiger;
    [SerializeField] private Animator _enemyAnimator;

    private bool _isSpawnShadowTigerISFinished = false;

    public void SpawnShadowTigers(int shadowTigerAmount, int pawnAttackAmount)
    {
        Debug.Log("SpawnShadowTigersCalled");
        _enemyAnimator.SetTrigger("isJumping");

        for (int i = 0; i < shadowTigerAmount; i++)
        {
            Transform selectedTransform = ReturnTheDistantPositionOfTheTrapPositions();
            _positionsAlreadySelected.Add(selectedTransform);
            Transform newTiger = Instantiate(_shadowTiger, selectedTransform.position, Quaternion.identity, null);
            newTiger.GetComponent<ShadowTigerEnemy>().StartActionFromEnemyLoop(pawnAttackAmount);
            _instantiatedShadowTigers.Add(newTiger.gameObject);
        }

        _positionsAlreadySelected.Clear();
    }

    public bool GetIsSpawnShadowTigerISFinished()
    {
        foreach (GameObject tiger in _instantiatedShadowTigers)
        {
            if (tiger.activeInHierarchy)
            {
                return false;
            }
        }

        //_enemyAnimator.SetBool("isEating", false);
        return true;
    }

    private Transform ReturnTheDistantPositionOfTheTrapPositions()
    {
        Transform selectedTransform = null;
        float distanceToPos = 0;
        foreach (Transform transform in _placesWhereTheShadowTigersCanBeInstantiated)
        {
            if (Vector2.Distance(_player.transform.position, transform.position) > distanceToPos &&
                !_positionsAlreadySelected.Contains(transform))
            {
                distanceToPos = Vector2.Distance(_player.transform.position, transform.position);
                selectedTransform = transform;
            }
        }
        return selectedTransform;
    }
}