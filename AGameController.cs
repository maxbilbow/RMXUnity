using UnityEngine;
using System.Collections;

namespace RMX {
	public abstract class AGameController<T> : Singletons.ASingleton<T> , IGameController
	where T : AGameController<T> , IGameController {

		public bool _debugHUD;
		public float _maxDisplayTime = 5f;
		public bool DebugMisc;
		public bool DebugEarlyInits;
		public bool DebugGameCenter;
		public bool DebugAchievements;
		public bool DebugExceptions;
		public bool DebugSingletons;
		public bool DebugGameDataLists;
		public bool DebugDatabase;
		public bool DebugPatches;
		public bool DebugEvents;
		public bool ClearAchievementsOnLoad;


		public Font mainFont;
		public Color backgroundColor = Color.black;
		public Color textColor = Color.white;
		
		public bool DebugHUD {
			get {
				return _debugHUD;
			}
		}
		public TextAsset _database;
		public TextAsset Database { 
			get { 
				return _database;
			} 
		}
		
		public float MaxDisplayTime { 
			get {
				return _maxDisplayTime;
			}
		}


		public Vector2 defaultGravity = new Vector2 (0f, -9.81f);

		protected void Start() {
			PreStart ();
			if (DebugHUD) {
				Bugger.HUD.Initialize();
			}
			Physics2D.gravity = defaultGravity;
			WillBeginEvent (Events.SingletonInitialization);
			StartSingletons ();
			#if MOBILE_INPUT
			StartMobile();
			#else
			StartDesktop();
			#endif
			DidFinishEvent (Events.SingletonInitialization);
			PostStart ();
			
		}

		/// <summary>
		/// Do at beginnig of Start block
		/// </summary>
		protected virtual void PreStart () {}

		/// <summary>
		/// Do at end of Start block
		/// </summary>
		protected virtual void PostStart () {}

		/// <summary>
		/// Initialise any additional singletons here, especially if they are essential to the workings of your game
		/// </summary>
		protected abstract void StartSingletons ();

		/// <summary>
		/// Initialise any Destop specific settings here
		/// </summary>
		protected abstract void StartDesktop ();

		/// <summary>
		/// Initialise any Mobile specific settings here
		/// </summary>
		protected abstract void StartMobile ();

		/// <summary>
		/// Initiate any pre-load update patches here.
		/// </summary>
		public abstract void Patch ();
		
		
		public abstract void PauseGame (bool pause, object args);
	

		public virtual bool IsDebugging(string feature){
			if (Singletons.Settings != null) {
				if (feature == Testing.Misc)
					return DebugMisc;
				else if (feature == Testing.GameCenter)
					return DebugGameCenter;
				else if (feature == Testing.Achievements)
					return DebugAchievements;
				else if (feature == Testing.Exceptions)
					return DebugExceptions;
				else if (feature == Testing.Singletons)
					return DebugSingletons;
				else if (feature == Testing.Patches)
					return DebugPatches;
				else if (feature == Testing.Database)
					return DebugDatabase;
				else if (feature == Testing.EventCenter)
					return DebugEvents;
				else if (feature == Testing.EarlyInits)
					return DebugEarlyInits;
				else
					Debug.LogWarning (feature.ToString () + " has not been recorded in Settings.IsTesting(feature)");
				return false;
			} else {
				Debug.LogWarning ("Setting not initialized so debugging anyway: " + feature);
				return true;
			}
		}
		
	}
}