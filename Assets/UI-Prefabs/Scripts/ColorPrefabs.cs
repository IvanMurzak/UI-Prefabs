#if UNITY_EDITOR
using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UIPrefabs
{
    [CreateAssetMenu(menuName = "UI-Prefabs/ColorPrefabs")]
    public class ColorPrefabs : ScriptableObject
    {
                            public Color                color;

        [SceneObjectsOnly]  public Image[]              images;
        [SceneObjectsOnly]  public SpriteRenderer[]     sprites;
        [SceneObjectsOnly]  public TMP_Text[]           texts;
        
        Transform PrefabRoot(Transform x)
        {
            if (PrefabInstanceStatus.Connected != PrefabUtility.GetPrefabInstanceStatus(x))
                return null;

            if (x.parent == null)
                return x;

            if (PrefabInstanceStatus.Connected != PrefabUtility.GetPrefabInstanceStatus(x.parent))
                return x;

            return PrefabRoot(x.parent);
        }

        [Button] void Refresh()
        {
            foreach (var x in images)   RefreshColor(x.transform, (color) => x.color = color, color);
            foreach (var x in sprites)  RefreshColor(x.transform, (color) => x.color = color, color);
            foreach (var x in texts)    RefreshColor(x.transform, (color) => x.color = color, color);
        }

        void RefreshColor(Component component, Action<Color> setter, Color color)
        {
            var root = PrefabRoot(component.transform);
            if (root != null)
            {
                Debug.Log($"Apply prefab: {root.name}");
                setter(color);

                PrefabUtility.ApplyPrefabInstance(root.gameObject, InteractionMode.UserAction);
                AssetDatabase.SaveAssets();
            }
        }
    }
}
#endif