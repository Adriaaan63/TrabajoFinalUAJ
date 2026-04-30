namespace Opsive.DeathmatchAIKit.Demo.Editor.Inspectors.UI
{
    using Opsive.DeathmatchAIKit.Demo.UI;
    using Opsive.UltimateCharacterController.Editor.Managers;
    using Opsive.UltimateCharacterController.Inventory;
    using Opsive.Shared.Editor.Inspectors.StateSystem;
    using System;
    using UnityEngine;
    using UnityEditor;

    /// <summary>
    /// Shows a custom inspector for the ItemWheelMonitor component.
    /// </summary>
    [CustomEditor(typeof(ItemWheelMonitor))]
    public class ItemWheelMonitorInspector : StateBehaviorInspector
    {
        /// <summary>
        /// Returns the actions to draw before the State list is drawn.
        /// </summary>
        /// <returns>The actions to draw before the State list is drawn.</returns>
        protected override Action GetDrawCallback()
        {
            var baseCallback = base.GetDrawCallback();

            baseCallback += () =>
            {
                EditorGUILayout.PropertyField(PropertyFromName("m_Character"));
                EditorGUILayout.PropertyField(PropertyFromName("m_Visible"));
                EditorGUILayout.PropertyField(PropertyFromName("m_ToggleItemWheel"));
                DrawCategories();
                EditorGUILayout.PropertyField(PropertyFromName("m_ActiveObjects"), true);
            };

            return baseCallback;
        }

        /// <summary>
        /// Draws all of the ItemSet categories within a dropdown.
        /// </summary>
        private void DrawCategories()
        {
            EditorGUI.BeginChangeCheck();

            var itemCollectionProperty = PropertyFromName("m_ItemCollection");
            var itemCollection = itemCollectionProperty.objectReferenceValue as ItemCollection;
            if (itemCollection == null) {
                itemCollection = ManagerUtility.FindItemCollection(this);
                GUI.changed = true;
            }
            itemCollection = EditorGUILayout.ObjectField("Item Collection", itemCollection, typeof(ItemCollection), false) as ItemCollection;
            if (itemCollection != itemCollectionProperty.objectReferenceValue as ItemCollection) {
                itemCollectionProperty.objectReferenceValue = itemCollection;
                GUI.changed = true;
            }

            var categoryIDProperty = PropertyFromName("m_CategoryID");
            var categoryNames = new string[((itemCollection != null && itemCollection.Categories != null) ? itemCollection.Categories.Length : 0) + 1];
            categoryNames[0] = "(Not Specified)";
            var selected = 0;
            if (categoryNames.Length > 1 && GUI.enabled) {
                for (int i = 0; i < itemCollection.Categories.Length; ++i) {
                    categoryNames[i + 1] = itemCollection.Categories[i].name;
                    if (categoryIDProperty.intValue == itemCollection.Categories[i].ID || (categoryIDProperty.intValue == itemCollection.Categories[i].ID - int.MaxValue)) {
                        selected = i + 1;
                    }
                }
            }

            var newSelected = EditorGUILayout.Popup("Category", selected != -1 ? selected : 0, categoryNames);
            if (selected != newSelected) {
                if (newSelected == 0) {
                    categoryIDProperty.intValue = 0;
                } else {
                    categoryIDProperty.intValue = (int)itemCollection.Categories[newSelected - 1].ID;
                }
            }
            if (EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
                Shared.Editor.Utility.EditorUtility.SetDirty(target);
            }
        }
    }
}