using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

internal class test : MonoBehaviour {

	private string s_Region;
	private List<Beacon> mybeacons = new List<Beacon>();

	private void Start() {
		s_Region = "iBeacon";
		BluetoothState.Init();

		iBeaconReceiver.BeaconRangeChangedEvent += OnBeaconRangeChanged;
		iBeaconReceiver.regions = new iBeaconRegion[]{new iBeaconRegion(s_Region, new Beacon())};
		iBeaconReceiver.Scan();
	}

	void DebugText(String str){
		Text textView1 = GameObject.Find("Canvas/Text").GetComponent<Text>();
		textView1.text = str;
	}
	
	void FindBeacons(){
		String beacon_str = "";
		foreach(Beacon b in mybeacons) {
			beacon_str = beacon_str + b.UUID.ToString() + " " + b.major.ToString() + " " + b.minor.ToString() + "\n\n";
		}
		DebugText(beacon_str);
	}
	
	private void OnBeaconRangeChanged(Beacon[] beacons) {
		foreach (Beacon b in beacons) {
			var index = mybeacons.IndexOf(b);
			if (index == -1) {
				mybeacons.Add(b);
			} else {
				mybeacons[index] = b;
			}
		}
		for (int i = mybeacons.Count - 1; i >= 0; --i) {
			if (mybeacons[i].lastSeen.AddSeconds(10) < DateTime.Now) {
				mybeacons.RemoveAt(i);
			}
		}
		FindBeacons();
	}
	
}