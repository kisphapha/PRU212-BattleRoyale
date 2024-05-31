using UnityEngine;

public class HealingItem : MonoBehaviour
{
    public string itemName; // Item name
    public int healAmount; // Amount of health to heal
    private PlayerProps player; // Reference to the player who picks up the item

    void Start()
    {
        itemName = gameObject.name; // Set the item name to the game object's name
    }

    public void PickUp(PlayerProps player)
    {
        this.player = player;
    }

    public void UseHealingItem()
    {

        // Apply healing
        player.TakeDamage(-healAmount); // Use negative damage to heal
        Debug.Log($"{itemName} healed {player.hp} HP");
        Destroy(gameObject); // Optionally destroy the item after use
    }
    public void OnHealingItemPickedUp(GameObject healingItem)
    {
        PlayerProps player = FindObjectOfType<PlayerProps>();
        healingItem.GetComponent<HealingItem>().PickUp(player);
    }

}
