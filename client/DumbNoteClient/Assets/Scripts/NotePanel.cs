using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace DumbNote
{
	public class NotePanel : MonoBehaviour
	{
		public InputField txtNote;

		public Button btnCommit;

		public void UpdateNote( string note )
		{
			txtNote.text = note;
		}

		private void Awake()
		{
			this.btnCommit.onClick.AddListener( this.OnCommitClicked );
		}

		private void OnDestroy()
		{
			this.btnCommit.onClick.RemoveListener( this.OnCommitClicked );
		}

		private void OnCommitClicked()
		{
			DumbNoteGame.instance.connector.CommitNote( txtNote.text );
		}
	}
}