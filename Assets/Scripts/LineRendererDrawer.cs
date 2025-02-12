using System.Collections;
using UnityEngine;

public class LineRendererDrawer : MonoBehaviour
{
    private RaycastHit2D hitInfo;

    public LineRenderer lineRenderer;

    public Banana banana;
    public Player player;
    public Weapon weapon;

    public Material lineRendererMaterial;

    private bool isLineRendererDrawn = false;
    public float multiplierValue;
    public float lineRendererIncreaseSpeed;

    private float trajectoryTimeOnTarget = 0;

    private bool lockedOnTarget;
    public float offsetDistanceWhenLockedOnTarget = 20;

    [SerializeField] private Transform _debuggerPoint1;
    [SerializeField] private Transform _debuggerPoint2;
    private bool _disableAttacks = false;

    [SerializeField] private bool _isThisWelcomeScene;

    public void DisableAttacks()
    {
        _disableAttacks = true;
    }

    public void EnableAttacks()
    {
        _disableAttacks = false;
    }

    private void DrawALineTowardsTarget()
    {
        lineRenderer.material.color = Color.white;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (!isLineRendererDrawn)
        {
            isLineRendererDrawn = true;
            multiplierValue = 25f;
        }

        multiplierValue += Time.deltaTime * lineRendererIncreaseSpeed;

        Vector2 directionVector = ((mousePos - (Vector2)player.transform.position).normalized);
        Vector2 offsetVector = directionVector * multiplierValue;
        Vector2 startPos = (Vector2)player.transform.position + (directionVector * 8);

        Vector2 tipOfTheLineRender = (startPos + offsetVector);

        hitInfo = Physics2D.Linecast(startPos, tipOfTheLineRender, LayerMask.GetMask("Enemy"));

        if (hitInfo)
        {
            tipOfTheLineRender = hitInfo.point;
            if (hitInfo.collider.tag == "enemy")
            {
                trajectoryTimeOnTarget += Time.deltaTime;
                lineRenderer.material.color = Color.Lerp(Color.white, Color.red, trajectoryTimeOnTarget / 2);
                if (trajectoryTimeOnTarget > 1f)
                {
                    ToggleLockedOnTarget();
                    Debug.Log("LockedOnTarget");
                    StartCoroutine(CooldownLockedOnTarget());
                }
            }
            else if (hitInfo.collider.tag.Contains("banana"))
            {
            }
            else
            {
                trajectoryTimeOnTarget = 0;
                lineRenderer.material.color = Color.white;
            }
        }

#if UNITY_EDITOR
        _debuggerPoint1.transform.position = startPos;
        _debuggerPoint2.transform.position = tipOfTheLineRender;
        //Debug.Log()

#endif

        Vector3[] positions = new Vector3[] { startPos, tipOfTheLineRender };
        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(positions);
    }

    private IEnumerator CooldownLockedOnTarget()
    {
        yield return new WaitForSeconds(3);
        ToggleLockedOnTarget();
    }

    private void ToggleLockedOnTarget()
    {
        if (_isThisWelcomeScene)
        {
            FindObjectOfType<TutorialManager>().SetLockedOnTargetColor();
        }

        lockedOnTarget = !lockedOnTarget;
        if (lockedOnTarget == true)
        {
            FindObjectOfType<LockedOnTarget>(true).EnableTheTextAndAnimation();
            Transform enemyTransform = FindObjectOfType<Enemy>().transform;
            player.SetLookAtTarget(enemyTransform);
        }
        else
        {
            FindObjectOfType<LockedOnTarget>(true).DisableTheTextAndAnimation();
            player.SetLookAtTarget(null);
            lineRenderer.positionCount = 0;
        }
    }

    private void Update()
    {
        if (lockedOnTarget)
        {
            Vector3[] positions = new Vector3[] { player.transform.position, FindObjectOfType<Enemy>().transform.position };
            lineRenderer.positionCount = 2;
            lineRenderer.SetPositions(positions);
        }

        if (_disableAttacks)
        {
            return;
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleLockedOnTarget();
        }
#endif
        if (Input.GetMouseButtonDown(1))
        {
            lineRenderer.positionCount = 0;
        }

        if (Input.GetMouseButton(1))
        {
            if (!lockedOnTarget)
            {
                DrawALineTowardsTarget();
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (_isThisWelcomeScene)
            {
                FindObjectOfType<TutorialManager>().SetRightClickAttackColor();
            }

            float lengthOfLineRenderer = Vector2.Distance(lineRenderer.GetPosition(1), lineRenderer.GetPosition(0));
            lineRenderer.positionCount = 0;
            isLineRendererDrawn = false;
            multiplierValue = -30;
            trajectoryTimeOnTarget = 0;

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 startPos = (Vector2)player.transform.position + ((mousePos - (Vector2)player.transform.position).normalized * 10);

            Banana newBanana = player.GetComponent<Weapon>().GetAvailableBananaFromTheBagAndReduceAmountInBag();
            if (newBanana == null)
            {
                return;
            }
            newBanana.gameObject.SetActive(true);

            float distanceToEnemy = Vector2.Distance(FindObjectOfType<Enemy>(true).transform.position, player.transform.position);

            float throwPercentage = Mathf.Clamp(lengthOfLineRenderer / distanceToEnemy, .5f, 1f);

            if (!lockedOnTarget)
            {
                newBanana.ThrowBananaDirectional(startPos, null, throwPercentage);
            }
            else
            {
                Enemy enemy = FindObjectOfType<Enemy>(true);
                newBanana.ThrowBananaDirectional(startPos, enemy.transform, throwPercentage);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            FindObjectOfType<PlayerSpriteAnimatorListener>(true).DoMeleeAttack();
        }

        if (Input.GetMouseButtonUp(0))
        {
        }
    }
}