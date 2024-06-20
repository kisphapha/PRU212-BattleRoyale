using UnityEngine;
using UnityEngine.UI;
public class PlayerProps : MonoBehaviour
{
    public string characterName; // Name property
    public float hp, hpMax = 100; // HP property
    public bool isHeldingGun = false;
    public bool isHoldingGrenade = false;
    public float angle = 0f;
    public float offsetDistance = 1.75f; // Constant distance between the player and the picked object
    public GameObject holdingItem;
    public InventoryController inventoryController;
    public ArrowMovement mover;

    public GameOver gameOverManager;

    private float checkBorderTimer;
    private bool isDead;
    private FloatingHealthBar floatingHealthBar;
    private FloatingName floatingName;
    // Start is called before the first frame update
    void Start()
    {
        var persistentData = FindObjectOfType<PersistentData>();
        if (persistentData != null)
        {
            characterName = persistentData.playerName;
        }
        inventoryController = GetComponent<InventoryController>();
        gameOverManager = GetComponent<GameOver>();
        floatingHealthBar = GetComponentInChildren<FloatingHealthBar>();
        floatingName = GetComponentInChildren<FloatingName>();
        mover = GetComponentInChildren<ArrowMovement>();
        floatingName.UpdateName(characterName);
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
        hp -= amount;
        if (hp > hpMax)
        {
            hp = hpMax;
        }
        floatingHealthBar.UpdateHealthBar(hp, hpMax);
        if (hp <= 0 && !isDead)
        {
            isDead = true;
            gameOverManager.gameOver();
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Roof")
        {
            FadingSprite o = collision.GetComponent<FadingSprite>();
            o.FadeOut();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Roof")
        {
            FadingSprite o = collision.GetComponent<FadingSprite>();
            o.FadeIn();
        }
    }
}
