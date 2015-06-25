using UnityEngine;

public class MouseController : MonoBehaviour
{
    public float jetpackSpeed = 75.0f;
    public float forwardMovementSpeed = 5.0f;

    public ParticleSystem ps;
    public GameObject groundChecker;
    public LayerMask layerMask;
    public Texture2D coinTexture;

    public AudioClip coinsSound;
    public AudioClip laserSound;

    public AudioSource footstepsAudioSource;
    public AudioSource jetpackAudioSource;

    private Rigidbody2D rb;
    private Animator animator;

    private bool grounded;
    private bool dead;

    private int coins = 0;

    public void Start()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.animator = this.GetComponent<Animator>();
    }

    public void FixedUpdate()
    {
        var jetpackActive = Input.GetButton("Fire1") && !dead;

        this.CheckIfOnGround();
        this.AdjustJetpack(jetpackActive);

        if (jetpackActive)
        {
            this.rb.AddForce(new Vector2(0, this.jetpackSpeed));
        }

        if (!this.dead)
        {
            var velocity = this.rb.velocity;
            velocity.x = this.forwardMovementSpeed;
            this.rb.velocity = velocity;
        }

        this.forwardMovementSpeed += 0.001f;
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Coin"))
        {
            this.coins++;
            AudioSource.PlayClipAtPoint(this.coinsSound, this.transform.position);
            Destroy(collider.gameObject);
        }
        else
        {
            this.dead = true;
            AudioSource.PlayClipAtPoint(this.laserSound, this.transform.position);
            this.animator.SetBool("Dead", true);
        }
    }

    public void OnGUI()
    {
        this.DisplayCoins();
        this.DisplayRestartButton();
    }

    private void DisplayCoins()
    {
        var coinIconRect = new Rect(10, 10, 32, 32);
        GUI.DrawTexture(coinIconRect, this.coinTexture);

        var coinsScoreRect = new Rect(50, 10, 32, 32);
        var coinsTextStyle = new GUIStyle();
        coinsTextStyle.fontSize = 30;
        coinsTextStyle.fontSize = 30;
        coinsTextStyle.fontStyle = FontStyle.Bold;
        coinsTextStyle.normal.textColor = Color.yellow;

        GUI.Label(coinsScoreRect, this.coins.ToString(), coinsTextStyle);
    }

    private void DisplayRestartButton()
    {
        if (this.dead && this.grounded)
        {
            var rect = new Rect(Screen.width / 2 - 50, Screen.height / 2 - 16, 100, 32);
            if (GUI.Button(rect, "Tap to restart!"))
            {
                Application.LoadLevel(Application.loadedLevelName);
            }
        }
    }

    private void AdjustJetpack(bool jetpackActive)
    {
        this.ps.enableEmission = !this.grounded;
        this.ps.emissionRate = jetpackActive ? 300 : 100;
        this.jetpackAudioSource.enabled = jetpackActive;
    }

    private void CheckIfOnGround()
    {
        var colliding = Physics2D.OverlapCircle(this.groundChecker.transform.position, 0.1f, this.layerMask);
        this.grounded = colliding == null ? false : true;
        this.animator.SetBool("Grounded", this.grounded);

        this.footstepsAudioSource.enabled = this.grounded;
    }
}