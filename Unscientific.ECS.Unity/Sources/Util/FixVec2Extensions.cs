﻿using UnityEngine;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Unity
{
    public static class FixVec2Extensions
    {
        public static Vector3 ToVector3(this FixVec2 self)
        {
            return new Vector3(self.X.AsFloat, self.Y.AsFloat, 0);
        }
    }
}