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

    public void ShootPlayer()
    {
        Vector3 direction = (_player.transform.position - this.transform.position).normalized;
        int radius = 4;
        Vector3 projectileShootOffsetValue = direction * radius;

        EnemyProjectile newProjectile = Instantiate(_enemyProjectile, this.transform.position + projectileShootOffsetValue, Quaternion.identity);
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
            newProjectile.gameObject.SetActive(true);

            Vector3 targetDirection = (points[i] - this.transform.position).normalized;
            newProjectile.ShootWithoutTarget(targetDirection);
        }
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

        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    SpawnMinion();
        //}
#endif
    }
}