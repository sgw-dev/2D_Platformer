using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
	Can probably condense stats into a single enum and int array/vector
  */

class CharacterData {

	enum CharStat {
		Stuff,
		Stuff2
	}

	//private int level;//??

	private int MAXHP;
	private int MAXXP;
	private int currenthp;
	private int currentxp;

	private int[] stats;

	public CharacterData() {}

	//put max hp and max xp in constructor
	public CharacterData(int _MAXHP, int _MAXXP) {
		this.MAXHP =_MAXHP;
		this.MAXXP = _MAXXP;
	}

	//same as abov, also includes an array of ints.
	public CharacterData(int _MAXHP, int _MAXXP,int[] statinitialvalue) {
		this.MAXHP =_MAXHP;
		this.MAXXP = _MAXXP;
		stats = new int[statinitialvalue.Length];
		for( int i = 0; i < statinitialvalue.Length; i++) {
			stats[i] = statinitialvalue[i];
		}
	}

	/*PUBLIC FUNCTIONS*/
	public void DecreaseHP(int h) {
		currenthp = currenthp - h;
		if (currenthp < 0) {
			currenthp = 0;
			//call game over or something
		}
	}

	public void IncreaseHP(int h) {
		currenthp = currenthp + h;
		if (currenthp > MAXXP) {
			currenthp = MAXHP;
		}
	}

	//LevelUp
	public void SetMAXXP(int newmaxxp) {
		MAXXP = newmaxxp;
	}

	public void GainXP(int x) {
		currentxp = currentxp + x;
		if (currentxp > MAXXP) {
			//LEVEL UP??
			//CHANGE MAX XP to some other value. Somekind of scale is needed
		}
	}


	public void IncreaseStat(int _stat,int value) {
		stats[_stat]+=value;
	}

	public void DecreaseStat(int _stat,int value) {
		stats[_stat]-=value;
	}


	public string ToString() {
		string tmp ="";
		int k =0;
		foreach (int i in stats) {
			tmp+="\nstats"+k+" : " + i;
		}

		return "MAXHP=" + MAXHP + "\nMAXXP=" + MAXXP + "\ncurrenthp=" + currenthp + "\ncurrentxp=" + currentxp
				+"\n"+tmp;
	}
}


public class PlayerController : MonoBehaviour {
    private CharacterData stats;	
	public UIElementManager uistuff;
	void Start() {
		//STR,DEX,CON,CHAR,WIS,INT
        stats = new CharacterData(100,100,new int[] {12,11,11,10,14,12});
		if (uistuff == null) {
			Debug.LogError("Attach UI Manager");
		}
    }

    // Update is called once per frame
    void Update() {
		uistuff.stattwindow.text=stats.ToString();
        //add code to update ui elements
    }
}
