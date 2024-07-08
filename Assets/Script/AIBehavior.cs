using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AIBehavior : MonoBehaviour
{
    public float speed;
    public float distanceBetween = 20f;
    public float distanceChasing = 50f;
    public float angle;
    public LayerMask obstacleLayer; // Layer mask to specify which objects are considered obstacles
    private GameObject enemy;
    private GameObject target;
    private float distanceToTarget;
    private bool stunned;
    public int behavior = 0; //0 : Walking arround //1:Chase player //2: Go get a weapon //3: Go get an item //4: Retreat 
    NavMeshAgent agent;
    private PlayerAIProps master;
    private AIInventory inventoryController;

    private float checkInterval = 3.0f; // Time interval in seconds
    private float walkingStep = 0f; // To count time walking freely
    private float maxStep = 0f; // To count time walking freely
    private float timer;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        //parent = transform.parent.gameObject;
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        master = GetComponent<PlayerAIProps>();
        inventoryController = GetComponent<AIInventory>();
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;
        // Check if the timer has reached the interval
        if (timer >= checkInterval)
        {
            // Call the method to get the nearest instance
            target = GetNearestPlayerInstance();
            //Debug.Log(Physics2D.Linecast((transform.position), (target.transform.position)));
            // Reset the timer
            timer = 0f;
        }
        DecisionMaking();
        switch (behavior)
        {
            case 0:
                WalkingAround();
                break;
            case 1:
                Attacking();
                break;
            case 2:
                GoFindItem(0);
                break;
            case 3:
                GoFindItem(1);
                break;

        }
    }
    void DecisionMaking()
    {

        if (!stunned && target != null)
        {
            Vector3 targetPosition = target.transform.position;
            Vector3 direction = targetPosition - transform.position;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(Vector3.forward * angle) * Quaternion.Euler(0f, 0f, 180f);
            var distanceToNearestEnemy = Vector2.Distance(transform.position, targetPosition);

            behavior = 0;
            if (!inventoryController.IsFull())
            {
                behavior = 3;
            }
            if (master.gunNumber > 0 && distanceToNearestEnemy < distanceChasing)
            {
                behavior = 1;
            }
            if (!inventoryController.IsFull() && master.gunNumber == 0)
            {
                behavior = 2;
            }
        }
    }
    void Attacking()
    {
        if (target != null)
        {
            //Approach player if close enough
            Vector3 targetPosition = target.transform.position;
            distanceToTarget = Vector2.Distance(transform.position, targetPosition);
            var r = new System.Random();
            if (r.Next(60) <= 1)
            {
                int weaponSlot = inventoryController.FindWeaponSlot();
                if (weaponSlot != -1)
                {
                    inventoryController.SwitchTo(weaponSlot + 1);
                }
            }
            if (r.Next(150) <= 1 && distanceToTarget < distanceBetween * 2)
            {
                int grenadeSlot = inventoryController.FindGrenadeSlot();
                if (grenadeSlot != -1)
                {
                    inventoryController.SwitchTo(grenadeSlot + 1);
                }
            }
            if (master.hp < master.hpMax && r.Next(300 - Math.Min(290, (int)(master.hpMax - master.hp) * 30)) <= 1)
            {
                int healerSlot = inventoryController.FindHealerSlot();
                if (healerSlot != -1)
                {
                    inventoryController.SwitchTo(healerSlot + 1);
                }
            }

            // If cannot shoot, walk around
            if (!Physics2D.Linecast(transform.position, targetPosition, obstacleLayer) && distanceToTarget < distanceBetween)
            {
                agent.isStopped = true;
                agent.ResetPath();
                WalkingAround();
                return;
            }

            agent.SetDestination(targetPosition);
            rb.velocity = Vector2.zero;
        }
    }


    void WalkingAround()
    {
        agent.isStopped = true;
        agent.ResetPath();
        System.Random r = new System.Random();
        walkingStep += Time.deltaTime;
        if (walkingStep > maxStep)
        {
            var randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
            rb.velocity = randomDirection * speed;
            maxStep = 0;
            walkingStep = 0;
        }
        if (maxStep == 0)
        {
            maxStep = r.Next(1, 10);
        }

    }
    void GoFindItem(int mode)
    {
        var target = GetNearestItem(mode);
        Vector3 targetPosition = target.transform.position;
        distanceToTarget = Vector2.Distance(transform.position, targetPosition);
        agent.SetDestination(targetPosition);
        rb.velocity = Vector2.zero;
    }
    public void Stunned(float duration)
    {
        StartCoroutine(StunnedCoroutine(duration));
    }

    private IEnumerator StunnedCoroutine(float duration)
    {
        stunned = true;
        yield return new WaitForSeconds(duration);
        stunned = false;
    }

    GameObject GetNearestPlayerInstance()
    {
        GameObject[] prefabInstances = GameObject.FindGameObjectsWithTag("Player");
        if (prefabInstances.Length == 0)
        {
            return null;
        }

        GameObject nearestInstance = null;
        float nearestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject instance in prefabInstances)
        {
            if (instance == gameObject)
            {
                continue;
            }
            float distance = Vector3.Distance(currentPosition, instance.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestInstance = instance;
            }
        }

        return nearestInstance;
    }

    GameObject GetNearestItem(int mode)
    {
        GameObject[] prefabInstances = GameObject.FindGameObjectsWithTag("PickUpItems");
        if (prefabInstances.Length == 0)
        {
            return null;
        }

        GameObject nearestInstance = null;
        float nearestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject instance in prefabInstances)
        {
            if (instance.transform.parent != null)
            {
                continue;
            }
            var gun = instance.GetComponent<GunEntity>();
            if (mode == 0 && gun == null)
            {
                continue;
            }
            if (gun != null)
            {
                if (mode == 1)
                    continue;
                if (gun.currentAmmo == 0)
                    continue;
            }
            float distance = Vector3.Distance(currentPosition, instance.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestInstance = instance;
            }
        }
        return nearestInstance;
    }

}