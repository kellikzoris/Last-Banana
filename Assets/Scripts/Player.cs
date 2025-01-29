using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _speed;
    [SerializeField] private int _playerHealth = 100;
    [SerializeField] private TMP_Text _playerHealthTextField;
    [SerializeField] private Image _playerHealthLeft;

    private Coroutine _returnBackToOriginalColorCO;
    public bool _enableControls = true;
    private Transform _lookAtTarget;
    private Coroutine _dashCoroutine;

    [Header("Dash Parameters")]
    [SerializeField] private float _dashMultiplier = 2;

    [SerializeField][Range(.01f, .5f)] private float _dashDuration;

    [Header("Visuals")]
    [SerializeField] private Transform _playerSpriteTransform;

    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private ParticleSystem _dashTrail;

    public void SetLookAtTarget(Transform transform)
    {
        _lookAtTarget = transform;
    }

    public void DoPlayerGotHit(Transform gotHitFrom)
    {
        _playerAnimator.SetTrigger("gotDamage");

        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        StartCoroutine(DoKickBackEffect(gotHitFrom));

        _playerHealth -= 10;
        if (_playerHealth <= 0)
        {
            Debug.Log("Player already eliminated");
        }
        _playerHealthLeft.fillAmount = (float)_playerHealth / 100;
    }

    private IEnumerator DoPlayerGotHitWithDelayCO(Transform gotHitFrom, float delayAmount)
    {
        _rigidbody.velocity = Vector2.zero;
        _enableControls = false;
        yield return new WaitForSeconds(delayAmount);
        DoPlayerGotHit(gotHitFrom);
        _enableControls = true;
    }

    public void DoPlayerGotHitWithDelay(Transform gotHitFrom, float delayAmount)
    {
        StartCoroutine(DoPlayerGotHitWithDelayCO(gotHitFrom, delayAmount));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag.Contains("banana"))
        {
            if (!collision.transform.GetComponent<Banana>().GetIsBananaOnTheFly())
            {
                if (collision.transform.GetComponent<Banana>().GetBananaOnGround())
                {
                    Debug.Log("Let's collect this banana");
                    GetComponent<Weapon>().CollectBananaFromGround(collision.collider.GetComponent<Banana>());
                }
            }
        }
    }

    private void MoveCharacter()
    {
        Vector2 inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDirection = mousePos - (Vector2)this.transform.position;

        //Debug.Log($"vector2Dot {Vector2.Dot(inputVector, lookDirection)}");
        if (Vector2.Dot(inputVector, lookDirection) < 0)
        {
            _playerAnimator.SetFloat("speedMultiplier", -1);
        }
        else
        {
            _playerAnimator.SetFloat("speedMultiplier", 1);
        }

        _playerAnimator.SetBool("isWalking", inputVector != Vector2.zero);
        _rigidbody.velocity = inputVector.normalized * _speed;
    }

    private void LookAtTheMouse()
    {
        //Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //transform.up = mousePos - new Vector2(transform.position.x, transform.position.y);

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePos.x - transform.position.x > 0)
        {
            _playerSpriteTransform.transform.localScale = Vector3.one;
        }
        else
        {
            _playerSpriteTransform.transform.localScale = new Vector3(-1, 1, 1);
        }
        //Debug.Log(mousePos.x - transform.position.x > 0);
    }

    private void LookAtTargetPosition()
    {
        Vector2 targetTransform = _lookAtTarget.transform.position;

        if (targetTransform.x - transform.position.x > 0)
        {
            _playerSpriteTransform.transform.localScale = Vector3.one;
        }
        else
        {
            _playerSpriteTransform.transform.localScale = new Vector3(-1, 1, 1);
        }
        //transform.up = (Vector2)_lookAtTarget.transform.position - new Vector2(transform.position.x, transform.position.y);
    }

    public void DisableControls()
    {
        _enableControls = false;
    }

    public void EnableControls()
    {
        _enableControls = true;
    }

    private IEnumerator DoKickBackEffect(Transform kickBackFrom)
    {
        DisableControls();
        Vector2 kickBackDirection = this.transform.position - kickBackFrom.transform.position;
        GetComponent<Rigidbody2D>().velocity = kickBackDirection.normalized * 10;
        yield return new WaitForSeconds(.1f);
        EnableControls();
    }

    private IEnumerator DoDashCoroutine()
    {
        _dashTrail.gameObject.SetActive(true);
        _speed *= _dashMultiplier;
        yield return new WaitForSeconds(_dashDuration);
        _speed /= _dashMultiplier;
        _dashTrail.gameObject.SetActive(false);

        _dashCoroutine = null;
    }

    private void Update()
    {
        if (_enableControls)
        {
            MoveCharacter();
            if (_lookAtTarget == null)
            {
                LookAtTheMouse();
            }
            else
            {
                LookAtTargetPosition();
            }
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.K))
        {
            Enemy enemy = FindObjectOfType<Enemy>(true);

            StartCoroutine(DoKickBackEffect(enemy.transform));
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_dashCoroutine == null)
            {
                _dashCoroutine = StartCoroutine(DoDashCoroutine());
            }
        }

#endif
    }
}