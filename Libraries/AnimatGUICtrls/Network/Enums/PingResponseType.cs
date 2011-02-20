using System;

namespace AnimatGuiCtrls.Network
{
	public enum PingResponseType
	{
		Ok = 0,
		CouldNotResolveHost,
		RequestTimedOut,
		ConnectionError,
		InternalError,
		Canceled
	}
}
