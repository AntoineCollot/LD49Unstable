using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRequestList : MonoBehaviour
{
    public static UIRequestList Instance;
    UIRequestItem[] items;


    [Header("Sprites")]
    public Sprite mushroomSprite = null;
    public Sprite rootSprite = null;
    public Sprite tearSprite = null;
    public Sprite toadSprite = null;
    public Sprite crystalSprite = null;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        items = GetComponentsInChildren<UIRequestItem>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(UIRequestItem item in items)
        {
            item.gameObject.SetActive(item.id < PotionState.Instance.requests.Count);
        }
    }

    public Sprite GetSpriteOfIngredient(Ingredient ingredient)
    {
        switch (ingredient)
        {
            case Ingredient.Mushrooms:
                return mushroomSprite;
            case Ingredient.Exploroots:
                return rootSprite;
            case Ingredient.GodessTears:
                return tearSprite;
            case Ingredient.ToadSaliva:
                return toadSprite;
            case Ingredient.Crystal:
                return crystalSprite;
            default:
                return null;
        }
    }
}
