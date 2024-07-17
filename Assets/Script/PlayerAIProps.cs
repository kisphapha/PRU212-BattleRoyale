using Photon.Pun;
using UnityEngine;

public class PlayerAIProps : MonoBehaviour
{
    public string characterName; // Name property
    public float hp = 100, hpMax = 100; // HP property
    public float offsetDistance = 1.75f; // Constant distance between the player and the picked object
    public GameObject holdingItem;
    [HideInInspector] public int gunNumber = 0 ;
    [HideInInspector] public int otherItemNumber = 0;

    private FloatingHealthBar floatingHealthBar;
    private FloatingName floatingName;
    private bool isDead;
    private float checkBorderTimer;
    private GameManager gameManager;
    private PhotonView view;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.PlayerChange(1);
        characterName = gameObject.name;
        floatingHealthBar = GetComponentInChildren<FloatingHealthBar>();
        floatingName = GetComponentInChildren<FloatingName>();
        floatingName.UpdateName(characterName);
        view = GetComponent<PhotonView>();

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
        if (floatingHealthBar != null)
        {
            floatingHealthBar.UpdateHealthBar(hp, hpMax);
            if (hp <= 0 && !isDead)
            {
                isDead = true;
                view.RPC("SyncDeath", RpcTarget.AllBuffered);
            }
            view.RPC("SyncHealth", RpcTarget.OthersBuffered, hp);
        }
    }

    public void OnDestroy()
    {
        gameManager.PlayerChange(-1);
    }

    [PunRPC]
    void SyncHealth(float syncHealth)
    {
        hp = syncHealth;
        if (floatingHealthBar != null)
        {
            floatingHealthBar.UpdateHealthBar(syncHealth, hpMax);
        }
    }
    [PunRPC]
    void SyncDeath()
    {
        Destroy(gameObject);
    }
}
