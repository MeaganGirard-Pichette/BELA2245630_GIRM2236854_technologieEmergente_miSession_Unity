// ...existing code...
using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class CutsceneControllerWithFade : MonoBehaviour
{
    [Header("Références principales")]
    public PlayableDirector cutsceneDirector;
    public GameObject vrCamera;
    public GameObject cutsceneCamera;

    [Header("Fade")]
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1.0f;

    [Header("Contrôle du collider")]
    [Tooltip("Collider précis du joueur qui doit entrer dans le trigger. Laisser vide pour accepter n'importe quel collider portant le tag 'Player'.")]
    public Collider requiredPlayerCollider;

    private bool playerInTrigger = false;

    private void Start()
    {
        if (cutsceneDirector.playOnAwake)
            cutsceneDirector.playOnAwake = false;

        if (cutsceneCamera != null)
            cutsceneCamera.SetActive(false);

        if (fadeCanvasGroup != null)
            fadeCanvasGroup.gameObject.SetActive(false);

        cutsceneDirector.stopped += OnCutsceneFinished;
    }

    /// <summary>
    /// Appelé depuis la Timeline principale (via un Signal)
    /// </summary>
    public void TryPlayCutscene()
    {
        if (!playerInTrigger)
        {
            Debug.Log("Signal reçu → joueur pas dans trigger → lancement de la cutscene.");
            StartCoroutine(PlayCutsceneWithFade());
        }
        else
        {
            Debug.Log("Signal reçu → joueur déjà dans trigger → cutscene ignorée.");
        }
    }

    private IEnumerator PlayCutsceneWithFade()
    {
        yield return StartCoroutine(FadeOut());

        // Bascule des caméras
        vrCamera.SetActive(false);
        cutsceneCamera.SetActive(true);

        yield return StartCoroutine(FadeIn());

        // Lancer la Timeline
        cutsceneDirector.Play();
    }

    private void OnCutsceneFinished(PlayableDirector director)
    {
        StartCoroutine(ReturnToVRWithFade());
    }

    private IEnumerator ReturnToVRWithFade()
    {
        yield return StartCoroutine(FadeOut());

        // Revenir à la caméra VR
        cutsceneCamera.SetActive(false);
        vrCamera.SetActive(true);

        yield return StartCoroutine(FadeIn());
    }

    private IEnumerator FadeOut()
    {
        fadeCanvasGroup.gameObject.SetActive(true);
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = 1;
    }

    private IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = 0;
        fadeCanvasGroup.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Vérifie d'abord le tag Player
        if (!other.CompareTag("Player"))
            return;

        // Si un collider précis est défini, exige que ce soit celui-ci
        if (requiredPlayerCollider != null && other != requiredPlayerCollider)
            return;

        playerInTrigger = true;
        Debug.Log("Le joueur (collider attendu) est entré dans le trigger.");
    }

    private void OnTriggerExit(Collider other)
    {
        // Vérifie d'abord le tag Player
        if (!other.CompareTag("Player"))
            return;

        // Si un collider précis est défini, n'annule la présence que si c'est celui-ci
        if (requiredPlayerCollider != null && other != requiredPlayerCollider)
            return;

        playerInTrigger = false;
        Debug.Log("Le joueur (collider attendu) a quitté le trigger.");
    }
}
// ...existing code...