using UnityEngine;

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
        if (holderAI != null)
        {
            holderAI.TakeDamage(-healAmount);

        } else if (holder != null)
        {
            holder.TakeDamage(-healAmount);
        }
    }

}
