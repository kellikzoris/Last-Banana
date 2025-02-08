using System.Collections;
using UnityEngine;

public class PawnAttack : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private EnemyLoop _enemyLoop;
    [SerializeField] private Player _player;
    [SerializeField] private int _pawnAttackAmount;
    private float _timeBetweenPawnAttacks;
    private bool _isPawnAttackFinished = false;
    private float _stopDistanceWithPlayer;
    private bool _isRunningTowardsPlayer;

    [SerializeField] private float _movementSpeed = 30;
    [SerializeField] private ParticleSystem _swordWaveYellow;

    [SerializeField] bool _withFireTrailBehind;
    private Vector2 _lastSpawnedFireTrailPos = Vector2.zero;


    public void StartPawnAttack(float movementSpeed, int pawnAttackAmount, float timeBetweenPawnAttacks, float stopDistanceWithPlayer, bool withFireTrailBehind)
    {
        _movementSpeed = movementSpeed;
        Debug.Log($"StartPawnAttackWithParameter {pawnAttackAmount} and {timeBetweenPawnAttacks} and {stopDistanceWithPlayer}");
        _withFireTrailBehind = withFireTrailBehind;

        _isRunningTowardsPlayer = true;
        _enemy.GetComponentInChildren<Animator>().SetBool("isRunning", true);

        _pawnAttackAmount = pawnAttackAmount;
        _timeBetweenPawnAttacks = timeBetweenPawnAttacks;
        _stopDistanceWithPlayer = stopDistanceWithPlayer;

        StartCoroutine(PawnAttackWithDelay());
    }

    public bool GetPawnAttackCycleFinished()
    {
        return _isPawnAttackFinished;
    }

    public void ResetIsPawnAttackFinished()
    {
        _isPawnAttackFinished = false;
    }

    private IEnumerator PawnAttackWithDelay()
    {
        yield return new WaitUntil(() => Vector2.Distance(_enemy.transform.position, _player.transform.position) <= _stopDistanceWithPlayer);

        for (int i = 0; i < _pawnAttackAmount; i++)
        {
            DoPawnAttack();
            yield return new WaitForSeconds(_timeBetweenPawnAttacks);
        }

        _isPawnAttackFinished = true;
    }

    private void DoPawnAttack()
    {
        //this should call one pawn attack for enabling and disabling the pawn attack game object collider.
        // if collision with player is happening then it should hurt the player.
        _swordWaveYellow.Play();
        _enemy.GetComponentInChildren<Animator>().SetTrigger("Attack");
        FindObjectOfType<SoundManager>().PlayTigerAttackSound();  
        GetComponent<Collider2D>().enabled = true;
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
        _enemyLoop.LookAtTargetVector3(_player.transform.position);

        if (_isRunningTowardsPlayer)
        {
            float step = _movementSpeed * Time.deltaTime;
            _enemy.transform.position = Vector2.MoveTowards(_enemy.transform.position, _player.transform.position, step);
            //Debug.Log($"Distance Between Enemy And Player {Vector2.Distance(_enemy.transform.position, _player.transform.position)}");
            if (Vector2.Distance(_enemy.transform.position, _player.transform.position) < _stopDistanceWithPlayer)
            {
                _isRunningTowardsPlayer = false;
                _enemy.GetComponentInChildren<Animator>().SetBool("isRunning", false);
            }

            if (_withFireTrailBehind)
            {
                if (Vector2.Distance(_lastSpawnedFireTrailPos, transform.position) >= 3)
                {
                    _lastSpawnedFireTrailPos = transform.position;
                    FindObjectOfType<FireTrailManager>(true).SpawnAvailableFireTrailOnThePositionOf(transform.position);
                }
            }
        }
    }
}