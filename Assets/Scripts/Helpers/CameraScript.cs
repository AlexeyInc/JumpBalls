using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProgramAce.Spikes.Characters
{
	public class CameraScript : MonoBehaviour
	{
		[SerializeField] private float _orthographicSize = 5;
		[SerializeField] private float _aspect = 1.33333f;

		private void Start()
		{
			Camera.main.projectionMatrix = Matrix4x4.Ortho(
				-_orthographicSize * _aspect, _orthographicSize * _aspect,
				-_orthographicSize, _orthographicSize,
				Camera.main.nearClipPlane, Camera.main.farClipPlane);
		}
	}
}
