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

namespace Crownwood.DotNetMagic.Common
{
	/// <summary>
	/// Class used to help supply colours used for drawing elements.
	/// </summary>
	public class ColorDetails : IDisposable
	{
		// Instance fields
		private bool _256Colors;
		private bool _defaultBaseColor;
		private bool _defaultTrackColor;
		private Color _baseColor;
		private Color _baseDarkerColor;
		private Color _baseDarkColor;
		private Color _baseLightColor;
		private Color _menuSeparatorColor;
		private Color _trackBaseColor;
		private Color _trackLightColor;
		private Color _trackLightLightColor;
		private Color _trackMenuInsideColor;
		private Color _trackMenuCheckInsideColor;
		private Color _menuCheckInsideColor;
		private Color _trackDarkColor;
		private Color _openBaseColor;
		private Color _openBorderColor;
		private VisualStyle _style;
		private ThemeColorHelper _themeColorHelper;
		
		/// <summary>
		/// Initializes a new instance of the ColorDetails class.
		/// </summary>
		public ColorDetails()
		{
			// Detect if we are working 256 color mode
			_256Colors = (ColorHelper.ColorDepth() == 8);

			// Create the theme color helper object
			_themeColorHelper = new ThemeColorHelper();
			
			// Note which colours are currently detault values
			_defaultBaseColor = true;
			_defaultTrackColor = true;
			
			// Default the visual style
            _style = VisualStyle.Office2003;
			
			// Setup remainder of colours from the basic ones
			DefineBaseColors(SystemColors.Control);
			DefineTrackColors(SystemColors.Highlight);
		}

		/// <summary>
		/// Dispose of object details.
		/// </summary>
		public void Dispose()
		{
			// Dispose of embedded objects
			_themeColorHelper.Dispose();
		}

		/// <summary>
		/// Gets and sets the visual style
		/// </summary>
		public virtual VisualStyle Style
		{
			get { return _style; }
			set { _style = value; }
		}

		/// <summary>
		/// Reset the theme state.
		/// </summary>
		public void Reset()
		{
			// Make sure the current theme is retested
			_themeColorHelper.Reset();
		}

		/// <summary>
		/// Gets and sets if the basecolor is defaulted
		/// </summary>
		public bool DefaultBaseColor
		{
			get { return _defaultBaseColor; }
			set { _defaultBaseColor = value; }
		}
		
		/// <summary>
		/// Gets and sets if the trackcolor is defaulted
		/// </summary>
		public bool DefaultTrackColor
		{
			get { return _defaultTrackColor; }
			set { _defaultTrackColor = value; }
		}

		/// <summary>
		/// Reset the colors based on the base.
		/// </summary>
		/// <param name="baseColor">Base color.</param>
		public void DefineBaseColors(Color baseColor)
		{
			_baseColor = baseColor;
			_baseDarkColor = CommandDraw.BaseDarkFromBase(_baseColor, _256Colors);
			_baseDarkerColor = ColorHelper.MergeColors(_baseColor, 0.95f, _baseDarkColor, .05f);
			_baseLightColor = CommandDraw.BaseLightFromBase(_baseColor, _256Colors);
			_openBaseColor = CommandDraw.OpenBaseFromBase(_baseColor, _256Colors);
			_openBorderColor = CommandDraw.OpenBorderFromBase(_baseColor, _256Colors);
			_menuSeparatorColor = CommandDraw.MenuSeparatorFromBase(_baseColor, _256Colors);
		}

		/// <summary>
		/// Reset the colors based on the tracking start color.
		/// </summary>
		/// <param name="track">Tracking start color.</param>
		public void DefineTrackColors(Color track)
		{
			_trackBaseColor = CommandDraw.TrackBaseFromTrack(track, _256Colors);
			_trackLightColor = CommandDraw.TrackLightFromTrack(track, _256Colors);
			_trackLightLightColor = CommandDraw.TrackLightLightFromTrack(track, _baseColor, _256Colors);
			_trackDarkColor = CommandDraw.TrackDarkFromTrack(track, _256Colors);
			_trackMenuInsideColor = CommandDraw.TrackMenuInsideFromTrack(track, _256Colors);
			_menuCheckInsideColor = CommandDraw.MenuCheckInsideColor(track, _256Colors);
			_trackMenuCheckInsideColor = CommandDraw.TrackMenuCheckInsideColor(track, _256Colors);
		}

		/// <summary>
		/// Base color used for drawing background related items.
		/// </summary>
		public Color BaseColor 
		{ 
			get 
			{
				// If supposed to use the theme specific value, then grab it now
				if (_defaultBaseColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.BaseColor(Style);
				else
					return _baseColor; 
			}
		}

		/// <summary>
		/// Base color used for drawing background related items.
		/// </summary>
		public Color BaseDarkerColor 
		{ 
			get 
			{
				return _baseDarkerColor;
			}
		}

		/// <summary>
		/// Light Base color used for drawing background related items.
		/// </summary>
		public Color BaseColor1 
		{ 
			get 
			{
				// If supposed to use the theme specific value, then grab it now
				if (_defaultBaseColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.BaseColor1(Style);
				else
					return _baseColor; 
			}
		}

		/// <summary>
		/// Dark Base color used for drawing background related items.
		/// </summary>
		public Color BaseColor2 
		{ 
			get 
			{
				// If supposed to use the theme specific value, then grab it now
				if (_defaultBaseColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.BaseColor2(Style);
				else
					return _baseColor; 
			}
		}

		/// <summary>
		/// Light base colour used in stubs.
		/// </summary>
		public Color BaseColorStub 
		{ 
			get 
			{
				// If supposed to use the theme specific value, then grab it now
				if (_defaultBaseColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.BaseColorStub(Style);
				else
					return _baseColor; 
			}
		}

		/// <summary>
		/// Dark Base color used for drawing background related items.
		/// </summary>
		public Color DarkBaseColor 
		{ 
			get 
			{
				// If supposed to use the theme specific value, then grab it now
				if (_defaultBaseColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.DarkBaseColor(Style);
				else
					return _baseDarkColor; 
			}
		}

		/// <summary>
		/// Dark Second Base color used for drawing background related items.
		/// </summary>
		public Color DarkBaseColor2 
		{ 
			get 
			{
				// If supposed to use the theme specific value, then grab it now
				if (_defaultBaseColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.DarkBaseColor2(Style);
				else
					return ControlPaint.Dark(_baseDarkColor); 
			}
		}

		/// <summary>
		/// Color used to draw menu separators.
		/// </summary>
		public Color MenuSeparatorColor 
		{ 
			get 
			{
				// If supposed to use the theme specific value, then grab it now
				if (_defaultBaseColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.MenuSeparatorColor(Style);
				else
					return _menuSeparatorColor; 
			}
		}

		/// <summary>
		/// Top Base color used for drawing hot tracking items.
		/// </summary>
		public Color TrackBaseColor1 
		{ 
			get
			{
				// If supposed to use the theme specific value, then grab it now
				if (_defaultTrackColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.TrackBaseColor1(Style);
				else
					return _trackBaseColor; 
			}
		}

		/// <summary>
		/// Bottom Base color used for drawing hot tracking items.
		/// </summary>
		public Color TrackBaseColor2 
		{ 
			get
			{
				// If supposed to use the theme specific value, then grab it now
				if (_defaultTrackColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.TrackBaseColor2(Style);
				else
					return _trackBaseColor; 
			}
		}

		/// <summary>
		/// Top Lighter color used for drawing hot tracking items.
		/// </summary>
		public Color TrackLightColor1
		{ 
			get 
			{ 
				// If supposed to use the theme specific value, then grab it now
				if (_defaultTrackColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.TrackLightColor1(Style);
				else
					return _trackLightColor; 
			}
		}

		/// <summary>
		/// Bottom Lighter color used for drawing hot tracking items.
		/// </summary>
		public Color TrackLightColor2
		{ 
			get 
			{ 
				// If supposed to use the theme specific value, then grab it now
				if (_defaultTrackColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.TrackLightColor2(Style);
				else
					return _trackLightColor; 
			}
		}

		/// <summary>
		/// Top Very light color used for drawing hot tracking items.
		/// </summary>
		public Color TrackLightLightColor1 
		{ 
			get 
			{ 
				// If supposed to use the theme specific value, then grab it now
				if (_defaultTrackColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.TrackLightLightColor1(Style);
				else
					return _trackLightLightColor; 
			}
		}

		/// <summary>
		/// Bottom Very light color used for drawing hot tracking items.
		/// </summary>
		public Color TrackLightLightColor2 
		{ 
			get 
			{ 
				// If supposed to use the theme specific value, then grab it now
				if (_defaultTrackColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.TrackLightLightColor2(Style);
				else
					return _trackLightLightColor; 
			}
		}

		/// <summary>
		/// Darker color used for drawing hot tracking items.
		/// </summary>
		public Color TrackDarkColor
		{ 
			get 
			{ 
				// If supposed to use the theme specific value, then grab it now
				if (_defaultTrackColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.TrackDarkColor(Style);
				else
					return _trackDarkColor; 
			}
		}

		/// <summary>
		/// Gets the menu border colour
		/// </summary>
		/// <returns></returns>
		public Color OpenBorderColor
		{
			get 
			{ 
				// If supposed to use the theme specific value, then grab it now
				if (_defaultBaseColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.OpenBorderColor(Style);
				else
					return _baseDarkColor; 
			}
		}
		
		/// <summary>
		/// Gets the menu item border colour
		/// </summary>
		/// <returns></returns>
		public Color MenuItemBorderColor
		{
			get 
			{ 
				// If supposed to use the theme specific value, then grab it now
				if (_defaultTrackColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.MenuItemBorderColor(Style);
				else
					return _trackDarkColor; 
			}
		}

		/// <summary>
		/// Gets the menu background color.
		/// </summary>
		/// <returns></returns>
		public Color MenuBackColor
		{
			get 
			{ 
				// If supposed to use the theme specific value, then grab it now
				if (_defaultTrackColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.MenuBackColor(Style);
				else
					return _trackDarkColor; 
			}
		}		

		/// <summary>
		/// Gets the tracking border colour for inside menu items.
		/// </summary>
		/// <returns></returns>
		public Color TrackMenuInsideColor
		{
			get 
			{ 
				// If supposed to use the theme specific value, then grab it now
				if (_defaultTrackColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.TrackMenuInsideColor(Style);
				else
					return _trackMenuInsideColor; 
			}
		}

		/// <summary>
		/// Gets the menu checkbox/radio button inside colour.
		/// </summary>
		/// <returns></returns>
		public Color MenuCheckInsideColor
		{
			get 
			{ 
				// If supposed to use the theme specific value, then grab it now
				if (_defaultTrackColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.MenuCheckInsideColor(Style);
				else
					return _menuCheckInsideColor; 
			}
		}

		/// <summary>
		/// Gets the tracking menu checkbox/radio button inside colour.
		/// </summary>
		/// <returns></returns>
		public Color TrackMenuCheckInsideColor
		{
			get 
			{ 
				// If supposed to use the theme specific value, then grab it now
				if (_defaultTrackColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.TrackMenuCheckInsideColor(Style);
				else
					return _trackMenuCheckInsideColor; 
			}
		}

		/// <summary>
		/// Light color used for captions
		/// </summary>
		public Color CaptionColor1
		{
			get 
			{ 
				// If supposed to use the theme specific value, then grab it now
				if (_defaultBaseColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.CaptionColor1(Style);
				else
					return _baseLightColor; 
			}
		}

		/// <summary>
		/// Dark color used for captions
		/// </summary>
		public Color CaptionColor2
		{
			get 
			{ 
				// If supposed to use the theme specific value, then grab it now
				if (_defaultBaseColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.CaptionColor2(Style);
				else
					return _baseDarkColor; 
			}
		}

		/// <summary>
		/// Light color used for selected captions
		/// </summary>
		public Color CaptionSelectColor1
		{
			get 
			{ 
				// If supposed to use the theme specific value, then grab it now
				if (_defaultBaseColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.CaptionSelectColor1(Style);
				else
					return _baseLightColor; 
			}
		}

		/// <summary>
		/// Dark color used for selected captions
		/// </summary>
		public Color CaptionSelectColor2
		{
			get 
			{ 
				// If supposed to use the theme specific value, then grab it now
				if (_defaultBaseColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.CaptionSelectColor2(Style);
				else
					return _baseDarkColor; 
			}
		}

		/// <summary>
		/// Dark color used for drawing spots
		/// </summary>
		public Color SpotColor1
		{
			get 
			{ 
				// If supposed to use the theme specific value, then grab it now
				if (_defaultBaseColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.SpotColor1(Style);
				else
					return _baseDarkColor; 
			}
		}

		/// <summary>
		/// Light color used for drawing spots
		/// </summary>
		public Color SpotColor2
		{
			get 
			{ 
				// If supposed to use the theme specific value, then grab it now
				if (_defaultBaseColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.SpotColor2(Style);
				else
					return _baseLightColor; 
			}
		}

		/// <summary>
		/// Dark color used for separators.
		/// </summary>
		public Color SepDarkColor
		{
			get 
			{ 
				// If supposed to use the theme specific value, then grab it now
				if (_defaultBaseColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.SepDarkColor(Style);
				else
					return _baseDarkColor; 
			}
		}

		/// <summary>
		/// Light color used for separators.
		/// </summary>
		public Color SepLightColor
		{
			get 
			{ 
				// If supposed to use the theme specific value, then grab it now
				if (_defaultBaseColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.SepLightColor(Style);
				else
					return _baseLightColor; 
			}
		}

		/// <summary>
		/// Top Base color used for drawing open menu items.
		/// </summary>
		public Color OpenBaseColor1
		{ 
			get 
			{ 
				// If supposed to use the theme specific value, then grab it now
				if (_defaultBaseColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.OpenBaseColor1(Style);
				else
					return _openBaseColor; 
			}
		}

		/// <summary>
		/// Bottom Base color used for drawing open menu items.
		/// </summary>
		public Color OpenBaseColor2
		{ 
			get 
			{ 
				// If supposed to use the theme specific value, then grab it now
				if (_defaultBaseColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.OpenBaseColor2(Style);
				else
					return _openBaseColor; 
			}
		}

		/// <summary>
		/// Left color used for drawing image column.
		/// </summary>
		public Color ColumnBaseColor1
		{ 
			get 
			{ 
				// If supposed to use the theme specific value, then grab it now
				if (_defaultBaseColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.ColumnBaseColor1(Style);
				else
					return _openBaseColor; 
			}
		}

		/// <summary>
		/// Right color used for drawing image column.
		/// </summary>
		public Color ColumnBaseColor2
		{ 
			get 
			{ 
				// If supposed to use the theme specific value, then grab it now
				if (_defaultBaseColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.ColumnBaseColor2(Style);
				else
					return _openBaseColor; 
			}
		}

		/// <summary>
		/// Color used for border in docking and tabcontrol.
		/// </summary>
		public Color ActiveBorderColor
		{
			get 
			{ 
				// If supposed to use the theme specific value, then grab it now
				if (_defaultBaseColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.ActiveBorderColor(Style);
				else
					return _baseDarkColor;
			}
		}

		/// <summary>
		/// Color using to fill inside of an active tab.
		/// </summary>
		public Color ActiveTabColor
		{
			get 
			{ 
				// If supposed to use the theme specific value, then grab it now
				if (_defaultBaseColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.ActiveTabColor(Style);
				else
					return ControlPaint.LightLight(_baseColor);
			}
		}
		
		/// <summary>
		/// Color using to for tab buttons.
		/// </summary>
		public Color ActiveTabButtonColor
		{
			get 
			{ 
				// If supposed to use the theme specific value, then grab it now
				if (_defaultBaseColor && ((Style == VisualStyle.Office2003) || (Style == VisualStyle.IDE2005)))
					return _themeColorHelper.ActiveTabButtonColor(Style);
				else
					return ControlPaint.DarkDark(_baseColor);
			}
		}
	}
}
