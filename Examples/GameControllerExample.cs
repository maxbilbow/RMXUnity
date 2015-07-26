using UnityEngine;
using System.Collections;
using RMX;

namespace RMX.Examples {
	public enum MyTests {
		CustomTest
	}
	public class GameControllerExample : AGameController<GameControllerExample> {



		public bool DebugCustomTest;
		/// <summary>
		/// Do at beginnig of Start block
		/// </summary>
		protected override void PreStart () {


		}
		
		/// <summary>
		/// Do at end of Start block
		/// </summary>
		protected override void PostStart () {


		}
		
		/// <summary>
		/// Initialise any additional singletons here, especially if they are essential to the workings of your game
		/// </summary>
		protected override void StartSingletons () {

		}
		
		/// <summary>
		/// Initialise any Destop specific settings here
		/// </summary>
		protected override void StartDesktop () {

		}
		
		/// <summary>
		/// Initialise any Mobile specific settings here
		/// </summary>
		protected override void StartMobile () {

		}
		
		/// <summary>
		/// Initiate any pre-load update patches here.
		/// </summary>
		public override void Patch () {

		}

		/// <summary>
		/// Determines whether this instance is debugging the specified feature.
		/// </summary>
		/// <returns><c>true</c> if this instance is debugging the specified feature; otherwise, <c>false</c>.</returns>
		/// <param name="feature">Feature.</param>
		public override bool IsDebugging(System.Enum feature){
			if (feature.Equals( MyTests.CustomTest))
				return DebugCustomTest;
			else
				return base.IsDebugging (feature);
		}

		/// <summary>
		/// Pauses the game.
		/// </summary>
		/// <param name="pause">If set to <c>true</c> pause.</param>
		/// <param name="args">Arguments.</param>
		public override void PauseGame (bool pause, object args)
		{
			throw new System.NotImplementedException ();
		}
	}
}