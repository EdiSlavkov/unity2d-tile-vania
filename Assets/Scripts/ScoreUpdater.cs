using UnityEngine;
using TMPro;

public class ScoreUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lifeText;
    [SerializeField] private TextMeshProUGUI moneyText;

    public void UpdateMoneyText(float money)
    {
        moneyText.text = $"Money: {money}";
    }

    public void UpdateLifeText(float lives)
    {
        lifeText.text = $"Lives: {lives}";
    }
}
