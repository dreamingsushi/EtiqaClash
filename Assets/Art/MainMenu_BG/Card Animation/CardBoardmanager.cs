using UnityEngine;

public class CardBoardmanager : MonoBehaviour
{
    public Animator CardBanim;

    public GameObject BC1;
    public GameObject BC2;
    public GameObject BC3;
    public GameObject BC4;
    public GameObject BC5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowCardBoard()
    {
        AudioManager.Instance.PlaySFX("MenuButton");
        CardBanim.SetTrigger("Open");
    }
    public void HideCardBoard()
    {
        AudioManager.Instance.PlaySFX("MenuButton");
        CardBanim.SetTrigger("Close");
    }


    public void ShowBC1()
    {
        AudioManager.Instance.PlaySFX("MenuButton");
        BC1.SetActive(true);
    }
    public void HideBC1()
    {
        BC1.SetActive(false);
    }

    public void ShowBC2()
    {
        AudioManager.Instance.PlaySFX("MenuButton");
        BC2.SetActive(true);
    }
    public void HideBC2()
    {
        BC2.SetActive(false);
    }

    public void ShowBC3()
    {
        AudioManager.Instance.PlaySFX("MenuButton");
        BC3.SetActive(true);
    }
    public void HideBC3()
    {
        BC3.SetActive(false);
    }

    public void ShowBC4()
    {
        AudioManager.Instance.PlaySFX("MenuButton");
        BC4.SetActive(true);
    }
    public void HideBC4()
    {
        BC4.SetActive(false);
    }

    public void ShowBC5()
    {
        AudioManager.Instance.PlaySFX("MenuButton");
        BC5.SetActive(true);
    }
    public void HideBC5()
    {
        BC5.SetActive(false);
    }

}
