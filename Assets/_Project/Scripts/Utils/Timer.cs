using System;

namespace Platformer {
	public abstract class Timer {
		protected float initialTime;
		protected float Time { get; set; }
		public bool IsRunning { get; protected set; }

		public float Progress => Time / initialTime;
		public Action OnTimerStart = delegate { };
		public Action OnTimerStop = delegate { };

		protected Timer(float value) {
			initialTime = value;
			IsRunning = false;
		}
		public void Start() {
			Time = initialTime;
			if (!IsRunning) {
				IsRunning = true;
				OnTimerStart.Invoke();
			}
		}
		public void Stop() {
			if (IsRunning) {
				IsRunning = false;
				OnTimerStop.Invoke();
			}
		}
		public void Resume() => IsRunning = true;
		public void Pause() => IsRunning = false;

		public abstract void Tick(float deltaTime);
	}
	public class CountdownTimer : Timer {
		public CountdownTimer(float value) : base(value) { }
		public override void Tick(float deltaTime) {
			if (IsRunning) {
				if (Time <= 0f) {
					Stop();
					return;
				}
				Time -= deltaTime;
			}
		}
		public bool IsFinished => Time <= 0f;

		public void Reset() => Time = initialTime;

		public void Reset(float value) {
			initialTime = value;
			Reset();
		}
	}

	public class StopwatchTimer : Timer {
		public StopwatchTimer() : base(0f) { }
		public override void Tick(float deltaTime) {
			if (IsRunning) {
				Time += deltaTime;
			}
		}
		public void Reset() => Time = 0f;
	}
}
