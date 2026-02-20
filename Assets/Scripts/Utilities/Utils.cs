using UnityEngine;

public static class Utils
{
	public static Vector3 PointerToWorldXZ(Camera camera, Vector2 mousePosOnScreen)
	{
		if (!camera.pixelRect.Contains(mousePosOnScreen))
		{ Debug.Log("OUTSIDE RANGE"); return Vector3.zero; }

		Ray ray = camera.ScreenPointToRay(mousePosOnScreen);
		float diff = Vector3.Dot(Vector3.up, ray.direction);
		if (Mathf.Abs(diff) < float.Epsilon)
			return Vector3.zero;

		float delta = Vector3.Dot(-ray.origin, Vector3.up) / diff;
		return ray.origin + ray.direction * delta;
	}
}
