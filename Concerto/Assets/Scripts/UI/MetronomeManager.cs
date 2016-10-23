using UnityEngine;
using System.Collections;

public class MetronomeManager : MonoBehaviour {

	enum MetronomeModeEnum {CircleBeatBig, CircleBeatSmall, CircleTwinBeatSmall, Border};
	MetronomeModeEnum MetronomeMode;

	//enum CirclePosEnum {N, NE, E, SE, S, SW, W, NW};
	//CirclePosEnum CirclePos
	//{
	//	get{return CirclePos;}
	//	set{circlePosSetter(value);}
	//}

	enum CirclePosXEnum{West, Center, East};
	CirclePosXEnum CirclePosX;
	enum CirclePosYEnum{North, Center, South};
	CirclePosYEnum CirclePosY;

	enum BarBehaviorEnum {Off, Center, OneSided};
	BarBehaviorEnum BarBehavior;

	/// <summary>
	/// The circle X positions, in order from East, Center, and West
	/// </summary>
	Vector3 CircleXPositions;
	/// <summary>
	/// The circle Y positions, in order from North, Center, and South
	/// </summary>
	Vector3 CircleYPositions;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Toggles the bar behavior and circle pos when circle pos is set.
	/// To only be called in the setter for circlePos
	/// </summary>
	private void circlePosSetter(CirclePosXEnum xValue, CirclePosYEnum yValue)
	{
		switch (CirclePosX) {
		case CirclePosXEnum.East:
		{
			break;
		}
		case CirclePosXEnum.Center:
		{
			break;
		}
		case CirclePosXEnum.West:
		{
			break;
		}
		default:
			break;
		}

		switch (CirclePosY) {
		case CirclePosYEnum.North:
		{			 
			break;	
		}			 
		case CirclePosYEnum.Center:
		{			 
			break;	
		}			 
		case CirclePosYEnum.South:
		{
			break;
		}
		default:
			break;
		}
		//Actually set the value of the circle pos enum variable
		CirclePosX = xValue;
		CirclePosY = yValue;
	}
}
