using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] GameObject _heartContainerPrefab;
    [SerializeField, Tooltip("Sprite order: [0] = Empty, [1] = Half, [2] = Full")] 
    Sprite[] _heartSprites;
    List<Image> _heartContainers = new List<Image>();
    private PlayerInformation _playerInformation;

    private void Start()
    {
        _heartContainers.Clear();
        _heartContainers = GetComponentsInChildren<Image>().ToList();
        _playerInformation = GameObject.FindFirstObjectByType<PlayerInformation>();

        CalculateHearts(_playerInformation.CurrentHealth);
    }

    [ContextMenu("Test Hearts UI")]
    private void TestHearts()
    {
        CalculateHearts(_playerInformation.CurrentHealth);
    }

    public void CalculateHearts(int health)
    {
        // Divide health into full hearts (2 HP) and remainder half-heart (1 HP)
        int fullHearts = health / 2;
        int halfHearts = health % 2;

#if UNITY_EDITOR
        // Debug log for current health values (for testing/verification only)
        Debug.Log($"{health}/{_playerInformation.MaxHealth}");
        Debug.Log($"Full Hearts: {fullHearts}  & Half Hearts: {halfHearts}");
#endif

        // Count how many heart slots are currently displayed in the UI
        int childCount = transform.childCount;
        // Maximum number of heart slots needed (based on max health)
        int maxHeartCount = _playerInformation.MaxHealth / 2;

        // If we don’t have enough heart slots, create new ones until we match max health
        if (childCount != maxHeartCount)
        {
            for (int i = childCount; i < maxHeartCount; i++)
            {
                // Instantiate a new heart container prefab as a child of this transform
                Image go = Instantiate(_heartContainerPrefab, this.transform).GetComponent<Image>();
                _heartContainers.Add(go); // Store reference for updating sprites later
            }
        }

        // Fill in all full hearts with the “full heart” sprite
        for (int i = 0; i < fullHearts; i++)
        {
            _heartContainers[i].sprite = _heartSprites[2];
        }

        if (fullHearts < _heartContainers.Count)
        {
            // If there’s a half heart, place it after the full hearts
            if (halfHearts > 0)
                _heartContainers[fullHearts].sprite = _heartSprites[1];
            else
                _heartContainers[fullHearts].sprite = _heartSprites[0];
        }

        // Set all remaining hearts (after full + half) to empty
        for (int i = fullHearts + 1; i < transform.childCount; i++)
        {
            _heartContainers[i].sprite = _heartSprites[0];
        }
    }
}
