using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using System.Collections;
using System.Collections.Generic;

public class DoorTriggerVR : MonoBehaviour
{
	[Tooltip("Timeline qui anime la porte")]
	public PlayableDirector doorTimeline;

	[Tooltip("Référence au DynamicMoveProvider qui gère le mouvement de l'XR Origin (peut rester vide pour auto-find)")]
	public DynamicMoveProvider moveProvider;

	// Liste des composants de mouvement désactivés temporairement
	private List<Behaviour> _disabledMovementComponents = new List<Behaviour>();

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			// Trigger de la porte : lorsqu'un joueur entre dans le trigger,
			// on désactive le mouvement, on lance la timeline d'ouverture,
			// puis on réactive le mouvement une fois l'animation terminée.

			// Si le champ n'est pas assigné dans l'inspecteur, on essaie de le trouver
			// sur le joueur (ou dans la scène) pour plus de robustesse.
			// On désactive ici tous les composants de locomotion possibles
			DisableMovementComponentsOnPlayerRoot(other);

			if (doorTimeline != null)
				doorTimeline.Play();

			StartCoroutine(ReenableMovement());
		}
	}

	private IEnumerator ReenableMovement()
	{
		// Attend la fin de la timeline si elle est définie
		while (doorTimeline != null && doorTimeline.state == PlayState.Playing)
			yield return null;

		// Réactive tous les composants désactivés
		foreach (var comp in _disabledMovementComponents)
		{
			if (comp != null)
				comp.enabled = true;
		}
		if (_disabledMovementComponents.Count > 0)
			Debug.Log($"Reenabled {_disabledMovementComponents.Count} movement component(s)");

		_disabledMovementComponents.Clear();
	}

	// Cherche et désactive les composants de mouvement sur la racine du joueur
	private void DisableMovementComponentsOnPlayerRoot(Collider other)
	{
		_disabledMovementComponents.Clear();

		var root = other.transform.root;

		// 1) DynamicMoveProvider (samples)
		var dyns = root.GetComponentsInChildren<DynamicMoveProvider>(true);
		foreach (var d in dyns)
			TryDisableAndStore(d);

		// 2) ContinuousMoveProvider (type may be unavailable at compile time in some setups)
		TryFindAndDisableByTypeName(root, "UnityEngine.XR.Interaction.Toolkit.Movement.ContinuousMoveProvider");

		// 3) ActionBasedContinuousMoveProvider (deprecated in some versions) - try by type name
		TryFindAndDisableByTypeName(root, "UnityEngine.XR.Interaction.Toolkit.ActionBasedContinuousMoveProvider");

		// 4) ContinuousMoveProviderBase (older fallback) - try by type name
		TryFindAndDisableByTypeName(root, "UnityEngine.XR.Interaction.Toolkit.Movement.ContinuousMoveProviderBase");

		if (_disabledMovementComponents.Count == 0)
		{
			// Fallback global: cherche n'importe quel DynamicMoveProvider dans la scène
			var fallback = FindAnyObjectByType<DynamicMoveProvider>();
			if (fallback != null)
				TryDisableAndStore(fallback);
		}

		Debug.Log($"Disabled {_disabledMovementComponents.Count} movement component(s) on player root: {root.name}");
	}

	private void TryDisableAndStore(Behaviour comp)
	{
		if (comp != null && comp.enabled)
		{
			comp.enabled = false;
			_disabledMovementComponents.Add(comp);
			Debug.Log($"Disabled movement component: {comp.GetType().Name} on {comp.gameObject.name}");
		}
	}

	// Cherche des composants par nom de type (reflection) et les désactive
	private void TryFindAndDisableByTypeName(Transform root, string typeFullName)
	{
		var type = System.Type.GetType(typeFullName);
		if (type == null)
			return;

		var comps = root.GetComponentsInChildren(type, true);
		foreach (var c in comps)
		{
			if (c is Behaviour b)
				TryDisableAndStore(b);
		}
	}
}

