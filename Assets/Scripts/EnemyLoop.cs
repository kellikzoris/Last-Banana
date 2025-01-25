using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLoop : MonoBehaviour
{
    [SerializeField] private float speed;

    private Vector3 targetPosition = Vector2.zero;

    [SerializeField] private EnemyWeapon enemyWeapon;

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
    private bool isChargeAttackRun;

    [Serializable]
    public class EnemyMovement
    {
        public string name;

        [Space(10)]
        public bool isChargeAttackRun;

        [Space(10)]
        public float timeForThisAction;

        public bool isEndConditionForReachingTarget;

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
    }

    [SerializeField] private int currentIndex;

    [SerializeField] private List<EnemyMovement> _currentPhaseEnemyMovement;
    [SerializeField] private List<EnemyMovement> enemyMovements;
    [SerializeField] private List<EnemyMovement> enemyMovementsForTheSecondPhase;
    [SerializeField] private List<EnemyMovement> enemyMovementsForTheThirdPhase;

    [Header("Visuals")]
    [SerializeField] private Transform _enemySpriteTransform;

    [SerializeField] private Animator _enemyAnimator;

    private void Start()
    {
        currentIndex = 0;
        StartCoroutine(DoEnemyMovementCycles());
    }

    private List<EnemyMovement> HandleBossPhaseChanges()
    {
        int enemyHealth = GetComponent<Enemy>().GetEnemyHealth();

        Debug.Log($"enemy health {enemyHealth}");

        if (enemyHealth <= 25)
        {
            return enemyMovementsForTheThirdPhase;
        }
        else if ((enemyHealth <= 50) && (25 < enemyHealth))
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

        isChargeAttackRun = currentEnemyMovement.isChargeAttackRun;

        if (!isChargeAttackRun)
        {
            if (isMoving)
            {
                _enemyAnimator.SetBool("isWalking", true);
            }
            else
            {
                _enemyAnimator.SetBool("isWalking", false);
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
                _enemyAnimator.SetBool("isWalking", false);

                _enemyAnimator.SetBool("chargeAttackRun", false);
            }
        }

        isShootingToPlayer = currentEnemyMovement.isShootingToPlayer;

        isShootingAllDirection = currentEnemyMovement.isShootingAllDirection;

        isDashingThroughScreen = currentEnemyMovement.isDashingThroughScreen;

        isSpawningMinions = currentEnemyMovement.isSpawningMinions;

        isSpawningShields = currentEnemyMovement.isSpawningShield;

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

        if (!currentEnemyMovement.isEndConditionForReachingTarget)
        {
            yield return new WaitForSeconds(currentEnemyMovement.timeForThisAction);
        }
        else if (isDashingThroughScreen == true)
        {
            yield return new WaitUntil(() => GetComponent<EnemyDashingThroughScreen>().GetDashThroughScreenIsFinished());
        }
        else
        {
            yield return new WaitUntil(() => Vector2.Distance(transform.position, targetPosition) < .5f);
        }

        currentIndex += 1;

        StartCoroutine(DoEnemyMovementCycles());
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

    private void LookAtTargetVector3()
    {
        //transform.up = (Vector2)targetPosition - new Vector2(transform.position.x, transform.position.y);
        Vector2 targetTransform = targetPosition;

        if (targetTransform.x - transform.position.x > 0)
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
            LookAtTargetVector3();
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