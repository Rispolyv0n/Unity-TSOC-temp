using UnityEngine;
using System;

public enum BeaconRange {
	UNKNOWN,
	FAR,
	NEAR,
	IMMEDIATE
}

public enum BeaconType {
	Any,
	iBeacon,
	EddystoneUID,
	EddystoneURL,
	EddystoneEID,
}

[Serializable]
public class Beacon : IEquatable<Beacon> {
	public BeaconType type {
		get {
			return _type;
		}
	}

	public string UUID {
		get {
			return _UUID;
		}
	}

	public int major {
		get {
			return _major;
		}
	}

	public int minor {
		get {
			return _minor;
		}
	}

	public string instance {
		get {
			return _instance;
		}
	}

	public int rssi {
		get {
			return _rssi;
		}
	}

	public BeaconRange range {
		get {
			return _range;
		}
	}

	public int strength {
		get {
			return _strength;
		}
	}

	public double accuracy {
		get {
			return _accuracy;
		}
	}

	public DateTime lastSeen {
		get {
			return _lastSeen;
		}
	}

	public Telemetry telemetry {
		get {
			if (_telemetry == null) {
				try {
					_telemetry = new Telemetry(_rawTelemetry);
				} catch (Exception) {
				}
			}
			return _telemetry;
		}
	}

	public string regionName {
		get {
			return _regionName;
		}
	}

	[SerializeField]
	private BeaconType _type;
	[SerializeField]
	[BeaconPropertyAttribute(true, "_type", BeaconType.Any, BeaconType.EddystoneEID)]
	private string _UUID;
	[SerializeField]
	[BeaconPropertyAttribute("_type", BeaconType.iBeacon)]
	private int _major;
	[SerializeField]
	[BeaconPropertyAttribute("_type", BeaconType.iBeacon)]
	private int _minor;
	[SerializeField]
	[BeaconPropertyAttribute("_type", BeaconType.EddystoneUID)]
	private string _instance;

	[SerializeField]
	[HideInInspector]
	private int _rssi;
	[SerializeField]
	[HideInInspector]
	private BeaconRange _range;
	[SerializeField]
	[HideInInspector]
	private int _strength;
	[SerializeField]
	[HideInInspector]
	private double _accuracy;
	[SerializeField]
	[HideInInspector]
	private string _rawTelemetry;
	[SerializeField]
	[HideInInspector]
	private string _regionName;

	private DateTime _lastSeen;
	private Telemetry _telemetry;

	public Beacon()
		: this(BeaconType.Any) {
	}

	public Beacon(BeaconType _type)
		: this(_type, "", 0, 0, "", BeaconRange.UNKNOWN, 127, -1, 127) {
		if (_type != BeaconType.Any && _type != BeaconType.EddystoneEID) {
			throw new ArgumentException("Only BeaconType.Any or BeaconType.EddystoneEID is allowed in this overloaded constructor.");
		}
	}

	public Beacon(string _uuid, int _major, int _minor)
		: this(_uuid, _major, _minor, BeaconRange.UNKNOWN, 127, -1, 127) {
	}

	private Beacon(string _uuid, int _major, int _minor, BeaconRange _range, int _strength, double _accuracy, int _rssi)
		: this(BeaconType.iBeacon, _uuid, _major, _minor, "", _range, _strength, _accuracy, _rssi) {
	}

	public Beacon(string _namespace, string _instance)
		: this(_namespace, _instance, BeaconRange.UNKNOWN, 127, -1, 127) {
	}

	private Beacon(string _namespace, string _instance, BeaconRange _range, int _strength, double _accuracy, int _rssi)
		: this(BeaconType.EddystoneUID, _namespace, 0, 0, _instance, _range, _strength, _accuracy, _rssi) {
	}

	public Beacon(string _url)
		: this(_url, BeaconRange.UNKNOWN, 127, -1, 127) {
	}

	private Beacon(string _url, BeaconRange _range, int _strength, double _accuracy, int _rssi)
		: this(BeaconType.EddystoneURL, _url, 0, 0, "", _range, _strength, _accuracy, _rssi) {
	}

	private Beacon(BeaconType _type, string _uuid, int _major, int _minor, string _instance, BeaconRange _range, int _strength, double _accuracy, int _rssi) {
		this._type = _type;
		this._UUID = _uuid;
		this._major = _major;
		this._minor = _minor;
		this._instance = _instance;
		this._range = _range;
		this._strength = _strength;
		this._accuracy = _accuracy;
		this._lastSeen = DateTime.Now;
		this._rssi = _rssi;
	}

	public override string ToString() {
		switch (type) {
		case BeaconType.iBeacon:
			return "Type: iBeacon\nUUID: " + this.UUID + "\nMajor: " + this.major + "\nMinor: " + this.minor + "\nRange: " + this.range.ToString();
		case BeaconType.EddystoneUID:
			return "Type: Eddystone-UID\nNamespace: " + this.UUID + "\nInstance: " + this.instance + "\nRange: " + this.range.ToString();
		case BeaconType.EddystoneURL:
			return "Type: Eddystone-URL\nURL: " + this.UUID + "\nRange: " + this.range.ToString();
		case BeaconType.EddystoneEID:
			return "Type: Eddystone-Eid\nEphemeral Identifier: " + this.UUID + "\nRange: " + this.range.ToString();
		default:
			return base.ToString();
		}
	}

	public bool Equals(Beacon other) {
		if (other == null) {
			return false;
		}

		if (type != other.type) {
			return false;
		}

		if (_UUID != null) {
			_UUID = _UUID.ToUpper();
		}
		if (_instance != null) {
			_instance = _instance.ToUpper();
		}

		switch (type) {
		case BeaconType.iBeacon:
			return UUID.Equals(other.UUID) && major.Equals(other.major) && minor.Equals(other.minor);
		case BeaconType.EddystoneUID:
			return UUID.Equals(other.UUID) && instance.Equals(other.instance);
		case BeaconType.EddystoneURL:
			return UUID.Equals(other.UUID);
		}

		return base.Equals(other);
	}

	public override bool Equals(object obj) {
		if (obj == null) {
			return false;
		}

		Beacon beaconObj = obj as Beacon;
		if (beaconObj == null) {
			return false;
		}

		return Equals(beaconObj);
	}

	public override int GetHashCode() {
		if (_UUID != null) {
			_UUID = _UUID.ToUpper();
		}
		if (_instance != null) {
			_instance = _instance.ToUpper();
		}

		switch (type) {
		case BeaconType.iBeacon:
			return type.GetHashCode() ^ UUID.GetHashCode() ^ major.GetHashCode() ^ minor.GetHashCode();
		case BeaconType.EddystoneUID:
			return type.GetHashCode() ^ UUID.GetHashCode() ^ instance.GetHashCode();
		case BeaconType.EddystoneURL:
			return type.GetHashCode() ^ UUID.GetHashCode();
		}

		return base.GetHashCode();
	}

	public static bool operator ==(Beacon beacon1, Beacon beacon2) {
		if (((object)beacon1) == null || ((object)beacon2) == null) {
			return object.Equals(beacon1, beacon2);
		}

		return beacon1.Equals(beacon2);
	}

	public static bool operator !=(Beacon beacon1, Beacon beacon2) {
		return !(beacon1 == beacon2);
	}

	public void ResetLastSeen() {
		_lastSeen = DateTime.Now;
	}
}
