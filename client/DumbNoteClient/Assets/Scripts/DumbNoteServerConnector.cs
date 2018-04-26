using UnityEngine;
using System.Collections;

namespace DumbNote
{
	public class DumbNoteServerConnector : DumbNoteServerConnectorBase
	{
		protected override void RespondRegister( RegisterResult result )
		{
			Debug.Log( "Register: " + result );

			if( result == RegisterResult.Succeeded )
			{
				DumbNoteGame.instance.ShowLoginPanel();
			}
		}

		protected override void RespondLogin( LoginResult result )
		{
			Debug.Log( "Login: " + result );

			if( result == LoginResult.Succeeded )
			{
				DumbNoteGame.instance.ShowNotePanel();
			}
		}

		protected override void UpdateNote( string note )
		{
			Debug.Log( "Update Note: " + note );

			DumbNoteGame.instance.UpdateNote( note );
		}
	}
}