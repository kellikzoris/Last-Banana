using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLoop : MonoBehaviour
{
    [SerializeField] private Player _playerTransform;
    [SerializeField] private float speed;

    private Vector3 targetPosition = Vector2.zero;

    [SerializeField] private EnemyWeapon enemyWeapon;

    [SerializeField] private ShadowTigerManager _shadowTigerManager;

    private StompAttack _stompAttack;

    private Coroutine callShootCO;
    public bool isShootingToPlayer;
    private bool isMoving;
    private float timeBetweenShots = 1f;

    private bool isShootingAllDirection;
    private float timeBetweenAllDirectionShots = 1f;
    private bool isDashingThroughScreen = false;

    private bool isSpawningMinions = false;
    private float timeBetweenSpawningMinions = 1f;
    private Coroutine spawnMinionCO = null;

    private bool isSpawningShields = false;
    private bool isChargeAttackRun = false;
    private bool withFireTrailBehind = false;
    private bool isStompAttack = false;
    private bool is360RotationAttack = false;
    private bool isSpawningTornado = false;
    private bool isRunningTowardsPlayer = false;
    private bool isPlantingATrap = false;

    [Serializable]
    public class EnemyMovement
    {
        public string name;

        [Space(10)]
        public bool isChargeAttackRun;

        public bool withFireTrailBehind = false;

        [Space(10)]
        public bool isStompAttack;

        public int stompAmount;
        public float stompSize;

        [Space(10)]
        public bool is360RotationAttack;

        public bool isSpawningTornado;
        public int numberOfTornadoes;
        public float delayBetweenTornadoes;

        public enum TornadoType
        {
            whiteTornado,
            yellowTornado,
            redTornado
        }

        public TornadoType tornadoType;

        [Space(10)]
        public float timeForThisAction;

        public bool isEndConditionForReachingTarget;

        [Space(10)]
        public bool isRunningTowardsPlayer;
        [Range(30, 50)]
        public float movementSpeedWhileRunningTowardsPlayer;

        [Range(12, 15)]
        public float stopDistanceWithPlayer;

        public int amountOfPawnAttacks;
        public float timeDelayBetweenPawnAttacks;

        [Space(10)]
        public bool isMoving;

        [EnableIf("isMoving")]
        public float movementSpeed;

        [EnableIf("isMoving")]
        public Vector2 targetPosition;

        [Space(10)]
        public bool isShootingToPlayer;

        [EnableIf("isShootingToPlayer")]
        public float timeBetweenShots;

        [Space(10)]
        public bool isShootingAllDirection;

        [EnableIf("isShootingAllDirection")]
        public float timeBetweenAllDirectionShots;

        [Space(10)]
        public bool isDashingThroughScreen;

        [Space(10)]
        public bool isSpawningMinions;

        [EnableIf("isSpawningMinions")]
        public float timeBetweenSpawningMinions;

        [Space(10)]
        public bool isSpawningShield;

        [Space(10)]
        public bool isPlantingATrap = false;

        public int trapAmount;
        public float timeBetweenTraps;

        [Space(10)]
        public bool isShadowAttacking;

        public int shadowTigerAmount;
        public int pawnAttackAmount;

        [Space(10)]
        public bool darkenEnvironment;

        public int darkenEnvironmentTime;
    }

    [SerializeField] private int currentIndex;

    [SerializeField] private List<EnemyMovement> _currentPhaseEnemyMovement;
    [SerializeField] private List<EnemyMovement> enemyMovements;
    [SerializeField] private List<EnemyMovement> enemyMovementsForTheSecondPhase;
    [SerializeField] private List<EnemyMovement> enemyMovementsForTheThirdPhase;

    [Header("Visuals")]
    [SerializeField] private Transform _enemySpriteTransform;

    [SerializeField] private Animator _enemyAnimator;

    [SerializeField] private AnimationClip _stompAttackAnimation;
    private Coroutine _stompAttackInCyclesCO = null;
    private int _currentAmountInStompAttack = 0;
    private Vector2 _lastSpawnedFireTrailPos = Vector2.zero;

    [SerializeField] private GorillaTrapPlanter _gorillaTrapPlanter;

    private Coroutine _enemyCycleCO;
    private bool _isEnemyDead = false;

    private void Start()
    {
        currentIndex = 0;
        _enemyCycleCO = StartCoroutine(DoEnemyMovementCycles());
    }

    public void StopEnemyMovementCycle()
    {
        _isEnemyDead = true;
        if (_enemyCycleCO != null)
        {
            StopCoroutine(_enemyCycleCO);
        }
    }

    private List<EnemyMovement> HandleBossPhaseChanges()
    {
        int enemyHealth = GetComponent<Enemy>().GetEnemyHealth();

        Debug.Log($"enemy health {enemyHealth}");

        if (enemyHealth <= 35)
        {
            _enemyAnimator.SetLayerWeight(0, 0);
            _enemyAnimator.SetLayerWeight(1, 1);
            return enemyMovementsForTheThirdPhase;
        }
        else if ((enemyHealth <= 70) && (35 < enemyHealth))
        {
            return enemyMovementsForTheSecondPhase;
        }
        else
        {
            return enemyMovements;
        }
    }

    private IEnumerator DoEnemyMovementCycles()
    {
        _currentPhaseEnemyMovement = HandleBossPhaseChanges();
        if (currentIndex >= _currentPhaseEnemyMovement.Count)
        {
            currentIndex = 0;
        }

        GetComponent<EnemyWeapon>().StopSpawnMinionCO();

        EnemyMovement currentEnemyMovement = _currentPhaseEnemyMovement[currentIndex];

        if (currentEnemyMovement.isMoving)
        {
            targetPosition = currentEnemyMovement.targetPosition;
        }

        isMoving = currentEnemyMovement.isMoving;

        isRunningTowardsPlayer = currentEnemyMovement.isRunningTowardsPlayer;

        isChargeAttackRun = currentEnemyMovement.isChargeAttackRun;
        withFireTrailBehind = currentEnemyMovement.withFireTrailBehind;

        if (!isChargeAttackRun)
        {
            if (isMoving)
            {
                _enemyAnimator.SetBool("isRunning", true);
            }
            else
            {
                _enemyAnimator.SetBool("isRunning", false);
                _enemyAnimator.SetBool("chargeAttackRun", false);
            }
        }

        if (isChargeAttackRun)
        {
            if (isMoving)
            {
                _enemyAnimator.SetBool("chargeAttackRun", true);
            }
            else
            {
                _enemyAnimator.SetBool("isRunning", false);
                _enemyAnimator.SetBool("chargeAttackRun", false);
            }
        }

        isShootingToPlayer = currentEnemyMovement.isShootingToPlayer;

        isShootingAllDirection = currentEnemyMovement.isShootingAllDirection;

        isDashingThroughScreen = currentEnemyMovement.isDashingThroughScreen;

        isSpawningMinions = currentEnemyMovement.isSpawningMinions;

        isSpawningShields = currentEnemyMovement.isSpawningShield;

        is360RotationAttack = currentEnemyMovement.is360RotationAttack;

        isPlantingATrap = currentEnemyMovement.isPlantingATrap;

        if (currentEnemyMovement.darkenEnvironment)
        {
            FindObjectOfType<TrackGorillaLocation>(true).SetTrackGorilla(currentEnemyMovement.darkenEnvironmentTime);
        }

        if (currentEnemyMovement.isShadowAttacking)
        {
            _shadowTigerManager.SpawnShadowTigers(currentEnemyMovement.shadowTigerAmount, currentEnemyMovement.pawnAttackAmount);
            yield return new WaitUntil(() => _shadowTigerManager.GetIsSpawnShadowTigerISFinished());
        }

        if (isRunningTowardsPlayer)
        {
            GetComponentInChildren<PawnAttack>().StartPawnAttack(currentEnemyMovement.movementSpeedWhileRunningTowardsPlayer, 
                currentEnemyMovement.amountOfPawnAttacks, currentEnemyMovement.timeDelayBetweenPawnAttacks, 
                currentEnemyMovement.stopDistanceWithPlayer, currentEnemyMovement.withFireTrailBehind);
            yield return new WaitUntil(() => GetComponentInChildren<PawnAttack>().GetPawnAttackCycleFinished());
            GetComponentInChildren<PawnAttack>().ResetIsPawnAttackFinished();
        }

        if (is360RotationAttack)
        {
            GetComponent<EnemyWeapon>().Start360Attack(currentEnemyMovement.isSpawningTornado, (int)currentEnemyMovement.tornadoType,
                currentEnemyMovement.numberOfTornadoes,
                currentEnemyMovement.delayBetweenTornadoes);
            GetComponent<Enemy>().CanGetHit(false);
        }

        if (isSpawningShields)
        {
            GetComponent<EnemyWeapon>().SpawnEnemyShield();
        }

        if (isSpawningMinions)
        {
            GetComponent<EnemyWeapon>().SpawnMinion(currentEnemyMovement.timeBetweenSpawningMinions);
        }

        if (isDashingThroughScreen == true)
        {
            GetComponent<EnemyDashingThroughScreen>().DashThroughScreen();
        }

        if (isPlantingATrap)
        {
            _gorillaTrapPlanter.StartPlantingATrap(currentEnemyMovement.trapAmount, currentEnemyMovement.timeBetweenTraps);
            yield return new WaitUntil(() => _gorillaTrapPlanter.IsTheCycleFinised() == true);
            _gorillaTrapPlanter.ResetTrapCycleFinished();
        }

        if (currentEnemyMovement.isStompAttack)
        {
            _stompAttackInCyclesCO = StartCoroutine(StompAttackInCycles(currentEnemyMovement.stompSize, currentEnemyMovement.stompAmount));
            yield return new WaitUntil(() => _stompAttackInCyclesCO == null);
            _currentAmountInStompAttack = 0;
        }
        else if (!currentEnemyMovement.isEndConditionForReachingTarget)
        {
            yield return new WaitForSeconds(currentEnemyMovement.timeForThisAction);
            GetComponent<Enemy>().CanGetHit(true);
        }
        else if (isDashingThroughScreen == true)
        {
            yield return new WaitUntil(() => GetComponent<EnemyDashingThroughScreen>().GetDashThroughScreenIsFinished());
        }
        else
        {
            yield return new WaitUntil(() => Vector2.Distance(transform.position, targetPosition) < .5f);
            isMoving = false;
        }

        currentIndex += 1;

        if (is360RotationAttack)
        {
            GetComponent<EnemyWeapon>().Stop360Attack();
        }

        if (!_isEnemyDead)
        {
            _enemyCycleCO = StartCoroutine(DoEnemyMovementCycles());
        }
    }

    private IEnumerator StompAttackInCycles(float stompMaxScale, int loopAmount)
    {
        if (_currentAmountInStompAttack >= loopAmount)
        {
            _stompAttackInCyclesCO = null;
            yield break;
        }

        Debug.Log($"StompAttackInCycles with {_currentAmountInStompAttack} and {loopAmount} and {_currentAmountInStompAttack <= loopAmount} and {_stompAttackAnimation.length}");
        GetComponent<EnemyWeapon>().StompAttack(stompMaxScale);
        yield return new WaitForSeconds(_stompAttackAnimation.length + .5f);

        _currentAmountInStompAttack++;
        _stompAttackInCyclesCO = StartCoroutine(StompAttackInCycles(stompMaxScale, loopAmount));
    }

    private IEnumerator ShootPlayerWithDelay(float delay)
    {
        enemyWeapon.ShootPlayer();
        yield return new WaitForSeconds(delay);
        if (isShootingToPlayer == false)
        {
            yield break;
        }
        else
        {
            callShootCO = StartCoroutine(ShootPlayerWithDelay(2f));
        }
    }

    public void LookAtTargetVector3(Vector2 targetVector2)
    {
        if (targetVector2.x - transform.position.x > 0)
        {
            _enemySpriteTransform.transform.localScale = Vector3.one;
        }
        else
        {
            _enemySpriteTransform.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            float step = _currentPhaseEnemyMovement[currentIndex].movementSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);

            //Debug.Log($"Vector2.Distance(transform.position, targetPosition) {Vector2.Distance(transform.position, targetPosition)}");
            //Debug.Log($"Distance To Target Position {Vector2.Distance(this.transform.position, targetPosition)}");
            //if (Vector2.Distance(this.transform.position, targetPosition) < 2f)
            //{
            //    isMoving = false;
            //}

            LookAtTargetVector3(targetPosition);
            if (withFireTrailBehind)
            {
                if (Vector2.Distance(_lastSpawnedFireTrailPos, transform.position) >= 3)
                {
                    _lastSpawnedFireTrailPos = transform.position;
                    FindObjectOfType<FireTrailManager>(true).SpawnAvailableFireTrailOnThePositionOf(transform.position);
                }
            }
        }

        if (isShootingToPlayer)
        {
            timeBetweenShots -= Time.deltaTime;
            if (timeBetweenShots <= 0)
            {
                enemyWeapon.ShootPlayer();
                timeBetweenShots = _currentPhaseEnemyMovement[currentIndex].timeBetweenShots;
            }
        }

        if (isShootingAllDirection)
        {
            timeBetweenAllDirectionShots -= Time.deltaTime;
            if (timeBetweenAllDirectionShots <= 0)
            {
                enemyWeapon.CircleAttack();
                //TODO: when circle attach is triggered there is a weird behaviour inside the enemy movement.
                timeBetweenAllDirectionShots = _currentPhaseEnemyMovement[currentIndex].timeBetweenAllDirectionShots;
            }
        }
    }
}