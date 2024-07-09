using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class DamageCircle : MonoBehaviour
{
    private static DamageCircle instance;

    [SerializeField] private Transform targetCircleTransform;

    private Transform circleTransform;
    private Transform topTransform;
    private Transform bottomTransform;
    private Transform leftTransform;
    private Transform rightTransform;

    public float circleShrinkSpeed = 20f;
    public float roomSize = 233f;
    public float shrinkTimeBetween = 30f;
    public float startAfter = 30f;
    private float shrinkTimer;
    private bool isStarted = false;

    private Vector3 circleSize;
    private Vector3 circlePosition;

    private Vector3 targetCircleSize;
    private Vector3 targetCirclePosition;

    private void Awake()
    {
        instance = this;
        circleTransform = transform.Find("circle");
        topTransform = transform.Find("top");
        bottomTransform = transform.Find("bottom");
        leftTransform = transform.Find("left");
        rightTransform = transform.Find("right");
        GameObject backGround = GameObject.FindGameObjectWithTag("BackGroundMapTag");
        Vector3 targetPosition = new Vector3(backGround.transform.position.x, backGround.transform.position.y);
        SetCircleSize(targetPosition, new Vector3(roomSize, roomSize));
        SetTargetCircle(targetPosition, new Vector3(roomSize, roomSize), 0f);
        StartAfter(startAfter);
    }
    
    private void Update()
    {
        shrinkTimer -= Time.deltaTime;

        if (shrinkTimer < 0 && isStarted)
        {
            Vector3 sizeChangeVector = (targetCircleSize - circleSize).normalized;
            Vector3 newCircleSize = circleSize + sizeChangeVector * Time.deltaTime * circleShrinkSpeed;

            Vector3 circleMoveDir = (targetCirclePosition - circlePosition).normalized;
            Vector3 newCirclePosition = circlePosition + circleMoveDir * Time.deltaTime * circleShrinkSpeed;

            SetCircleSize(newCirclePosition, newCircleSize);

            float distanceTestAmount = .1f;
            if (Vector3.Distance(newCircleSize, targetCircleSize) < distanceTestAmount && Vector3.Distance(newCirclePosition, targetCirclePosition) < distanceTestAmount)
            {
                GenerateTargetCircle();
            }
        }
    }
    private void StartAfter(float duration)
    {
        StartCoroutine(StartShrinking(duration));
    }

    private IEnumerator StartShrinking(float duration)
    {
        yield return new WaitForSeconds(duration);
        isStarted = true;
    }
    private void GenerateTargetCircle()
    {
        float shrinkSizeAmount = Random.Range(3f, 12f);
        Vector3 generatedTargetCircleSize = circleSize - new Vector3(shrinkSizeAmount, shrinkSizeAmount) * 2f;

        // Set a minimum size
        if (generatedTargetCircleSize.x < 20f) generatedTargetCircleSize = Vector3.one * 20f;

        // Ensure the new position is within the current circle bounds
        float maxOffsetX = (circleSize.x - generatedTargetCircleSize.x) / 2;
        float maxOffsetY = (circleSize.y - generatedTargetCircleSize.y) / 2;

        Vector3 generatedTargetCirclePosition = circlePosition +
            new Vector3(Random.Range(-maxOffsetX, maxOffsetX), Random.Range(-maxOffsetY, maxOffsetY));

        float shrinkTime = shrinkTimeBetween;

        SetTargetCircle(generatedTargetCirclePosition, generatedTargetCircleSize, shrinkTime);
    }

    private void SetCircleSize(Vector3 position, Vector3 size)
    {
        circlePosition = position;
        circleSize = size;

        transform.position = position;

        circleTransform.localScale = size;

        topTransform.localScale = new Vector3(1000, 1000);
        topTransform.localPosition = new Vector3(0, topTransform.localScale.y * .5f + size.y * .5f);

        bottomTransform.localScale = new Vector3(1000, 1000);
        bottomTransform.localPosition = new Vector3(0, -topTransform.localScale.y * .5f - size.y * .5f);

        leftTransform.localScale = new Vector3(1000, size.y);
        leftTransform.localPosition = new Vector3(-leftTransform.localScale.x * .5f - size.x * .5f, 0f);

        rightTransform.localScale = new Vector3(1000, size.y);
        rightTransform.localPosition = new Vector3(+leftTransform.localScale.x * .5f + size.x * .5f, 0f);
    }

    private void SetTargetCircle(Vector3 position, Vector3 size, float shrinkTimer)
    {
        this.shrinkTimer = shrinkTimer;

        targetCircleTransform.position = position;
        targetCircleTransform.localScale = size;

        targetCirclePosition = position;
        targetCircleSize = size;
    }

    private bool IsOutsideCircle(Vector3 position)
    {
        return Vector3.Distance(position, circlePosition) > circleSize.x * .5f;
    }

    public static bool IsOutsideCircle_Static(Vector3 position)
    {
        return instance.IsOutsideCircle(position);
    }
}
