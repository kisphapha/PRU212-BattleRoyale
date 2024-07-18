using Photon.Pun;
using UnityEngine;

public class HealingItem : PickableItem
{
    public int healAmount; // Amount of health to heal
    public AudioClip HealClip;
    public PhotonView view;
    private void Start()
    {
        Stackable = true;
        view = GetComponent<PhotonView>();
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
            if (view.IsMine)
            {
                AudioSource.PlayClipAtPoint(HealClip, holder.transform.position);
            }
        }
    }

}
