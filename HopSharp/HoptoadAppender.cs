using System;
using System.IO;
using System.Reflection;
using log4net.Appender;
using log4net.Core;

namespace HopSharp
{

	public class HoptoadAppender : BufferingAppenderSkeleton
	{
		// Methods
		protected override void SendBuffer(LoggingEvent[] events)
		{
			var testMode = System.Environment.GetEnvironmentVariable("HOPTOAD_TEST");
			if (!string.IsNullOrEmpty(testMode))
				return;

			foreach (var logEvent in events)
			{
				HoptoadNotice notice = new HoptoadNotice();

				notice.ErrorClass = logEvent.LocationInformation.FullInfo;

				var errorMessage = logEvent.RenderedMessage;
				Exception exception = null;
				if (logEvent.ExceptionObject != null)
					exception = logEvent.ExceptionObject;

				notice.ErrorMessage = errorMessage;
				notice.Backtrace = "" + exception;
				notice.ApiKey = this.HopToadApiKey;

				//notice.File = message.LocationInformation;

				new HoptoadClient().Send(notice);
			}

		}

		// Properties
		public string HopToadApiKey { get; set; }
	}
}