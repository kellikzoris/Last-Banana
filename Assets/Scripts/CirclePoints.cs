using UnityEngine;

public class CirclePoints : MonoBehaviour
{
    public int pointCount = 20;
    public float radius = 1f;

    public Transform center; // Add this line
    public Transform startPoint; // Add this line

    private Vector3[] points;
    private Vector3[] pointsWithHit;

    private RaycastHit2D hitInfo;
    private int closestIndexToHit;

    public LineRenderer lineRenderer;

    public bool clockwise;

    public Banana banana;
    public Player player;
    public Weapon weapon;

    public Material lineRendererMaterial;
    public Texture spriteForCircle;
    public Texture spriteForArc;

    public Vector2 _offsetValue;
    public Vector2 _targetVector;
    public Vector2 _targetVectorDifferenceOffset;
    private bool isLineRendererDrawn = false;
    public float multiplierValue;
    public float lineRendererIncreaseSpeed;

    private float trajectoryTimeOnTarget = 0;

    private bool lockedOnTarget;
    public float offsetDistanceWhenLockedOnTarget = 20;

    private void Start()
    {
        //CalculateCirclePoints();
    }

    private void UpdateLineRendererPositionWithMouseMovement()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = mousePos;
        radius = Vector2.Distance(mousePos, startPoint.transform.position);
    }

    private void DrawALineTowardsTarget()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (!isLineRendererDrawn)
        {
            isLineRendererDrawn = true;
            multiplierValue = -15f;
        }

        Vector2 directionVector = ((mousePos - (Vector2)player.transform.position).normalized) * multiplierValue;
        multiplierValue += Time.deltaTime * lineRendererIncreaseSpeed;
        Vector2 tipOfTheLineRender = (mousePos + directionVector);

        Vector2 startPos = (Vector2)player.transform.position + ((mousePos - (Vector2)player.transform.position).normalized * 5);

        hitInfo = Physics2D.Linecast(startPos, tipOfTheLineRender);

        if (hitInfo)
        {
            tipOfTheLineRender = hitInfo.point;
            if (hitInfo.collider.tag == "enemy")
            {
                Debug.Log("Touched The Enemy");
                trajectoryTimeOnTarget += Time.deltaTime;
                lineRenderer.material.color = Color.Lerp(Color.white, Color.red, trajectoryTimeOnTarget / 2);
                if (trajectoryTimeOnTarget > 2)
                {
                    Debug.Log("LockedOnTarget");
                }
            }
            else
            {
                Debug.Log("Touched Something Else");

                trajectoryTimeOnTarget = 0;
                lineRenderer.material.color = Color.white;
            }
        }
        //lineRenderer.material.color = Color.white;

        Vector3[] positions = new Vector3[] { startPos, tipOfTheLineRender };
        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(positions);
    }

    private void Update()
    {
        if (lockedOnTarget)
        {
            //Vector3[] pointsArrayToDraw = CalculateCirclePointsFixedOnEnemy();
            //lineRenderer.positionCount = pointsArrayToDraw.Length;
            //lineRenderer.SetPositions(pointsArrayToDraw);
            Vector3[] positions = new Vector3[] { player.transform.position, FindObjectOfType<Enemy>().transform.position };
            lineRenderer.positionCount = 2;
            lineRenderer.SetPositions(positions);
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Q))
        {
            lockedOnTarget = !lockedOnTarget;
            if (lockedOnTarget == true)
            {
                Transform enemyTransform = FindObjectOfType<Enemy>().transform;
                player.SetLookAtTarget(enemyTransform);
            }
            else
            {
                player.SetLookAtTarget(null);
            }
        }
#endif
        if (Input.GetMouseButton(1))
        {
            DrawALineTowardsTarget();

            //commented out in 120124 in order to try another shooting type of projectiles.
            //UpdateLineRendererPositionWithMouseMovement();
            //clockwise = false;
            //CalculateTheTrajectory();
        }

        if (Input.GetMouseButtonUp(1))
        {
            lineRenderer.positionCount = 0;
            isLineRendererDrawn = false;
            multiplierValue = -30;
            trajectoryTimeOnTarget = 0;
            lineRenderer.material.color = Color.white;

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 startPos = (Vector2)player.transform.position + ((mousePos - (Vector2)player.transform.position).normalized * 10);

            Banana newBanana = Instantiate(banana, startPos, Quaternion.identity, null);
            newBanana.gameObject.SetActive(true);

            //Debug.Break();

            newBanana.ThrowBananaDirectional(startPos);
            //commented out in 120124 in order to try another shooting type of projectiles.

            //lineRenderer.positionCount = 0;
            ////Banana newBanana = Instantiate(banana, player.transform.position, Quaternion.identity, null);
            //Banana newBanana = weapon.GetAvailableBananaFromTheBagAndReduceAmountInBag();
            //if (newBanana != null)
            //{
            //    newBanana.ThrowBanana(false);
            //}
        }

        if (Input.GetMouseButton(0))
        {
            //FindObjectOfType<MeleeAttack>(true).DoMeleeAttack();

            //UpdateLineRendererPositionWithMouseMovement();
            //clockwise = true;
            //CalculateTheTrajectory();
        }

        if (Input.GetMouseButtonUp(0))
        {
            //lineRenderer.positionCount = 0;
            ////Banana newBanana = Instantiate(banana, player.transform.position, Quaternion.identity, null);
            //Banana newBanana = weapon.GetAvailableBananaFromTheBagAndReduceAmountInBag();
            //if (newBanana != null)
            //{
            //    newBanana.ThrowBanana(true);
            //}
        }
    }
}

//private void UpdateLineRendererMaterialTiling(int dranwPointCount)
//{
//    if (dranwPointCount != pointCount + 1)
//    {
//        lineRendererMaterial.mainTexture = spriteForArc;

//        //Debug.Log("It draws an arc;");
//    }
//    else
//    {
//        //Debug.Log("It draws a whole circle");

//        lineRendererMaterial.mainTexture = spriteForCircle;
//        lineRendererMaterial.mainTextureScale = new Vector2(1f * radius, 1);
//    }

//    //TODO: Fix this with the arc style trajectories.
//}

//private void CalculateTheTrajectory()
//{
//    if (clockwise)
//    {
//        Vector3[] pointsArrayToDraw = CalculateCirclePoints();
//        lineRenderer.positionCount = pointsArrayToDraw.Length;
//        UpdateLineRendererMaterialTiling(lineRenderer.positionCount);
//        lineRenderer.SetPositions(pointsArrayToDraw);
//    }
//    else
//    {
//        Vector3[] pointsArrayToDraw = CalculateCirclePointsOtherWay();
//        lineRenderer.positionCount = pointsArrayToDraw.Length;
//        UpdateLineRendererMaterialTiling(lineRenderer.positionCount);
//        lineRenderer.SetPositions(pointsArrayToDraw);
//    }
//}

//private Vector3[] CalculateCirclePointsFixedOnEnemy()
//{
//    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//    this.transform.position = mousePos;
//    //radius = Vector2.Distance(player.transform.position, FindObjectOfType<Enemy>(true).transform.position) / 2;

//    Vector2 middlePoint = (FindObjectOfType<Enemy>(true).transform.position + player.transform.position) / 2;
//    // change the center position with an offset value with the movement of mouse.
//    // in order to do this you need to update the radius accordingly.

//    Vector2 direction = (player.transform.position - FindObjectOfType<Enemy>(true).transform.position).normalized;
//    Vector2 perpendicularDirection = Vector2.Perpendicular(direction).normalized;

//    //offsetDistanceWhenLockedOnTarget = -20;
//    Vector2 newCenterPoint = middlePoint + perpendicularDirection * offsetDistanceWhenLockedOnTarget;

//    radius = Vector2.Distance(player.transform.position, newCenterPoint);
//    Debug.Log($"middlePoint {middlePoint} and newCenterPoint {newCenterPoint} and newRadius {radius}");

//    closestIndexToHit = 0;

//    int loopPointCount = pointCount + 1;
//    points = new Vector3[loopPointCount];

//    for (int i = 0; i < loopPointCount; i++)
//    {
//        Vector3 localPoint;
//        if (offsetDistanceWhenLockedOnTarget < 0)
//        {
//            float angle = (startPoint.eulerAngles.z - 90) * Mathf.Deg2Rad + i * Mathf.PI * 2 / pointCount;
//            localPoint = new Vector3(
//            Mathf.Cos(angle) * radius,
//            Mathf.Sin(angle) * radius,
//            0f
//        );
//        }
//        else
//        {
//            float angle = (startPoint.eulerAngles.z - 90) * Mathf.Deg2Rad - i * Mathf.PI * 2 / pointCount;
//            localPoint = new Vector3(
//            Mathf.Cos(angle) * radius,
//            Mathf.Sin(angle) * radius,
//            0f
//        );
//        }

//        points[i] = (Vector3)newCenterPoint + center.rotation * localPoint; // Use the center transform and rotation
//    }

//    Vector2 hitPosition = startPoint.transform.position;
//    for (int i = 0; i < pointCount; i++)
//    {
//        if (i == 0)
//        {
//            hitInfo = Physics2D.Linecast(startPoint.transform.position, points[0]);
//            if (hitInfo)
//            {
//                if (hitInfo.collider.tag.Contains("enemy"))
//                {
//                    Debug.Log("Touched The Enemy");
//                    //hitPosition = hitInfo.transform.position;
//                    hitPosition = hitInfo.point;

//                    break;
//                }
//            }
//        }
//        else
//        {
//            hitInfo = Physics2D.Linecast(points[i - 1], points[i]);
//            if (hitInfo)
//            {
//                if (hitInfo.collider.tag.Contains("enemy"))
//                {
//                    Debug.Log("Touched The Enemy");
//                    //hitPosition = hitInfo.transform.position;
//                    hitPosition = hitInfo.point;

//                    break;
//                }
//            }
//        }
//    }

//    if (hitPosition != (Vector2)startPoint.transform.position)
//    {
//        closestIndexToHit = 0;
//        float minDistance = Mathf.Infinity;
//        for (int i = 0; i < points.Length; i++)
//        {
//            float distanceToHit = Vector2.Distance(points[i], hitInfo.transform.position);
//            if (distanceToHit < minDistance)
//            {
//                minDistance = distanceToHit;
//                closestIndexToHit = i;
//            }
//        }

//        pointsWithHit = new Vector3[closestIndexToHit + 1];
//        for (int j = 0; j < closestIndexToHit; j++)
//        {
//            pointsWithHit[j] = points[j];
//        }

//        pointsWithHit[closestIndexToHit] = hitPosition;

//        return pointsWithHit;
//    }

//    return points;
//}

//private Vector3[] CalculateCirclePoints()
//{
//    closestIndexToHit = 0;

//    int loopPointCount = pointCount + 1;
//    points = new Vector3[loopPointCount];

//    for (int i = 0; i < loopPointCount; i++)
//    {
//        float angle = (startPoint.eulerAngles.z - 90) * Mathf.Deg2Rad + i * Mathf.PI * 2 / pointCount;
//        Vector3 localPoint = new Vector3(
//            Mathf.Cos(angle) * radius,
//            Mathf.Sin(angle) * radius,
//            0f
//        );
//        points[i] = center.position + center.rotation * localPoint; // Use the center transform and rotation
//    }

//    Vector2 hitPosition = startPoint.transform.position;
//    for (int i = 0; i < pointCount; i++)
//    {
//        if (i == 0)
//        {
//            hitInfo = Physics2D.Linecast(startPoint.transform.position, points[0]);
//            if (hitInfo)
//            {
//                if (hitInfo.collider.tag.Contains("enemy"))
//                {
//                    Debug.Log("Touched The Enemy");
//                    //hitPosition = hitInfo.transform.position;
//                    hitPosition = hitInfo.point;

//                    break;
//                }
//            }
//        }
//        else
//        {
//            hitInfo = Physics2D.Linecast(points[i - 1], points[i]);
//            if (hitInfo)
//            {
//                if (hitInfo.collider.tag.Contains("enemy"))
//                {
//                    Debug.Log("Touched The Enemy");
//                    //hitPosition = hitInfo.transform.position;
//                    hitPosition = hitInfo.point;

//                    break;
//                }
//            }
//        }
//    }

//    if (hitPosition != (Vector2)startPoint.transform.position)
//    {
//        closestIndexToHit = 0;
//        float minDistance = Mathf.Infinity;
//        for (int i = 0; i < points.Length; i++)
//        {
//            float distanceToHit = Vector2.Distance(points[i], hitInfo.transform.position);
//            if (distanceToHit < minDistance)
//            {
//                minDistance = distanceToHit;
//                closestIndexToHit = i;
//            }
//        }

//        pointsWithHit = new Vector3[closestIndexToHit + 1];
//        for (int j = 0; j < closestIndexToHit; j++)
//        {
//            pointsWithHit[j] = points[j];
//        }

//        pointsWithHit[closestIndexToHit] = hitPosition;

//        return pointsWithHit;
//    }

//    return points;
//}

//private Vector3[] CalculateCirclePointsOtherWay()
//{
//    closestIndexToHit = 0;

//    int loopPointCount = pointCount + 1;
//    points = new Vector3[loopPointCount];

//    for (int i = 0; i < loopPointCount; i++)
//    {
//        float angle = (startPoint.eulerAngles.z - 90) * Mathf.Deg2Rad - i * Mathf.PI * 2 / pointCount;
//        Vector3 localPoint = new Vector3(
//            Mathf.Cos(angle) * radius,
//            Mathf.Sin(angle) * radius,
//            0f
//        );
//        points[i] = center.position + center.rotation * localPoint; // Use the center transform and rotation
//    }

//    Vector2 hitPosition = startPoint.transform.position;
//    for (int i = 0; i < pointCount; i++)
//    {
//        if (i == 0)
//        {
//            hitInfo = Physics2D.Linecast(startPoint.transform.position, points[0]);
//            if (hitInfo)
//            {
//                if (hitInfo.collider.tag.Contains("enemy"))
//                {
//                    //Debug.Log("Touched The Enemy");
//                    //hitPosition = hitInfo.transform.position;
//                    hitPosition = hitInfo.point;

//                    break;
//                }
//            }
//        }
//        else
//        {
//            hitInfo = Physics2D.Linecast(points[i - 1], points[i]);
//            if (hitInfo)
//            {
//                if (hitInfo.collider.tag.Contains("enemy"))
//                {
//                    //Debug.Log("Touched The Enemy");
//                    //hitPosition = hitInfo.transform.position;
//                    hitPosition = hitInfo.point;

//                    break;
//                }
//            }
//        }
//    }

//    if (hitPosition != (Vector2)startPoint.transform.position)
//    {
//        closestIndexToHit = 0;
//        float minDistance = Mathf.Infinity;
//        for (int i = 0; i < points.Length; i++)
//        {
//            float distanceToHit = Vector2.Distance(points[i], hitInfo.transform.position);
//            if (distanceToHit < minDistance)
//            {
//                minDistance = distanceToHit;
//                closestIndexToHit = i;
//            }
//        }

//        pointsWithHit = new Vector3[closestIndexToHit + 1];
//        for (int j = 0; j < closestIndexToHit; j++)
//        {
//            pointsWithHit[j] = points[j];
//        }

//        pointsWithHit[closestIndexToHit] = hitPosition;

//        int index = 0;
//        Vector2 playerHitPosition;
//        for (int i = 1; i < pointsWithHit.Length; i++)
//        {
//            if (i == 0)
//            {
//                hitInfo = Physics2D.Linecast(pointsWithHit[i - 1], pointsWithHit[i]);
//                if (hitInfo)
//                {
//                    if (hitInfo.collider.tag.Contains("Player"))
//                    {
//                        playerHitPosition = hitInfo.point;
//                        index = i;
//                        pointsWithHit[index] = playerHitPosition;
//                        break;
//                    }
//                }
//            }
//        }

//        Vector3[] pointsWithHitStartingFromPlayer = new Vector3[pointsWithHit.Length - index];

//        for (int i = index; i < pointsWithHit.Length; i++)
//        {
//            pointsWithHitStartingFromPlayer[i - index] = pointsWithHit[i];
//        }

//        return pointsWithHitStartingFromPlayer;

//        return pointsWithHit;
//    }

//    return points;
//}

//private void OnDrawGizmosSelected()
//{
//    if (points == null)
//        return;

//    Gizmos.color = Color.red;

//    if (closestIndexToHit == 0)
//    {
//        for (int i = 0; i < points.Length; i++)
//        {
//            Gizmos.DrawSphere(points[i], 0.1f);
//        }
//    }
//    else
//    {
//        for (int i = 0; i < pointsWithHit.Length; i++)
//        {
//            Gizmos.DrawSphere(points[i], 0.1f);
//        }
//    }

//    // Draw start point as white sphere
//    Gizmos.color = Color.white;
//    Gizmos.DrawSphere(startPoint.position, 0.2f);

//    // Draw center (middle) point as green sphere
//    Gizmos.color = Color.green;
//    Gizmos.DrawSphere(center.position, 0.2f);
//}