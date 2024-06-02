using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class ShootingBehavior : MonoBehaviour
{
    public GunEntity gunEntity;
    // Start is called before the first frame update
    private void Start()
    {
        gunEntity = GetComponent<GunEntity>();
    }
    public virtual void Shoot()
    {

    }
}
