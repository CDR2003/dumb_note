using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace DumbNote
{
	public class RegisterPanel : MonoBehaviour
	{
		public InputField txtUsername;

		public InputField txtPassword;

		public Button btnRegister;

		public Button btnBack;

		private void Awake()
		{
			this.btnRegister.onClick.AddListener( this.OnRegisterClicked );
			this.btnBack.onClick.AddListener( this.OnBackClicked );
		}

		private void OnDestroy()
		{
			this.btnRegister.onClick.RemoveListener( this.OnRegisterClicked );
			this.btnBack.onClick.RemoveListener( this.OnBackClicked );
		}

		private void OnRegisterClicked()
		{
			var info = new UserInfo( txtUsername.text, txtPassword.text );
			DumbNoteGame.instance.connector.RequestRegister( info );
		}

		private void OnBackClicked()
		{
			DumbNoteGame.instance.ShowLoginPanel();
		}
	}
}