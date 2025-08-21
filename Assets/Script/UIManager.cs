using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    private HealthUI _healthUI;


    private void Start()
    {
        _healthUI = FindFirstObjectByType<HealthUI>();
    }

    public void UpdateHealth(int currentHealth)
    {
        Debug.Log($"UIManager: Current Health = {currentHealth}");
        _healthUI.CalculateHearts(currentHealth);
    }
}
