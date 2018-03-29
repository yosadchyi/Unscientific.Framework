// Based on code from http://www.kinematicsoup.com/news/2016/8/9/rrypp5tkubynjwxhxjzd42s3o034o8.
using UnityEngine;

namespace Unscientific.ECS.Unity
{
    /// <inheritdoc />
    /// <summary>
    /// Interpolates an object to the transform at the latest FixedUpdate from the transform at the previous FixedUpdate.
    /// It is critical this script's execution order is set before all other scripts that modify a transform from FixedUpdate.
    /// </summary>
    [RequireComponent(typeof(InterpolatedTransformUpdater))]
    public class InterpolatedTransform : MonoBehaviour
    {
        /// <summary>
        /// Stores transform data.
        /// </summary>
        private struct TransformData
        {
            public readonly Vector3 Position;
            public readonly Quaternion Rotation;
            public readonly Vector3 Scale;

            public TransformData(Vector3 position, Quaternion rotation, Vector3 scale)
            {
                Position = position;
                Rotation = rotation;
                Scale = scale;
            }
        }

        private readonly TransformData[] _lastTransforms = new TransformData[2]; // Stores the transform of the object from the last two FixedUpdates
        private int _newTransformIndex; // Keeps track of which index is storing the newest value.

        /// <summary>
        /// Initializes the list of previous orientations
        /// </summary>
        public void Initialize()
        {
            var t = new TransformData(transform.localPosition, transform.localRotation, transform.localScale);
            _lastTransforms[0] = t;
            _lastTransforms[1] = t;
            _newTransformIndex = 0;
        }

        /// <summary>
        /// Sets the object transform to what it was last FixedUpdate instead of where is was last interpolated to,
        /// ensuring it is in the correct position for gameplay scripts.
        /// </summary>
        private void FixedUpdate()
        {
            var mostRecentTransform = _lastTransforms[_newTransformIndex];
            transform.localPosition = mostRecentTransform.Position;
            transform.localRotation = mostRecentTransform.Rotation;
            transform.localScale = mostRecentTransform.Scale;
        }

        /// <summary>
        /// Runs after ofther scripts to save the objects's final transform.
        /// </summary>
        public void LateFixedUpdate()
        {
            _newTransformIndex = OldTransformIndex(); // Set new index to the older stored transform.
            _lastTransforms[_newTransformIndex] = new TransformData(transform.localPosition, transform.localRotation, transform.localScale);
        }

        /// <summary>
        /// Interpolates the object transform to the latest FixedUpdate's transform.
        /// </summary>
        private void Update()
        {
            var newestTransform = _lastTransforms[_newTransformIndex];
            var olderTransform = _lastTransforms[OldTransformIndex()];

            transform.localPosition = Vector3.Lerp(olderTransform.Position, newestTransform.Position, InterpolationController.InterpolationFactor);
            transform.localRotation = Quaternion.Slerp(olderTransform.Rotation, newestTransform.Rotation, InterpolationController.InterpolationFactor);
            transform.localScale = Vector3.Lerp(olderTransform.Scale, newestTransform.Scale, InterpolationController.InterpolationFactor);
        }

        /// <summary>
        /// Returns the index of the older stored transform. 
        /// </summary>
        /// <returns>The index of the older stored transform</returns>
        private int OldTransformIndex()
        {
            return _newTransformIndex == 0 ? 1 : 0;
        }
    }
}