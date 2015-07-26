using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace RMX {

	public interface KeyValueObserver {
		void OnValueForKeyWillChange(string key, object value);
		void OnValueForKeyDidChange(string key, object value);
	}
	public class RMXObject : MonoBehaviour, KeyValueObserver, EventListener {

	 	Dictionary<string, object> values = new Dictionary<string, object> ();
		List<KeyValueObserver> observers = new List<KeyValueObserver> ();


		/// <summary>
		/// Gets a value indicating whether this <see cref="RMX.ASingleton`1"/> add to global listeners.
		/// </summary>
		/// <value><c>true</c> if add to global listeners; otherwise, <c>false</c>.</value>
		private bool AddToGlobalListeners { 
			get {
				System.Type classType = this.GetType();
				foreach (string vMethod in ListenerMethods) {
					MethodInfo method = classType.GetMethod (vMethod);
					if (method.DeclaringType != typeof(RMXObject)) 
						return true;
				}
				return false;
			}
		}


		protected virtual void Awake() {
			if (AddToGlobalListeners)
				NotificationCenter.AddListener(this);
		}

//		protected virtual void OnDestroy() {
//			Notifications.RemoveListener (this);
//		}

		protected void WillBeginEvent(System.Enum theEvent){
			NotificationCenter.EventWillStart (theEvent);
		}

		protected void DidUpdateEvent(System.Enum theEvent) {
			NotificationCenter.EventWillStart (theEvent);
		}

		protected void DidFinishEvent(System.Enum theEvent){
			NotificationCenter.EventDidEnd (theEvent);
		}

		protected void DidCauseEvent(System.Enum theEvent){
			NotificationCenter.EventDidOccur (theEvent);
		}

		protected void WillBeginEvent(System.Enum theEvent, object info){
			NotificationCenter.EventWillStart (theEvent, info);
		}
		
		protected void DidUpdateEvent(System.Enum theEvent, object info) {
			NotificationCenter.EventWillStart (theEvent, info);
		}
		
		protected void DidFinishEvent(System.Enum theEvent, object info){
			NotificationCenter.EventDidEnd (theEvent, info);
		}

		protected void DidCauseEvent(System.Enum theEvent, object info){
			NotificationCenter.EventDidOccur (theEvent, info);
		}

		protected void WillChangeValueForKey(string key){
			foreach (KeyValueObserver observer in observers) {
				observer.OnValueForKeyWillChange(key, values[key]);
			}
		}
		
		protected void DidChangeValueForKey(string key) {
			foreach (KeyValueObserver observer in observers) {
				observer.OnValueForKeyDidChange(key, values[key]);
			}
		}

		public virtual void setValue(string forKey, object value) {
			if (values[forKey] != value) {
//				DidChangeValueForKey(forKey); ??
				values[forKey] = value;
				DidChangeValueForKey(forKey);
			}
		}

		public object getValue(string forKey) {
			return values [forKey];
		}

		public void AddObserver(KeyValueObserver observer) {
			if (!observers.Contains(observer))
				observers.Add(observer);
		}

		public void RemoveObserver(KeyValueObserver observer) {
			if (observers.Contains(observer))
				observers.Remove(observer);
		}


		public virtual void OnValueForKeyWillChange(string key, object value) {}
		public virtual void OnValueForKeyDidChange (string key, object value) {}

		protected static string[] ListenerMethods = {
			"OnEvent",
			"OnEventDidStart",
			"OnEventDidEnd"
		};

		public virtual void OnEvent(System.Enum theEvent, object args) {}
		
		public virtual void OnEventDidStart(System.Enum theEvent, object args){}
		
		public virtual void OnEventDidEnd(System.Enum theEvent, object args){}
		public static bool OneIn10 {
			get {
				return Random.Range(0,10) == 1;
			}
		}
	}
}
