using Assets.Script;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeEntity : PickableItem
{
    public float throwRate = 1.5f;
    public float grenadeSpeed = 15f;
    //public Text GrenadeDisplay; // Reference to the UI Text element
    public GameObject grenadePrefab;
    private ThrowingBehavior throwController;

    private void Start()
    {
        usingCoolDown = throwRate;
        Stackable = true;
        throwController = GetComponent<ThrowingBehavior>();
        //UpdateGrenadeDisplay(); // Initialize grenade display
    }
    public override void ItemEffect()
    {
        Debug.Log("Threw");
        ThrowGrenade();
    }

    public void ThrowGrenade()
    {
        throwController.Throw();
    }

}