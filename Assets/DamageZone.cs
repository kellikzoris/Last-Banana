using System.Collections;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    private void Start()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Contains("Player"))
        {
            if (this.gameObject.tag == "GorillaTrap")
            {
                collision.transform.GetComponent<Player>().DoPlayerGotHitWithDelay(this.transform, .1f);
                GetComponent<Animator>().SetTrigger("TriggerGorillaTrap");
            }
            else
            {
                collision.transform.GetComponent<Player>().DoPlayerGotHit(this.transform);
            }
        }
    }

    public void DisableGameObjectFromBanana()
    {
        GetComponent<Animator>().SetTrigger("TriggerGorillaTrap");
    }

    public void CallDisableGameObjectWithDelay(bool calledFromBanana)
    {
        StartCoroutine(DisableGameObjectWithDelay(7));
    }

    private IEnumerator DisableGameObjectWithDelay(float delayAmount)
    {
        yield return new WaitForSeconds(delayAmount);
        DisableThisGameObject(0);
    }

    public void DisableThisGameObject(int i)
    {
        this.gameObject.SetActive(false);
    }
}