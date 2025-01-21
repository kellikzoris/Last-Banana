using TMPro;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private Vector3 _targetTransformPosition;
    private bool _targetSet;
    [SerializeField] private float _projectileSpeed = 5;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Contains("Player"))
        {
            collision.transform.GetComponent<Player>().DoPlayerGotHit(this.transform);
            DisableEnemyProjectile();
        }

        if (collision.transform.tag.Contains("border"))
        {
            DisableEnemyProjectile();
        }
    }

    public void DisableEnemyProjectile()
    {
        _targetSet = false;
        this.gameObject.SetActive(false);
    }

    public void ShootToTarget(Transform targetTransform)
    {
        GetComponent<Rigidbody2D>().velocity = (targetTransform.transform.position - this.transform.position).normalized * _projectileSpeed;
    }

    public void ShootWithoutTarget(Vector3 targetDirection)
    {
        GetComponent<Rigidbody2D>().velocity = targetDirection.normalized * _projectileSpeed;
    }

}