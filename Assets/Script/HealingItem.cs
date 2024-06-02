using UnityEngine;
using UnityEngine.WSA;

public class HealingItem : PickableItem
{
    public int healAmount; // Amount of health to heal
    private void Start()
    {
        Stackable = true;
    }
    public override void ItemEffect()
    {
        UseHealingItem();
    }

    public void UseHealingItem()
    {

        // Apply healing
        holder.TakeDamage(-healAmount); // Use negative damage to heal
        Debug.Log($"{name} healed {holder.hp} HP");
        //Destroy(gameObject); // Optionally destroy the item after use
    }

}
