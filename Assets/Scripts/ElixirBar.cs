using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using ExitGames.Client.Photon.StructWrapping;

public class ElixirBar : MonoBehaviour
{
    public float curElixir;
    public float maxElixir = 10;
    public float elixirGainRate = 1f;
    public GameObject honeyDripVFX; 
    private Image elixirDisplay;
    public TextMeshProUGUI elixirCount;

    private int lastElixirInt;

    void Start()
    {
        elixirDisplay = GetComponent<Image>();
        StartCoroutine(GainElixir());
    }

    IEnumerator GainElixir()
    {
        while (true)
        {
            yield return null;
            if (curElixir < maxElixir)
            {
                curElixir += elixirGainRate * Time.deltaTime;
                curElixir = Mathf.Min(curElixir, maxElixir);
            }

            int currentElixirInt = Mathf.FloorToInt(curElixir);
            if (currentElixirInt > lastElixirInt) // Detects only when a new int is reached
            {
                lastElixirInt = currentElixirInt;
                OnElixirIntReached(currentElixirInt);
            }

            if (curElixir >= maxElixir)
            {
                honeyDripVFX.SetActive(true);
            }
            else
            {
                honeyDripVFX.SetActive(false);
            }
            UpdateElixirUI();
        }
    }

    void UpdateElixirUI()
    {
        if (elixirDisplay != null)
        {
            elixirDisplay.fillAmount = curElixir / maxElixir;
        }

        if (elixirCount != null)
        {
            elixirCount.text = Mathf.FloorToInt(curElixir).ToString();
        }
    }

    void OnElixirIntReached(int elixirInt)
    {
        AudioManager.Instance.PlaySFX("Elixir");
    }
}
