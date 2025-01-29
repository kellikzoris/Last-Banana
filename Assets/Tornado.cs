using UnityEngine;

public class Tornado : MonoBehaviour
{
    [SerializeField] private float _tornadoSpeed;
    [SerializeField] private bool isMoving;
    [SerializeField] private Vector2 targetDirection;

    public void StartMovingTowards(Transform targetTranform)
    {
        targetDirection = (targetTranform.position - this.transform.position).normalized * 1000;
        isMoving = true;
        Invoke("DisableWithDelay", 10);
    }

    void DisableWithDelay()
    {
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isMoving)
        {
            float step = _tornadoSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetDirection, step);
        }
    }
}