using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StressTestBot
{
	class Program
	{
		private static bool _running = true;

		static void Main( string[] args )
		{
			var threadCount = int.Parse( ConfigurationManager.AppSettings["threadCount"] );
			var threads = new Thread[threadCount];
			for( int i = 0; i < threadCount; i++ )
			{
				threads[i] = new Thread( WorkerThread );
				threads[i].Start( i );
			}

			Console.ReadKey();

			_running = false;
			Console.WriteLine( "Stopping..." );

			for( int i = 0; i < threads.Length; i++ )
			{
				threads[i].Join();
			}
		}

		private static void WorkerThread( object param )
		{
			var workerCount = int.Parse( ConfigurationManager.AppSettings["workerCount"] );
			var sleepTime = int.Parse( ConfigurationManager.AppSettings["sleepTime"] );
			var serverAddress = ConfigurationManager.AppSettings["serverAddress"];
			var serverPort = int.Parse( ConfigurationManager.AppSettings["serverPort"] );
			var workerIndex = (int)param;
			var bots = new Bot[workerCount];
			for( int i = 0; i < workerCount; i++ )
			{
				var botIndex = workerIndex * workerCount + i;
				bots[i] = new Bot( "User" + botIndex, serverAddress, serverPort );
				bots[i].Start();
			}

			Console.WriteLine( "Bots all started for worker #" + workerIndex );

			while( _running )
			{
				Thread.Sleep( sleepTime );
				for( int i = 0; i < bots.Length; i++ )
				{
					bots[i].Update();
				}
			}

			for( int i = 0; i < bots.Length; i++ )
			{
				bots[i].Close();
			}
		}
	}
}
