using UnityEngine;
public class ShowInfoAboutGame : MonoBehaviour
{
    [SerializeField] private GameObject _infoWindow;
    [SerializeField] private Animator _animationState;
    public void ShowInfo()
    {
        _infoWindow.SetActive(true);
        _animationState.SetBool("IsEnable", true);
    }
    public void CloseInfo()
    {
        _animationState.SetBool("IsEnable", false);
    }
}
