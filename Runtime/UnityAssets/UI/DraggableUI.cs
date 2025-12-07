using UnityEngine;
using UnityEngine.EventSystems;

namespace LucasWarwick02.UnityAssets
{
    /// <summary>
    /// A generic component for making UI elements draggable, detecting valid drop targets of type <c>T</c>, and optionally snapping back to their original position. It handles drag events, proximity-based target detection, and provides hooks for entering, exiting, and dropping onto targets.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [RequireComponent(typeof(RectTransform))]
    public abstract class DraggableUI<T> : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler where T : MonoBehaviour
    {
        /// <summary>
        /// Whether or not to move the item back to it's origin once let go.
        /// </summary>
        [Header("Draggable UI")]
        public bool snapBack = true;
        [Space]
        
        private RectTransform _rectTransform;
        private Canvas _canvas;
        private Camera _camera;
        
        private Vector3 _origin;
        private readonly Collider2D[] _overlapColliders = new Collider2D[10];
        
        protected T Target { private set; get; }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvas = GetComponentInParent<Canvas>();
            _camera = Camera.main;

            _origin = _rectTransform.localPosition;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Target = null;
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform,
                eventData.position,
                _canvas.worldCamera,
                out var position);

            _rectTransform.anchoredPosition = _canvas.transform.TransformPoint(position);
            
            var target = ClosestTarget();
            if (target != Target)
            {
                if (Target) OnTargetExit(Target);
                if (target) OnTargetEnter(target);

                Target = target;
            }
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            if (Target)
            {
                OnTargetDrop(Target);
                OnTargetExit(Target);
            }
            
            Target = null;
            
            if (snapBack) _rectTransform.localPosition = _origin;
        }

        private T ClosestTarget()
        {
            var worldPos = _camera.ScreenToWorldPoint(_rectTransform.position);
            var size = Physics2D.OverlapCircleNonAlloc(worldPos, OverlapRadius, _overlapColliders);

            T closest = null;
            float closestSqr = float.PositiveInfinity;

            // Iterate manually to avoid per-frame LINQ
            for (var i = 0; i < size; i++)
            {
                // Attempt to grab a (valid) target
                var comp = _overlapColliders[i].GetComponent<T>();
                if (comp == null || !IsValidTarget(comp)) continue;

                float sqr = (worldPos - comp.transform.position).sqrMagnitude;
                if (sqr < closestSqr)
                {
                    closest = comp;
                    closestSqr = sqr;
                }
            }

            return closest;
        }

        /// <summary>
        /// Radius to detect collisions.
        /// </summary>
        protected virtual float OverlapRadius => 0.5f;

        /// <summary>
        /// If we hover over a target, what makes it usable?
        /// </summary>
        /// <param name="target">Target object.</param>
        /// <returns></returns>
        protected abstract bool IsValidTarget(T target);

        /// <summary>
        /// Invoked when an item enters a target's radius.
        /// </summary>
        /// <param name="target"></param>
        protected abstract void OnTargetEnter(T target);
        
        /// <summary>
        /// Invoked when an item exits a target's radius.
        /// </summary>
        /// <param name="target"></param>
        protected abstract void OnTargetExit(T target);

        /// <summary>
        /// Invoked when an item is dropped onto a target.
        /// </summary>
        /// <param name="target"></param>
        protected abstract void OnTargetDrop(T target);
    }
}