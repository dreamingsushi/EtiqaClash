using UnityEngine;
using UnityEngine.UI;

public class CardDescription : MonoBehaviour
{
    public Image image;
    public static CardDescription Instance;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        image = GetComponent<Image>();
    }
}
