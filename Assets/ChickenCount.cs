using UnityEngine;
using TMPro;

public class ChickenCount : MonoBehaviour
{
    TMP_Text text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GetComponent<TMP_Text>();
        text.SetText(AiAndChicken.chickensEscaped.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        text.SetText(AiAndChicken.chickensEscaped.ToString());
    }
}
