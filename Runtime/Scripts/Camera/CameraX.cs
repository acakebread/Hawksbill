// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 11/06/2021 17:03:23 by seantcooper
using UnityEngine;
using Hawksbill;

namespace Hawksbill
{
    public static class CameraX
    {
        public static float getDistance(this Camera camera, Vector3 position) =>
            new Plane (camera.transform.forward, camera.transform.position).GetDistanceToPoint (position);

        public static bool isPositionVisible(this Camera camera, Vector3 position, float distance = float.MaxValue, float padding = 0)
        {
            var viewport = camera.WorldToViewportPoint (position);
            return viewport.x >= -padding && viewport.y >= -padding && viewport.x <= 1 + padding && viewport.y <= 1 + padding &&
                viewport.z >= 0 && viewport.z < distance;
        }

    }
}