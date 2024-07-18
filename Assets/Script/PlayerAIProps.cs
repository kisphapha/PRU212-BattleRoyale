using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
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
    private FloatingKillCounter floatingKillCounter;
    private FloatingName floatingName;
    private bool isDead;
    private float checkBorderTimer;
    private GameManager gameManager;
    private PhotonView view;
    private int killCount;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.UpdatePlayerCount();
        characterName = gameObject.name;
        floatingHealthBar = GetComponentInChildren<FloatingHealthBar>();
        floatingKillCounter = GetComponentInChildren<FloatingKillCounter>();
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

    public void TakeDamage(float amount, GameObject causer = null)
    {
        hp -= amount;
        if (hp > hpMax)
        {
            hp = hpMax;
        }
        if (floatingHealthBar != null)
        {
            floatingHealthBar.UpdateHealthBar(hp, hpMax);
        }
        if (hp <= 0 && !isDead)
        {
            isDead = true;
            if (causer != null)
            {
                var player = causer.GetComponent<PlayerProps>();
                if (player != null)
                {
                    player.EarnKill();
                }
                var ai = causer.GetComponent<PlayerAIProps>();
                if (ai != null)
                {
                    ai.EarnKill();
                }
            }
            view.RPC("SyncDeath", RpcTarget.AllBuffered);
        }
        view.RPC("SyncHealth", RpcTarget.OthersBuffered, hp);       
    }
    public void EarnKill()
    {
        killCount++;
        if (floatingKillCounter != null)
        {
            floatingKillCounter.UpdateKill(killCount);
        }
        view.RPC("SyncKills", RpcTarget.OthersBuffered, killCount);
    }
    public void OnDestroy()
    {
        if (gameManager != null)
        {
            gameManager.UpdatePlayerCount();
            
        }
    }
    [PunRPC]
    void SyncKills(int kills)
    {
        killCount = kills;
        if (floatingKillCounter != null)
        {
            floatingKillCounter.UpdateKill(kills);
        }
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
