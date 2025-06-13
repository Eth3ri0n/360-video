using UnityEngine;
using TMPro;

public class AnimatedMedalCounter : MonoBehaviour
{
    public TextMeshProUGUI counterText;

    [Header("Valeur animée par Timeline")]
    [Range(0, 8)]
    public int animatedMedalCount = 0;

    void Update()
    {
        // Conversion automatique de la valeur numérique en texte
        string plural = animatedMedalCount > 1 ? "s" : "";
        string newText = $"{animatedMedalCount} médaille{plural}";
        //string newText = $"{animatedMedalCount} médaille{plural} olympique{plural}";

        // Ne met à jour que si le texte a changé (optimisation)
        if (counterText.text != newText)
        {
            counterText.text = newText;
        }
    }
}