using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour{

    public float maxHealth;
    private float health;
    public GameObject healthbar;
    public GameObject sparkle;
    public GameObject loot;
    private float healthWidth;
    private float maxWidth;


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        maxWidth = healthbar.transform.localScale.x;
        healthWidth = maxWidth;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void applyDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Instantiate(sparkle, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -8f), gameObject.transform.rotation);
            Instantiate(loot, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -8f), gameObject.transform.rotation);
            Destroy(gameObject);
        }
        float ratio = maxWidth / maxHealth;
        float width = healthbar.transform.localScale.x;
        healthbar.transform.localScale = new Vector3(width - (ratio * damage), healthbar.transform.localScale.y, 1.0f);

    }
}
