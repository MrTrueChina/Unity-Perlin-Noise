using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FPSDisplay : MonoBehaviour
{
    Text _displayText;

    private void Awake()
    {
        _displayText = GetComponent<Text>();
    }

    private void Update()
    {
        _displayText.text = "FPS：" + (1 / Time.deltaTime);
    }
}
