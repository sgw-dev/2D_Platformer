using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject message;
    public GameObject loot;
    private Text text;
    private GameObject player;
    private Player playerScript;
    private int state;
    // Start is called before the first frame update
    void Start()
    {
        player = this.gameObject;
        text = message.transform.GetChild(0).gameObject.GetComponent<Text>();
        playerScript = player.GetComponent<Player>();
        playerScript.ToggleFrozen();
        nextMessage();
    }
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            message.SetActive(false);
            nextMessage();
        }
    }
    void nextMessage()
    {
        bool show = true;
        switch (state)
        {
            case 0:
                text.text = "Hello stranger! I'm glad to see that you are awake, you had me worried there.";
                state++;
                break;

            case 1:
                text.text = "Now that you are up, you are going to need this! It can be dangerous in this forest!";
                state++;
                GameObject item = Instantiate(loot, new Vector3(player.transform.position.x + 10, player.transform.position.y, 0f), Quaternion.identity);
                
                break;
            case 2:
                text.text = "You can open your inventory by pressing 'i', or clicking on the 'INV' button. \n Click and Drag to equip items, and Right Click to inspect them";
                state++;
                break;
            case 3:
                text.text = "Use W A S D to go to your sword. \n Hold Shift to sprint.";
                state++;
                break;
            case 4:
                playerScript.ToggleFrozen();
                state++;
                show = false;
                break;
            case 6:
                text.text = "Use the Space Bar to jump, you can even jump in the air!";
                playerScript.ToggleFrozen();
                state++;
                break;
            case 7:
                playerScript.ToggleFrozen();
                state++;
                show = false;
                break;
            case 9:
                text.text = "Left click to attack, and hold Right Mouse Button to block attacks";
                state++;
                break;
            case 10:
                state++;
                show = false;
                break;
            default:
                show = false;
                break;
        }
        message.SetActive(show);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collided with " + other.name);
        if(other.name == "tutorial2")
        {
            state = 6;
            nextMessage();
            Destroy(other.gameObject);
        }else if(other.name == "Sword(Clone)")
        {
            state = 9;
            nextMessage();
        }
    }
}
