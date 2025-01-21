using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMine : MonoBehaviour
{
    [SerializeField] Vector3 _targetTransformPosition;
    bool _targetSet;
    [SerializeField] float _projectileSpeed = 5;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Contains("Player"))
        {
            collision.transform.GetComponent<Player>().DoPlayerGotHit(this.transform);
            DisableEnemyProjectile();
        }
    }

    public void DisableEnemyProjectile()
    {
        _targetSet = false;
        this.gameObject.SetActive(false);
    }


    public void StartMovingToTargetTransform(Transform targetTransform)
    {
        _targetTransformPosition = targetTransform.transform.position;
        _targetSet = true;
    }


    private void Update()
    {
        if (_targetSet)
        {
            float step = _projectileSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, _targetTransformPosition, step);
            //transform.position += _targetTransformPosition * step;

            // TO-DO : Vector2 Move Towards can be used as mine when planeted by the enemy it flashes and explodes within 3 secons.


        }
    }
}
