using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _enemyHealth = 100;
    [SerializeField] private TMP_Text _enemyHealthTextField;
    [SerializeField] private Image _enemyHealthLeft;
    private Coroutine _returnBackToOriginalColorCO;

    public void DoEnemyGotHit()
    {
        Debug.Log("Enemy Got Hit");
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        _enemyHealth -= 10;
        if (_enemyHealth <= 0)
        {
            Debug.Log("Enemy already eliminated");
        }
        _enemyHealthTextField.text = $"Enemy Health: {_enemyHealth}";
        _enemyHealthLeft.fillAmount = (float)_enemyHealth / 100;

        Debug.Log($"_enemyHealthLeft.fillAmount: {_enemyHealth / 100}");
        Debug.Log($"_enemyHealthLeft.fillAmount: {_enemyHealthLeft.fillAmount}");

        GetComponent<SpriteRenderer>().color = Color.white;

        if (_returnBackToOriginalColorCO == null)
        {
            _returnBackToOriginalColorCO = StartCoroutine(ReturnBackToOriginalColor());
        }
        else
        {
            StopCoroutine(_returnBackToOriginalColorCO);
            _returnBackToOriginalColorCO = StartCoroutine(ReturnBackToOriginalColor());
        }
    }

    public int GetEnemyHealth()
    {
        return _enemyHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Contains("Player"))
        {
            collision.transform.GetComponent<Player>().DoPlayerGotHit(this.transform);
        }
    }

    private IEnumerator ReturnBackToOriginalColor()
    {
        yield return new WaitForSeconds(.1f);
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}