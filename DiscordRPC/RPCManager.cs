using DiscordRPC;
using DiscordRPC.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.DiscordRPC
{
    static class RPCManager
    {
		static DiscordRpcClient client;

		/// <summary>
		/// Run on initilization to start the RPC.
		/// clientID is the id of your discord application, richPresence is what presence to display on startup.
		/// </summary>
		public static void Initialize(string clientID, RichPresence richPresence)
		{
			client = new DiscordRpcClient(clientID);

			client.Logger = new ConsoleLogger() { Level = LogLevel.None };

			client.OnReady += (sender, e) =>
			{
				//Client Ready
			};

			client.OnPresenceUpdate += (sender, e) =>
			{
				//Client Updated
			};

			client.Initialize();

			UpdateState(richPresence);
		}

		/// <summary>
		/// Updates the status displayed on discord.
		/// </summary>
		/// <param name="presence">Information to show.</param>
		public static void UpdateState(RichPresence presence)
        {
			client.SetPresence(presence);
		}

		/// <summary>
		/// Run on window close to release data. FAILURE TO RUN THIS MAY CAUSE A MEMORY LEAK.
		/// </summary>
		public static void Dispose()
		{
			client.Dispose();
		}
	}
}
