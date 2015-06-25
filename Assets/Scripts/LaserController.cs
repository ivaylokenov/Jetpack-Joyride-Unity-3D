using UnityEngine;

public class LaserController : MonoBehaviour
{
    public Sprite laserOnSprite;
    public Sprite laserOffSprite;

    public float interval = 0.5f;
    public float rotationSpeed = 50.0f;

    private SpriteRenderer spriteRndr;
    private BoxCollider2D boxCollider;

    private float timeSinceStart;
    private bool laserOn;

    public void Start()
    {
        this.spriteRndr = this.GetComponent<SpriteRenderer>();
        this.boxCollider = this.GetComponent<BoxCollider2D>();
        this.timeSinceStart = 0;
    }

    public void FixedUpdate()
    {
        this.timeSinceStart += Time.fixedDeltaTime;

        if (this.timeSinceStart > this.interval)
        {
            this.laserOn = !this.laserOn;
            this.timeSinceStart = 0;

            this.spriteRndr.sprite = this.laserOn ? this.laserOnSprite : this.laserOffSprite;
            this.boxCollider.enabled = this.laserOn;
        }

        this.transform.Rotate(0, 0, rotationSpeed * Time.fixedDeltaTime);
        this.interval -= 0.001f;
    }
}
