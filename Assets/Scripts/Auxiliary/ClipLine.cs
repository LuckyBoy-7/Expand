using System;
using System.Collections.Generic;
using System.Linq;
using Lucky.Interactive;
using UnityEngine;

namespace Auxiliary
{
    public class ClipLine : InteractableUI
    {
        private LineRenderer clipLine;

        private Vector3 startPos;

        // 用来删除线的
        private List<Tuple<Building, Building>> buildingToBuilding = new();

        protected void Start()
        {
            clipLine = LineManager.instance.CreateLine();
            clipLine.startColor = clipLine.endColor = Color.red;
            clipLine.enabled = false;
        }

        protected override void OnCursorPress()
        {
            base.OnCursorPress();
            clipLine.enabled = true;
            startPos = GameCursor.MouseWorldPos;
            clipLine.SetPosition(0, GameCursor.MouseWorldPos);
            clipLine.SetPosition(1, GameCursor.MouseWorldPos);
        }

        private void Update()
        {
            if (!clipLine.enabled)
                return;
            clipLine.SetPosition(1, GameCursor.MouseWorldPos);
            var buildings = BuildingsManager.instance.buildings;
            buildingToBuilding.Clear();
            foreach (var b1 in buildings)
            {
                foreach (var b2 in b1.connectedBuildings)
                {
                    if (b1.buildingUIController.buildingToLine[b2] == null)
                        continue;
                    LineRenderer line = b1.buildingUIController.buildingToLine[b2];
                    // 判断相交
                    Vector2 A = b1.transform.position;
                    Vector2 B = b2.transform.position;
                    Vector2 C = startPos;
                    Vector2 D = GameCursor.MouseWorldPos;
                    float a1 = CrossArea(B - A, D - A);
                    float a2 = CrossArea(B - A, C - A);
                    float a3 = CrossArea(D - C, C - A);
                    float a4 = CrossArea(D - C, C - B);
                    if (OverlapSegSeg2d(a1, a2, a3, a4))
                    {
                        buildingToBuilding.Add(new(b1, b2));
                        line.startColor = line.endColor = Color.red;
                    }
                    else
                    {
                        buildingToBuilding.Remove(new(b1, b2));
                        line.startColor = line.endColor = Color.white;
                    }
                }
            }
        }

        protected override void OnCursorRelease()
        {
            base.OnCursorRelease();
            clipLine.enabled = false;
            foreach (var (b1, b2) in buildingToBuilding)
            {
                b1.BreakLineAndConnection(b2);
            }
        }


        public static bool OverlapSegSeg2d(float a1, float a2, float a3, float a4)
        {
            // print($"{a1*a2}");
            if (a1 * a2 < 0)
            {
                if (a3 * a4 < 0)
                    return true;
            }

            return false;
        }

        private static float CrossArea(Vector3 vec1, Vector3 vec2)
        {
            var v = Vector3.Cross(vec1, vec2);
            return Mathf.Sign(v.z) * v.magnitude;
        }
    }
}