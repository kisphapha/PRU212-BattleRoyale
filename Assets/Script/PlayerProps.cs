using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
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
    public PhotonView view;
    public int killCount;
    public AudioClip killEarnClip;
    public AudioClip failClip;

    private GameManager gameManager;
    private float checkBorderTimer;
    private bool isDead;
    private FloatingHealthBar floatingHealthBar;
    private FloatingKillCounter floatingKillCounter;
    private FloatingName floatingName;

    // Start is called before the first frame update
    void Start()
    {
        inventoryController = GetComponent<InventoryController>();
        floatingHealthBar = GetComponentInChildren<FloatingHealthBar>();
        floatingName = GetComponentInChildren<FloatingName>();
        floatingKillCounter = GetComponentInChildren<FloatingKillCounter>();
        mover = GetComponentInChildren<ArrowMovement>();
        gameManager = FindObjectOfType<GameManager>();

        view = GetComponent<PhotonView>();
        if (view.IsMine)
        {
            // Find and configure the Cinemachine virtual camera to follow this player
            gameManager.UpdatePlayerCount();
            characterName = PhotonNetwork.NickName;
            floatingName.UpdateName(characterName);

            var virtualCam = FindObjectOfType<CinemachineVirtualCamera>();
            if (virtualCam != null)
            {
                virtualCam.Follow = transform;
                virtualCam.LookAt = transform;
            }
        } else
        {
            characterName = view.Owner.CustomProperties["PlayerName"].ToString();
            floatingName.UpdateName(characterName);
        }
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
    public void EarnKill()
    {
        Debug.Log("Earned a kill!");
        killCount++;
        if (floatingKillCounter != null)
        {
            floatingKillCounter.UpdateKill(killCount);
        }
        AudioSource.PlayClipAtPoint(killEarnClip, transform.position);
        var killRecord = PlayerPrefs.GetInt("kills") + 1;
        PlayerPrefs.SetInt("kills",killRecord);
        view.RPC("SyncKills", RpcTarget.OthersBuffered, killCount);
    }
    public void TakeDamage(float amount, GameObject causer = null)
    {
        if (causer != null)
        {
            Debug.Log("Getting damaged from " + causer.name);
        }
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
            AudioSource.PlayClipAtPoint(failClip, transform.position);
            view.RPC("SyncDeath", RpcTarget.AllBuffered);
        }
        view.RPC("SyncHealth", RpcTarget.OthersBuffered, hp);        

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Roof" && view != null && view.IsMine)
        {
            FadingSprite o = collision.GetComponent<FadingSprite>();
            o.FadeOut();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Roof" && view != null && view.IsMine)
        {
            FadingSprite o = collision.GetComponent<FadingSprite>();
            o.FadeIn();
        }
    }
    private void OnDestroy()
    {
        if (view.IsMine)
        {
            gameManager.gameOverController.gameOver();
            gameManager.gameOverController.UpdateKillCount(killCount);
        }
        gameManager.UpdatePlayerCount();
    }
    public void Win()
    {
        if (view.IsMine)
        {
            var winRecord = PlayerPrefs.GetInt("wins") + 1;
            PlayerPrefs.SetInt("wins", winRecord);
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
    void SyncKills(int kills)
    {
        killCount = kills;
        if (floatingKillCounter != null)
        {
            floatingKillCounter.UpdateKill(kills);
        }
    }
    [PunRPC]
    void SyncDeath()
    {
        Destroy(gameObject);
    }
}
