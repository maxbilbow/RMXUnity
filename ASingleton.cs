// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace RMX
{


	public interface IGameController : ISingleton {
		void PauseGame(bool pause);
		void PauseGame (bool pause, object args);
		void Patch();
	}
	public interface ISettings : ISingleton {
		bool PrintToScreen { get; set; }
		TextAsset Database { get; }
		bool IsDebugging(string feature);
		float MaxDisplayTime { get;}
	}

	public interface ISingleton {
		string name { get; }
//		ISingleton Singleton { get; set; }
		bool Destroyed { get; }
		GameObject gameObject { get; }
	}

	public abstract class AGameController<T> : Singletons.ASingleton<T> , IGameController
	where T : AGameController<T> , IGameController {
		public Vector2 defaultGravity = new Vector2 (0f, -9.81f);
		protected void Start() {
			Physics2D.gravity = defaultGravity;
			WillBeginEvent (Events.SingletonInitialization);
			Bugger.Initialize ();
			Notifications.Initialize ();
			StartSingletons ();
			#if MOBILE_INPUT
			StartMobile();
			#else
			StartDesktop();
			#endif
			DidFinishEvent (Events.SingletonInitialization);
			
		}

		protected abstract void StartSingletons ();
		protected abstract void StartDesktop ();
		protected abstract void StartMobile ();
		public abstract void Patch ();
		public void PauseGame(bool pause) {
			PauseGame (pause, null);
		}

		public abstract void PauseGame (bool pause, object args);
	}

	public abstract class ASettings<T> : Singletons.ASingleton<T> , ISettings
	where T : ASettings<T> , ISettings {
		public bool DebugMisc;
		public bool DebugGameCenter;
		public bool DebugAchievements;
		public bool DebugExceptions;
		public bool DebugSingletons;
		public bool DebugGameDataLists;
		public bool DebugDatabase;
		public bool DebugPatches;
		public bool DebugEvents;
		public bool ClearAchievementsOnLoad;

		public abstract bool PrintToScreen { get; set; }
		public abstract TextAsset Database { get; }
		public abstract bool IsDebugging(string feature);
		public abstract float MaxDisplayTime { get;}


	}

	public static class Singletons {

//		static bool _gameControllerInitialized = false;
		public static bool GameControllerInitialized {
			get {
				return _gameController != null;//_gameControllerInitialized;
			}
		}
//		static bool _settingsInitialized = false;
		public static bool SettingsInitialized {
			get {
				return _settings != null;//_settingsInitialized;
			}
		}

		static IGameController _gameController;
	 	public static IGameController GameController {
			get{ 
				return _gameController;
			}
		}
		static ISettings _settings;
		public static ISettings Settings {
			get {
				return _settings;
			}
		}


		public abstract class ASingleton<T> : RMXObject, ISingleton, EventListener
		where T : RMXObject, EventListener, ISingleton 
		{

			private static T _singleton = null;

			private static bool _isInitialized = false;


			protected virtual bool SetupComplete {
				get{ 
					return true;
				}
			}

			public static bool IsInitialized {
				get {
					return _isInitialized && (_singleton as ASingleton<T>).SetupComplete;
				}
			}



			protected virtual IGameController gameController { 
				get {
					return _gameController;
				}
			}
	
			protected virtual ISettings settings {
				get {
					return _settings;
				}
			}
	
			public static T current {
				get {
					if (IsInitialized) {
						return _singleton as T;
					} else {
						return Initialize() as T;
					}
				}
			}

			const string tempName = "324329hrNhfeuwh9";
			private bool _destroyed = false;
			public bool Destroyed {
				get {
					return _destroyed;
				}
			}




			public static T Initialize() {
				if (IsInitialized) 
					return _singleton;
				else {
					var aSingleton = new GameObject (tempName).AddComponent<T> ();
					if ((aSingleton as ISingleton).Destroyed) {
						return null;
					}
					aSingleton.gameObject.name = aSingleton.GetType ().Name;
	
					if (!(aSingleton is IGameController) && Singletons.GameControllerInitialized) {
						var parent = Singletons.GameController.gameObject;
						aSingleton.gameObject.transform.SetParent (parent.transform);
					}
	
					return aSingleton;
				} 
			}

			/// <summary>
			/// Gets a value indicating whether this <see cref="RMX.ASingleton`1"/> add to global listeners.
			/// </summary>
			/// <value><c>true</c> if add to global listeners; otherwise, <c>false</c>.</value>
			private bool AddToGlobalListeners { 
				get {
					System.Type classType = typeof(T);
					foreach (string vMethod in ListenerMethods) {
						MethodInfo method = classType.GetMethod (vMethod);
						if (method.DeclaringType != typeof(RMXObject)) 
							return true;
					}
					return false;
				}
			}


			private void MainInitCheck() {
				if (this is IGameController && _gameController == null) {
					Singletons._gameController = this as IGameController;
					_gameController.Patch();
				}
				else if (this is ISettings && _settings == null) 
					Singletons._settings = this as ISettings;
			 	else if (_gameController == null)
					Debug.LogWarning ("GameController should be initialized before " + this.GetType().Name);
				else if (_settings == null)
					Debug.LogWarning ("Settings should be initialized before " + this.GetType().Name);
			}

			/// <summary>
			/// Checks whether a singleton already exists. If so, object is destroyed.
			/// Otherwise it checks whether the EventListener methods have been overriden. If so, the object is added to the global EventListeners.
			/// </summary>
			protected void Awake() {
				var message = "__new__ <color=lightblue>" + this.GetType().Name + "</color>()";
				if (_singleton == null) {
					DontDestroyOnLoad (gameObject);
					_singleton = this as T;// as T;
					if (AddToGlobalListeners)
						Notifications.AddListener(this);
					MainInitCheck();
				} 
				else if (_singleton != this) {
					if (gameObject.name == tempName) {// gameObject.name == this.GetType().Name &&
						message += " -- <color=red> DELETING REDUNDANT " + this.GetType().Name + "</color>()";
						_destroyed = true;
						Destroy (gameObject);
						Destroy (this);
					} else {
						message += " -- <color=orange> DELETING REDUNDANT ASingleton: </color> " + this.GetType().Name + "</color>()";
						_destroyed = true;
						Destroy(this);
					}
				}
				if (Bugger.WillLog (Testing.Singletons, message))
					Debug.Log (Bugger.Last);
				_isInitialized = true;
			}

		}
	}
}

