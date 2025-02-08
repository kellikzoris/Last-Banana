using Polyperfect.Common;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _enemyHealth = 100;
    [SerializeField] private TMP_Text _enemyHealthTextField;
    [SerializeField] private Image _enemyHealthLeft;

    [SerializeField] private Animator _enemyAnimator;
    private bool _canGetHit = true;

    [SerializeField] private bool _isThisWelcomeScene;

    private bool _isEnemyDead = false;
    [SerializeField] private int _enemyGotDamageAfterEachHit;

    private void Start()
    {
        _enemyHealthLeft.fillAmount = (float)_enemyHealth / 100;
        UpdateEnemyHealthText();
    }

    public void CanGetHit(bool _bl)
    {
        _canGetHit = _bl;
    }

    public void DoAfterEnemyDead()
    {
        _enemyAnimator.SetBool("isDead", true);
        FindObjectOfType<SoundManager>().PlayWinSound();
        if (SceneManager.GetActiveScene().name == "LastBanana_TigerFight")
        {
            FindObjectOfType<TrackGorillaLocation>(true).ResetDarkenEnvironment();
        }

        GetComponent<EnemyWeapon>().ResetSpawnTornadoesWithDelayCO();
        GetComponent<EnemyLoop>().StopEnemyMovementCycle();
        GetComponent<EnemyLoop>().enabled = false;
        _isEnemyDead = true;
        if (SceneManager.GetActiveScene().name == "LastBanana_TigerFight")
        {
            PlayerPrefs.SetInt("TigerFightCompleted", 1);
        }
        if (SceneManager.GetActiveScene().name == "LastBanana_BullFight")
        {
            _enemyAnimator.SetBool("chargeAttackRun", false);
            _enemyAnimator.SetBool("360Attack", false);

            PlayerPrefs.SetInt("BullFightCompleted", 1);
        }
        FindObjectOfType<LevelEndLogic>().CallLevelEndCondition(true);
    }

    public void DoEnemyGotHit()
    {
        if (!_canGetHit)
        {
            return;
        }

        _enemyAnimator.SetTrigger("gotDamage");

        Debug.Log("Enemy Got Hit");
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        _enemyHealth -= _enemyGotDamageAfterEachHit;
        if (_enemyHealth <= 0)
        {
            Debug.Log("Enemy already eliminated");
            _enemyHealthLeft.fillAmount = (float)_enemyHealth / 100;
            DoAfterEnemyDead();
            return;
        }
        UpdateEnemyHealthText();
        _enemyHealthLeft.fillAmount = (float)_enemyHealth / 100;

        if (_isThisWelcomeScene)
        {
            FindObjectOfType<TutorialManager>(true).DoApproachMonkeyImage();
            if (_enemyHealth <= 30)
            {
                _enemyHealth = 30;
            }
        }
    }

    public int GetEnemyHealth()
    {
        return _enemyHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isEnemyDead)
        {
            return;
        }

        if (collision.transform.tag.Contains("Player"))
        {
            collision.transform.GetComponent<Player>().DoPlayerGotHit(this.transform);
        }
    }

    private void UpdateEnemyHealthText()
    {
        _enemyHealthTextField.text = $"Enemy Health: {_enemyHealth}";
    }
}