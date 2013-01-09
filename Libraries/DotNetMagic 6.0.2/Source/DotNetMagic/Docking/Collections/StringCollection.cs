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
using System.Xml;
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Docking
{
	/// <summary>
	/// Provides a collection container for string instances.
	/// </summary>
    public class StringCollection : CollectionWithEvents
    {
		/// <summary>
		/// Adds the specified string object to the collection.
		/// </summary>
		/// <param name="value">The string object to add to the collection.</param>
		/// <returns>The string object added to the collection.</returns>
        public String Add(String value)
        {
            // Use base class to process actual collection operation
            base.List.Add(value as object);

            return value;
        }

		/// <summary>
		/// Adds an array of string objects to the collection.
		/// </summary>
		/// <param name="values">An array of string objects to add to the collection.</param>
        public void AddRange(String[] values)
        {
            // Use existing method to add each array entry
            foreach(String item in values)
                Add(item);
        }

		/// <summary>
		/// Removes a string from the collection.
		/// </summary>
		/// <param name="value">A string to remove from the collection.</param>
        public void Remove(String value)
        {
            // Use base class to process actual collection operation
            base.List.Remove(value as object);
        }

		/// <summary>
		/// Inserts a string instance into the collection at the specified location.
		/// </summary>
		/// <param name="index">The location in the collection where you want to add the string.</param>
		/// <param name="value">The string object to insert.</param>
        public void Insert(int index, String value)
        {
            // Use base class to process actual collection operation
            base.List.Insert(index, value as object);
        }

		/// <summary>
		/// Determines whether a string is in the collection.
		/// </summary>
		/// <param name="value">The string to locate in the collection.</param>
		/// <returns>true if item is found in the collection; otherwise, false.</returns>
        public bool Contains(String value)
        {
			// Value comparison
			foreach(String s in base.List)
				if (value.Equals(s))
					return true;

			return false;
        }

		/// <summary>
		/// Determines whether any of a group of strings is in the collection.
		/// </summary>
		/// <param name="values">The group of strings to locate in the collection.</param>
		/// <returns>true if an item is found in the collection; otherwise, false.</returns>
        public bool Contains(StringCollection values)
        {
			foreach(String c in values)
			{
	            // Use base class to process actual collection operation
				if (Contains(c))
					return true;
			}

			return false;
        }

		/// <summary>
		/// Gets the string at the specified index.
		/// </summary>
        public String this[int index]
        {
            // Use base class to process actual collection operation
            get 
			{ 
				// Limit check incoming index
				if ((index < 0) || (index >= base.List.Count))
					return null;		

				return (base.List[index] as String); 
			}
        }

		/// <summary>
		/// Returns the index of the first occurrence of the given string.
		/// </summary>
		/// <param name="value">The string to locate.</param>
		/// <returns>Index of object; otherwise -1</returns>
        public int IndexOf(String value)
        {
            // Find the 0 based index of the requested entry
            return base.List.IndexOf(value);
        }

		/// <summary>
		/// Save contents of collection into XML stream.
		/// </summary>
		/// <param name="name">Name of XML element to use when saving.</param>
		/// <param name="xmlOut">XML stream to use when saving.</param>
		public void SaveToXml(string name, XmlTextWriter xmlOut)
		{
			xmlOut.WriteStartElement(name);
			xmlOut.WriteAttributeString("Count", this.Count.ToString());

			foreach(String s in base.List)
			{
				xmlOut.WriteStartElement("Item");
				xmlOut.WriteAttributeString("Name", s);
				xmlOut.WriteEndElement();
			}

			xmlOut.WriteEndElement();
		}

		/// <summary>
		/// Load contents of collection from XML stream.
		/// </summary>
		/// <param name="name">Name of XML element to use when loading.</param>
		/// <param name="xmlIn">XML stream to use when loading.</param>
		public void LoadFromXml(string name, XmlTextReader xmlIn)
		{
			// Move to next xml node
			if (!xmlIn.Read())
				throw new ArgumentException("Could not read in next expected node");

			// Check it has the expected name
			if (xmlIn.Name != name)
				throw new ArgumentException("Incorrect node name found");

			this.Clear();

			// Grab raw position information
			string attrCount = xmlIn.GetAttribute(0);

			// Convert from string to proper types
			int count = int.Parse(attrCount);

			for(int index=0; index<count; index++)
			{
				// Move to next xml node
				if (!xmlIn.Read())
					throw new ArgumentException("Could not read in next expected node");

				// Check it has the expected name
				if (xmlIn.Name != "Item")
					throw new ArgumentException("Incorrect node name found");

				this.Add(xmlIn.GetAttribute(0));
			}

			if (count > 0)
			{
				// Move over the end element of the collection
				if (!xmlIn.Read())
					throw new ArgumentException("Could not read in next expected node");

				// Check it has the expected name
				if (xmlIn.Name != name)
					throw new ArgumentException("Incorrect node name found");
			}
		}
    }
}
