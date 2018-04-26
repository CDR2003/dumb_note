using UnityEngine;
using System.Collections;
using System;
using Brotorift;

namespace DumbNote
{
	public class DumbNoteGame : MonoBehaviour
	{
		public static DumbNoteGame instance { get; private set; }

		public float reconnectInterval = 3.0f;

		public RegisterPanel pnlRegister;

		public LoginPanel pnlLogin;

		public NotePanel pnlNote;

		public RectTransform pnlConnecting;

		public DumbNoteServerConnector connector { get; private set; }

		private bool _reconnecting = true;

		private float _currentTime = 0.0f;

		public void ShowLoginPanel()
		{
			this.HideAllPanels();
			pnlLogin.gameObject.SetActive( true );
		}

		public void ShowRegisterPanel()
		{
			this.HideAllPanels();
			pnlRegister.gameObject.SetActive( true );
		}

		public void ShowNotePanel()
		{
			this.HideAllPanels();
			pnlNote.gameObject.SetActive( true );
		}

		public void UpdateNote( string note )
		{
			pnlNote.UpdateNote( note );
		}

		private void Awake()
		{
			instance = this;
			this.connector = new DumbNoteServerConnector();
			this.connector.connect += this.OnConnected;
		}

		private void Start()
		{
			this.BeginConnect();
		}

		private void OnDestroy()
		{
			this.connector.Close();
		}

		private void Update()
		{
			if( _reconnecting )
			{
				_currentTime += Time.deltaTime;
				if( _currentTime >= this.reconnectInterval )
				{
					this.BeginConnect();
				}
			}

			this.connector.Update();
			if( this.connector.CurrentState == ClientState.Disconnected )
			{
				if( _reconnecting == false )
				{
					_reconnecting = true;
					_currentTime = 0.0f;
					Debug.Log( "Server connection lost. Reconnecting..." );
					this.BeginConnect();
				}
			}
		}

		private void HideAllPanels()
		{
			pnlConnecting.gameObject.SetActive( false );
			pnlLogin.gameObject.SetActive( false );
			pnlRegister.gameObject.SetActive( false );
			pnlNote.gameObject.SetActive( false );
		}

		private void ShowConnectingPanel()
		{
			this.HideAllPanels();
			pnlConnecting.gameObject.SetActive( true );
		}

		private void BeginConnect()
		{
			_currentTime = 0.0f;

			Debug.Log( "BeginConnect." );
			this.connector.BeginConnect( "localhost", 9000 );
			this.ShowConnectingPanel();
		}

		private void OnConnected()
		{
			_reconnecting = false;

			this.ShowLoginPanel();
			Debug.Log( "Connected." );
		}
	}
}