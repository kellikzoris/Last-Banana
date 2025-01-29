using NaughtyAttributes;
using UnityEngine;

public class ShadowTigerEnemy : MonoBehaviour
{
    [SerializeField] private float _shadowTigerHealth = 20;
    [SerializeField] private Animator _shadowTigerAnimator;
    [SerializeField] Transform _shadowTigerSpriteTransform;

    [Button]
    public void StartAction()
    {
        GetComponentInChildren<ShadowTiger>().StartPawnAttack(3, 2, 12, DisableThisGameObject);
    }

    public void StartActionFromEnemyLoop(int pawnAttackAmount)
    {
        GetComponentInChildren<ShadowTiger>().StartPawnAttack(3, 2, 12, DisableThisGameObject);
    }

    public void DoEnemyGotHit()
    {
        _shadowTigerAnimator.SetTrigger("gotDamage");

        Debug.Log("ShadowTigerEnemy Got Hit");
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        _shadowTigerHealth -= 10;
        if (_shadowTigerHealth <= 0)
        {
            Debug.Log("Enemy already eliminated");
            DisableThisGameObject();
        }
        //_enemyHealthTextField.text = $"Enemy Health: {_enemyHealth}";
        //_enemyHealthLeft.fillAmount = (float)_enemyHealth / 100;
    }

    public void DisableThisGameObject()
    {
        this.gameObject.SetActive(false);
    }

    public void LookAtTargetVector3(Vector2 targetVector2)
    {
        if (targetVector2.x - transform.position.x > 0)
        {
            _shadowTigerSpriteTransform.transform.localScale = Vector3.one;
        }
        else
        {
            _shadowTigerSpriteTransform.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Contains("Player"))
        {
            collision.transform.GetComponent<Player>().DoPlayerGotHit(this.transform);
        }
    }
}