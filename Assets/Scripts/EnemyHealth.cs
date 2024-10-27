using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public Animator anim;
    public float currentHealth;
    public float maxHealth;
    public bool isDead;
    public int goldToGive;
    public GameObject hitEffect;
    public GameObject splash;
    public AudioClip deathSfx;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        anim.SetTrigger("Hurt");
        GameObject obj = Instantiate(hitEffect, transform.position, transform.rotation);
        Destroy(obj,.5f);
        if(currentHealth <= 0)
        {
            GetComponent<Collider2D>().enabled = false;
            audioSource.PlayOneShot(deathSfx);
            isDead = true;
            GameManager.instance.enemiesOnscreen -= 1;
            currentHealth = 0;
            GameManager.instance.AddGold(goldToGive);
            anim.SetBool("IsDead", true);
            Destroy(gameObject, 2f);
            AudioManager.instance.PlaySFX(11);
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
        GameManager.instance.enemiesOnscreen -= 1;
        GameObject obj = Instantiate(splash, transform.position, transform.rotation);
        Destroy(obj, 0.7f);
    }
}
