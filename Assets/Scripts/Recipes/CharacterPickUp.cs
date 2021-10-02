using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPickUp : MonoBehaviour
{
    public float pickUpRadius = 0.5f;
    public LayerMask pickUpLayers = 0;

    PickableIngredient currentSelectedIngredient;
    [Header("Mushrooms")]
    public float bounceForce = 10;

    public Ingredients.IngredientEvent onIngredientPickUp = new Ingredients.IngredientEvent();


    public static CharacterPickUp Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        CastCollisions();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PickUp();
        }
    }

    void PickUp()
    {
        if (currentSelectedIngredient == null)
            return;

        Ingredient ingredient = currentSelectedIngredient.PlayerPickUp();
        print(ingredient);

        onIngredientPickUp.Invoke(ingredient);

        switch (ingredient)
        {
            case Ingredient.Mushrooms:
                MovementController.Instance.Jump(bounceForce);
                break;
            case Ingredient.Exploroots:
                break;
            case Ingredient.GodessTears:
                break;
            case Ingredient.ToadSaliva:
                break;
            case Ingredient.Crystal:
                break;
            default:
                break;
        }
    }

    void CastCollisions()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, pickUpRadius, pickUpLayers);

        foreach (Collider hitCol in hitColliders)
        {
            PickableIngredient hitIngrediant = hitCol.GetComponent<PickableIngredient>();
            if (hitIngrediant == null)
                continue;

            if (hitIngrediant == currentSelectedIngredient)
                continue;

            if (currentSelectedIngredient != null)
                currentSelectedIngredient.Highlight(false);

            currentSelectedIngredient = hitIngrediant;
            hitIngrediant.Highlight(true);

            return;
        }

        if (hitColliders.Length == 0)
        {
            if (currentSelectedIngredient != null)
                currentSelectedIngredient.Highlight(false);
            currentSelectedIngredient = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, pickUpRadius);
    }
}
