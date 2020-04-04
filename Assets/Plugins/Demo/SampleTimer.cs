using UnityEngine;
using System;
using System.Collections;

public class SampleTimer : MonoBehaviour {

	private DateTime simpleTimerEndTimestamp;
	private DateTime unbiasedTimerEndTimestamp;

	void Awake () {
		// Read PlayerPrefs to restore scheduled timers
		// By default initiliaze both timers in 60 seconds from now
		simpleTimerEndTimestamp = this.ReadTimestamp("simpleTimer", DateTime.Now.AddSeconds(60));
		unbiasedTimerEndTimestamp = this.ReadTimestamp("unbiasedTimer", UnbiasedTime.Instance.Now().AddSeconds(60));
	}
	
	void OnApplicationPause (bool paused) {
		if (paused) {
			this.WriteTimestamp("simpleTimer", simpleTimerEndTimestamp);
			this.WriteTimestamp("unbiasedTimer", unbiasedTimerEndTimestamp);
		}
		else {
			simpleTimerEndTimestamp = this.ReadTimestamp("simpleTimer", DateTime.Now.AddSeconds(60));
			unbiasedTimerEndTimestamp = this.ReadTimestamp("unbiasedTimer", UnbiasedTime.Instance.Now().AddSeconds(60));
		}
	}

	void OnApplicationQuit () {
		this.WriteTimestamp("simpleTimer", simpleTimerEndTimestamp);
		this.WriteTimestamp("unbiasedTimer", unbiasedTimerEndTimestamp);
	}

	void OnGUI () {
		// Calculate remaining time
		TimeSpan simpleRemaining = simpleTimerEndTimestamp - DateTime.Now;
		TimeSpan unbiasedRemaining = unbiasedTimerEndTimestamp - UnbiasedTime.Instance.Now();

		float w = Screen.width;
		float h = Screen.height;

		GUIStyle boxStyle = GUI.skin.box;
		boxStyle.fontSize = (int) (12 * h / 480);

		GUIStyle labelStyle = GUI.skin.label;
		labelStyle.fontSize = (int) (24 * h / 480);
		labelStyle.alignment = TextAnchor.UpperCenter;

		GUIStyle btnStyle = GUI.skin.button;
		btnStyle.fontSize = (int) (14 * h / 480);

		// Simple timer gui
		string simpleFormatted = "END";
		if (simpleRemaining.TotalSeconds > 0) {
			simpleFormatted = string.Format("{0}:{1:D2}:{2:D2}", simpleRemaining.Hours, simpleRemaining.Minutes, simpleRemaining.Seconds);
		}

		GUI.Box (new Rect(0.075f * w, 0.2f * h, 0.4f * w, 0.6f * h), "Simple timer", boxStyle);
		GUI.Label (new Rect(0.075f * w, 0.3f * h, 0.4f * w, 0.1f * h), simpleFormatted, labelStyle);

		if (GUI.Button (new Rect(0.1f * w, 0.5f * h, 0.35f * w, 0.1f * h), "+60 seconds", btnStyle)) {
			simpleTimerEndTimestamp = simpleTimerEndTimestamp.AddSeconds(60);
			this.WriteTimestamp("simpleTimer", simpleTimerEndTimestamp);
		}

		if (GUI.Button (new Rect(0.1f * w, 0.65f * h, 0.35f * w, 0.1f * h), "Reset", btnStyle)) {
			simpleTimerEndTimestamp = DateTime.Now.AddSeconds(60);
			this.WriteTimestamp("simpleTimer", simpleTimerEndTimestamp);
		}


		// Unbiased timer gui
		string unbiasedFormatted = "END";
		if (unbiasedRemaining.TotalSeconds > 0) {
			unbiasedFormatted = string.Format("{0}:{1:D2}:{2:D2}", unbiasedRemaining.Hours, unbiasedRemaining.Minutes, unbiasedRemaining.Seconds);
		}

		string unbiasedName;
		if (UnbiasedTime.Instance.IsUsingSystemTime()) {
			unbiasedName = "Unbiased fallback";
		}
		else {
			unbiasedName = "Unbiased timer";
		}

		GUI.Box (new Rect(0.525f * w, 0.2f * h, 0.4f * w, 0.6f * h), unbiasedName, boxStyle);
		GUI.Label (new Rect(0.525f * w, 0.3f * h, 0.4f * w, 0.1f * h), unbiasedFormatted, labelStyle);

		if (GUI.Button (new Rect(0.55f * w, 0.5f * h, 0.35f * w, 0.1f * h), "+60 seconds", btnStyle)) {
			unbiasedTimerEndTimestamp = unbiasedTimerEndTimestamp.AddSeconds(60);
			this.WriteTimestamp("unbiasedTimer", unbiasedTimerEndTimestamp);
		}

		if (GUI.Button (new Rect(0.55f * w, 0.65f * h, 0.35f * w, 0.1f * h), "Reset", btnStyle)) {
			unbiasedTimerEndTimestamp = UnbiasedTime.Instance.Now().AddSeconds(60);
			this.WriteTimestamp("unbiasedTimer", unbiasedTimerEndTimestamp);
		}
	}

	private DateTime ReadTimestamp (string key, DateTime defaultValue) {
		long tmp = Convert.ToInt64(PlayerPrefs.GetString(key, "0"));
		if ( tmp == 0 ) {
			return defaultValue;
		}
		return DateTime.FromBinary(tmp);
	}

	private void WriteTimestamp (string key, DateTime time) {
		PlayerPrefs.SetString(key, time.ToBinary().ToString());
	}
}
