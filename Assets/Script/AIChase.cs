using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class AIChase : MonoBehaviour
{
    public float speed;
    public float distanceBetween = 5f;
    //public LayerMask obstacleLayer; // Layer mask to specify which objects are considered obstacles
    private GameObject target;
    private bool stunned;
    Vector2 direction;
    NavMeshAgent agent;

    private float checkInterval = 3.0f; // Time interval in seconds
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
            // Reset the timer
            timer = 0f;
        }
        if (!stunned && target != null)
        {
            agent.SetDestination(target.transform.position);
            direction = target.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(Vector3.forward * angle) * Quaternion.Euler(0f,0f,180f);
            //distance = Vector2.Distance(transform.position, target.transform.position);
            //
            //direction.Normalize();
            //transform.LookAt(target.transform);
            //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            //if (distance < distanceBetween)
            //{
            //    rb.velocity = direction * speed;
            //    //transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
            //    transform.rotation = Quaternion.Euler(Vector3.forward * angle);

            //}
        }     
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
}
