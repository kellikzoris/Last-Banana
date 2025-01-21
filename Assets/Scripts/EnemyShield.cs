using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    Transform rotateAroundTransform;
    bool startRotateAround = false;
    [SerializeField] int rotateAroundSpeed = 5;


    public void StartTurningAroundEnemy(Transform enemyTransform) 
    {
        rotateAroundTransform = enemyTransform;
        startRotateAround = true;
    }

    void Update()
    {
        if (startRotateAround)
        {
            Vector3 centerPosition = rotateAroundTransform.position;
            transform.RotateAround(centerPosition, Vector3.forward, rotateAroundSpeed  * Time.deltaTime);
        }
    }

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
        this.gameObject.SetActive(false);
    }
}
