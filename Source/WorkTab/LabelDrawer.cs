using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace DivisionOfLabor.WorkTab
{
    /// <summary>
    /// Draws rotated Work tab header labels and returns the label rect and the matrix it was drawn under.
    /// </summary>
    public static class LabelDrawer
    {
        private static readonly Color LabelColor = new Color(0.8f, 0.8f, 0.8f);

        private static readonly MethodInfo GetTopRectMethod = AccessTools.Method("UnityEngine.GUIClip:GetTopRect");
        private static readonly MethodInfo UnclipMethod = AccessTools.Method("UnityEngine.GUIClip:Unclip", new[] { typeof(Vector2) });

        private static Rect GetTopRect()
        {
            return (Rect)GetTopRectMethod.Invoke(null, null)!;
        }

        private static Vector2 Unclip(Vector2 position)
        {
            return (Vector2)UnclipMethod.Invoke(null, new object[] { position })!;
        }

        // Rotates the label 90 degrees counter-clockwise so it reads bottom to top.
        public static (Rect rect, Matrix4x4 matrix) DrawVertical(Rect rect, string label)
        {
            Matrix4x4 originalMatrix = GUI.matrix;
            Rect topRect = GetTopRect();

            // Unclip in screen space, then rotate about the clip origin.
            GUI.matrix = Matrix4x4.identity;
            Vector2 origin = Unclip(Vector2.zero);
            Matrix4x4 matrix = originalMatrix
                * Matrix4x4.TRS(origin, Quaternion.identity, Vector3.one)
                * Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, -90f), Vector3.one)
                * Matrix4x4.TRS(new Vector2(-rect.yMax - origin.x, rect.xMin - origin.y), Quaternion.identity, Vector3.one);
            GUI.matrix = matrix;

            // Clip to the part of the header that is inside the visible table.
            float leftClip = Mathf.Min(rect.xMin, 0f);
            float rightClip = Mathf.Max(rect.xMax - topRect.width, 0f);
            float topClip = Mathf.Min(rect.yMin, 0f);
            float bottomClip = Mathf.Max(rect.yMax - topRect.height, 0f);
            GUI.BeginClip(new Rect(bottomClip, -leftClip, rect.height + topClip - bottomClip, rect.width + leftClip - rightClip));

            Rect labelRect = new Rect(-bottomClip + GenUI.GapTiny, leftClip, rect.height, rect.width + GenUI.GapTiny);
            DrawLabel(labelRect, label, true);

            GUI.EndClip();
            GUI.matrix = originalMatrix;
            return (labelRect, matrix);
        }

        // Rotates the label 60 degrees so its bottom-left corner sits at the bottom of the column's center line.
        public static (Rect rect, Matrix4x4 matrix) DrawInclined(Rect rect, string label)
        {
            Rect rotatedRect = new Rect(0f, 0f, rect.height, Text.CalcSize(label).y) { center = rect.center };

            float theta = 60f * Mathf.Deg2Rad;
            Vector2 corner = new Vector2(-rotatedRect.width / 2f, -rotatedRect.height / 2f);
            float rotatedCornerX = Mathf.Cos(theta) * corner.x - Mathf.Sin(theta) * corner.y + rotatedRect.center.x;
            rotatedRect.x += rect.xMax - rotatedCornerX;

            Matrix4x4 originalMatrix = GUI.matrix;
            GUI.matrix = Matrix4x4.identity;
            Vector2 pivot = Unclip(rotatedRect.center);
            Matrix4x4 matrix = originalMatrix
                * Matrix4x4.TRS(pivot, Quaternion.identity, Vector3.one)
                * Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, -60f), Vector3.one)
                * Matrix4x4.TRS(-pivot, Quaternion.identity, Vector3.one);
            GUI.matrix = matrix;

            DrawLabel(rotatedRect, label, false);
            Widgets.DrawLine(new Vector2(rotatedRect.xMax, rotatedRect.yMax), new Vector2(rotatedRect.xMin, rotatedRect.yMax), new Color(1f, 1f, 1f, 0.2f), 1f);

            GUI.matrix = originalMatrix;
            return (rotatedRect, matrix);
        }

        private static void DrawLabel(Rect rect, string label, bool wordWrap)
        {
            Color color = GUI.color;
            TextAnchor anchor = Text.Anchor;
            GameFont font = Text.Font;
            bool wrap = Text.WordWrap;

            GUI.color = LabelColor;
            Text.Anchor = TextAnchor.MiddleLeft;
            Text.Font = GameFont.Small;
            Text.WordWrap = wordWrap;
            Widgets.Label(rect, label);

            Text.WordWrap = wrap;
            Text.Font = font;
            Text.Anchor = anchor;
            GUI.color = color;
        }
    }
}
