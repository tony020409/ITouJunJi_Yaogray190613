/*
 *	Created by Philippe Groarke on 2016-08-28.
 *	Copyright (c) 2016 Tarfmagougou Games. All rights reserved.
 *
 *	Dedication : This code is dedicated to my sister Maureen, for the unending
 *		support she has provided throughout rough times. <3 Mo
 */ 

namespace tarfmagougou
{
	using UnityEngine;
	using UnityEditor;
	using System.Collections;
	using System.Collections.Generic;
	using System.Text;
	using System.Reflection;

	#if UNITY_5_5_OR_NEWER
	using UnityEngine.Profiling;
	#endif

	[System.Serializable]
	struct ObjectStat : System.IEquatable<ObjectStat>
	{
		static readonly string[] _memory_suffixes = { " B", " KB", " MB", " GB", " TB" };
		public string name;
		public ulong mesh_tris;
		public HashSet<int> mesh_total;
		public ulong mesh_instances;
		public double mesh_memory;
		public StringBuilder mesh_instances_log;
		public HashSet<int> mat_total;
		public ulong mat_instances;
		public StringBuilder mat_instances_log;
		public double mat_memory;
		public HashSet<int> texture_total;
		public double texture_memory;
		public ulong shader_passes;
		public ulong renderers_total;
		public ulong renderers_visible;
		public ulong renderers_batched;
		public HashSet<int> anim_total;
		public double anim_memory;
		public HashSet<int> audio_total;
		public double audio_memory;
		public ulong sprite_packed;

		public ObjectStat(string name)
		{
			this.name = name;
			mesh_tris = 0u;
			mesh_total = new HashSet<int>();
			mesh_instances_log = new StringBuilder();
			mesh_instances = 0u;
			mesh_memory = 0f;
			mat_total = new HashSet<int>();
			mat_instances = 0u;
			mat_instances_log = new StringBuilder();
			mat_memory = 0f;
			texture_total = new HashSet<int>();
			texture_memory = 0f;
			shader_passes = 0u;
			renderers_total = 0u;
			renderers_visible = 0u;
			renderers_batched = 0u;
			anim_total = new HashSet<int>();
			anim_memory = 0f;
			audio_total = new HashSet<int>();
			audio_memory = 0f;
			sprite_packed = 0u;
		}

		public override bool Equals(object obj)
		{
			return obj is ObjectStat && this.Equals((ObjectStat)obj);
		}

		public bool Equals(ObjectStat obj)
		{
			return name == obj.name
			&& mesh_tris == obj.mesh_tris
			&& mesh_total.Equals(obj.mesh_total)
			&& mesh_instances == obj.mesh_instances
			&& mesh_memory == obj.mesh_memory
			&& mat_total.Equals(obj.mat_total)
			&& mat_instances == obj.mat_instances
			&& mat_memory == obj.mat_memory
			&& texture_total.Equals(obj.texture_total)
			&& texture_memory == obj.texture_memory
			&& shader_passes == obj.shader_passes
			&& renderers_total == obj.renderers_total
			&& renderers_visible == obj.renderers_visible
			&& renderers_batched == obj.renderers_batched
			&& anim_total.Equals(obj.anim_total)
			&& anim_memory == obj.anim_memory
			&& audio_total.Equals(obj.audio_total)
			&& audio_memory == obj.audio_memory
			&& sprite_packed == obj.sprite_packed;
		}

		public override int GetHashCode()
		{
			return name.GetHashCode()
			^ mesh_tris.GetHashCode()
			^ mesh_total.GetHashCode()
			^ mesh_instances.GetHashCode()
			^ mesh_memory.GetHashCode()
			^ mat_total.GetHashCode()
			^ mat_instances.GetHashCode()
			^ mat_memory.GetHashCode()
			^ texture_total.GetHashCode()
			^ texture_memory.GetHashCode()
			^ shader_passes.GetHashCode()
			^ renderers_total.GetHashCode()
			^ renderers_visible.GetHashCode()
			^ renderers_batched.GetHashCode()
			^ anim_total.GetHashCode()
			^ anim_memory.GetHashCode()
			^ audio_total.GetHashCode()
			^ audio_memory.GetHashCode()
			^ sprite_packed.GetHashCode();
		}

		static string GetAppropriateSize(double mem)
		{
			int s = 0;
			while (mem > 1024f && ++s < _memory_suffixes.Length) {
				mem /= 1024f;
			}
			return mem.ToString("F") + _memory_suffixes[s];
		}

		public string TotalMem {
			get { return GetAppropriateSize(mesh_memory + mat_memory + texture_memory + anim_memory + audio_memory); }
		}

		public string MeshMem {
			get { return GetAppropriateSize(mesh_memory); }
		}

		public string MatMem {
			get { return GetAppropriateSize(mat_memory); }
		}

		public string TextureMem {
			get { return GetAppropriateSize(texture_memory); }
		}

		public string AnimMem {
			get { return GetAppropriateSize(anim_memory); }
		}

		public string AudioMem {
			get { return GetAppropriateSize(audio_memory); }
		}

		public override string ToString()
		{
			StringBuilder ret = new StringBuilder(name + "\n");
			ret.EnsureCapacity(512);

			ret.AppendLine(new string('-', name.Length));
			ret.AppendLine("");
//			ret.AppendFormat("{0, 0}{1, 40}\n", "test", TotalMem);
			ret.AppendLine("Total Memory : " + TotalMem);
			ret.AppendLine("Mesh Memory : " + MeshMem);
			ret.AppendLine("Material Memory : " + MatMem);
			ret.AppendLine("Texture Memory : " + TextureMem);
			ret.AppendLine("AnimationClips Memory : " + AnimMem);
			ret.AppendLine("AudioClip Memory : " + AudioMem);
			ret.AppendLine("Renderers : " + renderers_total);
			ret.AppendLine("Visible Renderers : " + renderers_visible);
			ret.AppendLine("Static Batches : " + renderers_batched);
			ret.AppendLine("Packed Sprites : " + sprite_packed);
			ret.AppendLine("Triangles : " + mesh_tris);
			ret.AppendLine("Meshes : " + mesh_total.Count);
			ret.AppendLine("Mesh Instances : " + mesh_instances);
			ret.AppendLine("Materials : " + mat_total.Count);
			ret.AppendLine("Material Instances : " + mat_instances);
			ret.AppendLine("Maximum Shader Passes : " + shader_passes);
			ret.AppendLine("Textures : " + texture_total.Count);
			ret.AppendLine("AnimationClips : " + anim_total.Count);
			ret.AppendLine("AudioClips : " + audio_total.Count);
			if (mesh_instances > 0) {
				ret.AppendLine("");
				ret.AppendLine("Mesh Instances : ");
				ret.Append(mesh_instances_log.ToString());
			}
			if (mat_instances > 0) {
				ret.AppendLine("");
				ret.AppendLine("Material Instances : ");
				ret.AppendLine(mat_instances_log.ToString());
			}
			ret.AppendLine("");
			return ret.ToString();
		}
	}

	public class NoScopeStatsPanel : EditorWindow
	{
		const uint _billboard_tris = 2u;

		[SerializeField] bool _display_in_edit_mode = true;
		[SerializeField] bool _enable_cache = false;

		ObjectStat[] _selected_objects = new ObjectStat[0];
		Dictionary<GameObject, ObjectStat> _cache = new Dictionary<GameObject, ObjectStat>();

		Vector2 _scroll_pos;
		GUIContent _refresh_button;
		GUIContent _log_button;
		GUIContent _editmode_button;
		GUIContent _disable_cache_button;

		[MenuItem("Window/NoScope Stats Panel")]
		public static void ShowWindow()
		{
			NoScopeStatsPanel w = EditorWindow.GetWindow<NoScopeStatsPanel>();
			TarfmagougouHelperNSSP.SetWindowTitle(w, "NoScopeStats");
		}

		void OnEnable()
		{
			_refresh_button = new GUIContent(EditorGUIUtility.IconContent("d_preAudioLoopOff").image,
				"Refresh : Delete the cache and reprocess the current selection.");
			_log_button = new GUIContent(EditorGUIUtility.IconContent("console.infoicon.sml").image,
				"Log Report : Output stats and extra available info on current selected object.");
			_editmode_button = new GUIContent("Edit Mode",
				"Update in Edit Mode : You can disable stats update during edit mode (helps performance on big scenes).");
			_disable_cache_button = new GUIContent("Cache",
				"Enable Cache : Use caching to speed up UI for big hierarchies.");
		}

		void OnGUI()
		{
			/* Toolbar */
			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
			if (GUILayout.Button(_refresh_button, EditorStyles.toolbarButton)) {
				OnHierarchyChange();
			}
			if (GUILayout.Button(_log_button, EditorStyles.toolbarButton)) {
				Log();
			}

			GUILayout.FlexibleSpace();

			_display_in_edit_mode = GUILayout.Toggle(_display_in_edit_mode, _editmode_button, EditorStyles.toolbarButton);
			_enable_cache = GUILayout.Toggle(_enable_cache, _disable_cache_button, EditorStyles.toolbarButton);
			EditorGUILayout.EndHorizontal();

			if (!_display_in_edit_mode && !EditorApplication.isPlaying) {
				return;
			}

			/* Data */
			EditorGUILayout.BeginVertical();
			_scroll_pos = EditorGUILayout.BeginScrollView(_scroll_pos);

			foreach (ObjectStat x in _selected_objects) {
				if (x.mesh_total == null || x.mat_total == null)
					continue;


				EditorGUILayout.Space();
				EditorGUILayout.LabelField(x.name + " - " + x.TotalMem, EditorStyles.boldLabel);

				EditorGUI.indentLevel++;
				EditorGUILayout.BeginHorizontal();
				EditorGUIUtility.labelWidth = 90;
				const int total_width = 130;
				EditorGUILayout.LabelField("Meshes : " + x.MeshMem, EditorStyles.miniLabel, GUILayout.Width(total_width));
				EditorGUILayout.LabelField("Materials : " + x.MatMem, EditorStyles.miniLabel);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Textures : " + x.TextureMem, EditorStyles.miniLabel, GUILayout.Width(total_width));
				EditorGUILayout.LabelField("AnimationClips : " + x.AnimMem, EditorStyles.miniLabel);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("AudioClips : " + x.AudioMem, EditorStyles.miniLabel, GUILayout.Width(total_width));
				EditorGUILayout.EndHorizontal();

//				EditorGUILayout.Space();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Renderers : " + x.renderers_total.ToString("N0"), EditorStyles.miniLabel, GUILayout.Width(total_width));
				EditorGUILayout.LabelField("Visible : " + x.renderers_visible.ToString("N0"), EditorStyles.miniLabel);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Static Batches : " + x.renderers_batched.ToString("N0"), EditorStyles.miniLabel, GUILayout.Width(total_width));
				EditorGUILayout.LabelField("Packed Sprites : " + x.sprite_packed.ToString("N0"), EditorStyles.miniLabel);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.Space();

				EditorGUIUtility.labelWidth = 125;
				EditorGUILayout.LabelField("Tris :", x.mesh_tris.ToString("N0"));
				EditorGUILayout.LabelField("Meshes :", x.mesh_total.Count.ToString("N0")
				+ " (" + x.mesh_instances.ToString("N0") + " instances)");
				EditorGUILayout.LabelField("Materials :", x.mat_total.Count.ToString("N0")
				+ " (" + x.mat_instances.ToString("N0") + " instances)");
				EditorGUILayout.LabelField("Shader Passes :", x.shader_passes.ToString("N0") + " (max)");
//				EditorGUILayout.LabelField("AnimationClips :", x.anim_total.Count.ToString("N0"));
//				EditorGUILayout.LabelField("AudioClips :", x.audio_total.Count.ToString("N0"));
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
		}

		/* User selected one or multiple objects, display and/or calculate stats. */
		void OnSelectionChange()
		{
			if (!_display_in_edit_mode && !EditorApplication.isPlaying)
				return;

			_selected_objects = new ObjectStat[Selection.gameObjects.Length];

			for (int i = 0; i < Selection.gameObjects.Length; ++i) {
				_selected_objects[i] = GetStats(Selection.gameObjects[i]);
			}

			Repaint();
		}

		/* Cache gets invalidated while editing scene. Reprocess stats. */
		void OnHierarchyChange()
		{
			_cache.Clear();
			OnSelectionChange();
		}

		/* Print report and extra stats to console. */
		void Log()
		{
			StringBuilder log = new StringBuilder("NoScope Stats Panel - Report\n\n");
			foreach (ObjectStat x in _selected_objects) {
				log.Append(x.ToString());
			}
			EditorGUIUtility.systemCopyBuffer = log.ToString();
			log.AppendLine("This report has been copied to your clipboard, for your convenience :)");

			TarfmagougouHelperNSSP.DeactivateLogTrace();
			Debug.Log(log);
			TarfmagougouHelperNSSP.ActivateLogTrace();
		}

		/* Return a cached stat object or generate new one. */
		ObjectStat GetStats(GameObject obj)
		{
			if (_cache.ContainsKey(obj)) {
				return _cache[obj];
			}
			ObjectStat ret = new ObjectStat(obj.name);
			RecurseStats(obj, ref ret);

			if (_enable_cache) {
				_cache[obj] = ret;
			}
			return ret;
		}

		/* Drill down hierarchy and collect stats. */
		static void RecurseStats(GameObject obj, ref ObjectStat ret)
		{
			bool is_static_batched = false;
			Renderer r = obj.GetComponent<Renderer>();
			if (r) {
				ret.renderers_total += 1u;
				ret.renderers_visible += r.isVisible ? 1u : 0u;
				is_static_batched = r.isPartOfStaticBatch;
				ret.renderers_batched += is_static_batched ? 1u : 0u;
				GetMaterialStats(r.sharedMaterials, ref ret, obj);
			}

			MeshFilter mf = obj.GetComponent<MeshFilter>();
			if (mf) {
				GetSharedMeshStats(mf.sharedMesh, ref ret, is_static_batched, obj);
			}

			SkinnedMeshRenderer smr = obj.GetComponent<SkinnedMeshRenderer>();
			if (smr) {
				GetSharedMeshStats(smr.sharedMesh, ref ret, is_static_batched, obj);
			}

			SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
			if (sr) {
				GetSpriteStats(sr.sprite, ref ret);
			}

			ParticleSystem ps = obj.GetComponent<ParticleSystem>();
			if (ps) {
				GetParticleStats(ps, ref ret);
			}

			Animator a = obj.GetComponent<Animator>();
			if (a) {
				GetAnimationClipStats(a, ref ret);
			}

			AudioSource au = obj.GetComponent<AudioSource>();
			if (au) {
				GetAudioStats(au.clip, ref ret);
			}

			for (int i = 0; i < obj.transform.childCount; ++i) {
				RecurseStats(obj.transform.GetChild(i).gameObject, ref ret);
			}

			/* TODO : Recurse on importers and assets. */
		}

		static void GetSharedMeshStats(Mesh sm, ref ObjectStat ret, bool is_static_batched, GameObject parent)
		{
			if (sm == null)
				return;

			/* First time we encounter or is an instance. */
			if (!ret.mesh_total.Contains(sm.GetInstanceID())) {
				ret.mesh_memory += (double)Profiler.GetRuntimeMemorySize(sm);
				ret.mesh_tris += (ulong)(sm.triangles.LongLength / 3);
				ret.mesh_total.Add(sm.GetInstanceID());
			} else if (!is_static_batched) {
				ret.mesh_tris += (ulong)(sm.triangles.LongLength / 3);
			}

			/* Is an instance. */
			if (sm.name.Contains("Instance")) {
				++ret.mesh_instances;
				ret.mesh_instances_log.AppendLine("    " + parent.name + " : " + sm.name);
			}
		}

		static void GetMaterialStats(Material[] mats, ref ObjectStat ret, GameObject parent)
		{
			if (mats == null)
				return;

			for (int i = 0; i < mats.Length; ++i) {
				if (mats[i] == null)
					continue;

				/* First time we encounter or is an instance. */
				if (!ret.mat_total.Contains(mats[i].GetInstanceID())) {
					ret.shader_passes += (ulong)mats[i].passCount;
					ret.mat_memory += (double)Profiler.GetRuntimeMemorySize(mats[i]);
					ret.mat_total.Add(mats[i].GetInstanceID());

					/* Get all textures from shader. */
					GetTextureStats(mats[i], ref ret);
				}

				/* Is an instance. */
				if (mats[i].name.Contains("Instance")) {
					++ret.mat_instances;
					ret.mat_instances_log.AppendLine("    " + parent.name + " : " + mats[i].name);
				}
			}
		}

		static void GetTextureStats(Material m, ref ObjectStat ret)
		{
			if (m == null)
				return;

			Shader s = m.shader;

			if (s == null)
				return;

			for (int i = 0; i < ShaderUtil.GetPropertyCount(s); ++i) {
				if (ShaderUtil.GetPropertyType(s, i) != ShaderUtil.ShaderPropertyType.TexEnv)
					continue;

				Texture t = m.GetTexture(ShaderUtil.GetPropertyName(s, i));
				if (t == null)
					continue;

				/* First encounter. */
				if (!ret.texture_total.Contains(t.GetInstanceID())) {
					ret.texture_memory += (double)Profiler.GetRuntimeMemorySize(t);
					ret.texture_total.Add(t.GetInstanceID());
				}
			}
		}

		static void GetParticleStats(ParticleSystem ps, ref ObjectStat ret)
		{
			if (ps == null)
				return;

			ParticleSystemRenderer psr = ps.GetComponent<ParticleSystemRenderer>();
			if (!psr)
				return;

			if (psr.renderMode != ParticleSystemRenderMode.Mesh) {
				ret.mesh_tris += _billboard_tris * (ulong)ps.particleCount;
			} else {
				ret.mesh_tris += (ulong)(psr.mesh.triangles.LongLength / 3) * (ulong)ps.particleCount;
				ret.mesh_total.Add(psr.mesh.GetInstanceID());
			}
		}

		static void GetAnimationClipStats(Animator a, ref ObjectStat ret)
		{
			RuntimeAnimatorController rac = a.runtimeAnimatorController;
			if (rac == null)
				return;
			
			AnimationClip[] acs = rac.animationClips;
			if (acs == null)
				return;

			for (int i = 0; i < acs.Length; ++i) {
				if (acs[i] == null)
					continue;

				/* First encounter. */
				if (!ret.anim_total.Contains(acs[i].GetInstanceID())) {
					ret.anim_memory += (double)Profiler.GetRuntimeMemorySize(acs[i]);
					ret.anim_total.Add(acs[i].GetInstanceID());
				}
			}
		}

		static void GetAudioStats(AudioClip ac, ref ObjectStat ret)
		{
			if (ac == null)
				return;

			if (!ret.audio_total.Contains(ac.GetInstanceID())) {
				ret.audio_memory += (double)Profiler.GetRuntimeMemorySize(ac);
				ret.audio_total.Add(ac.GetInstanceID());
			}
		}

		static void GetSpriteStats(Sprite s, ref ObjectStat ret)
		{
			if (s == null)
				return;

			ret.sprite_packed += s.packed ? 1u : 0u;
			ret.mesh_tris += (ulong)(s.triangles.LongLength / 3);

			if (s.texture != null) {
				if (!ret.texture_total.Contains(s.texture.GetInstanceID())) {
					ret.texture_memory += (double)Profiler.GetRuntimeMemorySize(s.texture);
					ret.texture_total.Add(s.texture.GetInstanceID());
				}
				
			}
			
			if (TarfmagougouHelperNSSP.GetAssociatedAlphaSplitTexture(s) != null) {
				if (!ret.texture_total.Contains(TarfmagougouHelperNSSP.GetAssociatedAlphaSplitTexture(s).GetInstanceID())) {
					ret.texture_memory += (double)Profiler.GetRuntimeMemorySize(TarfmagougouHelperNSSP.GetAssociatedAlphaSplitTexture(s));
					ret.texture_total.Add(TarfmagougouHelperNSSP.GetAssociatedAlphaSplitTexture(s).GetInstanceID());
				}				
			}
		}
	}
}
