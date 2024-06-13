using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTBarrels : MonoBehaviour
{
    public float hp = 100;
    public GameObject explosionPrefab;
    public GameObject smokeParticlePrefab;
    public float explosionRadius;
    public float explosionForce;
    public float explosionDuration; // Duration for explosion effect to stay in scene

    private float checkBorderTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkBorderTimer += Time.deltaTime;
        if (checkBorderTimer >= 1.5f)
        {
            if (DamageCircle.IsOutsideCircle_Static(gameObject.transform.position))
            {
                TakeDamage(10);
            }
            checkBorderTimer = 0f;
        }
    }
    public void TakeDamage(float amount)
    {
        hp -= amount;
        Debug.Log(hp);
        if (hp <= 0)
        {
            var exploder = gameObject.AddComponent<ExplodeOnCollision>();
            exploder.Setup(explosionRadius, explosionForce,explosionDuration,0.5f);
            exploder.explosionPrefab = explosionPrefab;
            exploder.smokeParticlePrefab = smokeParticlePrefab;
        }
    }
}
