using UnityEngine;
using TMPro;

public class AnimatedMedalCounter : MonoBehaviour
{
    public TextMeshProUGUI counterText;

    [Header("Valeur anim�e par Timeline")]
    [Range(0, 8)]
    public int animatedMedalCount = 0;

    void Update()
    {
        // Conversion automatique de la valeur num�rique en texte
        string plural = animatedMedalCount > 1 ? "s" : "";
        string newText = $"{animatedMedalCount} m�daille{plural}";
        //string newText = $"{animatedMedalCount} m�daille{plural} olympique{plural}";

        // Ne met � jour que si le texte a chang� (optimisation)
        if (counterText.text != newText)
        {
            counterText.text = newText;
        }
    }
}