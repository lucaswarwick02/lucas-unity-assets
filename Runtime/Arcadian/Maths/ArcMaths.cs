using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Arcadian.Maths
{
    public static class ArcMaths
    {
        /// <summary>
        /// Randomly get a value between -1f and 1f.
        /// </summary>
        public static float signedValue => Random.value * 2f - 1f;

        /// <summary>
        /// Find points in a circle which are spread out evenly.
        /// </summary>
        /// <param name="center">Center position of the circle.</param>
        /// <param name="radius">Radius of the circle.</param>
        /// <param name="count">Number of points to find.</param>
        /// <returns>List of positions for each point.</returns>
        public static List<Vector3> PointsInRadius2D(Vector3 center, float radius, int count)
        {
            var points = new List<Vector3>(count);

            float goldenAngle = Mathf.PI * (3f - Mathf.Sqrt(5f));

            // One random rotation for the entire layout
            float rotationOffset = Random.value * Mathf.PI * 2f;

            // Randomly mirror X and/or Y
            bool mirrorX = Random.value > 0.5f;
            bool mirrorY = Random.value > 0.5f;

            int rings = Mathf.Clamp(count / 5, 1, 3); // 1–3 concentric rings
            int pointsPerRing = Mathf.CeilToInt(count / (float)rings);
            int index = 0;

            for (int ring = 0; ring < rings; ring++)
            {
                // radial distance: evenly spread rings from center to full radius
                float ringRadius = radius * (ring + 1) / rings;

                // per-ring rotation variation
                float ringRotation = rotationOffset + ring * Random.value * Mathf.PI / 3f;

                for (int i = 0; i < pointsPerRing && index < count; i++, index++)
                {
                    float t = (i + 0.5f) / pointsPerRing;
                    float r = ringRadius * t; // linear spacing in the ring

                    float angle = i * goldenAngle + ringRotation;

                    float x = Mathf.Cos(angle) * r;
                    float y = Mathf.Sin(angle) * r;

                    if (mirrorX) x = -x;
                    if (mirrorY) y = -y;

                    points.Add(new Vector3(center.x + x, center.y + y, center.z));
                }
            }

            return points;
        }
    }
}