using UnityEngine;
using UnityEngine.UI;

public class ApplicationQuit : MonoBehaviour
{

    [SerializeField] private Button quitButton;

    private void Awake()
    {
        quitButton.onClick.AddListener(Quit);
    }

    private void Quit() => Application.Quit();
}
