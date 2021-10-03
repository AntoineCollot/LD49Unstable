using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static bool gameIsOver = false;
    public UnityEvent onGameOver = new UnityEvent();

    public bool ignoreGameOver = false;
    int baseScore = 0;

    public static GameManager Instance;

    public int IngredientScore { get => PotionState.Instance.CorrectIngredientPickedUp * 10; }
    public int TotalScore { get => baseScore + IngredientScore; }

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        gameIsOver = false;
    }

    private void Start()
    {
        StartCoroutine(IncreaseScore());
    }

    IEnumerator IncreaseScore()
    {
        while(!gameIsOver)
        {
            yield return new WaitForSeconds(1);
            baseScore++;
        }
    }

    public void GameOver()
    {
#if UNITY_EDITOR
        if (ignoreGameOver)
            return;
#endif
        if (gameIsOver)
            return;

        MovementController.Instance.GetComponentInChildren<Animator>().SetBool("Death", true);
        gameIsOver = true;
        print("Game Over");
        onGameOver.Invoke();

        SoundManager.PlaySound(12);
    }
}
