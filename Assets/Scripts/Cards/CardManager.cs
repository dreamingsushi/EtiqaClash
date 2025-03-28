using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [SerializeField] private Transform cardDeckHolder;
    [SerializeField] private Image nextCardHolder;
    [SerializeField] private GameObject[] cardPrefabs = new GameObject[5]; // Store prefab references
    private GameObject[] cardInstances; // Store instantiated cards
    [SerializeField] private float assignDelay = 0.5f;
    private int assignedCardCount = 0;
    public static CardManager Instance;
    private Card selectedCard;
    private GameObject nextCardInstance;
    private int nextCardIndex = 3;
    public Sprite[] cardImages;

    void Awake()
    {
        Instance = this;
    }
    public void Start()
    {
        CreateCards();
        ShuffleCards();

        ShowNextCard();
        StartCoroutine(AssignCardsToDeck(cardDeckHolder, 5));
    }

    private void CreateCards()
    {
        cardInstances = new GameObject[cardPrefabs.Length];

        for (int i = 0; i < cardPrefabs.Length; i++)
        {
            cardInstances[i] = Instantiate(cardPrefabs[i], transform.position, Quaternion.identity);
            cardInstances[i].SetActive(false);
        }
    }

    public IEnumerator AssignCardsToDeck(Transform parent, int count)
    {
        for (int i = 0; i < count; i++)
        {
            int index = assignedCardCount % cardInstances.Length; 
            cardInstances[index].SetActive(true);
            cardInstances[index].transform.SetParent(parent, false);
            cardInstances[index].GetComponent<Animator>().Play("JumpIn");

            assignedCardCount++;
            yield return new WaitForSeconds(assignDelay);
        }
    }

    private void ShuffleCards()
    {
        for (int i = cardInstances.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (cardInstances[i], cardInstances[randomIndex]) = (cardInstances[randomIndex], cardInstances[i]);

            (cardImages[i], cardImages[randomIndex]) = (cardImages[randomIndex], cardImages[i]);
        }
    }

    public void SelectCard(Card newCard)
    {
        if (selectedCard == newCard)
    {
        if (selectedCard != null) 
        {
            selectedCard.Deselect(); // Only deselect if it's not null
        }
        selectedCard = null;
        return;
    }

    if (selectedCard != null)
    {
        selectedCard.Deselect();
    }

    if (newCard != null) // Ensure we don't call Select() on null
    {
        selectedCard = newCard;
        selectedCard.Select();
    }
    else
    {
        selectedCard = null;
    }
    }

    public void SpawnNewCard(Card.CardType cardType)
    {
        foreach (GameObject cardPrefab in cardPrefabs)
        {
            Card cardComponent = cardPrefab.GetComponent<Card>();
            if (cardComponent != null && cardComponent.currentCard == cardType)
            {
                GameObject newCard = Instantiate(cardPrefab, cardDeckHolder);
                newCard.GetComponent<Animator>().Play("JumpIn");
                return;
            }
        }
    }

    public void ShowNextCard()
    {
        Debug.Log(nextCardIndex);
        nextCardHolder.sprite = cardImages[nextCardIndex];
    }

    void UseCard(int usedCardIndex)
    {
        if (usedCardIndex < 0 || usedCardIndex >= 3) return; // Only shift within the hand

        GameObject temp = cardInstances[usedCardIndex]; // Store the used card
        Sprite tempSprite = cardImages[usedCardIndex]; // Store the corresponding sprite

        // Shift all cards and their images left within the hand
        for (int i = usedCardIndex; i < 2; i++) // Stops at index 2
        {
            cardInstances[i] = cardInstances[i + 1];
            cardImages[i] = cardImages[i + 1];
        }

        // Move the next card (index 3) to last hand slot (index 2)
        cardInstances[2] = cardInstances[3];
        cardImages[2] = cardImages[3];

        // Move the stored used card to the last deck slot (index 4)
        cardInstances[3] = cardInstances[4];
        cardImages[3] = cardImages[4];

        // Place the used card in the deck's last slot
        cardInstances[4] = temp;
        cardImages[4] = tempSprite;

        // Update nextCardIndex (always index 3)
        nextCardIndex = 3;

        // Debugging output
        Debug.Log("New next card image: " + cardImages[nextCardIndex].name);
        }

    public void DestroySelectedCard()
    {
        if (selectedCard != null)
        {
            int cardIndex = System.Array.IndexOf(cardInstances, selectedCard.gameObject);

            Card.CardType cardType = selectedCard.currentCard;

            Destroy(selectedCard.gameObject);
            selectedCard = null;

            UseCard(cardIndex);
            SpawnNewCard(cardType);
            ShowNextCard();
        }
    }
}

