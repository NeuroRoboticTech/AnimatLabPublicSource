// *****************************************************************************
// 
//  (c) Crownwood Software Ltd 2004-2005. All rights reserved. 
//	The software and associated documentation supplied hereunder are the 
//	proprietary information of Crownwood Software Ltd, Bracknell, 
//	Berkshire, England and are supplied subject to licence terms.
// 
//  Version 6.0.1.0 	www.crownwood.net
// *****************************************************************************

using System;
using System.Windows.Forms;

namespace Crownwood.DotNetMagic.Common
{
	/// <summary>
	/// Internal class used to decide if a NAG screen should be shown.
	/// </summary>
	internal class NAG
	{
		/// <summary>
		/// Called by every DotNetMagic control constructor to ensure NAG shown.
		/// </summary>
		public static void NAG_Start()
		{
            // Do nothing on the paid for version
		}
	}
}
