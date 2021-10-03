using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Ingredient { Mushrooms, Exploroots, GodessTears, ToadSaliva, Crystal}

public static class Ingredients
{
    static Ingredient lastRandomIngredient = Ingredient.Crystal;

    public static Ingredient GetRandomIngredient()
    {
        Ingredient ingredient = lastRandomIngredient;
        int i = 0;
        while (ingredient == lastRandomIngredient && i<100)
        {
            System.Array values = System.Enum.GetValues(typeof(Ingredient));
            ingredient = (Ingredient)values.GetValue(Random.Range(0, values.Length));
            i++;
        }
        lastRandomIngredient = ingredient;
        return ingredient;
    }

    [System.Serializable]
    public class IngredientEvent : UnityEvent<Ingredient> { }
}