using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public Transform player;
    [HideInInspector]
    public Player_Controller playerController;
    public Camera mainCamera;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        playerController = player.GetComponent<Player_Controller>();
    }
}
