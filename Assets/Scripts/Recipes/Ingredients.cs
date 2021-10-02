using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Ingredient { Mushrooms, Exploroots, GodessTears, ToadSaliva, Crystal}

public static class Ingredients
{
    public static Ingredient GetRandomIngredient()
    {
        System.Array values = System.Enum.GetValues(typeof(Ingredient));
        return (Ingredient)values.GetValue(Random.Range(0, values.Length));
    }

    public class IngredientEvent : UnityEvent<Ingredient> { }
}