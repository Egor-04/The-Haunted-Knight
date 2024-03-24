using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void OpenPanel(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void OpenURl(string url)
    {
        Application.OpenURL(url);
    }
}
