﻿using System;
using MassTransit.Transports.AzureQueue.Utils;

namespace MassTransit.Transports.AzureQueue
{
	/// <summary>
	/// The envelope that we're shoving into AppFabric ServiceBus Queues.
	/// </summary>
	[Serializable]
	public class MessageEnvelope
	{
		public MessageEnvelope([NotNull] byte[] actualBody)
		{
			if (actualBody == null) throw new ArgumentNullException("actualBody");
			ActualBody = actualBody;
		}

		/// <summary> for serialization </summary>
		[Obsolete("for serialization")]
		protected MessageEnvelope()
		{
		}

		public byte[] ActualBody { get; protected set; }
	}
}