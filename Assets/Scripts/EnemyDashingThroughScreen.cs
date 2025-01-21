using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDashingThroughScreen : MonoBehaviour
{
    [SerializeField] private List<Transform> _moveBetweenTransforms;
    [SerializeField] private int currentIndex = 0;
    bool isMoving = false;
    [SerializeField] float movementSpeed = 30;
    Vector2 targetPosition;
    Vector3 startPosition;
    bool dashThroughScreenIsFinished = false;

    [SerializeField] Transform _enemySpriteTransform;

    public void DashThroughScreen()
    {
        dashThroughScreenIsFinished = false;
        startPosition = this.transform.position;
        currentIndex = 0;
        StartCoroutine(DashThroughScreenCO());
    }

    public bool GetDashThroughScreenIsFinished()
    {
        return dashThroughScreenIsFinished;
    }

    IEnumerator DashThroughScreenCO()
    {
        DisableThisObject();
        yield return new WaitForSeconds(2);
        this.transform.position = _moveBetweenTransforms[currentIndex].transform.position;
        EnableThisObject();
        currentIndex++;
        targetPosition = _moveBetweenTransforms[currentIndex].transform.position;
        isMoving = true;
        yield return new WaitUntil(() => Vector2.Distance(this.transform.position, targetPosition) < .5f);
        isMoving = false;

        currentIndex++;
        Debug.Log($"Check Dash Through Screen Values {currentIndex} {_moveBetweenTransforms.Count} {currentIndex >= _moveBetweenTransforms.Count}");
        if (currentIndex >= _moveBetweenTransforms.Count)
        {
            this.transform.position = startPosition;
            dashThroughScreenIsFinished = true;
            yield break;
        }


        StartCoroutine(DashThroughScreenCO());
    }

    private void DisableThisObject()
    {
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }

    private void EnableThisObject()
    {
        GetComponentInChildren<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    private void LookAtTargetVector3()
    {
        //transform.up = (Vector2)targetPosition - new Vector2(transform.position.x, transform.position.y);


        Vector2 targetTransform = targetPosition;

        if (targetTransform.x - transform.position.x > 0)
        {
            _enemySpriteTransform.transform.localScale = Vector3.one;
        }
        else
        {
            _enemySpriteTransform.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void Update()
    {

        if (isMoving)
        {
            LookAtTargetVector3();
            float step = movementSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.V))
        {
            DashThroughScreen();
        }
#endif
    }
}