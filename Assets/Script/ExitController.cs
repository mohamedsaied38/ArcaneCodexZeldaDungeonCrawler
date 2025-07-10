using UnityEngine;

public class ExitController : MonoBehaviour
{
    [SerializeField] private Transform _wallTransform;
    [SerializeField] private Transform _doorTransform;

    [SerializeField] private bool _isExit = false;

    public void SetExit(bool isOpen)
    {
        _isExit = isOpen;
        CheckStatus();
    }

    [ContextMenu("Check Status")]
    private void CheckStatus()
    {
        //if (_isExit)
        //{
        //    _doorTransform.gameObject.SetActive(true);
        //    _wallTransform.gameObject.SetActive(false);
        //}
        //else
        //{
        //    _doorTransform.gameObject.SetActive(false);
        //    _wallTransform.gameObject.SetActive(true);
        //}

        _doorTransform.gameObject.SetActive(_isExit);
        _wallTransform.gameObject.SetActive(!_isExit);
    }
}
