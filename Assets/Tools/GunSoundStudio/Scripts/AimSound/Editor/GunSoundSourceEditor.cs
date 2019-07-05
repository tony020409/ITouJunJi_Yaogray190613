using UnityEngine;
using UnityEditor;

namespace AimSound
{
    [CustomEditor(typeof(GunSoundSource))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class GunSoundSourceEditor:Editor
    {
        [MenuItem("GameObject/Audio/AimSound Gun Sound Source")]
        static void CreateGameObject(MenuCommand menuCommand)
        {
            // Create a custom game object
            GameObject go = new GameObject("AimSound Gun Sound Source");
            Undo.RegisterCreatedObjectUndo(go, "Create Gun Sound GameObject");
            go.AddComponent<GunSoundSource>();
            Selection.activeObject = go;
        }

        GunSoundSource lastPlaying;
        void OnDisable()
        {
			StopPlay();
        }
        void StartPlay(GunSoundSource gunSoundSource)
        {
			gunSoundSource.Play();
			if(!gunSoundSource.setting.isOneShot)
			{
				lastPlaying = gunSoundSource;
				EditorApplication.update += OnUpdateAudio;
			}
        }
		void OnUpdateAudio()
		{
			if(lastPlaying)
			{
				lastPlaying.UpdateDistanceAndVolume();
			}
			else
				EditorApplication.update -= OnUpdateAudio;
		}
        void StopPlay()
        {
			if(lastPlaying && lastPlaying.needStop)
			{
				lastPlaying.Stop();
			}
			EditorApplication.update -= OnUpdateAudio;
            lastPlaying = null;
        }
        public override void OnInspectorGUI() 
        {
            if(targets.Length==1)
            {
                var gunSoundSource = (GunSoundSource)target;
                if(lastPlaying)
                {
                    if(GUILayout.Button("Stop"))
                    {
						StopPlay();
                    }
                }
                else if(gunSoundSource.setting && gunSoundSource.gameObject.scene.path!=null)
                {
                    if(GUILayout.Button("Play"))
                    {
						StartPlay(gunSoundSource);
                    }
                }
            }
            
            base.OnInspectorGUI();
        }

    }
}