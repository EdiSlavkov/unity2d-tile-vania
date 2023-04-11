using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    [SerializeField] private float lives = 3;
    private float money;
    private ScoreUpdater scoreUpdater;

    private void Start()
    {
        scoreUpdater = FindObjectOfType<ScoreUpdater>();
        scoreUpdater.UpdateLifeText(lives);
        scoreUpdater.UpdateMoneyText(money);
    }

    public void AddMoney(float value)
    {
        money += value;
        scoreUpdater.UpdateMoneyText(money);
    }

    public void TakeLife()
    {
        lives--;
        scoreUpdater.UpdateLifeText(lives);
    }

    public float GetPlayerLifes()
    {
        return lives;
    }
}