// Based on code from http://www.kinematicsoup.com/news/2016/8/9/rrypp5tkubynjwxhxjzd42s3o034o8.
using UnityEngine;

namespace Unscientific.ECS.Unity
{
    /// <inheritdoc />
    /// <summary>
    /// Used to allow a later script execution order for FixedUpdate than in GameplayTransform.
    /// It is critical this script runs after all other scripts that modify a transform from FixedUpdate.
    /// </summary>
    public class InterpolatedTransformUpdater : MonoBehaviour
    {
        private InterpolatedTransform _interpolatedTransform;
    
        private void Awake()
        {
            _interpolatedTransform = GetComponent<InterpolatedTransform>();
        }
	
        private void FixedUpdate()
        {
            _interpolatedTransform.LateFixedUpdate();
        }
    }
}