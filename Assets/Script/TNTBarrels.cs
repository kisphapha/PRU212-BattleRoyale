using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TNTBarrels : MonoBehaviour
{
    public float hp = 100;
    public float explosionRadius;
    public float explosionForce;
    public float explosionDuration; // Duration for explosion effect to stay in scene
    private PhotonView view;
    private float checkBorderTimer;

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
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
        if (view != null)
        {
            view.RPC("SyncDamaged", RpcTarget.AllViaServer, amount);
        }
    }

    [PunRPC]    
    private void SyncDamaged(float amount)
    {
        hp -= amount;
        var exploder = gameObject.GetComponent<ExplodeOnCollision>();
        if (hp <= 0 && exploder != null)
        {
            exploder.isTriggered = true;
            exploder.Setup(explosionRadius, explosionForce, explosionDuration, 0.5f);
        }
    }
}
