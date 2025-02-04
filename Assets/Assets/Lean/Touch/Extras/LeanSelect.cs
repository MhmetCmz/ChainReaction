using UnityEngine;
using System.Collections.Generic;

namespace Lean.Touch
{
	/// <summary>This component allows you to select LeanSelectable components.
	/// To use it, you can call the SelectScreenPosition method from somewhere (e.g. the LeanFingerTap.OnTap event).</summary>
	[HelpURL(LeanTouch.HelpUrlPrefix + "LeanSelect")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Select")]
	public class LeanSelect : MonoBehaviour
	{
		public enum SelectType
		{
			Manually = -1,
			Raycast3D,
			Overlap2D,
			CanvasUI,
			ScreenDistance,
			Intersect2D
		}

		public enum SearchType
		{
			GetComponent,
			GetComponentInParent,
			GetComponentInChildren
		}

		public enum ReselectType
		{
			KeepSelected,
			Deselect,
			DeselectAndSelect,
			SelectAgain
		}

		/// <summary>This stores all active and enabled LeanSelect instances in the scene.</summary>
		public static LinkedList<LeanSelect> Instances = new LinkedList<LeanSelect>();

		/// <summary>Which kinds of objects should be selectable from this component?</summary>
		[Tooltip("Which kinds of objects should be selectable from this component?")]
		public SelectType SelectUsing;

		/// <summary>If SelectUsing fails, you can set an alternative method here.</summary>
		[Tooltip("If SelectUsing fails, you can set an alternative method here.")]
		public SelectType SelectUsingAlt = SelectType.Manually;

		/// <summary>If SelectUsingAlt fails, you can set an alternative method here.</summary>
		[Tooltip("If SelectUsingAlt fails, you can set an alternative method here.")]
		public SelectType SelectUsingAltAlt = SelectType.Manually;

		[Space]

		/// <summary>The layers you want the raycast/overlap to rcHit.</summary>
		[Tooltip("The layers you want the raycast/overlap to rcHit.")]
		public LayerMask LayerMask = Physics.DefaultRaycastLayers;

		/// <summary>The camera used to calculate the ray.
		/// None = MainCamera.</summary>
		[Tooltip("The camera used to calculate the ray.\n\nNone = MainCamera.")]
		public Camera Camera;

		/// <summary>The maximum number of selectables that can be selected at the same time.
		/// 0 = Unlimited.</summary>
		[Tooltip("The maximum number of selectables that can be selected at the same time.\n\n0 = Unlimited.")]
		public int MaxSelectables;

		/// <summary>How should the candidate GameObjects be searched for the LeanSelectable component?</summary>
		[Tooltip("How should the candidate GameObjects be searched for the LeanSelectable component?")]
		public SearchType Search = SearchType.GetComponentInParent;

		/// <summary>If you select an already selected selectable, what should happen?</summary>
		[Tooltip("If you select an already selected selectable, what should happen?")]
		public ReselectType Reselect = ReselectType.SelectAgain;

		/// <summary>Automatically deselect everything if nothing was selected?</summary>
		[Tooltip("Automatically deselect everything if nothing was selected?")]
		public bool AutoDeselect;

		/// <summary>When using the <b>ScreenDistance</b> selection mode, this allows you to set how many scaled pixels from the mouse/finger you can select.</summary>
		[Tooltip("When using the ScreenDistance selection mode, this allows you to set how many scaled pixels from the mouse/finger you can select.")]
		public float MaxScreenDistance = 50;

		/// <summary>Having multiple <b>LeanSelect</b> components in your scene is usually a mistake, and this component will warn you about this. If you really know what you're doing and need multiple then you can enable this to hide the warning.</summary>
		[Tooltip("Having multiple LeanSelect components in your scene is usually a mistake, and this component will warn you about this. If you really know what you're doing and need multiple then you can enable this to hide the warning.")]
		public bool SuppressMultipleSelectWarning;

		[System.NonSerialized]
		private LinkedListNode<LeanSelect> node;

		private static RaycastHit[] raycastrcHits = new RaycastHit[1024];

		private static RaycastHit2D[] raycastrcHit2Ds = new RaycastHit2D[1024];

		// NOTE: This must be called from somewhere
		public void SelectStartScreenPosition(LeanFinger finger)
		{
			SelectScreenPosition(finger, finger.StartScreenPosition);
		}

		// NOTE: This must be called from somewhere
		public void SelectScreenPosition(LeanFinger finger)
		{
			SelectScreenPosition(finger, finger.ScreenPosition);
		}

		// NOTE: This must be called from somewhere
		public void SelectScreenPosition(LeanFinger finger, Vector2 screenPosition)
		{
			// Stores the component we rcHit (Collider or Collider2D)
			var component = default(Component);

			TryGetComponent(SelectUsing, screenPosition, ref component);

			if (component == null)
			{
				TryGetComponent(SelectUsingAlt, screenPosition, ref component);

				if (component == null)
				{
					TryGetComponent(SelectUsingAltAlt, screenPosition, ref component);
				}
			}

			Select(finger, component);
		}

		private static int GetClosestRaycastrcHitsIndex(int count)
		{
			var closestIndex    = -1;
			var closestDistance = float.PositiveInfinity;

			for (var i = 0; i < count; i++)
			{
				var distance = raycastrcHits[i].distance;

				if (distance < closestDistance)
				{
					closestIndex    = i;
					closestDistance = distance;
				}
			}

			return closestIndex;
		}

		private void TryGetComponent(SelectType selectUsing, Vector2 screenPosition, ref Component component)
		{
			switch (selectUsing)
			{
				case SelectType.Raycast3D:
				{
					// Make sure the camera exists
					var camera = LeanTouch.GetCamera(Camera, gameObject);

					if (camera != null)
					{
						var ray   = camera.ScreenPointToRay(screenPosition);
						var count = Physics.RaycastNonAlloc(ray, raycastrcHits, float.PositiveInfinity, LayerMask);

						if (count > 0)
						{
							component = raycastrcHits[GetClosestRaycastrcHitsIndex(count)].transform;
						}
					}
					else
					{
						Debug.LogError("Failed to find camera. Either tag your cameras MainCamera, or set one in this component.", this);
					}
				}
				break;

				case SelectType.Overlap2D:
				{
					// Make sure the camera exists
					var camera = LeanTouch.GetCamera(Camera, gameObject);

					if (camera != null)
					{
						var ray   = camera.ScreenPointToRay(screenPosition);
						var slope = -ray.direction.z;

						if (slope != 0.0f)
						{
							var point = ray.GetPoint(ray.origin.z / slope);

							component = Physics2D.OverlapPoint(point, LayerMask);
						}
					}
					else
					{
						Debug.LogError("Failed to find camera. Either tag your cameras MainCamera, or set one in this component.", this);
					}
				}
				break;

				case SelectType.CanvasUI:
				{
					var results = LeanTouch.RaycastGui(screenPosition, LayerMask);

					if (results != null && results.Count > 0)
					{
						component = results[0].gameObject.transform;
					}
				}
				break;

				case SelectType.ScreenDistance:
				{
					var bestDistance = MaxScreenDistance * LeanTouch.ScalingFactor;

					bestDistance *= bestDistance;

					// Make sure the camera exists
					var camera = LeanTouch.GetCamera(Camera, gameObject);

					if (camera != null)
					{
						foreach (var selectable in LeanSelectable.Instances)
						{
							var distance = Vector2.SqrMagnitude(GetScreenPoint(camera, selectable.transform) - screenPosition);

							if (distance <= bestDistance)
							{
								bestDistance = distance;
								component    = selectable;
							}
						}
					}
				}
				break;

				case SelectType.Intersect2D:
				{
					// Make sure the camera exists
					var camera = LeanTouch.GetCamera(Camera, gameObject);

					if (camera != null)
					{
						var ray   = camera.ScreenPointToRay(screenPosition);
						var count = Physics2D.GetRayIntersectionNonAlloc(ray, raycastrcHit2Ds, float.PositiveInfinity, LayerMask);

						if (count > 0)
						{
							component = raycastrcHit2Ds[0].transform;
						}
					}
					else
					{
						Debug.LogError("Failed to find camera. Either tag your cameras MainCamera, or set one in this component.", this);
					}
				}
				break;
			}
		}

		private static Vector2 GetScreenPoint(Camera camera, Transform transform)
		{
			if (transform is RectTransform)
			{
				var canvas = transform.GetComponentInParent<Canvas>();

				if (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceOverlay)
				{
					return RectTransformUtility.WorldToScreenPoint(null, transform.position);
				}
			}

			return camera.WorldToScreenPoint(transform.position);
		}

		public void Select(LeanFinger finger, Component component)
		{
			// Stores the selectable we will search for
			var selectable = default(LeanSelectable);

			// Was a collider found?
			if (component != null)
			{
				switch (Search)
				{
					case SearchType.GetComponent:           selectable = component.GetComponent          <LeanSelectable>(); break;
					case SearchType.GetComponentInParent:   selectable = component.GetComponentInParent  <LeanSelectable>(); break;
					case SearchType.GetComponentInChildren: selectable = component.GetComponentInChildren<LeanSelectable>(); break;
				}
			}

			// Select the selectable
			Select(finger, selectable);
		}

		public void Select(LeanFinger finger, LeanSelectable selectable)
		{
			// Something was selected?
			if (selectable != null && selectable.isActiveAndEnabled == true)
			{
				if (selectable.HideWithFinger == true)
				{
					foreach (var otherSelectable in LeanSelectable.Instances)
					{
						if (otherSelectable.HideWithFinger == true && otherSelectable.IsSelected == true)
						{
							return;
						}
					}
				}

				// Did we select a new LeanSelectable?
				if (selectable.IsSelected == false)
				{
					// Deselect some if we have too many
					if (MaxSelectables > 0)
					{
						LeanSelectable.Cull(MaxSelectables - 1);
					}

					// Select
					selectable.Select(finger);
				}
				// Did we reselect the current LeanSelectable?
				else
				{
					switch (Reselect)
					{
						case ReselectType.Deselect:
						{
							selectable.Deselect();
						}
						break;

						case ReselectType.DeselectAndSelect:
						{
							selectable.Deselect();
							selectable.Select(finger);
						}
						break;

						case ReselectType.SelectAgain:
						{
							selectable.Select(finger);
						}
						break;
					}
				}
			}
			// Nothing was selected?
			else
			{
				// Deselect?
				if (AutoDeselect == true)
				{
					DeselectAll();
				}
			}
		}

		[ContextMenu("Deselect All")]
		public void DeselectAll()
		{
			LeanSelectable.DeselectAll();
		}

		protected virtual void OnEnable()
		{
			if (Instances.Count > 0 && SuppressMultipleSelectWarning == false)
			{
				Debug.LogWarning("Your scene contains multiple LeanSelect components, which is very likely to be a mistake. Your scene should normally only contain one.", this);
			}

			node = Instances.AddLast(this);
		}

		protected virtual void OnDisable()
		{
			Instances.Remove(node); node = null;
		}
	}
}