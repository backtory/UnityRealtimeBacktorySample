using System.Threading;

namespace Realtime.Messaging.Internal {
	public class CountDownLatch {
		private int m_remain;
		private EventWaitHandle m_event;
		
		public CountDownLatch(int count) {
			m_remain = count;
			m_event = new ManualResetEvent(false);
		}
		
		public void Signal() {
			// The last thread to signal also sets the event.
			if (Interlocked.Decrement(ref m_remain) == 0)
				m_event.Set();
		}
		
		public bool Wait(int millisecondsTimeout) {
			return m_event.WaitOne (millisecondsTimeout);
		}
	}
}