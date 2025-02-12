using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ElixirBar : MonoBehaviour
{
    public float curElixir;
    public float maxElixir = 10;
    public float elixirGainRate = 1f;
    
    private Image elixirDisplay;

    void Start()
    {
        elixirDisplay = GetComponent<Image>();
        StartCoroutine(GainElixir());
    }

    IEnumerator GainElixir()
    {
        while (true)
        {
            yield return null; // Wait for next frame
            if (curElixir < maxElixir)
            {
                curElixir += elixirGainRate * Time.deltaTime;
                curElixir = Mathf.Min(curElixir, maxElixir); // Clamping
            }
            UpdateElixirUI();
        }
    }

    void UpdateElixirUI()
    {
        if (elixirDisplay != null)
        {
            elixirDisplay.fillAmount = curElixir / maxElixir; // Normalize to 0-1
        }
    }
}
