namespace Tuner.Net
{
	public class AgentHolder
	{


		public virtual void  Init (ITNetAdapter adapter)
		{
			mAdapter = adapter;
		}

		protected ITNetAdapter mAdapter = null;

		public ITNetAdapter Adapter {
			get {
				return mAdapter;
			}
		}

		protected ITNetWriter mWriter = null;

		public ITNetWriter Writer {
			get {
				if (mWriter == null) {
					mWriter = mAdapter.createMsgWriter ();
				}
				return mWriter;			
			}
		}

		protected ITNetReader mReader = null;

		public ITNetReader Reader {
			get {
				if (mReader == null) {
					mReader = mAdapter.createMsgReader ();
				}
				return mReader;
			}
		}

		public virtual void closeAgent (Agent agent)
		{
			agent.Release ();
		}
				
		public virtual void Update ()
		{
			//Update the contain Agent.
		}
	}
}