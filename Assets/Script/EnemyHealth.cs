using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour{

    public int maxHealth;
    private int health;

    public GameObject healthbar;
    private float healthWidth;
    private float healthScale;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthScale = healthbar.transform.localScale.x;
        healthWidth = healthScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void applyDamage(int damage) {
        health -= damage;
        if (health <= 0)
        {
            die();
        }
        float ratio = (float)damage / healthWidth;
        healthScale = healthbar.transform.localScale.x;
        healthbar.transform.localScale = new Vector3 (healthScale - (healthWidth*ratio), healthbar.transform.localScale.y, 1.0f);

    }
    public void die() {
        Destroy(gameObject);
    }
}
