// *****************************************************************************
// 
//  (c) Crownwood Software Ltd 2004-2006. All rights reserved. 
//	The software and associated documentation supplied hereunder are the 
//	proprietary information of Crownwood Software Ltd, Bracknell, 
//	Berkshire, England and are supplied subject to licence terms.
// 
//  Version 6.0.1.0 	www.crownwood.net
// *****************************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Crownwood.DotNetMagic.Docking
{
	/// <summary>
	/// Collection of helper routines for dealing with Zone instances.
	/// </summary>
    public class ZoneHelper
    {
		/// <summary>
		/// Find collection of content instances inside a Zone.
		/// </summary>
		/// <param name="z">Zone to use.</param>
		/// <returns>Collection of content.</returns>
		public static ContentCollection Contents(Zone z)
		{
			// Container for returned group of found Content objects
			ContentCollection cc = new ContentCollection();

			// Process each Window in the Zone
			foreach(Window w in z.Windows)
			{
				WindowContent wc = w as WindowContent;

				// Is the Zone a Content derived variation?
				if (wc != null)
				{
					// Add each Content into the collection
					foreach(Content c in wc.Contents)
						cc.Add(c);
				}
			}

			return cc;
		}

		/// <summary>
		/// Find collection of content names inside a Zone.
		/// </summary>
		/// <param name="z">Zone to use.</param>
		/// <returns>Collection of content names.</returns>
		public static StringCollection ContentNames(Zone z)
		{
			// Container for returned group of found String objects
			StringCollection sc = new StringCollection();

			// Process each Window in the Zone
			foreach(Window w in z.Windows)
			{
				WindowContent wc = w as WindowContent;

				// Is the Zone a Content derived variation?
				if (wc != null)
				{
					// Add each Content into the collection
					foreach(Content c in wc.Contents)
					{
						if ((c.UniqueName != null) && (c.UniqueName.Length > 0))
							sc.Add(c.UniqueName);
						else
							sc.Add(c.Title);
					}
				}
			}

			return sc;
		}

		/// <summary>
		/// Find collection of content names inside a Zone in order of priority.
		/// </summary>
		/// <param name="z">Zone to use.</param>
		/// <param name="c">Content to use as basis of priority.</param>
		/// <returns>Collection of content names.</returns>
		public static StringCollection ContentNamesInPriority(Zone z, Content c)
		{
			// Container for returned group of found Content objects
			StringCollection sc = new StringCollection();

			// Process each Window in the Zone
			foreach(Window w in z.Windows)
			{
				WindowContent wc = w as WindowContent;

				// Is the Zone a Content derived variation?
				if (wc != null)
				{
					// Does this contain the interesting Content?
					if (wc.Contents.Contains(c))
					{
						// All Content of this Window are given priority and 
						// added into the start of the collection
						foreach(Content content in wc.Contents)
						{
							if ((content.UniqueName != null) && (content.UniqueName.Length > 0))
								sc.Insert(0, content.UniqueName);
							else
								sc.Insert(0, content.Title);
						}
					}
					else
					{
						// Lower priority Window and so contents are always
						// added to the end of the collection
						foreach(Content content in wc.Contents)
						{
							if ((content.UniqueName != null) && (content.UniqueName.Length > 0))
								sc.Add(content.UniqueName);
							else
								sc.Add(content.Title);
						}
					}
				}
			}

			return sc;
		}
    }
}
