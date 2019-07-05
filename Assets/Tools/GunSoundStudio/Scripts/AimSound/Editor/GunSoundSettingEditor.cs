using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace AimSound
{
    [CustomEditor(typeof(GunSoundSetting))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class GunSoundSettingEditor :Editor
    {
        
        public GunSoundSource previewSoundSource;
        void OnEnable()
        {
            CreateLineMat();
            var previewObject = new GameObject();
            previewObject.hideFlags = HideFlags.HideAndDontSave;
            previewObject.name = "previewSoundSource";
            previewSoundSource = previewObject.AddComponent<GunSoundSource>();
            previewSoundSource.setting = (GunSoundSetting)target;

            UpdateSoundPosition();
        }

        void OnDisable()
        {
            DestroyImmediate(previewSoundSource.gameObject);
        }

        AudioListener listener;
        public float listenerDistance;
        void UpdateSoundPosition()
        {
            if(!listener)
                listener = Object.FindObjectOfType<AudioListener>();
            if(listener)
            {
                var soundPosition = pointPosition;
                soundPosition.y = -soundPosition.y;
                soundPosition*=previewSoundSource.maxDistance;
                soundPosition = listener.transform.rotation * ((Vector3)soundPosition) +listener.transform.position;
                previewSoundSource.transform.position = soundPosition;
                listenerDistance = soundPosition.magnitude;
                previewSoundSource.UpdateDistanceAndVolume();
            }
        }
        bool playing;
        bool lastPlayDown;
        readonly string[] environmentType = new string[]{"outdoor","indoor",};
        public override void OnInspectorGUI() 
        {
            if(targets.Length==1)
            {
                var gunSoundSetting = (GunSoundSetting)target;
                var gunSoundSourceWindowType = System.Type.GetType("AimSound.GunSoundSourceWindow");
                if(gunSoundSourceWindowType!=null)
                {
                    if(GUILayout.Button("Open Editor Windows"))
                    {
                        gunSoundSourceWindowType.GetMethod("Init").Invoke(null,new object[]{ target });
                        // GunSoundSourceWindow.Init().gunSoundSetting = (GunSoundSetting)target;
                    }
                    GUILayout.Space(20);
                }
                if(GUILayout.Button("Create gun sound source"))
                {
                    CreateGunSoundGameObject(gunSoundSetting,Vector3.zero);
                }
                if(gunSoundSetting.isOneShot)
                    GUILayout.Label("single shot");
                else
                    GUILayout.Label("every minute "+(60f/gunSoundSetting.shotLoopInterval)+" shots");
                GUILayout.Space(10);
                
                var newEnvironmentType = (EnvironmentType)EditorGUILayout.Popup("Environment",(int)previewSoundSource.environmentType,environmentType);
                if(newEnvironmentType!=previewSoundSource.environmentType)
                {
                    previewSoundSource.environmentType = newEnvironmentType;
                }
                var newPlay = GUILayout.RepeatButton("Play");
                var eventType = Event.current.type;
                
                if(newPlay!=lastPlayDown && eventType==EventType.repaint)
                {
                    if(((GunSoundSetting)target).isOneShot)
                    {
                        if(newPlay)
                        {
                            previewSoundSource._Clear();
                            previewSoundSource.Play();
                        }
                    }
                    else
                    {
                        if(newPlay)
                        {
                            previewSoundSource._Clear();
                            previewSoundSource.Play();
                            playing = true;
                        }
                        else
                        {
                            previewSoundSource.Stop();
                            playing = false;
                        }
                    }
                    lastPlayDown = newPlay;
                }
                // if(playing)
                // {
                //     if(GUILayout.Button("Stop"))
                //     {
                //         SwitchPlay();
                //     }
                // }
                // else
                // {
                //     if(GUILayout.Button("Play"))
                //     {
                //         SwitchPlay();
                //     }
                // }

                var radius = (EditorGUIUtility.currentViewWidth-20)/2f;
                var lastRect = GUILayoutUtility.GetLastRect();
                var coordinateRect = new Rect(10,lastRect.yMax+10,radius*2,radius*2);
                EditorGUILayout.BeginVertical();
                DrawCoordinate(coordinateRect);
                GUILayout.Space(radius*2+20);
                EditorGUILayout.EndVertical();
            }
        }

        public Vector2 pointPosition;
        void onCoordinateDrag()
        {
            UpdateSoundPosition();
        }
        Material mat;
        void OnDestroy()
        {
            // rect = new Rect(500,100,500,500);
            Object.DestroyImmediate(mat);
        }
        void CreateLineMat()
        {

            var shader = Shader.Find("Hidden/Internal-Colored");
            mat = new Material(shader);
            mat.hideFlags = HideFlags.HideAndDontSave;
        }
        void DrawCoordinate(Rect circleRect)
        {

            var circle = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/AimSound/Textures/circle.png");
            // GUI.DrawTexture(new Rect(0,0,rect.width,rect.height),circle,ScaleMode.ScaleToFit);
            var radius = circleRect.width/2f;
            // var circleRect = new Rect(10,30,radius*2,radius*2);
            GUI.color = Color.gray;
            GUI.DrawTexture(circleRect,circle,ScaleMode.ScaleToFit);

            var center = circleRect.center;
            Vector2 positionInRect = center + pointPosition*radius;

            var eventType = Event.current.type;
            if(eventType==EventType.MouseDown || eventType==EventType.MouseDrag)
            {
                var mousePosition = Event.current.mousePosition;
                if((mousePosition - center).sqrMagnitude<=radius*radius)
                {
                    positionInRect = mousePosition;
                    pointPosition = (mousePosition - center)/radius;
                    Event.current.Use();
                    onCoordinateDrag();
                }
            }
            GUI.color = Color.white;
            var centerRadius = radius/20f;
            GUI.DrawTexture(new Rect(positionInRect.x-centerRadius,positionInRect.y-centerRadius,centerRadius*2,centerRadius*2),
                circle,ScaleMode.ScaleToFit);

            // EditorGUILayout.EndScrollView();

            //https://answers.unity.com/questions/1360515/how-do-i-draw-lines-in-a-custom-inspector.html
            if (Event.current.type == EventType.Repaint)
            {
                var centerX = center.x;
                var centerY = circleRect.height/2f;
                GUI.BeginClip(circleRect,Vector2.zero,clipRenderOffser,false);

                GL.PushMatrix();
                GL.Clear(true, false, Color.black);
                mat.SetPass(0);

                GL.Begin(GL.LINES);
                GL.Color(Color.black);
                GL.Vertex3(centerX - radius, centerY, 0);
                GL.Vertex3(centerX + radius, centerY, 0);

                GL.Vertex3(centerX , centerY- radius, 0);
                GL.Vertex3(centerX , centerY + radius, 0);

                GL.End();
                GL.PopMatrix();
                GUI.EndClip();
            }
        }
        Vector2 clipRenderOffser = new Vector2(9.5f,0);

#region create sound source

        //drag and drop
		struct SpawnMenuData {
			public Vector3 spawnPoint;
			public GunSoundSetting asset;
		}

		static GunSoundSettingEditor () 
        {
			// Hierarchy Icons
			// Drag and Drop
			SceneView.onSceneGUIDelegate -= SceneViewDragAndDrop;
			SceneView.onSceneGUIDelegate += SceneViewDragAndDrop;
			EditorApplication.hierarchyWindowItemOnGUI -= HierarchyDragAndDrop;
			EditorApplication.hierarchyWindowItemOnGUI += HierarchyDragAndDrop;
		}
        
		static void HierarchyDragAndDrop (int instanceId, Rect selectionRect)
         {
            
			var current = UnityEngine.Event.current;
			var eventType = current.type;
			bool isDraggingEvent = eventType == EventType.DragUpdated;
			bool isDropEvent = eventType == EventType.DragPerform;
			if (isDraggingEvent || isDropEvent)
             {
				var mouseOverWindow = EditorWindow.mouseOverWindow;
				if (mouseOverWindow != null) {

					// One, existing, valid SkeletonDataAsset
					var references = DragAndDrop.objectReferences;
					if (references.Length == 1) {
                        GameObject gameObject = references[0] as GameObject;
                        if(gameObject!=null && gameObject.scene.path==null && gameObject.GetComponent<GunSoundSetting>())
                        {
							var soundSetting = gameObject.GetComponent<GunSoundSetting>();
							const string HierarchyWindow = "UnityEditor.SceneHierarchyWindow";
							if (HierarchyWindow.Equals(mouseOverWindow.GetType().ToString(), System.StringComparison.Ordinal)) 
                            {
								if (isDraggingEvent) 
                                {
									DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
									current.Use();
								} 
                                else if (isDropEvent) 
                                {
									ShowInstantiateContextMenu(soundSetting, Vector3.zero);
									DragAndDrop.AcceptDrag();
									current.Use();
									return;
								}
							}
								
						}
					}
				}
			}

		}
		public static void ShowInstantiateContextMenu (GunSoundSetting soundSetting, Vector3 spawnPoint) {
			var menu = new GenericMenu();

			// SkeletonAnimation
			menu.AddItem(new GUIContent("Create Gun Sound Source"), false, HandleComponentDrop, new SpawnMenuData {
				asset = soundSetting,
				spawnPoint = spawnPoint,
			});

			menu.ShowAsContext();
		}

		public static void HandleComponentDrop (object menuData)
         {
			var data = (SpawnMenuData)menuData;

			var activeGameObject = Selection.activeGameObject;
            var gunSoundSource = CreateGunSoundGameObject(data.asset,data.spawnPoint);

			if (activeGameObject != null && activeGameObject.scene.path!=null)
				gunSoundSource.transform.parent = activeGameObject.transform.parent;
		}

        static GunSoundSource CreateGunSoundGameObject( GunSoundSetting soundSetting, Vector3 position )
        {
			GameObject newGameObject = new GameObject("AimSound Gun Sound Source("+soundSetting.gameObject.name+")");
            var gunSoundSource = newGameObject.AddComponent<GunSoundSource>();
            gunSoundSource.setting = soundSetting;
			var transform = newGameObject.transform;
            transform.position = position;

			Selection.activeGameObject = newGameObject;
            
			Undo.RegisterCreatedObjectUndo(newGameObject, "Create Gun Sound GameObject");
            return gunSoundSource;
        }

        
		static void SceneViewDragAndDrop (SceneView sceneview) 
        {
			var current = UnityEngine.Event.current;
			var references = DragAndDrop.objectReferences;
			if (current.type == EventType.Repaint || current.type == EventType.Layout) return;

			if (references.Length == 1) 
            {
                GameObject gameObject = references[0] as GameObject;
                if(gameObject!=null && gameObject.scene.path==null && gameObject.GetComponent<GunSoundSetting>())
                {

                    if (current.type == EventType.DragPerform)
                    {
                        var soundSetting = gameObject.GetComponent<GunSoundSetting>();
                        var mousePos = current.mousePosition;
                        #if UNITY_EDITOR_OSX
                        mousePos *= 2f;
                        #endif
                        Vector3 spawnPoint = MousePointToWorldPoint(mousePos, sceneview.camera);
                        ShowInstantiateContextMenu(soundSetting, spawnPoint);
                        DragAndDrop.AcceptDrag();
                        current.Use();
                    }
				}
			}
		}
        
		static Vector3 MousePointToWorldPoint (Vector2 mousePosition, Camera camera) 
        {
			var screenPos = new Vector3(mousePosition.x, camera.pixelHeight - mousePosition.y, 10f);
			return camera.ScreenToWorldPoint(screenPos);
		}
#endregion
    }

    
}