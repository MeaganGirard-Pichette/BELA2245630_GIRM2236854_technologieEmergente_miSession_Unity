using UnityEngine;

public class Matelas : MonoBehaviour
{
    [SerializeField] private GameObject _matelas1;
    [SerializeField] private GameObject _matelas2;
    [SerializeField] private GameObject _revetements;

    [SerializeField] private AudioClip _clip1;
    [SerializeField] private AudioClip _clip2;

    private AudioSource _audioSource1;
    private AudioSource _audioSource2;

    private int trigger1;
    private int trigger2;

    private int etatMatelas;
    private string prendreMatelas = "Vous avez pris le matelas...";
    private string deposerMatelas = "Vous avez déposé le matelas...";

    void Start()
    {
        _audioSource1 = _matelas1.GetComponent<AudioSource>(); // Va chercher le component "Audio Source" dans l'objet "_matelas1".
        _audioSource2 = _matelas2.GetComponent<AudioSource>(); // Va chercher le component "Audio Source" dans l'objet "_matelas2".

        _matelas1.SetActive(true); // Le "matelas1" est actif.
        _matelas2.SetActive(false); // Le "matelas2" est innactif.

        etatMatelas = 0; // Le matelas est dans son 1er état.

        trigger1 = 0; // Initialisation du trigger 1.
        trigger2 = 0; // Initialisation du trigger 2.
    }

    void Update()
    {
        PrendreMatelas();
        DeposerMatelas();
    }

    private void PrendreMatelas()
    {
        if (trigger1 != 0) { return; } // Si la valeur du trigger a changée, ignorer cette méthode.

        float distance = Vector3.Distance(_matelas1.transform.position, GameManager.instance.positionJ); // Calcul la distance entre le matelas1 et le joueur.

        // Si le joueur est proche de le matelas1.
        if (distance < 5)
        {
            // Si le matelas est dans son 1er état, que les mains sont vides et que le joueur peut intéragir avec le matelas.
            if (etatMatelas == 0 && GameManager.instance.mainsVides == true && GameManager.instance.connaitDanger == true)
            {
                trigger1 = 1; // La valeur du trigger change pour que cette méthode soit ignorée après être utilisée.

                GameManager.instance.Afficher(prendreMatelas + GameManager.instance.txtMains); // Texte pour informer le joueur.
                Debug.Log(prendreMatelas + GameManager.instance.txtMains);

                GameManager.instance.mainsVides = false; // Les mains sont maintenant pleines.
                _audioSource1.PlayOneShot(_clip1); // Joue l'audio du joueur qui prend le matelas.
                _matelas1.SetActive(false); // Le matelas est déactiver pour qu'il ne soit plus visible au joueur.
                etatMatelas = 1; // Le matelas est dans sa deuxième état.
            }

            // Si le joueur peut intéragir avec le matelas, mais les mains sont remplis.
            else if (GameManager.instance.mainsVides == false && GameManager.instance.connaitDanger == true)
            {
                GameManager.instance.Afficher(GameManager.instance.txtMains); // Texte qui informe que les mains sont remplis.
                Debug.Log(GameManager.instance.txtMains);
            }
        }
    }

    private void DeposerMatelas()
    {
        if (trigger2 != 0) { return; } // Si la valeur du trigger a changée, ignorer cette méthode.

        float distance = Vector3.Distance(_matelas2.transform.position, GameManager.instance.positionJ); // Calcul la distance entre le matelas2 et le joueur.

        // Si le joueur est proche de le matelas2 et qu'elle est dans son 2ème état.
        if (distance < 5 && etatMatelas == 1)
        {
            trigger2 = 1; // La valeur du trigger change pour que cette méthode soit ignorée après être utilisée.

            GameManager.instance.Afficher(deposerMatelas); // Texte pour informer le joueur.
            Debug.Log(deposerMatelas);

            GameManager.instance.mainsVides = true; // Les mains sont vides.
            _audioSource2.PlayOneShot(_clip2); // Joue l'audio du joueur qui dépose le matelas.
            _matelas2.SetActive(true); // Le matelas est activer pour qu'il soit visible au joueur.
            etatMatelas = 2; // Le matelas est dans son état final.
        }
    }
}
