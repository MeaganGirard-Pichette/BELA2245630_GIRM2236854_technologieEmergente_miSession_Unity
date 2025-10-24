using UnityEngine;

public class Aquarium : MonoBehaviour
{
    [SerializeField] private GameObject _aquarium1;
    [SerializeField] private GameObject _aquarium2;

    [SerializeField] private AudioClip _clip1;
    [SerializeField] private AudioClip _clip2;

    private AudioSource _audioSource1;
    private AudioSource _audioSource2;

    private int trigger1;
    private int trigger2;

    private int etatAquarium;
    private string prendreAquarium = "Vous avez pris l'aquarium...";
    private string deposerAquarium = "Vous avez déposé l'aquarium...";

    void Start()
    {
        _audioSource1 = _aquarium1.GetComponent<AudioSource>(); // Va chercher le component "Audio Source" dans l'objet "_aquarium1".
        _audioSource2 = _aquarium2.GetComponent<AudioSource>(); // Va chercher le component "Audio Source" dans l'objet "_aquarium2".

        _aquarium1.SetActive(true); // L'"aquarium1" est actif.
        _aquarium2.SetActive(false); // L'"aquarium2" est innactif.

        etatAquarium = 0; // L'aquarium est dans son 1er état.

        trigger1 = 0; // Initialisation du trigger 1.
        trigger2 = 0; // Initialisation du trigger 2.
    }

    void Update()
    {
        PrendreAquarium();
        DeposerAquarium();
    }

    private void PrendreAquarium()
    {
        if (trigger1 != 0) { return; } // Si la valeur du trigger a changée, ignorer cette méthode.

        float distance = Vector3.Distance(_aquarium1.transform.position, GameManager.instance.positionJ); // Calcul la distance entre l'aquarium1 et le joueur.

        // Si le joueur est proche de l'aquarium1.
        if (distance < 5)
        {
            // Si l'aquarium est dans son 1er état, que les mains sont vides et que le joueur peut intéragir avec l'aquarium.
            if (etatAquarium == 0 && GameManager.instance.mainsVides == true && GameManager.instance.connaitDanger == true)
            {
                trigger1 = 1; // La valeur du trigger change pour que cette méthode soit ignorée après être utilisée.

                GameManager.instance.Afficher(prendreAquarium + GameManager.instance.txtMains); // Texte pour informer le joueur.
                Debug.Log(prendreAquarium + GameManager.instance.txtMains);

                GameManager.instance.mainsVides = false; // Les mains sont maintenant pleines.
                _audioSource1.PlayOneShot(_clip1); // Joue l'audio du joueur qui prend l'aquarium.
                _aquarium1.SetActive(false); // L'aquarium est déactiver pour qu'il ne soit plus visible au joueur.
                etatAquarium = 1; // L'aquarium est dans sa deuxième état.
            }

            // Si le joueur peut intéragir avec l'aquarium, mais les mains sont remplis.
            else if (GameManager.instance.mainsVides == false && GameManager.instance.connaitDanger == true)
            {
                GameManager.instance.Afficher(GameManager.instance.txtMains); // Texte qui informe que les mains sont remplis.
                Debug.Log(GameManager.instance.txtMains);
            }
        }
    }

    private void DeposerAquarium()
    {
        if (trigger2 != 0) { return; } // Si la valeur du trigger a changée, ignorer cette méthode.

        float distance = Vector3.Distance(_aquarium2.transform.position, GameManager.instance.positionJ); // Calcul la distance entre l'aquarium2 et le joueur.

        // Si le joueur est proche de l'aquarium2 et qu'elle est dans son 2ème état.
        if (distance < 5 && etatAquarium == 1)
        {
            trigger2 = 1; // La valeur du trigger change pour que cette méthode soit ignorée après être utilisée.

            GameManager.instance.Afficher(deposerAquarium); // Texte pour informer le joueur.
            Debug.Log(deposerAquarium);

            GameManager.instance.mainsVides = true; // Les mains sont vides.
            _audioSource2.PlayOneShot(_clip2); // Joue l'audio du joueur qui dépose l'aquarium.
            _aquarium2.SetActive(true); // L'aquarium est activer pour qu'il soit visible au joueur.
            etatAquarium = 2; // L'aquarium est dans son état final.
        }
    }
}
