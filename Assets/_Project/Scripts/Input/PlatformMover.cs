using UnityEngine;
using DG.Tweening;
using System;

namespace Platformer.Input {
	public class PlatformMover : MonoBehaviour {
		[SerializeField] private Vector3 _moveTo = Vector3.zero;
		[SerializeField] private float _moveTime = 1f;
		[SerializeField] private Ease _ease = Ease.InOutQuad;

		private Vector3 _startPosition;

		private void Start() {
			_startPosition = transform.position;
			Move();
		}

		private void Move() {
			transform.DOMove(_startPosition + _moveTo, _moveTime)
				.SetEase(_ease)
				.SetLoops(-1, LoopType.Yoyo);
		}
	}
}
