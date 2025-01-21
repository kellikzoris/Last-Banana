using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Banana : MonoBehaviour
{
    public float _radius = 1f;
    public Player player;
    public float _throwSpeed;
    public float _rotateAroundSpeed;

    private Vector3 _centerPosition;
    private Vector3 _startPositionOfBanana;
    private bool _isBananaOnTheFly;
    public float _throwAngle;

    public bool _isTravelingClockwise;
    public bool _isBananaOnGround = true;
    public float _lifeTimeOFTheBanana = 1;

    public enum BananaType
    {
        yellow,
        green,
        red
    }

    public BananaType _bananaType;

    public BananaType GetBananaType()
    {
        return _bananaType;
    }

    public bool GetBananaOnGround()
    {
        return _isBananaOnGround;
    }

    private void Start()
    {
        _isBananaOnGround = true;
    }

    public void ThrowBanana(bool clockWise)
    {
        _isBananaOnGround = false;

        this.transform.position = player.transform.position;
        _startPositionOfBanana = this.transform.position;
        _isBananaOnTheFly = true;
        StartCoroutine(DisableWithDelay());
        _throwAngle = 0;
        _centerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _radius = Vector2.Distance(_centerPosition, player.transform.position);

        _isTravelingClockwise = clockWise;
    }

    public void ThrowBananaDirectional(Vector2 startThrowPosition, Transform targetTransform = null, float throwPower = 1)
    {
        _isBananaOnGround = false;

        StartCoroutine(DisableWithDelay());

        this.transform.position = startThrowPosition;
        if (targetTransform == null)
        {
            _centerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            _centerPosition = targetTransform.position;
        }
        GetComponent<Rigidbody2D>().velocity = (_centerPosition - this.transform.position).normalized * _throwSpeed * throwPower;
        GetComponent<Rigidbody2D>().angularVelocity = 50 * 30;
    }

    IEnumerator DisableWithDelay()
    {
        yield return new WaitForSeconds(_lifeTimeOFTheBanana);
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ThrowBanana(true);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ThrowBanana(false);
        }

        float throwSpeed = _throwSpeed * 3000 * Time.deltaTime / _radius;

        if (_isBananaOnTheFly)
        {
            if (_isTravelingClockwise)
            {
                this.transform.Rotate(0, 0, _rotateAroundSpeed * 1000 * Time.deltaTime);
                transform.RotateAround(_centerPosition, Vector3.forward, throwSpeed);
            }
            else
            {
                this.transform.Rotate(0, 0, -_rotateAroundSpeed * 1000 * Time.deltaTime);
                transform.RotateAround(_centerPosition, Vector3.forward, -throwSpeed);
            }

            _throwAngle += throwSpeed;
        }
        if (_isBananaOnTheFly && _throwAngle >= 360)
        {
            _isBananaOnTheFly = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Collision With {collision.gameObject.name}");
        if (!_isBananaOnGround)
        {
            if (collision.transform.CompareTag("enemy"))
            {
                collision.transform.GetComponent<Enemy>().DoEnemyGotHit();
            }
            if (collision.transform.CompareTag("enemyProjectile"))
            {
                collision.transform.GetComponent<EnemyProjectile>().DisableEnemyProjectile();
            }
            if (collision.transform.CompareTag("enemyShield"))
            {
                collision.transform.GetComponent<EnemyShield>().DisableEnemyProjectile();
            }
            if (collision.transform.CompareTag("minion"))
            {
                collision.transform.GetComponent<Minion>().DisableThis();
            }

            this.gameObject.SetActive(false);
        }

        //commented out in 13.01.2025 in order to implement directional hit.
        //if (collision.transform.CompareTag("enemy"))
        //{
        //    if (_isBananaOnTheFly)
        //    {
        //        collision.transform.GetComponent<Enemy>().DoEnemyGotHit();
        //    }
        //    _isBananaOnTheFly = false;
        //}

        //if (collision.transform.CompareTag("enemyShield"))
        //{
        //    _isBananaOnTheFly = false;
        //}

        //if (collision.transform.CompareTag("enemyProjectile"))
        //{
        //    if (_isBananaOnTheFly)
        //    {
        //        collision.transform.GetComponent<EnemyProjectile>().DisableEnemyProjectile();
        //    }
        //    _isBananaOnTheFly = false;
        //}

        //this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public bool GetIsBananaOnTheFly()
    {
        return _isBananaOnTheFly;
    }
}