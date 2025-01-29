using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] private EnemyProjectile _enemyProjectile;
    [SerializeField] private EnemyShield _enemyShield;
    [SerializeField] private Minion _enemyMinion;

    [SerializeField] private List<EnemyShield> _enemyShields = new List<EnemyShield>();
    [SerializeField] private List<Vector2> _enemyShieldSpawnPoints = new List<Vector2>();

    [SerializeField] private Player _player;
    private Vector3[] points;

    [SerializeField] private int shootAmountInCircleAttack = 5;
    [SerializeField] private int enemyShieldAmount = 3;

    private Coroutine spawnMinionCO;
    [SerializeField] private ParticleSystem _swordWhirlwindYellow;
    [SerializeField] private Transform _tornadoReference;
    [SerializeField] private Transform _tornadoReferenceYellow;
    [SerializeField] private Transform _tornadoReferenceRed;

    public void ShootPlayer()
    {
        Vector3 direction = (_player.transform.position - this.transform.position).normalized;
        int radius = 4;
        Vector3 projectileShootOffsetValue = direction * radius;

        EnemyProjectile newProjectile = Instantiate(_enemyProjectile, this.transform.position + projectileShootOffsetValue, Quaternion.identity);

        newProjectile.transform.up = (Vector2)_player.transform.position - new Vector2(transform.position.x, transform.position.y);

        //Debug.Break();
        newProjectile.gameObject.SetActive(true);
        newProjectile.ShootToTarget(_player.transform);
    }

    public void CircleAttack()
    {
        int loopPointCount = shootAmountInCircleAttack;
        points = new Vector3[loopPointCount];

        Vector3 direction = (_player.transform.position - this.transform.position).normalized;
        int radius = 4;
        Vector3 projectileShootOffsetValue = direction * radius;

        for (int i = 0; i < loopPointCount; i++)
        {
            float angle = (90) * Mathf.Deg2Rad + i * Mathf.PI * 2 / shootAmountInCircleAttack;
            Vector3 localPoint = new Vector3(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius,
                0f
            );
            points[i] = this.transform.position + this.transform.rotation * localPoint; // Use the center transform and rotation
        }

        for (int i = 0; i < points.Length; i++)
        {
            EnemyProjectile newProjectile = Instantiate(_enemyProjectile, points[i], Quaternion.identity);
            Vector3 actualDirection = (points[i] - this.transform.position).normalized;

            newProjectile.transform.up = actualDirection;

            newProjectile.gameObject.SetActive(true);

            Vector3 targetDirection = (points[i] - this.transform.position).normalized;
            newProjectile.ShootWithoutTarget(targetDirection);
        }
    }

    public void StompAttack(float stompMaxScale)
    {
        Debug.Log("StompAttack");
        GetComponentInChildren<StompAttack>().SetStompParameter(stompMaxScale);
        GetComponentInChildren<Animator>().SetTrigger("isJumping");
    }

    public void SpawnEnemyShield()
    {
        if (_enemyShields.Count != 0)
        {
            for (int i = 0; i < _enemyShields.Count; i++)
            {
                _enemyShields[i].gameObject.SetActive(true);
                _enemyShields[i].transform.position = _enemyShieldSpawnPoints[i];
            }

            return;
        }

        int loopPointCount = enemyShieldAmount;
        points = new Vector3[loopPointCount];

        Vector3 direction = (_player.transform.position - this.transform.position).normalized;
        int radius = 10;
        Vector3 projectileShootOffsetValue = direction * radius;

        for (int i = 0; i < loopPointCount; i++)
        {
            float angle = (90) * Mathf.Deg2Rad + i * Mathf.PI * 2 / enemyShieldAmount;
            Vector3 localPoint = new Vector3(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius,
                0f
            );
            points[i] = this.transform.position + this.transform.rotation * localPoint; // Use the center transform and rotation
        }

        for (int i = 0; i < points.Length; i++)
        {
            EnemyShield enemyShield = Instantiate(_enemyShield, points[i], Quaternion.identity, this.transform);
            _enemyShields.Add(enemyShield);
            _enemyShieldSpawnPoints.Add(points[i]);

            enemyShield.gameObject.SetActive(true);

            Vector3 targetDirection = (points[i] - this.transform.position).normalized;
            enemyShield.StartTurningAroundEnemy(this.transform);
        }
    }

    public void StopSpawnMinionCO()
    {
        if (spawnMinionCO != null)
        {
            StopCoroutine(spawnMinionCO);
        }
    }

    public void Start360Attack(bool isSpawningTornadoes, int tornadoType /*whichCanBe:0,1,2*/, int numberOfTornadoes = 1, float delayBetweenTornadoes = 2)
    {
        Debug.Log("Do360Attack");
        GetComponentInChildren<Animator>().SetBool("360Attack", true);
        _swordWhirlwindYellow.gameObject.SetActive(true);
        if (isSpawningTornadoes)
        {
            StartCoroutine(SpawnTornadoesWithDelayCO(numberOfTornadoes, delayBetweenTornadoes,tornadoType));
        }
    }

    private IEnumerator SpawnTornadoesWithDelayCO(int numberOfTornadoes, float delayBetweenTornadoes, int tornadoType)
    {
        if (tornadoType == 0)
        {
            Debug.Log("SpawnWhiteTornado");
            for (int i = 0; i < numberOfTornadoes; i++)
            {
                Transform newTornado = Instantiate(_tornadoReference, this.transform.position, Quaternion.identity, null);
                newTornado.GetComponent<Tornado>().StartMovingTowards(_player.transform);
                yield return new WaitForSeconds(delayBetweenTornadoes);
            }
        }
        if (tornadoType == 1)
        {
            Debug.Log("SpawnYellowTornado");
            for (int i = 0; i < numberOfTornadoes; i++)
            {
                Transform newTornado = Instantiate(_tornadoReferenceYellow, this.transform.position, Quaternion.identity, null);
                newTornado.GetComponent<Tornado>().StartMovingTowards(_player.transform);
                yield return new WaitForSeconds(delayBetweenTornadoes);
            }
        }
        if (tornadoType == 2)
        {
            Debug.Log("SpawnRedTornado");
            for (int i = 0; i < numberOfTornadoes; i++)
            {
                Transform newTornado = Instantiate(_tornadoReferenceRed, this.transform.position, Quaternion.identity, null);
                newTornado.GetComponent<Tornado>().StartMovingTowards(_player.transform);
                yield return new WaitForSeconds(delayBetweenTornadoes);
            }
        }
    }

    public void Stop360Attack()
    {
        Debug.Log("Stop360Attack");
        GetComponentInChildren<Animator>().SetBool("360Attack", false);
        _swordWhirlwindYellow.gameObject.SetActive(false);
    }

    public void SpawnMinion(float timeBetweenMinionSpawns)
    {
        spawnMinionCO = StartCoroutine(SpawnMinionWithDelay(timeBetweenMinionSpawns));
    }

    private IEnumerator SpawnMinionWithDelay(float timeBetweenMinionSpawns)
    {
        Vector2 directionTowardsThePlayer = (_player.transform.position - this.transform.position).normalized;
        Vector2 spawnPosition = (Vector2)this.transform.position + directionTowardsThePlayer;

        Minion enemyMinion = Instantiate(_enemyMinion, spawnPosition, Quaternion.identity);
        enemyMinion.gameObject.SetActive(true);
        enemyMinion.StartMovingTowardsTarget();
        yield return new WaitForSeconds(timeBetweenMinionSpawns);
        spawnMinionCO = StartCoroutine(SpawnMinionWithDelay(timeBetweenMinionSpawns));
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.M))
        {
            ShootPlayer();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            CircleAttack();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            SpawnEnemyShield();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            SpawnEnemyShield();
        }
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    StompAttack(.3f);
        //}

#endif
    }
}