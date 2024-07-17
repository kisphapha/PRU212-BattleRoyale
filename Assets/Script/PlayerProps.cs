using Cinemachine;
using Photon.Pun;
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

    public GameOver gameOverManager;

    private GameManager gameManager;
    private float checkBorderTimer;
    private bool isDead;
    private FloatingHealthBar floatingHealthBar;
    private FloatingName floatingName;
    // Start is called before the first frame update
    void Start()
    {
        inventoryController = GetComponent<InventoryController>();
        gameOverManager = GetComponent<GameOver>();
        floatingHealthBar = GetComponentInChildren<FloatingHealthBar>();
        floatingName = GetComponentInChildren<FloatingName>();
        mover = GetComponentInChildren<ArrowMovement>();
        gameManager = FindObjectOfType<GameManager>();
        gameManager.PlayerChange(1);
        view = GetComponent<PhotonView>();
        if (view.IsMine)
        {
            // Find and configure the Cinemachine virtual camera to follow this player

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
    private void OnDestroy()
    {
        gameManager.PlayerChange(-1);
        gameOverManager.gameOver();
    }
    //[PunRPC]
    //void SyncName(string name)
    //{
    //    if (floatingName != null)
    //    {
    //        characterName = name;
    //        floatingName.UpdateName(name);
    //    }
    //}
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
