using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimObject : MonoBehaviour
{
    private static readonly int RIGHT = 1;
    private static readonly int LEFT = -1;

    private Vector2 cursorPos;
    private float angle;
    private SpriteRenderer gun;
    private bool back;
    private Quaternion wobble;
    private float recoil;
    public int intensity;
    private float startTime;
    public float recoilDuration;
    public float readyangle;
    private float recoilIntensity;
    public AudioClip shot;

    public CharacterController2D cc2d;
    public Transform parent;
    public Sprite weapon;
    public Sprite weaponShoot;
    public Sprite weaponFlip;
    public Sprite weaponFlipShoot;
    private AudioSource sound;
    public float BulletSpeed;
    public bool ready;

    public int level = 1;
    public GameObject Bullet;
    public GameObject BulletSpawnPoint;
    private GameObject BulletObj;

    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();
        recoilIntensity = 0;
        ready = true;
        recoil = 0f;
        gun = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame

    private void FixedUpdate()
    {

        float t = (Time.time - startTime) / recoilDuration;
        recoil = Mathf.SmoothStep(recoilIntensity, 0, t);
        if (t > readyangle / 10 && !ready)
        {
            ready = true;
            if (gun.sprite == weaponShoot)
            {
                gun.sprite = weapon;
            }
            else
            {
                gun.sprite = weaponFlip;
            }
        }

        transform.position = parent.position;

        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        angle = (Mathf.Rad2Deg * Mathf.Atan2((cursorPos.x - parent.position.x), (parent.position.y - cursorPos.y))) - 90;
        if (angle < -90 && !back) { FlipSide(); back = true; gun.flipY = true; }
        if (angle > -90 && back) { FlipSide(); back = false; gun.flipY = false; }

        if (angle > -90)
        {
            wobble = Quaternion.Inverse(wobble);
        }
        if (angle < -90)
        {
            recoil = -recoil;
        }
        transform.rotation = Quaternion.Euler(0, 0, angle) * parent.rotation * wobble * Quaternion.Euler(0, 0, recoil);
    }

    public void Flip()
    {
        gun.sortingOrder = gun.sortingOrder * -1;
    }

    private void FlipSide()
    {
        if (gun.sprite.Equals(weapon)) gun.sprite = weaponFlip;
        else if (gun.sprite.Equals(weaponFlip)) gun.sprite = weapon;
        else if (gun.sprite.Equals(weaponShoot)) gun.sprite = weaponFlipShoot;
        else gun.sprite = weaponShoot;
    }

    public void Wobble(float velocity)
    {
        wobble = Quaternion.Euler(0, 0, velocity/level);
    }

    public void Shoot()
    {
        BulletObj = Instantiate(Bullet, BulletSpawnPoint.transform.position, BulletSpawnPoint.transform.rotation, null);
        BulletObj.GetComponent<Rigidbody2D>().AddForce((BulletObj.transform.position - transform.position)*BulletSpeed);
        startTime = Time.time;
        sound.PlayOneShot(shot);
        if (recoil < 0) recoil = -recoil;
        recoilIntensity = (intensity / level) + recoil;
        ready = false;
        if (gun.sprite == weapon)
        {
            gun.sprite = weaponShoot;
        }else
        {
            gun.sprite = weaponFlipShoot;
        }
    }
}
