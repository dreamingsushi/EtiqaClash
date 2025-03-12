
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TutorialCards : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum CardType{
        Power, Speed, Power2, Speed2, Mix
    }
    public CardType currentCard;
    public bool isSelected;
    private Animator animator;
    private TutorialManager gameManager;
    private TutorialCardManager cardManager;

    private float touchStartTime;
    private bool isHolding;
    public float holdThreshold = 1f;
    public Sprite powerDescription;
    public Sprite speedDescription;
    public Sprite power2Description;
    public Sprite speed2Description;
    public Sprite mixDescription;
    public CardDescription description;
    void Start()
    {
        gameManager = FindAnyObjectByType<TutorialManager>();
        animator = GetComponent<Animator>();
        cardManager = TutorialCardManager.Instance;
    }
    void Update()
    {
        if (isHolding && Time.time - touchStartTime >= holdThreshold)
        {
            HoldOnCard();
        }

        if(Input.GetMouseButtonDown(0))
        {
            HideDescriptions();

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider == null || hit.collider.GetComponent<Card>() == null)
            {
                cardManager.SelectCard(null);
            }
        }

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        touchStartTime = Time.time;
        isHolding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;

        if (Time.time - touchStartTime < holdThreshold) // If it's a tap
        {
            TapOnCard();
        }

    }


    public void Select()
    {
        isSelected = true;
        animator.Play("Selected");
    }

    public void Deselect()
    {
        isSelected = false;
        animator.Play("Idle");
    }

    private void TapOnCard()
    {
        if (isSelected) return;

        cardManager.SelectCard(this);

            switch (currentCard)
            {
                case CardType.Power:
                    gameManager.powerupValue1 = 1;
                    gameManager.ApplyingPower();
                    break;

                case CardType.Speed:
                    gameManager.speedupValue1 = 1;
                    gameManager.ApplyingSpeed();
                    break;

                case CardType.Speed2:
                    gameManager.speedupValue1 = 2;
                    gameManager.ApplyingSpeed2();
                    break;

                case CardType.Power2:
                    gameManager.powerupValue1 = 2;
                    gameManager.ApplyingPower2();
                    break;

                case CardType.Mix:
                    gameManager.speedupValue1 = 1;
                    gameManager.powerupValue1 = 1;
                    gameManager.ApplyingMix();
                    break;
            }
    }
    

    private void HoldOnCard()
    {
        if (!isSelected) // Only select if it's not already selected
        {
            cardManager.SelectCard(this);
        }
        switch (currentCard)
            {
                case CardType.Power:
                    CardDescription.Instance.image.sprite = powerDescription;
                    break;

                case CardType.Speed:
                    CardDescription.Instance.image.sprite = speedDescription;
                    break;

                case CardType.Speed2:
                    CardDescription.Instance.image.sprite = speed2Description;
                    break;

                case CardType.Power2:
                    CardDescription.Instance.image.sprite = power2Description;
                    break;  

                case CardType.Mix:
                    CardDescription.Instance.image.sprite = mixDescription;
                    break;
            }
            
        CardDescription.Instance.image.enabled = true;
    }

    private void HideDescriptions()
    {
        CardDescription.Instance.image.enabled = false;
    }

}
