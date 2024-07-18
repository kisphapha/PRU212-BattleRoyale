using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class ExplodeOnCollision : MonoBehaviour
{
    public GameObject explosionPrefab;
    public GameObject smokeParticlePrefab;
    public bool isTriggered = true;
    private float explosionRadius;
    private float explosionForce;
    private float explosionDuration; // Duration for explosion effect to stay in scene
    private float explosionDelay;
    private PhotonView view;
    public GameObject master;
    private float timer;
    public AudioClip explodeAudioClip;

    //public int maxColliders = 10; // Limit the number of colliders processed
    private void Start()
    {
        // Start the explosion countdown
        view = GetComponent<PhotonView>();
        //StartCoroutine(ExplosionCountdown());
    }
    private void Update()
    {
        if (view != null && view.IsMine)
        {
            if (isTriggered)
            {            
                if (timer <= explosionDelay)
                {
                    timer += Time.deltaTime * 2;
                    view.RPC("SyncCounting", RpcTarget.AllBufferedViaServer, timer, explosionDelay, isTriggered);

                }
            }
        }
    }
    public void Setup(float explosionRadius, float explosionForce, float explosionDuration, float explosionDelay)
    {
        this.explosionRadius = explosionRadius;
        this.explosionForce = explosionForce;
        this.explosionDuration = explosionDuration;
        this.explosionDelay = explosionDelay;
    }

    private void Explode()
    {
        if (view != null)
        {
            if (explosionPrefab == null)
            {
                Debug.LogError("Explosion prefab not assigned.");
                return;
            }

            GameObject explosion = PhotonNetwork.Instantiate(explosionPrefab.name, transform.position, transform.rotation);
            GameObject smoke = PhotonNetwork.Instantiate(smokeParticlePrefab.name, transform.position, transform.rotation);
            explosion.transform.localScale = new Vector3(explosionRadius * 2, explosionRadius * 2, explosionRadius * 2);

            var time = explosion.GetComponent<DestroyAfterATime>();
            time.destroyDelay = explosionDuration;
            time.Unlock();
            view.RPC("PlayExplodeSound", RpcTarget.All);
            view.RPC("DestroyObjectOnExplode", RpcTarget.AllBufferedViaServer);
        }
    }
    [PunRPC]
    private void PlayExplodeSound()
    {
        if (explodeAudioClip != null)
        {
            float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
            if (distance <= 120)
            {
                AudioSource.PlayClipAtPoint(explodeAudioClip, transform.position);
            }
        }
    }
    [PunRPC]
    private void DestroyObjectOnExplode()
    {
        Debug.Log("Explode");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        Debug.Log("Number of colliders detected: " + colliders.Length);

        int processedCount = 0;
        foreach (Collider2D nearbyObject in colliders)
        {
            float distance = Vector3.Distance(transform.position, nearbyObject.transform.position);
            Rigidbody2D rb = nearbyObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 explosionDirection = nearbyObject.transform.position - transform.position;
                rb.AddForce(explosionDirection.normalized * explosionForce);
            }
            PlayerProps player = nearbyObject.GetComponent<PlayerProps>();
            if (player != null)
            {
                player.TakeDamage(500 / Mathf.Max(1, distance), master);
                var movement = nearbyObject.GetComponentInChildren<ArrowMovement>();
                if (movement != null)
                {
                    movement.Stunned(2);
                }
            }
            PlayerAIProps playerAI = nearbyObject.GetComponent<PlayerAIProps>();
            if (playerAI != null)
            {
                playerAI.TakeDamage(500 / Mathf.Max(1, distance), master);
                var movement = nearbyObject.GetComponent<AIBehavior>();
                if (movement != null)
                {
                    movement.Stunned(2);
                }
            }
            BreakBoxes box = nearbyObject.GetComponent<BreakBoxes>();
            if (box != null)
            {
                box.TakeDamage(500 / Mathf.Max(1, distance));
            }
            TNTBarrels tnt = nearbyObject.GetComponent<TNTBarrels>();
            if (tnt != null)
            {
                tnt.TakeDamage(500 / Mathf.Max(1, distance));
            }
            processedCount++;
        }
        Destroy(gameObject);
        //Destroy(explode,explosionDuration);
        //}
    }

    [PunRPC]
    private void SyncCounting(float syncedTimer, float syncedExplosionDelay, bool syncedIsTriggered)
    {
        var explode = gameObject.GetComponent<ExplodeOnCollision>();
        timer = syncedTimer;
        explosionDelay = syncedExplosionDelay;
        isTriggered = syncedIsTriggered;
        if (isTriggered)
        {
            if (timer >= explosionDelay)
            {
                explode.Explode();
                
            }
        }
    }
}
