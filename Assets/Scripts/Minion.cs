using UnityEngine;

public class Minion : MonoBehaviour
{
    private bool isMoving = false;
    private Transform targetTransform = null;
    [SerializeField] private float movementSpeed = 5;

    public void StartMovingTowardsTarget()
    {
        targetTransform = FindObjectOfType<Player>(true).transform;
        isMoving = true;
    }

    private void LookAtTargetPosition()
    {
        transform.up = (Vector2)targetTransform.transform.position - new Vector2(transform.position.x, transform.position.y);
    }

    public void DisableThis()
    {
        this.gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Contains("Player"))
        {
            collision.transform.GetComponent<Player>().DoPlayerGotHit(this.transform);
            DisableThis();
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            float step = movementSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetTransform.position, step);
            LookAtTargetPosition();
        }
    }
}