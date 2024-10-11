using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlUpdate : MonoBehaviour
{
    public static ControlUpdate instance;
    private Player player;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void Rebinds()
    {
        player = GameObject.Find("TestPlayer").GetComponent<Player>();
        Debug.Log(player);
        InputAction dodgeAction = player.inputActions.FindAction("Gameplay/Dodge");
        Debug.Log(dodgeAction);
        if (dodgeAction == null)
        {
            return;
        }
        player.inputActions.FindAction("Gameplay/Dodge").AddBinding("<Keyboard>/x");
        Debug.Log(dodgeAction);
    }

    private void ChangeBind()
    {
       
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
