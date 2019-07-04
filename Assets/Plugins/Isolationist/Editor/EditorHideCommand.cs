// ---------------------------------------------------------------------
// Copyright (c) 2017 Magic Leap. All Rights Reserved.
// Magic Leap Confidential and Proprietary
// ---------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Plugins.Isolationist.Editor {
	[InitializeOnLoad]
	public static class EditorHideCommand {
		private const string HIDE_SETTING_PREFIX = "HideToggle";

		private static bool _ranThisFrame;

		private static EditorHotKey HotKey { get; set; }

		static EditorHideCommand() {
			HotKey = new EditorHotKey(HIDE_SETTING_PREFIX, KeyCode.I, defaultShift: true);
			EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
			SceneView.onSceneGUIDelegate += OnSceneGUI;
		}

		private static void OnSceneGUI(SceneView sceneview) { OnGUI(); }

		private static void OnHierarchyWindowItemOnGUI(int instanceid, Rect selectionrect) { OnGUI(); }

		private static void OnGUI() {
			if (_ranThisFrame || !HotKey.Pressed || !Selection.activeGameObject) { return; }

			if (EditorGUIUtility.editingTextField && !string.IsNullOrEmpty(GUI.GetNameOfFocusedControl())) {
				Debug.Log(string.Format("Skipped. In text field. :P Control ID: {0} aka {1}", GUIUtility.GetControlID(FocusType.Keyboard), GUI.GetNameOfFocusedControl()));
				return;
			}

			ToggleHide();
		}

		public static void PreferencesGUI() { HotKey.OnGUI(); }

		[MenuItem("Tools/Toggle Hide", true)]
		public static bool CanToggleHide() {
			return Selection.activeGameObject || IsolateInfo.IsIsolated;
		}

		[MenuItem("Tools/Toggle Hide")]
		public static void ToggleHide() {
			bool activeState = Selection.activeGameObject.activeSelf;
			string undoName = string.Format("{0} {1}", activeState ? "Hide" : "Unhide", Selection.gameObjects.Length > 1 ? Selection.gameObjects.Length + " Objects" : Selection.activeGameObject.name);
			Undo.RecordObjects(Selection.objects, undoName);
			Selection.gameObjects.ForEach(go => go.SetActive(!activeState));

			Debug.Log(undoName);

			_ranThisFrame = true;
			EditorApplication.delayCall += () => _ranThisFrame = false;
		}
	}

	internal static class Utils {
		internal static void ForEach<T>(this IEnumerable<T> items, Action<T> action) {
			if (items == null) { return; }
			foreach (T obj in items) { action(obj); }
		}

		[ContractAnnotation("source:null => true")]
		internal static bool IsNullOrEmpty(this string source) {
			return string.IsNullOrEmpty(source);
		}

		private static bool IsParent(this Transform parent, Transform transform) {
			while (parent) {
				if (parent == transform) { return true; }
				parent = parent.parent;
			}

			return false;
		}

		private static bool IsParent(this GameObject parent, GameObject go) { return parent && go && IsParent(parent.transform, go.transform); }

		internal static bool IsRelative(this GameObject go1, GameObject go2) { return go2.IsParent(go1) || go1.IsParent(go2); }

		internal static IEnumerable<Transform> GetChildren(this Transform t) {
			List<Transform> children = new List<Transform>();
			for (int i = 0; i < t.childCount; i++) { children.Add(t.GetChild(i)); }
			return children;
		}

		private static IEnumerable<GameObject> GetRootSceneObjects() {
#if UNITY_5_3_OR_NEWER
			List<GameObject> rootObjects = new List<GameObject>();
			for (int i = 0; i < SceneManager.sceneCount; i++) {
				Scene scene = SceneManager.GetSceneAt(i);
				rootObjects.AddRange(scene.GetRootGameObjects());
			}
			return rootObjects;
#else
			var prop = new HierarchyProperty(HierarchyType.GameObjects);
			var expanded = new int[0];
			while (prop.Next(expanded)) yield return prop.pptrValue as GameObject;
#endif
		}

		internal static IEnumerable<Transform> GetRootTransforms() { return GetRootSceneObjects().Where(go => go).Select(go => go.transform); }
	}
}