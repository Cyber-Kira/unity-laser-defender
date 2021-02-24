using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Enemy : MonoBehaviour
{

    [Header("Enemy")]
    [SerializeField] float health = 100;
    [SerializeField] int scoreValue = 100;

    [Header("Projectile")]
    [SerializeField] GameObject projectile;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBerweenShots = 3f;
    [SerializeField] float projectileSpeed = 10f;
    float shotCounter;

    [Header("VFX")]
    [SerializeField] GameObject explosionVFX;

    [Header("Sounds")]
    [SerializeField] AudioClip explosionSound;
    [SerializeField] [Range(0,1)] float volume = 1;

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBerweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    public void CountDownAndShoot()
    {
        shotCounter = shotCounter - Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBerweenShots);
        }
    }

    public void Fire()
    {
        GameObject laser = Instantiate(
                                        projectile, 
                                        transform.position, 
                                        Quaternion.identity) as GameObject;

        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -projectileSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health = health - damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        PlaySound();
        Destroy(gameObject);
        GameObject explosion = Instantiate(
            explosionVFX,
            transform.position,
            Quaternion.identity
        ) as GameObject;
        Destroy(explosion, 0.5f);
    }

    private void PlaySound()
    {
        AudioClip clip = explosionSound;
        Vector3 position = new Vector3(0, 0, -5);
        AudioSource.PlayClipAtPoint(clip, position, volume);
    }
}
