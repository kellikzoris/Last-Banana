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

    bool isSpawningShields = false;

    [Serializable]
    public class EnemyMovement
    {
        public float timeForThisAction;
        public bool isEndConditionForReachingTarget;

        [Space(10)]
        public bool isMoving;

        public float movementSpeed;
        public Vector2 targetPosition;

        [Space(10)]
        public bool isShootingToPlayer;

        public float timeBetweenShots;

        [Space(10)]
        public bool isShootingAllDirection;

        public float timeBetweenAllDirectionShots;

        [Space(10)]
        public bool isDashingThroughScreen;

        [Space(10)]
        public bool isSpawningMinions;

        public float timeBetweenSpawningMinions;

        [Space (10)]
        public bool isSpawningShield;
    }

    [SerializeField] private int currentIndex;

    [SerializeField] private List<EnemyMovement> _currentPhaseEnemyMovement;
    [SerializeField] private List<EnemyMovement> enemyMovements;
    [SerializeField] private List<EnemyMovement> enemyMovementsForTheSecondPhase;
    [SerializeField] private List<EnemyMovement> enemyMovementsForTheThirdPhase;

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
        transform.up = (Vector2)targetPosition - new Vector2(transform.position.x, transform.position.y);
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