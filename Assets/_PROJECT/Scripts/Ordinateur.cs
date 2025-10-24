using UnityEngine;

public class Ordinateur : MonoBehaviour
{
    [SerializeField] private GameObject _ordinateur1;
    [SerializeField] private GameObject _ordinateur2;

    [SerializeField] private AudioClip _clip1;
    [SerializeField] private AudioClip _clip2;

    private AudioSource _audioSource1;
    private AudioSource _audioSource2;

    private int trigger1;
    private int trigger2;

    private int etatOrdinateur;
    private string prendreOrdinateur = "Vous avez pris l'ordinateur...";
    private string deposerOdinateur = "Vous avez déposé l'ordinateur...";

    void Start()
    {
        _audioSource1 = _ordinateur1.GetComponent<AudioSource>(); // Va chercher le component "Audio Source" dans l'objet "_ordinateur1".
        _audioSource2 = _ordinateur2.GetComponent<AudioSource>(); // Va chercher le component "Audio Source" dans l'objet "_ordinateur2".

        _ordinateur1.SetActive(true); // L'"ordinateur1" est actif.
        _ordinateur2.SetActive(false); // L'"ordinateur2" est innactif.

        etatOrdinateur = 0; // L'ordinateur est dans son 1er état.

        trigger1 = 0; // Initialisation du trigger 1.
        trigger2 = 0; // Initialisation du trigger 2.
    }

    void Update()
    {
        PrendreOrdinateur();
        DeposerOdinateur();
    }

    private void PrendreOrdinateur()
    {
        if (trigger1 != 0) { return; } // Si la valeur du trigger a changée, ignorer cette méthode.

        float distance = Vector3.Distance(_ordinateur1.transform.position, GameManager.instance.positionJ); // Calcul la distance entre l'ordinateur1 et le joueur.

        // Si le joueur est proche de l'ordinateur1.
        if (distance < 5)
        {
            // Si l'ordinateur est dans son 1er état, que les mains sont vides et que le joueur peut intéragir avec l'ordinateur.
            if (etatOrdinateur == 0 && GameManager.instance.mainsVides == true && GameManager.instance.connaitDanger == true)
            {
                trigger1 = 1; // La valeur du trigger change pour que cette méthode soit ignorée après être utilisée.

                GameManager.instance.Afficher(prendreOrdinateur + GameManager.instance.txtMains); // Texte pour informer le joueur.
                Debug.Log(prendreOrdinateur + GameManager.instance.txtMains);

                GameManager.instance.mainsVides = false; // Les mains sont maintenant pleines.
                _audioSource1.PlayOneShot(_clip1); // Joue l'audio du joueur qui prend l'ordinateur.
                _ordinateur1.SetActive(false); // L'ordinateur est déactiver pour qu'il ne soit plus visible au joueur.
                etatOrdinateur = 1; // L'ordinateur est dans sa deuxième état.
            }

            // Si le joueur peut intéragir avec l'ordinateur, mais les mains sont remplis.
            else if (GameManager.instance.mainsVides == false && GameManager.instance.connaitDanger == true)
            {
                GameManager.instance.Afficher(GameManager.instance.txtMains); // Texte qui informe que les mains sont remplis.
                Debug.Log(GameManager.instance.txtMains);
            }
        }
    }

    private void DeposerOdinateur()
    {
        if (trigger2 != 0) { return; } // Si la valeur du trigger a changée, ignorer cette méthode.

        float distance = Vector3.Distance(_ordinateur2.transform.position, GameManager.instance.positionJ); // Calcul la distance entre l'ordinateur2 et le joueur.

        // Si le joueur est proche de l'ordinateur2 et qu'elle est dans son 2ème état.
        if (distance < 5 && etatOrdinateur == 1)
        {
            trigger2 = 1; // La valeur du trigger change pour que cette méthode soit ignorée après être utilisée.

            GameManager.instance.Afficher(deposerOdinateur); // Texte pour informer le joueur.
            Debug.Log(deposerOdinateur);

            GameManager.instance.mainsVides = true; // Les mains sont vides.
            _audioSource2.PlayOneShot(_clip2); // Joue l'audio du joueur qui dépose l'ordinateur.
            _ordinateur2.SetActive(true); // L'ordinateur est activer pour qu'il soit visible au joueur.
            etatOrdinateur = 2; // L'ordinateur est dans son état final.
        }
    }
}
