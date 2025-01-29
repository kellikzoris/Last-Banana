using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;

public class ShadowTiger : MonoBehaviour
{
    [SerializeField] private Transform _enemy;

    [SerializeField] private Player _player;

    [SerializeField] private int _pawnAttackAmount;
    private float _timeBetweenPawnAttacks;
    private bool _isPawnAttackFinished = false;
    private float _stopDistanceWithPlayer;
    private bool _isRunningTowardsPlayer;

    [SerializeField] private float movementSpeed = 30;
    [SerializeField] private ParticleSystem _swordWaveYellow;
    [SerializeField] ShadowTigerEnemy _shadowTigerEnemy;

    

    public void StartPawnAttack(int pawnAttackAmount, float timeBetweenPawnAttacks, float stopDistanceWithPlayer, Action callbackFunctionWhenCycleFinished)
    {
        Debug.Log($"StartPawnAttackWithParameter {pawnAttackAmount} and {timeBetweenPawnAttacks} and {stopDistanceWithPlayer}");
        _isRunningTowardsPlayer = true;
        _enemy.GetComponentInChildren<Animator>().SetBool("isRunning", true);

        _pawnAttackAmount = pawnAttackAmount;
        _timeBetweenPawnAttacks = timeBetweenPawnAttacks;
        _stopDistanceWithPlayer = stopDistanceWithPlayer;

        StartCoroutine(PawnAttackWithDelay(callbackFunctionWhenCycleFinished));
    }

    public bool GetPawnAttackCycleFinished()
    {
        return _isPawnAttackFinished;
    }

    public void ResetIsPawnAttackFinished()
    {
        _isPawnAttackFinished = false;
    }

    private IEnumerator PawnAttackWithDelay(Action callbackFunctionWhenCycleFinished)
    {
        yield return new WaitUntil(() => Vector2.Distance(_enemy.transform.position, _player.transform.position) <= _stopDistanceWithPlayer);

        for (int i = 0; i < _pawnAttackAmount; i++)
        {
            DoPawnAttack();
            yield return new WaitForSeconds(_timeBetweenPawnAttacks);
        }

        callbackFunctionWhenCycleFinished?.Invoke();
        //_isPawnAttackFinished = true;
    }

    private void DoPawnAttack()
    {
        //this should call one pawn attack for enabling and disabling the pawn attack game object collider.
        // if collision with player is happening then it should hurt the player.
        _swordWaveYellow.Play();
        _enemy.GetComponentInChildren<Animator>().SetTrigger("Attack");
        GetComponent<Collider2D>().enabled = true;
        //Debug.Break();
    }

    public void DisablePawnAttack()
    {
        GetComponent<Collider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.transform.GetComponent<Player>().DoPlayerGotHit(this.transform);
        }
    }

    private void LookAtTargetVector3()
    {
        Vector2 targetTransform = _player.transform.position;

        if (targetTransform.x - transform.position.x > 0)
        {
            this.transform.localEulerAngles = new Vector3(0, 0, -90);
            this.transform.localPosition = new Vector3(1.28f, 0, 0);
        }
        else
        {
            this.transform.localEulerAngles = new Vector3(0, 0, 90);
            this.transform.localPosition = new Vector3(-1.28f, 0, 0);
        }
    }

    private void Update()
    {
        LookAtTargetVector3();
        _shadowTigerEnemy.LookAtTargetVector3(_player.transform.position);

        if (_isRunningTowardsPlayer)
        {
            float step = movementSpeed * Time.deltaTime;
            _enemy.transform.position = Vector2.MoveTowards(_enemy.transform.position, _player.transform.position, step);
            Debug.Log($"Distance Between Enemy And Player {Vector2.Distance(_enemy.transform.position, _player.transform.position)}");
            if (Vector2.Distance(_enemy.transform.position, _player.transform.position) < _stopDistanceWithPlayer)
            {
                _isRunningTowardsPlayer = false;
                _enemy.GetComponentInChildren<Animator>().SetBool("isRunning", false);
            }
        }
    }
}