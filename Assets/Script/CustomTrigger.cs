 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTrigger : MonoBehaviour
{
   public event System.Action<Collider2D> onTriggerEnter;
   public event System.Action<Collider2D> onTriggerExit;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        onTriggerEnter?.Invoke(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        onTriggerExit?.Invoke(collision);
    }
}
