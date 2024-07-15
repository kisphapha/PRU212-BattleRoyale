using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
public class PlayerProps : MonoBehaviour
{
    public string characterName; // Name property
    public float hp = 100, hpMax = 100; // HP property
    public bool isHeldingGun = false;
    public bool isHoldingGrenade = false;
    public float angle = 0f;
    public float offsetDistance = 1.75f; // Constant distance between the player and the picked object
    public GameObject holdingItem;
    public InventoryController inventoryController;
    public ArrowMovement mover;

    public GameOver gameOverManager;

    private GameManager gameManager;
    private float checkBorderTimer;
    private bool isDead;
    private FloatingHealthBar floatingHealthBar;
    private FloatingName floatingName;
    private CinemachineVirtualCamera cmv;
    // Start is called before the first frame update
    void Start()
    {
        cmv = GameObject.Find("FollowCamera").GetComponent<CinemachineVirtualCamera>();
        cmv.Follow = gameObject.transform;

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
        gameManager = FindObjectOfType<GameManager>();
        gameManager.PlayerChange(1);
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
            gameManager.PlayerChange(-1);
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
