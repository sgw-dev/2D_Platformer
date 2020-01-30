using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *	Class to stoe player values
 * 
 */
public class PlayerStats
{

    //getter.setters
    private int level { get; set; }
    private int maxHealth { get; set; }
    private int currentHealth { get; set; }
    private int experience { get; set; }

    /**
	 *  example of new player
	 *	= new PlayerStats(100,100,1);
	 */
    public PlayerStats(int maxHealth, int currentHealth, int experience)
    {
        this.maxHealth = maxHealth;
        this.currentHealth = currentHealth;
        this.experience = experience;
    }

    public void GainXP(int amount)
    {
        experience += amount;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    public void Heal(int amount)
    {
        currentHealth -= amount;
    }


}