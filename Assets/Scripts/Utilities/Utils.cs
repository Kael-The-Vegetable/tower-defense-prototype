using UnityEngine;

public static class Utils
{
	public static Vector3 PointerToWorldOnXZPlane(Camera camera, Vector2 mousePosOnScreen)
	{
		Vector3 relative = camera.ScreenToWorldPoint(new Vector3(mousePosOnScreen.x, mousePosOnScreen.y, camera.farClipPlane));
		Vector3 camPos = camera.transform.position;
		Vector3 difference = relative - camPos;
		if (difference.y >= 0) difference.y = -1f;
		float delta = -camPos.y / difference.y;
		return camPos + delta * difference;
	}
}
