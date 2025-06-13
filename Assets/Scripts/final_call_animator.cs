using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

/// <summary>
/// Anime l'apparition des informations finales du club
/// Crée un appel à l'action motivant et informatif pour la séquence 4
/// </summary>
public class FinalCallAnimator : MonoBehaviour
{
    [Header("Éléments UI à animer")]
    public TextMeshProUGUI clubNameText;
    public TextMeshProUGUI historyText;
    public TextMeshProUGUI olympicText;
    public TextMeshProUGUI championsText;
    public TextMeshProUGUI locationText;
    public TextMeshProUGUI ageText;
    public TextMeshProUGUI callToActionText;
    
    [Header("Configuration d'animation")]
    public float delayBetweenElements = 0.3f;
    public float fadeInDuration = 0.8f;
    public float scaleAnimationDuration = 0.5f;
    public AnimationCurve scaleAnimation = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    
    [Header("Effets visuels")]
    public ParticleSystem celebrationParticles;
    public bool usePulseEffect = true;
    public Color highlightColor = Color.yellow;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip appearanceSound;
    public AudioClip finalCallSound;
    
    // Référence à tous les textes pour animation de groupe
    private TextMeshProUGUI[] allTexts;
    private Vector3[] originalScales;
    private Color[] originalColors;
    private bool animationStarted = false;
    
    void Start()
    {
        InitializeFinalCall();
    }
    
    /// <summary>
    /// Initialise tous les éléments pour l'animation finale
    /// </summary>
    void InitializeFinalCall()
    {
        // Collecte tous les textes dans un tableau pour faciliter l'animation
        allTexts = new TextMeshProUGUI[]
        {
            clubNameText, historyText, olympicText, championsText,
            locationText, ageText, callToActionText
        };
        
        // Sauvegarde les propriétés originales
        originalScales = new Vector3[allTexts.Length];
        originalColors = new Color[allTexts.Length];
        
        for (int i = 0; i < allTexts.Length; i++)
        {
            if (allTexts[i] != null)
            {
                originalScales[i] = allTexts[i].transform.localScale;
                originalColors[i] = allTexts[i].color;
                
                // Commence invisible
                allTexts[i].transform.localScale = Vector3.zero;
                Color transparentColor = originalColors[i];
                transparentColor.a = 0f;
                allTexts[i].color = transparentColor;
            }
        }
        
        // Configure le contenu des textes avec les vraies informations du club
        SetupClubInformation();
        
        Debug.Log("FinalCallAnimator initialisé - Prêt pour l'animation finale !");
    }
    
    /// <summary>
    /// Configure le contenu textuel avec les informations du club
    /// Basé sur le dossier de sponsoring de l'ATE
    /// </summary>
    void SetupClubInformation()
    {
        if (clubNameText != null)
            clubNameText.text = "🤺 AMICALE TARBAISE D'ESCRIME";
            
        if (historyText != null)
            historyText.text = "Plus de 100 ans de passion";
            
        if (olympicText != null)
            olympicText.text = "🥇 8 médailles olympiques";
            
        if (championsText != null)
            championsText.text = "🏆 200 champions de France";
            
        if (locationText != null)
            locationText.text = "🏠 Maison de l'Escrime - Tarbes";
            
        if (ageText != null)
            ageText.text = "👶 Dès 4 ans - Tous niveaux";
            
        if (callToActionText != null)
            callToActionText.text = "✨ TON AVENTURE COMMENCE ICI ! ✨";
    }
    
    /// <summary>
    /// Démarre l'animation complète de l'appel final
    /// Appelée par Timeline ou manuellement
    /// </summary>
    [ContextMenu("Démarrer l'appel final")]
    public void StartFinalCall()
    {
        if (animationStarted)
        {
            Debug.LogWarning("L'animation de l'appel final a déjà commencé !");
            return;
        }
        
        animationStarted = true;
        StartCoroutine(AnimateFinalCallSequence());
        
        Debug.Log("Animation de l'appel final démarrée !");
    }
    
    /// <summary>
    /// Séquence d'animation complète de l'appel final
    /// Chaque élément apparaît progressivement avec des effets
    /// </summary>
    IEnumerator AnimateFinalCallSequence()
    {
        // Séquence 1 : Nom du club (impactant)
        yield return StartCoroutine(AnimateTextElement(0, true, true)); // clubNameText
        yield return new WaitForSeconds(delayBetweenElements);
        
        // Séquence 2 : Histoire (crédibilité)
        yield return StartCoroutine(AnimateTextElement(1, false, false)); // historyText
        yield return new WaitForSeconds(delayBetweenElements);
        
        // Séquence 3 : Médailles olympiques (prestige)
        yield return StartCoroutine(AnimateTextElement(2, true, true)); // olympicText
        yield return new WaitForSeconds(delayBetweenElements);
        
        // Séquence 4 : Champions de France (ampleur)
        yield return StartCoroutine(AnimateTextElement(3, true, true)); // championsText
        yield return new WaitForSeconds(delayBetweenElements);
        
        // Séquence 5 : Localisation (pratique)
        yield return StartCoroutine(AnimateTextElement(4, false, false)); // locationText
        yield return new WaitForSeconds(delayBetweenElements);
        
        // Séquence 6 : Âge et accessibilité (inclusion)
        yield return StartCoroutine(AnimateTextElement(5, false, false)); // ageText
        yield return new WaitForSeconds(delayBetweenElements * 2); // Pause plus longue avant l'appel final
        
        // Séquence 7 : APPEL FINAL (climax émotionnel)
        yield return StartCoroutine(AnimateTextElement(6, true, true)); // callToActionText
        
        // Explosion finale de célébration
        TriggerFinalCelebration();
    }
    
    /// <summary>
    /// Anime l'apparition d'un élément textuel spécifique
    /// </summary>
    IEnumerator AnimateTextElement(int textIndex, bool useHighlight, bool playSound)
    {
        if (textIndex < 0 || textIndex >= allTexts.Length || allTexts[textIndex] == null)
            yield break;
        
        TextMeshProUGUI targetText = allTexts[textIndex];
        
        // Son d'apparition
        if (playSound && audioSource != null && appearanceSound != null)
        {
            audioSource.PlayOneShot(appearanceSound);
        }
        
        // Animation simultanée de scale et fade
        float elapsedTime = 0f;
        
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / fadeInDuration;
            
            // Animation du scale avec courbe personnalisée
            float scaleProgress = scaleAnimation.Evaluate(progress);
            targetText.transform.localScale = originalScales[textIndex] * scaleProgress;
            
            // Animation du fade
            Color currentColor = originalColors[textIndex];
            currentColor.a = progress;
            targetText.color = currentColor;
            
            yield return null;
        }
        
        // Finalise l'animation
        targetText.transform.localScale = originalScales[textIndex];
        targetText.color = originalColors[textIndex];
        
        // Effet de highlight pour les éléments importants
        if (useHighlight)
        {
            yield return StartCoroutine(HighlightElement(targetText));
        }
    }
    
    /// <summary>
    /// Effet de highlight pour mettre en valeur un élément important
    /// </summary>
    IEnumerator HighlightElement(TextMeshProUGUI text)
    {
        Color originalColor = text.color;
        float highlightDuration = 0.8f;
        float elapsedTime = 0f;
        
        while (elapsedTime < highlightDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / highlightDuration;
            
            // Oscillation entre couleur originale et couleur highlight
            float highlightAmount = Mathf.Sin(progress * Mathf.PI * 3) * 0.5f + 0.5f;
            text.color = Color.Lerp(originalColor, highlightColor, highlightAmount * 0.3f);
            
            // Légère pulsation de scale
            float scaleMultiplier = 1f + Mathf.Sin(progress * Mathf.PI * 3) * 0.05f;
            text.transform.localScale = originalScales[System.Array.IndexOf(allTexts, text)] * scaleMultiplier;
            
            yield return null;
        }
        
        // Retour à l'état normal
        text.color = originalColor;
        text.transform.localScale = originalScales[System.Array.IndexOf(allTexts, text)];
    }
    
    /// <summary>
    /// Déclenche la célébration finale avec particules et son
    /// </summary>
    void TriggerFinalCelebration()
    {
        // Explosion de particules de célébration
        if (celebrationParticles != null)
        {
            celebrationParticles.Play();
            
            // Configuration dynamique des particules pour l'apothéose
            var main = celebrationParticles.main;
            main.startColor = new Color(1f, 0.8f, 0f, 1f); // Couleur dorée personnalisée
            main.maxParticles = 200;
            
            var emission = celebrationParticles.emission;
            emission.rateOverTime = 0f;
            emission.SetBursts(new ParticleSystem.Burst[]
            {
                new ParticleSystem.Burst(0.0f, 100), // Explosion immédiate
                new ParticleSystem.Burst(0.5f, 50),  // Seconde vague
                new ParticleSystem.Burst(1.0f, 30)   // Finale
            });
        }
        
        // Son de célébration finale
        if (audioSource != null && finalCallSound != null)
        {
            audioSource.PlayOneShot(finalCallSound);
        }
        
        // Pulsation finale du texte d'appel à l'action
        if (callToActionText != null && usePulseEffect)
        {
            StartCoroutine(PulseFinalCall());
        }
        
        Debug.Log("🎉 Célébration finale déclenchée ! L'aventure commence ! 🎉");
    }
    
    /// <summary>
    /// Effet de pulsation continue pour l'appel à l'action final
    /// </summary>
    IEnumerator PulseFinalCall()
    {
        float pulseDuration = 3f; // Pulse pendant 3 secondes
        float elapsedTime = 0f;
        Vector3 baseScale = originalScales[allTexts.Length - 1]; // callToActionText est le dernier
        
        while (elapsedTime < pulseDuration)
        {
            elapsedTime += Time.deltaTime;
            
            // Pulsation douce et continue
            float pulseMultiplier = 1f + Mathf.Sin(elapsedTime * 4f) * 0.1f;
            callToActionText.transform.localScale = baseScale * pulseMultiplier;
            
            yield return null;
        }
        
        // Retour à la taille normale
        callToActionText.transform.localScale = baseScale;
    }
    
    /// <summary>
    /// Réinitialise l'animation pour pouvoir la relancer
    /// </summary>
    [ContextMenu("Réinitialiser l'appel final")]
    public void ResetFinalCall()
    {
        StopAllCoroutines();
        animationStarted = false;
        
        // Remet tous les textes dans leur état initial
        for (int i = 0; i < allTexts.Length; i++)
        {
            if (allTexts[i] != null)
            {
                allTexts[i].transform.localScale = Vector3.zero;
                Color transparentColor = originalColors[i];
                transparentColor.a = 0f;
                allTexts[i].color = transparentColor;
            }
        }
        
        // Arrête les particules
        if (celebrationParticles != null)
        {
            celebrationParticles.Stop();
            celebrationParticles.Clear();
        }
        
        Debug.Log("Appel final réinitialisé et prêt à être relancé.");
    }
    
    /// <summary>
    /// Interface de debug pour tester l'animation
    /// </summary>
    void OnGUI()
    {
        if (Application.isPlaying)
        {
            GUI.Box(new Rect(10, 500, 300, 100), "Contrôles Appel Final");
            
            GUI.Label(new Rect(20, 525, 280, 20), $"État: {(animationStarted ? "EN COURS" : "PRÊT")}");
            
            if (!animationStarted && GUI.Button(new Rect(20, 550, 120, 25), "Démarrer"))
            {
                StartFinalCall();
            }
            
            if (GUI.Button(new Rect(150, 550, 120, 25), "Réinitialiser"))
            {
                ResetFinalCall();
            }
        }
    }
}