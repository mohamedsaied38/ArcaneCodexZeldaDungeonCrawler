using UnityEngine;

public class HeartPiece : MonoBehaviour, ICollidable
{
    InventoryManager _invManager;
    PlayerInformation _playerInfo;
    [SerializeField] int _heartPieceID;
    [SerializeField] int _numOfPiecesForFullHeart = 4;
    [SerializeField] int _healthIncreaseAmount = 2;


    private void Start()
    {
        _invManager = FindAnyObjectByType<InventoryManager>();
        _playerInfo = FindFirstObjectByType<PlayerInformation>();

        if (_invManager.InventoryAmount(_heartPieceID) == -1)
            Debug.LogWarning("This Id does not exist in the Inventory Master List", this.gameObject);
    }

    public void OnCollide(Transform other)
    {
        Debug.Log("Collided with Something");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Collided with Player");
            if (_invManager != null)
            {
                _invManager.AddToInventory(_heartPieceID, 1);
                if (_invManager.InventoryAmount(_heartPieceID) >= _numOfPiecesForFullHeart)
                    HeartMerge();
                else
                    Debug.Log($"Heart Pieces Collected = {_invManager.InventoryAmount(_heartPieceID)}");

                if (transform.parent != null)
                    Destroy(transform.parent);
                Destroy(this.gameObject);
            }
        }
    }

    private void HeartMerge()
    {
        _invManager.RemoveFromInventory(_heartPieceID, _numOfPiecesForFullHeart);
        _playerInfo?.IncreaseMaxHealth(_healthIncreaseAmount, true);
    }
}
