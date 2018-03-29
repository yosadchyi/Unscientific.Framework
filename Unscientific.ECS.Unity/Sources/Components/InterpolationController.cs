// Based on code from http://www.kinematicsoup.com/news/2016/8/9/rrypp5tkubynjwxhxjzd42s3o034o8.

using UnityEngine;

namespace Unscientific.ECS.Unity
{
    /// <inheritdoc />
    /// <summary>
    /// Manages the interpolation factor that InterpolatedTransforms use to position themselves.
    /// Must be attached to a single object in each scene, such as a gamecontroller.
    /// It is critical this script's execution order is set before InterpolatedTransform.
    /// </summary>
    public class InterpolationController : MonoBehaviour
    {
        private float _oldTime;
        private float _newTime;

        // The proportion of time since the previous FixedUpdate relative to fixedDeltaTime

        public static float InterpolationFactor { get; private set; }

        /// <summary>
        /// Record the time of the current FixedUpdate and remove the oldest value.
        /// </summary>
        public void FixedUpdate()
        {
            _oldTime = _newTime;
            _newTime = Time.fixedTime;
        }

        /// <summary>
        /// Sets the interpolation factor.
        /// </summary>
        private void Update()
        {
            if (_newTime != _oldTime)
            {
                InterpolationFactor = (Time.time - _newTime) / (_newTime - _oldTime);
            }
            else
            {
                InterpolationFactor = 1;
            }
        }
    }
}