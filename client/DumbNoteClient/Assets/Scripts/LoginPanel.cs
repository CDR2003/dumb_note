using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace DumbNote
{
	public class LoginPanel : MonoBehaviour
	{
		public InputField txtUsername;

		public InputField txtPassword;

		public Button btnLogin;

		public Button btnRegister;

		private void Awake()
		{
			this.btnLogin.onClick.AddListener( this.OnLoginClicked );
			this.btnRegister.onClick.AddListener( this.OnRegisterClicked );
		}

		private void OnDestroy()
		{
			this.btnLogin.onClick.RemoveListener( this.OnLoginClicked );
			this.btnRegister.onClick.RemoveListener( this.OnRegisterClicked );
		}

		private void OnLoginClicked()
		{
			var info = new UserInfo( txtUsername.text, txtPassword.text );
			DumbNoteGame.instance.connector.RequestLogin( info );
		}

		private void OnRegisterClicked()
		{
			DumbNoteGame.instance.ShowRegisterPanel();
		}
	}
}