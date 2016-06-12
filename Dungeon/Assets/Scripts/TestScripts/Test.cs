using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class Test : MonoBehaviour
{
	public bool blToolTipOn;
	public Camera cam;
	public Canvas canvas;
	
	// Update is called once per frame
	void Update()
	{
		if (blToolTipOn)
		{
			
			RectTransform rt = GetComponent<RectTransform>();
			Vector3 input = Input.mousePosition; 
			input.x -= ((rt.rect.width * canvas.scaleFactor) / 2); // The important part!
			
			// In my case, I needed to do this aswell, you probable don't need this in your setup and can just set rt.position with the input instead
			Vector3 output;
			RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, input, cam, out output);
			rt.position = output;
		}
	}
}