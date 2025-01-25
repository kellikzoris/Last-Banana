using UnityEngine;

public class StompAttack : MonoBehaviour
{
    [SerializeField] private bool _isStomping = false;
    [SerializeField] private float _stompSpeed;
    [SerializeField] Vector3 _targetStompScale = Vector3.one;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            Debug.Log("Stomp Attack Hits the player!");
            collision.transform.GetComponent<Player>().DoPlayerGotHit(this.transform);
        }
    }

    public void StartStomping()
    {
        _isStomping = false;
        this.transform.localScale = Vector3.zero;
        _isStomping = true;
    }

    public void SetStompParameter(float targetStompSize)
    {
        _targetStompScale = Vector3.one * targetStompSize;
    }

    private void Update()
    {
        if (_isStomping)
        {
            this.transform.localScale += Vector3.one * _stompSpeed * Time.deltaTime;
            if (this.transform.localScale.magnitude >= _targetStompScale.magnitude)
            {
                _isStomping = false;
                this.transform.localScale = Vector3.zero;
            }
        }
    }
}