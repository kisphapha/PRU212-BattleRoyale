using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 offset;

    private void Start()
    {
    }
    public void UpdateHealthBar(float health, float healthMax)
    {
        healthBar.value = health / healthMax;
    }
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity;
        transform.position = target.transform.position + offset;
    }
}
