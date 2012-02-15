using System;
using System.Threading.Tasks;
using Magnum.Extensions;
using Microsoft.ServiceBus.Messaging;

namespace MassTransit.Transports.AzureServiceBus
{
	class QueueClientImpl : QueueClient
	{
		bool _disposed;
		Microsoft.ServiceBus.Messaging.QueueClient _inner;
		readonly Func<Task<Microsoft.ServiceBus.Messaging.QueueClient>> _drain;

		public QueueClientImpl(Microsoft.ServiceBus.Messaging.QueueClient inner,
		                       Func<Task<Microsoft.ServiceBus.Messaging.QueueClient>> drain)
		{
			_inner = inner;
			_drain = drain;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool managed)
		{
			if (!managed || _disposed) 
				return;

			try
			{
				_inner.Close();
			}
			finally
			{
				_disposed = true;
			}
		}

		public Task<BrokeredMessage> Receive()
		{
			return Receive(8.Seconds());
		}

		public Task<BrokeredMessage> Receive(TimeSpan serverWaitTime)
		{
			return Task.Factory.FromAsync<TimeSpan, BrokeredMessage>(
				_inner.BeginReceive, _inner.EndReceive, serverWaitTime, null);
		}

		public IAsyncResult BeginReceive(TimeSpan serverWaitTime, AsyncCallback callback, object state)
		{
			return _inner.BeginReceive(serverWaitTime, callback, state);
		}

		public BrokeredMessage EndReceive(IAsyncResult result)
		{
			return _inner.EndReceive(result);
		}

		public Task Send(BrokeredMessage message)
		{
			return Task.Factory.FromAsync(_inner.BeginSend, _inner.EndSend, message, null);
		}

		public IAsyncResult BeginSend(BrokeredMessage message, AsyncCallback callback, object state)
		{
			return _inner.BeginSend(message, callback, state);
		}

		public void EndSend(IAsyncResult result)
		{
			_inner.EndSend(result);
		}

		public void Drain()
		{
			_inner = _drain().Result;
		}

		public ReceiveMode Mode
		{
			get { return _inner.Mode; }
		}

		public int PrefetchCount
		{
			get { return _inner.PrefetchCount; }
			set { _inner.PrefetchCount = value; }
		}

		public string Path
		{
			get { return _inner.Path; }
		}
	}
}