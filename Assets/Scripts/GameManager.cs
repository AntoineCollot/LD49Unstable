using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static bool gameIsOver = false;
    public UnityEvent onGameOver = new UnityEvent();

    public static GameManager Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        gameIsOver = false;
    }

   public void GameOver()
    {
        gameIsOver = true;
        onGameOver.Invoke();
    }
}
