using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XboxCtrlrInput;
using XInputDotNetPure;

public class OnScreenKeyboard : MonoBehaviour
{
	public enum RotationPoint
	{
		NORTH,
		NORTHEAST,
		EAST,
		SOUTHEAST,
		SOUTH,
		SOUTHWEST,
		WEST,
		NORTHWEST,
		NONE }
	;
	public enum AlphabetLetters
	{
		A,
		B,
		C,
		D,
		E,
		F,
		G,
		H,
		I,
		J,
		K,
		L,
		M,
		N,
		O,
		P,
		Q,
		R,
		S,
		T,
		U,
		V,
		W,
		X,
		Y,
		Z,
		EXTRA1,
		EXTRA2,
		EXTRA3,
		EXTRA4,
		EXTRA5,
		EXTRA6,
		NONE
	}
	public enum MenuOption
	{
		UPPERCASE,
		LOWERCASE,
		NUMBERS,
		NONE
	}

	public string[] _lowerCaseLetterArray = new string[32];
	public string[] _upperCaseLetterArray = new string[32];
	public string[] _numberLetterArray = new string[32];

	public RectTransform selectorTransform, buttonTransform;
	public GameObject centreCircle;
	public Text textObject;
	public Text[] letterArray;
	public int maxStringLength;
	public Font textFont;
	public GameObject xboxStickSprite;
	public GameObject startButtonSprite;

	private RotationPoint _selectedRotation;
	private AlphabetLetters _selectedLetter;
	private MenuOption _selectedOption;
	private bool _confirmName = false;

	void Start ()
	{
		textObject.text = "";
		_selectedOption = MenuOption.LOWERCASE;
		SetFont ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (!(textObject.text == "")) {
			xboxStickSprite.SetActive (false);
			startButtonSprite.SetActive (true);
		} else {
			xboxStickSprite.SetActive (true);
			startButtonSprite.SetActive (false);
		}

		if (!_confirmName) {
			SpecialCases ();
			_selectedOption = SwapMenuModes ();
			ChangeDisplayedLetters (_selectedOption);

			_selectedRotation = GetControllerAxis ();
			if (textObject.text.Length < maxStringLength) {
				_selectedLetter = SelectionLetterInput (_selectedRotation);
				InterpretInformation (_selectedOption, _selectedLetter);
			}

			if (_selectedRotation == RotationPoint.NONE) {
				centreCircle.SetActive (true);
				selectorTransform.gameObject.SetActive (false);
			} else {
				centreCircle.SetActive (false);
				selectorTransform.gameObject.SetActive (true);
			}
		}
	}

	RotationPoint GetControllerAxis ()
	{
		//Get left analogue stick input (movement)
		float _hori = XCI.GetAxis (XboxAxis.LeftStickX);
		float _vert = XCI.GetAxis (XboxAxis.LeftStickY);
		float rotAngle = ((Mathf.Atan2 (-_hori, _vert) * (180 / Mathf.PI)));

		//Set the Angle of the rotation of the selector
		if (_hori == 0 && _vert == 0) {
			return RotationPoint.NONE;
		}
		if (rotAngle > 157) {
			selectorTransform.rotation = Quaternion.Euler (0, 0, 180);
			buttonTransform.rotation = Quaternion.Euler (0, 0, 0);
			return RotationPoint.SOUTH;
		} else if (rotAngle > 112) {
			selectorTransform.rotation = Quaternion.Euler (0, 0, 135);
			buttonTransform.rotation = Quaternion.Euler (0, 0, 0);
			return RotationPoint.SOUTHWEST;
		} else if (rotAngle > 67) {
			selectorTransform.rotation = Quaternion.Euler (0, 0, 90);
			buttonTransform.rotation = Quaternion.Euler (0, 0, 0);
			return RotationPoint.WEST;
		} else if (rotAngle > 22) {
			selectorTransform.rotation = Quaternion.Euler (0, 0, 45);
			buttonTransform.rotation = Quaternion.Euler (0, 0, 0);
			return RotationPoint.NORTHWEST;
		} else if (rotAngle > -22) {
			selectorTransform.rotation = Quaternion.Euler (0, 0, 0);
			buttonTransform.rotation = Quaternion.Euler (0, 0, 0);
			return RotationPoint.NORTH;
		} else if (rotAngle > -67) {
			selectorTransform.rotation = Quaternion.Euler (0, 0, -45);
			buttonTransform.rotation = Quaternion.Euler (0, 0, 0);
			return RotationPoint.NORTHEAST;
		} else if (rotAngle > -112) {
			selectorTransform.rotation = Quaternion.Euler (0, 0, -90);
			buttonTransform.rotation = Quaternion.Euler (0, 0, 0);
			return RotationPoint.EAST;
		} else if (rotAngle > -157) {
			selectorTransform.rotation = Quaternion.Euler (0, 0, -135);
			buttonTransform.rotation = Quaternion.Euler (0, 0, 0);
			return RotationPoint.SOUTHEAST;
		} else if (rotAngle < -135) {
			selectorTransform.rotation = Quaternion.Euler (0, 0, -180);
			buttonTransform.rotation = Quaternion.Euler (0, 0, 0);
			return RotationPoint.SOUTH;
		}
		return RotationPoint.NONE;
	}

	AlphabetLetters SelectionLetterInput (RotationPoint _currentRotation)
	{
		switch (_currentRotation) {
            #region NorthSelection
		case RotationPoint.NORTH:
			if (XCI.GetButtonUp (XboxButton.Y)) {
				return AlphabetLetters.B;
			} else if (XCI.GetButtonUp (XboxButton.X)) {
				return AlphabetLetters.A;
			} else if (XCI.GetButtonUp (XboxButton.B)) {
				return AlphabetLetters.C;
			} else if (XCI.GetButtonUp (XboxButton.A)) {
				return AlphabetLetters.D;
			}
			break;
            #endregion
            #region NorthEastSelection
		case RotationPoint.NORTHEAST:
			if (XCI.GetButtonUp (XboxButton.Y)) {
				return AlphabetLetters.F;
			} else if (XCI.GetButtonUp (XboxButton.X)) {
				return AlphabetLetters.E;
			} else if (XCI.GetButtonUp (XboxButton.B)) {
				return AlphabetLetters.G;
			} else if (XCI.GetButtonUp (XboxButton.A)) {
				return AlphabetLetters.H;
			}
			break;
            #endregion
            #region EastSelection
		case RotationPoint.EAST:
			if (XCI.GetButtonUp (XboxButton.Y)) {
				return AlphabetLetters.J;
			} else if (XCI.GetButtonUp (XboxButton.X)) {
				return AlphabetLetters.I;
			} else if (XCI.GetButtonUp (XboxButton.B)) {
				return AlphabetLetters.K;
			} else if (XCI.GetButtonUp (XboxButton.A)) {
				return AlphabetLetters.L;
			}
			break;
            #endregion
            #region SouthEastSelection
		case RotationPoint.SOUTHEAST:
			if (XCI.GetButtonUp (XboxButton.Y)) {
				return AlphabetLetters.N;
			} else if (XCI.GetButtonUp (XboxButton.X)) {
				return AlphabetLetters.M;
			} else if (XCI.GetButtonUp (XboxButton.B)) {
				return AlphabetLetters.O;
			} else if (XCI.GetButtonUp (XboxButton.A)) {
				return AlphabetLetters.P;
			}
			break;
            #endregion
            #region SouthSelection
		case RotationPoint.SOUTH:
			if (XCI.GetButtonUp (XboxButton.Y)) {
				return AlphabetLetters.R;
			} else if (XCI.GetButtonUp (XboxButton.X)) {
				return AlphabetLetters.Q;
			} else if (XCI.GetButtonUp (XboxButton.B)) {
				return AlphabetLetters.S;
			} else if (XCI.GetButtonUp (XboxButton.A)) {
				return AlphabetLetters.T;
			}
			break;
            #endregion
            #region SouthWestSelection
		case RotationPoint.SOUTHWEST:
			if (XCI.GetButtonUp (XboxButton.Y)) {
				return AlphabetLetters.V;
			} else if (XCI.GetButtonUp (XboxButton.X)) {
				return AlphabetLetters.U;
			} else if (XCI.GetButtonUp (XboxButton.B)) {
				return AlphabetLetters.W;
			} else if (XCI.GetButtonUp (XboxButton.A)) {
				return AlphabetLetters.X;
			}
			break;
            #endregion
            #region WestSelection
		case RotationPoint.WEST:
			if (XCI.GetButtonUp (XboxButton.Y)) {
				return AlphabetLetters.Z;
			} else if (XCI.GetButtonUp (XboxButton.X)) {
				return AlphabetLetters.Y;
			} else if (XCI.GetButtonUp (XboxButton.B)) {
				return AlphabetLetters.EXTRA1;
			} else if (XCI.GetButtonUp (XboxButton.A)) {
				return AlphabetLetters.EXTRA2;
			}
			break;
            #endregion
            #region NorthWestSelection
		case RotationPoint.NORTHWEST:
			if (XCI.GetButtonUp (XboxButton.Y)) {
				return AlphabetLetters.EXTRA4;
			} else if (XCI.GetButtonUp (XboxButton.X)) {
				return AlphabetLetters.EXTRA3;
			} else if (XCI.GetButtonUp (XboxButton.B)) {
				return AlphabetLetters.EXTRA5;
			} else if (XCI.GetButtonUp (XboxButton.A)) {
				return AlphabetLetters.EXTRA6;
			}
			break;
                #endregion
		}
		return AlphabetLetters.NONE;
	}

	void InterpretInformation (MenuOption _menuType, AlphabetLetters _letter)
	{
		if (_letter != AlphabetLetters.NONE) {
			switch (_menuType) {
                #region UpperCaseLetters
			case MenuOption.UPPERCASE:
				switch (_letter) {
				case AlphabetLetters.A:
					textObject.text = textObject.text + _upperCaseLetterArray [0];
					break;
				case AlphabetLetters.B:
					textObject.text = textObject.text + _upperCaseLetterArray [1];
					break;
				case AlphabetLetters.C:
					textObject.text = textObject.text + _upperCaseLetterArray [2];
					break;
				case AlphabetLetters.D:
					textObject.text = textObject.text + _upperCaseLetterArray [3];
					break;
				case AlphabetLetters.E:
					textObject.text = textObject.text + _upperCaseLetterArray [4];
					break;
				case AlphabetLetters.F:
					textObject.text = textObject.text + _upperCaseLetterArray [5];
					break;
				case AlphabetLetters.G:
					textObject.text = textObject.text + _upperCaseLetterArray [6];
					break;
				case AlphabetLetters.H:
					textObject.text = textObject.text + _upperCaseLetterArray [7];
					break;
				case AlphabetLetters.I:
					textObject.text = textObject.text + _upperCaseLetterArray [8];
					break;
				case AlphabetLetters.J:
					textObject.text = textObject.text + _upperCaseLetterArray [9];
					break;
				case AlphabetLetters.K:
					textObject.text = textObject.text + _upperCaseLetterArray [10];
					break;
				case AlphabetLetters.L:
					textObject.text = textObject.text + _upperCaseLetterArray [11];
					break;
				case AlphabetLetters.M:
					textObject.text = textObject.text + _upperCaseLetterArray [12];
					break;
				case AlphabetLetters.N:
					textObject.text = textObject.text + _upperCaseLetterArray [13];
					break;
				case AlphabetLetters.O:
					textObject.text = textObject.text + _upperCaseLetterArray [14];
					break;
				case AlphabetLetters.P:
					textObject.text = textObject.text + _upperCaseLetterArray [15];
					break;
				case AlphabetLetters.Q:
					textObject.text = textObject.text + _upperCaseLetterArray [16];
					break;
				case AlphabetLetters.R:
					textObject.text = textObject.text + _upperCaseLetterArray [17];
					break;
				case AlphabetLetters.S:
					textObject.text = textObject.text + _upperCaseLetterArray [18];
					break;
				case AlphabetLetters.T:
					textObject.text = textObject.text + _upperCaseLetterArray [19];
					break;
				case AlphabetLetters.U:
					textObject.text = textObject.text + _upperCaseLetterArray [20];
					break;
				case AlphabetLetters.V:
					textObject.text = textObject.text + _upperCaseLetterArray [21];
					break;
				case AlphabetLetters.W:
					textObject.text = textObject.text + _upperCaseLetterArray [22];
					break;
				case AlphabetLetters.X:
					textObject.text = textObject.text + _upperCaseLetterArray [23];
					break;
				case AlphabetLetters.Y:
					textObject.text = textObject.text + _upperCaseLetterArray [24];
					break;
				case AlphabetLetters.Z:
					textObject.text = textObject.text + _upperCaseLetterArray [25];
					break;
				case AlphabetLetters.EXTRA1:
					textObject.text = textObject.text + _upperCaseLetterArray [26];
					break;
				case AlphabetLetters.EXTRA2:
					textObject.text = textObject.text + _upperCaseLetterArray [27];
					break;
				case AlphabetLetters.EXTRA3:
					textObject.text = textObject.text + _upperCaseLetterArray [28];
					break;
				case AlphabetLetters.EXTRA4:
					textObject.text = textObject.text + _upperCaseLetterArray [29];
					break;
				case AlphabetLetters.EXTRA5:
					textObject.text = textObject.text + _upperCaseLetterArray [30];
					break;
				case AlphabetLetters.EXTRA6:
					textObject.text = textObject.text + _upperCaseLetterArray [31];
					break;
				}
				break;
                #endregion
                #region LowerCaseLetters
			case MenuOption.LOWERCASE:
				switch (_letter) {
				case AlphabetLetters.A:
					textObject.text = textObject.text + _lowerCaseLetterArray [0];
					break;
				case AlphabetLetters.B:
					textObject.text = textObject.text + _lowerCaseLetterArray [1];
					break;
				case AlphabetLetters.C:
					textObject.text = textObject.text + _lowerCaseLetterArray [2];
					break;
				case AlphabetLetters.D:
					textObject.text = textObject.text + _lowerCaseLetterArray [3];
					break;
				case AlphabetLetters.E:
					textObject.text = textObject.text + _lowerCaseLetterArray [4];
					break;
				case AlphabetLetters.F:
					textObject.text = textObject.text + _lowerCaseLetterArray [5];
					break;
				case AlphabetLetters.G:
					textObject.text = textObject.text + _lowerCaseLetterArray [6];
					break;
				case AlphabetLetters.H:
					textObject.text = textObject.text + _lowerCaseLetterArray [7];
					break;
				case AlphabetLetters.I:
					textObject.text = textObject.text + _lowerCaseLetterArray [8];
					break;
				case AlphabetLetters.J:
					textObject.text = textObject.text + _lowerCaseLetterArray [9];
					break;
				case AlphabetLetters.K:
					textObject.text = textObject.text + _lowerCaseLetterArray [10];
					break;
				case AlphabetLetters.L:
					textObject.text = textObject.text + _lowerCaseLetterArray [11];
					break;
				case AlphabetLetters.M:
					textObject.text = textObject.text + _lowerCaseLetterArray [12];
					break;
				case AlphabetLetters.N:
					textObject.text = textObject.text + _lowerCaseLetterArray [13];
					break;
				case AlphabetLetters.O:
					textObject.text = textObject.text + _lowerCaseLetterArray [14];
					break;
				case AlphabetLetters.P:
					textObject.text = textObject.text + _lowerCaseLetterArray [15];
					break;
				case AlphabetLetters.Q:
					textObject.text = textObject.text + _lowerCaseLetterArray [16];
					break;
				case AlphabetLetters.R:
					textObject.text = textObject.text + _lowerCaseLetterArray [17];
					break;
				case AlphabetLetters.S:
					textObject.text = textObject.text + _lowerCaseLetterArray [18];
					break;
				case AlphabetLetters.T:
					textObject.text = textObject.text + _lowerCaseLetterArray [19];
					break;
				case AlphabetLetters.U:
					textObject.text = textObject.text + _lowerCaseLetterArray [20];
					break;
				case AlphabetLetters.V:
					textObject.text = textObject.text + _lowerCaseLetterArray [21];
					break;
				case AlphabetLetters.W:
					textObject.text = textObject.text + _lowerCaseLetterArray [22];
					break;
				case AlphabetLetters.X:
					textObject.text = textObject.text + _lowerCaseLetterArray [23];
					break;
				case AlphabetLetters.Y:
					textObject.text = textObject.text + _lowerCaseLetterArray [24];
					break;
				case AlphabetLetters.Z:
					textObject.text = textObject.text + _lowerCaseLetterArray [25];
					break;
				case AlphabetLetters.EXTRA1:
					textObject.text = textObject.text + _lowerCaseLetterArray [26];
					break;
				case AlphabetLetters.EXTRA2:
					textObject.text = textObject.text + _lowerCaseLetterArray [27];
					break;
				case AlphabetLetters.EXTRA3:
					textObject.text = textObject.text + _lowerCaseLetterArray [28];
					break;
				case AlphabetLetters.EXTRA4:
					textObject.text = textObject.text + _lowerCaseLetterArray [29];
					break;
				case AlphabetLetters.EXTRA5:
					textObject.text = textObject.text + _lowerCaseLetterArray [30];
					break;
				case AlphabetLetters.EXTRA6:
					textObject.text = textObject.text + _lowerCaseLetterArray [31];
					break;
				}
				break;
                #endregion
                #region NumberLetters
			case MenuOption.NUMBERS:
				switch (_letter) {
				case AlphabetLetters.A:
					textObject.text = textObject.text + _numberLetterArray [0];
					break;
				case AlphabetLetters.B:
					textObject.text = textObject.text + _numberLetterArray [1];
					break;
				case AlphabetLetters.C:
					textObject.text = textObject.text + _numberLetterArray [2];
					break;
				case AlphabetLetters.D:
					textObject.text = textObject.text + _numberLetterArray [3];
					break;
				case AlphabetLetters.E:
					textObject.text = textObject.text + _numberLetterArray [4];
					break;
				case AlphabetLetters.F:
					textObject.text = textObject.text + _numberLetterArray [5];
					break;
				case AlphabetLetters.G:
					textObject.text = textObject.text + _numberLetterArray [6];
					break;
				case AlphabetLetters.H:
					textObject.text = textObject.text + _numberLetterArray [7];
					break;
				case AlphabetLetters.I:
					textObject.text = textObject.text + _numberLetterArray [8];
					break;
				case AlphabetLetters.J:
					textObject.text = textObject.text + _numberLetterArray [9];
					break;
				case AlphabetLetters.K:
					textObject.text = textObject.text + _numberLetterArray [10];
					break;
				case AlphabetLetters.L:
					textObject.text = textObject.text + _numberLetterArray [11];
					break;
				case AlphabetLetters.M:
					textObject.text = textObject.text + _numberLetterArray [12];
					break;
				case AlphabetLetters.N:
					textObject.text = textObject.text + _numberLetterArray [13];
					break;
				case AlphabetLetters.O:
					textObject.text = textObject.text + _numberLetterArray [14];
					break;
				case AlphabetLetters.P:
					textObject.text = textObject.text + _numberLetterArray [15];
					break;
				case AlphabetLetters.Q:
					textObject.text = textObject.text + _numberLetterArray [16];
					break;
				case AlphabetLetters.R:
					textObject.text = textObject.text + _numberLetterArray [17];
					break;
				case AlphabetLetters.S:
					textObject.text = textObject.text + _numberLetterArray [18];
					break;
				case AlphabetLetters.T:
					textObject.text = textObject.text + _numberLetterArray [19];
					break;
				case AlphabetLetters.U:
					textObject.text = textObject.text + _numberLetterArray [20];
					break;
				case AlphabetLetters.V:
					textObject.text = textObject.text + _numberLetterArray [21];
					break;
				case AlphabetLetters.W:
					textObject.text = textObject.text + _numberLetterArray [22];
					break;
				case AlphabetLetters.X:
					textObject.text = textObject.text + _numberLetterArray [23];
					break;
				case AlphabetLetters.Y:
					textObject.text = textObject.text + _numberLetterArray [24];
					break;
				case AlphabetLetters.Z:
					textObject.text = textObject.text + _numberLetterArray [25];
					break;
				case AlphabetLetters.EXTRA1:
					textObject.text = textObject.text + _numberLetterArray [26];
					break;
				case AlphabetLetters.EXTRA2:
					textObject.text = textObject.text + _numberLetterArray [27];
					break;
				case AlphabetLetters.EXTRA3:
					textObject.text = textObject.text + _numberLetterArray [28];
					break;
				case AlphabetLetters.EXTRA4:
					textObject.text = textObject.text + _numberLetterArray [29];
					break;
				case AlphabetLetters.EXTRA5:
					textObject.text = textObject.text + _numberLetterArray [30];
					break;
				case AlphabetLetters.EXTRA6:
					textObject.text = textObject.text + _numberLetterArray [31];
					break;
				}
				break;
                    #endregion
			}
		}
	}

	MenuOption SwapMenuModes ()
	{
		if ((XCI.GetAxisRaw (XboxAxis.RightTrigger)) > 0.2) {
			return MenuOption.NUMBERS;
		}
		if ((XCI.GetAxisRaw (XboxAxis.LeftTrigger)) < 0.2) {
			return MenuOption.LOWERCASE;
		}
		if ((XCI.GetAxisRaw (XboxAxis.LeftTrigger)) > 0.2) {
			return MenuOption.UPPERCASE;
		}

		return MenuOption.NONE;
	}

	void ChangeDisplayedLetters (MenuOption _menuType)
	{
		if (_menuType != MenuOption.NONE) {
			switch (_menuType) {
                #region UppercaseLetterSwitch
			case MenuOption.UPPERCASE:
				letterArray [0].text = _upperCaseLetterArray [0];
				letterArray [1].text = _upperCaseLetterArray [1];
				letterArray [2].text = _upperCaseLetterArray [2];
				letterArray [3].text = _upperCaseLetterArray [3];
				letterArray [4].text = _upperCaseLetterArray [4];
				letterArray [5].text = _upperCaseLetterArray [5];
				letterArray [6].text = _upperCaseLetterArray [6];
				letterArray [7].text = _upperCaseLetterArray [7];
				letterArray [8].text = _upperCaseLetterArray [8];
				letterArray [9].text = _upperCaseLetterArray [9];
				letterArray [10].text = _upperCaseLetterArray [10];
				letterArray [11].text = _upperCaseLetterArray [11];
				letterArray [12].text = _upperCaseLetterArray [12];
				letterArray [13].text = _upperCaseLetterArray [13];
				letterArray [14].text = _upperCaseLetterArray [14];
				letterArray [15].text = _upperCaseLetterArray [15];
				letterArray [16].text = _upperCaseLetterArray [16];
				letterArray [17].text = _upperCaseLetterArray [17];
				letterArray [18].text = _upperCaseLetterArray [18];
				letterArray [19].text = _upperCaseLetterArray [19];
				letterArray [20].text = _upperCaseLetterArray [20];
				letterArray [21].text = _upperCaseLetterArray [21];
				letterArray [22].text = _upperCaseLetterArray [22];
				letterArray [23].text = _upperCaseLetterArray [23];
				letterArray [24].text = _upperCaseLetterArray [24];
				letterArray [25].text = _upperCaseLetterArray [25];
				letterArray [26].text = _upperCaseLetterArray [26];
				letterArray [27].text = _upperCaseLetterArray [27];
				letterArray [28].text = _upperCaseLetterArray [28];
				letterArray [29].text = _upperCaseLetterArray [29];
				letterArray [30].text = _upperCaseLetterArray [30];
				letterArray [31].text = _upperCaseLetterArray [31];
				break;
                #endregion
                #region LowercaseLetterSwitch
			case MenuOption.LOWERCASE:
				letterArray [0].text = _lowerCaseLetterArray [0];
				letterArray [1].text = _lowerCaseLetterArray [1];
				letterArray [2].text = _lowerCaseLetterArray [2];
				letterArray [3].text = _lowerCaseLetterArray [3];
				letterArray [4].text = _lowerCaseLetterArray [4];
				letterArray [5].text = _lowerCaseLetterArray [5];
				letterArray [6].text = _lowerCaseLetterArray [6];
				letterArray [7].text = _lowerCaseLetterArray [7];
				letterArray [8].text = _lowerCaseLetterArray [8];
				letterArray [9].text = _lowerCaseLetterArray [9];
				letterArray [10].text = _lowerCaseLetterArray [10];
				letterArray [11].text = _lowerCaseLetterArray [11];
				letterArray [12].text = _lowerCaseLetterArray [12];
				letterArray [13].text = _lowerCaseLetterArray [13];
				letterArray [14].text = _lowerCaseLetterArray [14];
				letterArray [15].text = _lowerCaseLetterArray [15];
				letterArray [16].text = _lowerCaseLetterArray [16];
				letterArray [17].text = _lowerCaseLetterArray [17];
				letterArray [18].text = _lowerCaseLetterArray [18];
				letterArray [19].text = _lowerCaseLetterArray [19];
				letterArray [20].text = _lowerCaseLetterArray [20];
				letterArray [21].text = _lowerCaseLetterArray [21];
				letterArray [22].text = _lowerCaseLetterArray [22];
				letterArray [23].text = _lowerCaseLetterArray [23];
				letterArray [24].text = _lowerCaseLetterArray [24];
				letterArray [25].text = _lowerCaseLetterArray [25];
				letterArray [26].text = _lowerCaseLetterArray [26];
				letterArray [27].text = _lowerCaseLetterArray [27];
				letterArray [28].text = _lowerCaseLetterArray [28];
				letterArray [29].text = _lowerCaseLetterArray [29];
				letterArray [30].text = _lowerCaseLetterArray [30];
				letterArray [31].text = _lowerCaseLetterArray [31];
				break;
                #endregion
                #region NumberLetterSwitch
			case MenuOption.NUMBERS:
				letterArray [0].text = _numberLetterArray [0];
				letterArray [1].text = _numberLetterArray [1];
				letterArray [2].text = _numberLetterArray [2];
				letterArray [3].text = _numberLetterArray [3];
				letterArray [4].text = _numberLetterArray [4];
				letterArray [5].text = _numberLetterArray [5];
				letterArray [6].text = _numberLetterArray [6];
				letterArray [7].text = _numberLetterArray [7];
				letterArray [8].text = _numberLetterArray [8];
				letterArray [9].text = _numberLetterArray [9];
				letterArray [10].text = _numberLetterArray [10];
				letterArray [11].text = _numberLetterArray [11];
				letterArray [12].text = _numberLetterArray [12];
				letterArray [13].text = _numberLetterArray [13];
				letterArray [14].text = _numberLetterArray [14];
				letterArray [15].text = _numberLetterArray [15];
				letterArray [16].text = _numberLetterArray [16];
				letterArray [17].text = _numberLetterArray [17];
				letterArray [18].text = _numberLetterArray [18];
				letterArray [19].text = _numberLetterArray [19];
				letterArray [20].text = _numberLetterArray [20];
				letterArray [21].text = _numberLetterArray [21];
				letterArray [22].text = _numberLetterArray [22];
				letterArray [23].text = _numberLetterArray [23];
				letterArray [24].text = _numberLetterArray [24];
				letterArray [25].text = _numberLetterArray [25];
				letterArray [26].text = _numberLetterArray [26];
				letterArray [27].text = _numberLetterArray [27];
				letterArray [28].text = _numberLetterArray [28];
				letterArray [29].text = _numberLetterArray [29];
				letterArray [30].text = _numberLetterArray [30];
				letterArray [31].text = _numberLetterArray [31];
				break;
                    #endregion
			}
		}
	}

	void SpecialCases ()
	{
		//BackSpace Simulation
		if (XCI.GetButtonUp (XboxButton.LeftBumper) || XCI.GetButtonUp (XboxButton.RightBumper)) {
			if (textObject.text.Length != 0) {
				textObject.text = textObject.text.Substring (0, textObject.text.Length - 1);
			}
		}
		if (XCI.GetButtonUp (XboxButton.Start) && textObject.text.Length >= 1) {
			_confirmName = true;
		}
	}

	void SetFont ()
	{
		foreach (var item in letterArray) {
			item.font = textFont;
		}
	}

	public string ReturnUsername ()
	{
		if (_confirmName) {
			return textObject.text.ToString ();
			;
		}
		return "";
	}
}