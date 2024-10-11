using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class ControlUpdate : MonoBehaviour
{
    public static ControlUpdate instance;
    private Player player;

    private string dodgeBind;
    void Start()
    {
        ChangeBind("dodge", "<Keyboard>/x");
    }

    void Update()
    {
        
    }
    public void Rebinds()
    {
        player = GameObject.Find("TestPlayer").GetComponent<Player>();
        Debug.Log(player);
        if (player.inputActions.FindAction("Gameplay/Dodge") != null && dodgeBind != null)
        {
            player.inputActions.FindAction("Gameplay/Dodge").AddBinding(dodgeBind);
        }
        Debug.Log(player.inputActions.FindAction("Gameplay/Dodge"));
    }

    private void ChangeBind(string action, string binding)
    {
       if (action == "dodge") { dodgeBind = binding; }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }


}
