using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour{

    public float maxHealth;
    private float health;

    //public float weakness;

    public GameObject healthbar;
    private float healthWidth;
    private float maxWidth;

    // Start is called before the first frame update
    void Start()
    {
        //maxHealth = healthbar.transform.localScale.x;
        health = maxHealth;
        maxWidth = healthbar.transform.localScale.x;
        healthWidth = maxWidth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void applyDamage(float damage) {

        //damage = damage * weakness;
        health -= damage;
        if (health <= 0)
        {
            die();
        }
        float ratio = maxWidth / maxHealth;
        float width = healthbar.transform.localScale.x;
        healthbar.transform.localScale = new Vector3 (width - (ratio*damage), healthbar.transform.localScale.y, 1.0f);

    }
    public void die() {
        Destroy(gameObject);
    }
}
