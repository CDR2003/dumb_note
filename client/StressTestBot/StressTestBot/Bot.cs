using Brotorift;
using DumbNote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StressTestBot
{
	public class Bot
	{
		private enum State
		{
			Connected,
			Registered,
			LoggedIn,
		}

		public const string Password = "123456";

		private Connector _connector;

		private string _username;

		private State _currentState;

		private string _serverAddress;

		private int _serverPort;

		public Bot( string username, string serverAddress, int serverPort )
		{
			_connector = new Connector();
			_username = username;
			_currentState = State.Connected;
			_serverAddress = serverAddress;
			_serverPort = serverPort;
		}

		public void Start()
		{
			while( _connector.CurrentState != ClientState.Connected )
			{
				this.TryConnect();
			}
		}

		public void Close()
		{
			_connector.Close();
		}

		public void Update()
		{
			switch( _currentState )
			{
				case State.Connected:
					_connector.RequestRegister( new UserInfo( _username, Password ) );
					break;
				case State.Registered:
					_connector.RequestLogin( new UserInfo( _username, Password ) );
					break;
				case State.LoggedIn:
					var note = Guid.NewGuid().ToString();
					_connector.CommitNote( note );
					break;
				default:
					break;
			}
			_connector.Update();
		}

		private void TryConnect()
		{
			try
			{
				_connector.Connect( _serverAddress, _serverPort );
			}
			catch( Exception )
			{
			}
		}
	}
}
