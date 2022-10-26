using System;
using UnityEditor;
using UnityEngine;

namespace WorldBuilder.Autotiling
{
    [InitializeOnLoad]
    public static class PrototypeGizmoDrawer
    {
        private const float CORNER_SIZE = 0.15f;

        private static TileType[] s_terrainTypes = Array.Empty<TileType>();

        static PrototypeGizmoDrawer()
        {
            LoadTileTypes();
        }

        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
        public static void OnGizmos(Prototype prototype, GizmoType gizmoType)
        {
            if (prototype == null || prototype.Corners.Length == 0)
                return;

            for (int i = 0; i < prototype.Corners.Length; i++)
            {
                Gizmos.color = prototype.Corners[i].DebugColor;
                Gizmos.DrawSphere(GetCornerPoint(i, prototype), CORNER_SIZE);
            }
        }

        public static void OnSceneGUI(Prototype prototype)
        {
            if (prototype == null || prototype.Corners.Length == 0)
                return;

            Vector2 size = new Vector2(50, 30 * (s_terrainTypes.Length + 1));
            Color originalColor = GUI.backgroundColor;

            for (int i = 0; i < prototype.Corners.Length; i++)
            {
                Handles.color = prototype.Corners[i].DebugColor;
                Vector3 point = GetCornerPoint(i, prototype);

                GUILayout.BeginArea(new Rect(
                    HandleUtility.WorldToGUIPoint(point) + new Vector2(-size.x * 0.5f, -size.y * 0.5f - 25f), size));

                foreach (TileType terrain in s_terrainTypes)
                {
                    if (prototype.Corners[i].TerrainID == terrain.ID)
                        GUI.backgroundColor = terrain.DebugColor;

                    if (GUILayout.Button(terrain.name.Replace("tile_", "")))
                    {
                        prototype.Corners[i]._tile = terrain;
                        UnityEditor.EditorUtility.SetDirty(prototype);
                    }

                    GUI.backgroundColor = originalColor;
                }

                GUILayout.EndArea();
            }
        }

        public static void LoadTileTypes()
        {
            s_terrainTypes = EditorUtility.LoadAssets<TileType>();
        }

        private static Vector3 GetCornerPoint(int i, Prototype prototype)
        {
            Vector3 point = Vector3.Scale(TileUtility.CORNER_DIRECTIONS[i], prototype.Size / 2);
            point -= TileUtility.CORNER_DIRECTIONS[i] * (CORNER_SIZE * 1.5f);
            point += new Vector3(0f, prototype.Size.y * 0.5f, 0f);

            return prototype.transform.TransformPoint(point);
        }
    }
}