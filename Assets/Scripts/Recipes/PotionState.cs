using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionState : MonoBehaviour
{
    [HideInInspector] public float instability01 = 0;

    public List<IngredientRequest> requests = new List<IngredientRequest>();
    public Ingredients.IngredientEvent onNewRequest = new Ingredients.IngredientEvent();

    public int CorrectIngredientPickedUp = 0;

    public static PotionState Instance;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CharacterPickUp.Instance.onIngredientPickUp.AddListener(OnIngredientPickUp);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameIsOver)
            return;

        instability01 += Time.deltaTime / DifficultyManager.Instance.InstabilityProgressTime;

        while(instability01>1)
        {
            instability01 -= 1;
            AddNewIngredientRequest();
        }

        UpdateRequests();
    }

    void UpdateRequests()
    {
        foreach(IngredientRequest request in requests)
        {
            request.remainingTime01 -= Time.deltaTime / request.totalTime;

            if (request.remainingTime01 < 0)
            {
                GameManager.Instance.GameOver();
            }
        }
    }

    public void PickUpWrongIngredient()
    {
        instability01 += 0.5f;
    }

    public void AddNewIngredientRequest()
    {
        if (requests.Count >= 5)
        {
            print("Max Request Reached");
            return;
        }

        Ingredient ingredient = Ingredients.GetRandomIngredient();
        IngredientRequest request = new IngredientRequest(ingredient);
        requests.Add(request);
        onNewRequest.Invoke(ingredient);
        SoundManager.PlaySound(8);
    }

    void OnIngredientPickUp(Ingredient ingredient)
    {
        //Check all request
        foreach (IngredientRequest request in requests)
        {
            if(request.ingredient== ingredient)
            {
                CorrectIngredientPickedUp++;
                requests.Remove(request);
                SoundManager.PlaySound(9);
                PickUpParticles.Instance.SetCorrect(true);
                return;
            }
        }

        //If not a good ingredient
        PickUpWrongIngredient();
        PickUpParticles.Instance.SetCorrect(false);
    }

    public class IngredientRequest
    {
        public Ingredient ingredient;
        public float remainingTime01;
        public float totalTime;

        public IngredientRequest(Ingredient ingredient)
        {
            this.ingredient = ingredient;
            this.remainingTime01 = 1;
            this.totalTime = DifficultyManager.Instance.RequestTotalTime;
        }
    }

}
