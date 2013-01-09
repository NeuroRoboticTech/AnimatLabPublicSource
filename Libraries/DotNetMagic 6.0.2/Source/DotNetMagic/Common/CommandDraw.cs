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
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Diagnostics;

namespace Crownwood.DotNetMagic.Common
{
	/// <summary>
	/// Collection of static helper methods for drawing command related visual elements.
	/// </summary>
	public class CommandDraw
	{
		// Static fields
		private static CommandImage _checkMark;
		private static CommandImage _radioCheckMark;
		private static CommandImage _subMenuMark;

		static CommandDraw()
		{
			// Load the standard menu images
			Bitmap checkBitmap = ResourceHelper.LoadBitmap(typeof(CommandDraw), "Crownwood.DotNetMagic.Resources.MenuCheckMark.bmp", new Point(0,0));
			Bitmap radioCheckBitmap = ResourceHelper.LoadBitmap(typeof(CommandDraw), "Crownwood.DotNetMagic.Resources.MenuRadioCheckMark.bmp", new Point(0,0));
			Bitmap subMenuBitmap = ResourceHelper.LoadBitmap(typeof(CommandDraw), "Crownwood.DotNetMagic.Resources.MenuSubMenuMark.bmp", new Point(0,0));

			// Create image helpers for caching
			_checkMark = new CommandImage(checkBitmap);
			_radioCheckMark = new CommandImage(radioCheckBitmap);
			_subMenuMark = new CommandImage(subMenuBitmap);
		}

		/// <summary>
		/// Draw a button according to a variety of specified styles.
		/// </summary>
		/// <param name="g">Graphics reference needed for drawing.</param>
		/// <param name="style">Required visual style of button.</param>
		/// <param name="direction">Direction of flow button is in.</param>
		/// <param name="drawRect">Rectangle that encloses drawing area.</param>
		/// <param name="state">Visual state of button.</param>
		/// <param name="enabled">Is the command enabled.</param>
		/// <param name="edge">Which edge to draw text around the image.</param>
		/// <param name="font">Font to use when drawing text.</param>
		/// <param name="textColor">Color to draw enabled text inside command.</param>
		/// <param name="baseColor">Base color used for drawing.</param>
		/// <param name="text">Text to draw inside command.</param>
		/// <param name="image">Image to draw inside command.</param>
		/// <param name="imageAttr">ImageAttributes used to modify drawing of command.</param>
		/// <param name="trackBase1">Top Base color used for tracking.</param>
		/// <param name="trackBase2">Bottom Base color used for tracking.</param>
		/// <param name="trackLight1">Top Light color used for tracking.</param>
		/// <param name="trackLight2">Bottom Light color used for tracking.</param>
		/// <param name="trackLightLight1">Top Very light color used for tracking.</param>
		/// <param name="trackLightLight2">Bottom Very light color used for tracking.</param>
		/// <param name="trackBorder">Border color used for tracking.</param>
		/// <param name="buttonStyle">Style of button operation.</param>
		/// <param name="pushed">Is button pressed down.</param>
		/// <param name="staticButton">Should buttons be forced to stay static.</param>
		/// <param name="drawBorder">Should border be forced to draw.</param>
        /// <param name="hAlign">Horizontal alignment of string.</param>
        /// <param name="vAlign">Vertical alignment of string.</param>
        public static void DrawButtonCommand(Graphics g, VisualStyle style, LayoutDirection direction, 
											 Rectangle drawRect, ItemState state, bool enabled,
											 TextEdge edge, Font font, Color textColor,
											 Color baseColor, string text, CommandImage image,
											 ImageAttributes imageAttr,
											 Color trackBase1, Color trackBase2,
											 Color trackLight1, Color trackLight2,
											 Color trackLightLight1, Color trackLightLight2,
											 Color trackBorder, ButtonStyle buttonStyle, 
											 bool pushed, bool staticButton, bool drawBorder,
                                             StringAlignment hAlign, StringAlignment vAlign)
		{
			// Draw background parts of the command first
            DrawButtonCommandBack(g, style, direction, drawRect, state,
                                  baseColor, trackBase1, trackBase2,
                                  trackLight1, trackLight2, trackLightLight1,
                                  trackLightLight2, trackBorder, buttonStyle, pushed);

			// Draw inside contents on top of the background
            DrawButtonCommandInside(g, style, direction, drawRect, state,
                                    enabled, edge, font, textColor, baseColor,
                                    text, image, imageAttr, staticButton, hAlign, 
                                    vAlign, pushed);

			// Finally draw any outside parts
            DrawButtonCommandOutline(g, style, direction, drawRect, state,
                                     baseColor, trackBase1, trackBase2, trackLight1,
                                     trackLight2, trackLightLight1, trackLightLight2,
                                     trackBorder, buttonStyle, pushed, drawBorder);
		}

		/// <summary>
		/// Draw a button outline in appropriate visual style.
		/// </summary>
		/// <param name="g">Graphics reference needed for drawing.</param>
		/// <param name="style">Required visual style of button.</param>
		/// <param name="direction">Direction of flow button is in.</param>
		/// <param name="drawRect">Rectangle that encloses drawing area.</param>
		/// <param name="state">Visual state of button.</param>
		/// <param name="baseColor">Base color used for drawing.</param>
		/// <param name="trackBase1">Top Base color used for tracking.</param>
		/// <param name="trackBase2">Bottom Base color used for tracking.</param>
		/// <param name="trackLight1">Top Light color used for tracking.</param>
		/// <param name="trackLight2">Bottom Light color used for tracking.</param>
		/// <param name="trackLightLight1">Top Very light color used for tracking.</param>
		/// <param name="trackLightLight2">Bottom Very light color used for tracking.</param>
		/// <param name="trackBorder">Border color used for tracking.</param>
		/// <param name="buttonStyle">Style of button operation.</param>
		/// <param name="pushed">Is button pressed down.</param>
		public static void DrawButtonCommandBack(Graphics g, VisualStyle style, LayoutDirection direction, 
												 Rectangle drawRect, ItemState state, Color baseColor,
												 Color trackBase1, Color trackBase2,
												 Color trackLight1, Color trackLight2,
												 Color trackLightLight1, Color trackLightLight2,
												 Color trackBorder, ButtonStyle buttonStyle, bool pushed)
		{
			Rectangle rect = new Rectangle(drawRect.Left, drawRect.Top, drawRect.Width - 1, drawRect.Height - 1);
        
			// Draw background according to style
			switch(style)
			{
				case VisualStyle.Plain:
					// We always draw the button in solid background color unless it is a toggle button
					// that is currently pressed down but not being hottracked, pressed or open
					if ((buttonStyle == ButtonStyle.ToggleButton) && pushed && (state == ItemState.Normal))
					{
						// We need to draw a stipled background
						using(HatchBrush backBrush = new HatchBrush(HatchStyle.Percent50, baseColor, ControlPaint.Light(baseColor)))
							g.FillRectangle(backBrush, drawRect);
					}
					else
					{
						// Draw background with back color
						using(SolidBrush backBrush = new SolidBrush(baseColor))
							g.FillRectangle(backBrush, drawRect);
					}
					break;
				case VisualStyle.IDE2005:
                case VisualStyle.Office2003:
                    // Draw the gradiant different depending on the direction
					float angle = (direction == LayoutDirection.Horizontal) ? 90 : 0;
					
					// If the button is a toggle button that is currently pressed down 
					if ((buttonStyle == ButtonStyle.ToggleButton) && pushed)
					{
						// Is there any rectangle size to draw?
						if ((drawRect.Width > 0) && (drawRect.Height > 0))
						{
							switch(state)
							{
								case ItemState.Normal:
									using(Brush backBrush = new LinearGradientBrush(drawRect, trackLightLight1, trackLightLight2, angle))
										g.FillRectangle(backBrush, drawRect);
									break;
								case ItemState.HotTrack:
								case ItemState.Open:
								case ItemState.Pressed:
									using(Brush trackBrush = new LinearGradientBrush(drawRect, trackBase1, trackBase2, angle))
										g.FillRectangle(trackBrush, drawRect);
									break;
								default:
									// Should never happen!
									Debug.Assert(false);
									break;
							}
						}						
					}
					else
					{				
						// Is there any rectangle size to draw?
						if ((drawRect.Width > 0) && (drawRect.Height > 0))
						{
							// Otherwise draw according to state
							switch(state)
							{
								case ItemState.Normal:
									// Do nothing, background is drawn already
									break;
								case ItemState.HotTrack:
								case ItemState.Open:
									using(Brush trackBrush = new LinearGradientBrush(drawRect, trackLight1, trackLight2, angle))
										g.FillRectangle(trackBrush, drawRect);
									break;
								case ItemState.Pressed:
									using(Brush trackBrush = new LinearGradientBrush(drawRect, trackBase1, trackBase2, angle))
										g.FillRectangle(trackBrush, drawRect);
									break;
								default:
									// Should never happen!
									Debug.Assert(false);
									break;
							}
						}
					}
					break;
                case VisualStyle.Office2007Blue:
                case VisualStyle.Office2007Silver:
                case VisualStyle.Office2007Black:
                    if ((drawRect.Width > 0) && (drawRect.Height > 0))
                        Office2007Renderer.DrawButtonCommandBack(g, style, direction, drawRect, 
                                                                 state, buttonStyle, pushed, true);
                    break;
                case VisualStyle.MediaPlayerBlue:
                case VisualStyle.MediaPlayerOrange:
                case VisualStyle.MediaPlayerPurple:
                    if ((drawRect.Width > 0) && (drawRect.Height > 0))
                        MediaPlayerRenderer.DrawButtonCommandBack(g, style, direction, drawRect,
                                                                 state, buttonStyle, pushed, true);
                    break;
                default:
					// Should never happen!
					Debug.Assert(false);
					break;
			}
		}
        
		/// <summary>
		/// Draw a button outline in appropriate visual style.
		/// </summary>
		/// <param name="g">Graphics reference needed for drawing.</param>
		/// <param name="style">Required visual style of button.</param>
		/// <param name="direction">Direction of flow button is in.</param>
		/// <param name="drawRect">Rectangle that encloses drawing area.</param>
		/// <param name="state">Visual state of button.</param>
		/// <param name="enabled">Is the command enabled.</param>
		/// <param name="edge">Which edge to draw text around the image.</param>
		/// <param name="font">Font to use when drawing text.</param>
		/// <param name="textColor">Color to draw enabled text inside command.</param>
		/// <param name="baseColor">Base color to draw inside command.</param>
		/// <param name="text">Text to draw inside command.</param>
		/// <param name="image">Image to draw inside command.</param>
		/// <param name="imageAttr">ImageAttributes used to modify drawing of command.</param>
		/// <param name="staticButton">Should buttons be forced to stay static.</param>
        /// <param name="hAlign">Horizontal alignment of string.</param>
        /// <param name="vAlign">Vertical alignment of string.</param>
        /// <param name="pushed">Is the button already checked.</param>
        public static void DrawButtonCommandInside(Graphics g, VisualStyle style, LayoutDirection direction, 
												   Rectangle drawRect, ItemState state, bool enabled,
												   TextEdge edge, Font font, Color textColor,
												   Color baseColor, string text, CommandImage image,
												   ImageAttributes imageAttr, bool staticButton,
            									   StringAlignment hAlign, StringAlignment vAlign, 
                                                   bool pushed)
		{
			// If there is no image, then drawing just text is simple
			if ((image == null) || (image.Image == null))
			{
				// Draw text using the entire available draw rectangle
				DrawCommandText(g, style, direction, state, enabled, drawRect, 
								font, textColor, baseColor, hAlign, vAlign, text, pushed);
			}
			else
			{
				// If there is no text, then drawing just an image is simple
				if ((text == null) || (text.Length == 0))
				{
					// Draw image in entire available draw rectangle.
					DrawCommandImage(g, style, state, enabled, drawRect, baseColor, image, imageAttr, staticButton);
				}
				else
				{
					// Calculate size of the text for drawing
					Size sizeText = TextSize(g, font, text);

					// Calculate edge specific values
					int total = 0;
					int required = 0;
					int imageVector = 0;

					// Handle the horizontal drawing direction
					if (direction == LayoutDirection.Horizontal)
					{
						switch(edge)
						{
							case TextEdge.Left:
							case TextEdge.Right:
								total = drawRect.Width;
								required = image.ImageSpace(style).Width + sizeText.Width;
								imageVector = image.ImageSpace(style).Width;
								break;
							case TextEdge.Top:
							case TextEdge.Bottom:
								total = drawRect.Height;
								required = image.ImageSpace(style).Height + sizeText.Height;
								imageVector = image.ImageSpace(style).Height;
								break;
							default:
								// Should never happen!
								Debug.Assert(false);
								break;
						}
					}
					else
					{
						// Handle vertical drawing direction
						switch(edge)
						{
							case TextEdge.Left:
							case TextEdge.Right:
								total = drawRect.Height;
								required = image.ImageSpace(style).Height + sizeText.Width;
								imageVector = image.ImageSpace(style).Width;
								break;
							case TextEdge.Top:
							case TextEdge.Bottom:
								total = drawRect.Width;
								required = image.ImageSpace(style).Width + sizeText.Height;
								imageVector = image.ImageSpace(style).Height;
								break;
							default:
								// Should never happen!
								Debug.Assert(false);
								break;
						}
					}

					// If there is not enough room even for the whole image?
					if (total <= imageVector)
					{
						// No, so just draw the image alone
						DrawCommandImage(g, style, state, enabled, drawRect, baseColor, image, imageAttr, staticButton);
					}
					else
					{
						// Is there enough room for image plus the text?
						if (total <= required)
						{
							// No, so position image flush against its edge
							Rectangle imageRect = ImageFromEdge(drawRect, image.ImageSpace(style), edge, direction, 0);

							// Draw the actual image first
							DrawCommandImage(g, style, state, enabled, imageRect, baseColor, image, imageAttr, staticButton);

							// Use remaining space for the text
							Rectangle textRect = TextFromImageEdge(drawRect, imageRect, edge, direction, 0);

							// Draw text using the entire calculated area
							DrawCommandText(g, style, direction, state, enabled, textRect, font, 
                                            textColor, baseColor, hAlign, vAlign, text, pushed);
						}
						else
						{
							// Gap to indent the image from its edge and between image and text
							int gap = (total - required) / 3;

							// No, so position image flush against its edge
							Rectangle imageRect = ImageFromEdge(drawRect, image.ImageSpace(style), edge, direction, gap);

							// Draw the actual image first
							DrawCommandImage(g, style, state, enabled, imageRect, baseColor, image, imageAttr, staticButton);

							// Use remaining space for the text
							Rectangle textRect = TextFromImageEdge(drawRect, imageRect, edge, direction, gap);

							// Draw text using the entire calculated area
							DrawCommandText(g, style, direction, state, enabled, textRect, font, 
                                            textColor, baseColor, hAlign, vAlign, text, pushed);
						}
					}
				}
			}
		}

		/// <summary>
		/// Draw a button outline in appropriate visual style.
		/// </summary>
		/// <param name="g">Graphics reference needed for drawing.</param>
		/// <param name="style">Required visual style of button.</param>
		/// <param name="direction">Direction of flow button is in.</param>
		/// <param name="drawRect">Rectangle that encloses drawing area.</param>
		/// <param name="state">Visual state of button.</param>
		/// <param name="baseColor">Base color used for drawing.</param>
		/// <param name="trackBase1">Top Base color used for tracking.</param>
		/// <param name="trackBase2">Bottom Base color used for tracking.</param>
		/// <param name="trackLight1">Top Light color used for tracking.</param>
		/// <param name="trackLight2">Bottom Light color used for tracking.</param>
		/// <param name="trackLightLight1">Top Very light color used for tracking.</param>
		/// <param name="trackLightLight2">Bottom Very light color used for tracking.</param>
		/// <param name="trackBorder">Border color used for tracking.</param>
		/// <param name="buttonStyle">Style of button operation.</param>
		/// <param name="pushed">Is button pressed down.</param>
		/// <param name="drawBorder">Should border be forced to draw.</param>
		public static void DrawButtonCommandOutline(Graphics g, VisualStyle style, LayoutDirection direction, 
													Rectangle drawRect, ItemState state, Color baseColor,
													Color trackBase1, Color trackBase2, Color trackLight1, 
													Color trackLight2, Color trackLightLight1, Color trackLightLight2,
													Color trackBorder, ButtonStyle buttonStyle, 
													bool pushed, bool drawBorder)
		{
			Rectangle rect = new Rectangle(drawRect.Left, drawRect.Top, drawRect.Width - 1, drawRect.Height - 1);
        
			// Draw background according to style
			switch(style)
			{
				case VisualStyle.Plain:
					// If a toggle button that is currently pushed in
					if ((buttonStyle == ButtonStyle.ToggleButton) && pushed)
					{
						// Then always draw sunken
						DrawPlainSunken(g, rect, baseColor);
					}
					else
					{				
						// Otherwise draw according to state
						switch(state)
						{
							case ItemState.Normal:
								// Are we forced to draw a borer
								if (drawBorder)
									ControlPaint.DrawBorder(g, rect, ControlPaint.Light(ControlPaint.Dark(baseColor)), ButtonBorderStyle.Solid);
								break;
							case ItemState.HotTrack:
							case ItemState.Open:
								DrawPlainRaised(g, rect, baseColor);
								break;
							case ItemState.Pressed:
								DrawPlainSunken(g, rect, baseColor);
								break;
							default:
								// Should never happen!
								Debug.Assert(false);
								break;
						}
					}
					break;
				case VisualStyle.IDE2005:
				case VisualStyle.Office2003:
					// If a toggle button that is currently pushed in
					if ((buttonStyle == ButtonStyle.ToggleButton) && pushed)
					{
						// Then always draw with a highlight border
						using(Pen trackPen = new Pen(trackBorder))
							g.DrawRectangle(trackPen, rect);
					}
					else
					{
						// Draw according to state
						switch(state)
						{
							case ItemState.Normal:
								// Are we forced to draw a borer
								if (drawBorder)
								{
									using(Pen trackPen = new Pen(ControlPaint.Light(ControlPaint.Dark(baseColor))))
										g.DrawRectangle(trackPen, rect);
								}
								break;
							case ItemState.HotTrack:
							case ItemState.Open:
							case ItemState.Pressed:
								using(Pen trackPen = new Pen(trackBorder))
									g.DrawRectangle(trackPen, rect);
								break;
							default:
								// Should never happen!
								Debug.Assert(false);
								break;
						}
					}
					break;
                case VisualStyle.Office2007Blue:
                case VisualStyle.Office2007Silver:
                case VisualStyle.Office2007Black:
                    if (drawBorder && 
                        (buttonStyle == ButtonStyle.PushButton) &&
                        (state == ItemState.Normal))
                    {
                        Office2007Renderer.DrawButtonBorder(g, style, drawRect);
                    }
                   break;
                case VisualStyle.MediaPlayerBlue:
                case VisualStyle.MediaPlayerOrange:
                case VisualStyle.MediaPlayerPurple:
                   if (drawBorder &&
                       (buttonStyle == ButtonStyle.PushButton) &&
                       (state == ItemState.Normal))
                   {
                       MediaPlayerRenderer.DrawButtonBorder(g, style, drawRect);
                   }
                   break;
            }
		}

		/// <summary>
		/// Draw text inside given rectangle using appropriate appearance.
		/// </summary>
		/// <param name="g">Graphics reference needed for drawing.</param>
		/// <param name="style">Required visual style of button.</param>
		/// <param name="direction">Direction of flow button is in.</param>
		/// <param name="state">Visual state of button.</param>
		/// <param name="enabled">Is the command enabled.</param>
		/// <param name="drawRect">Rectangle that encloses drawing area.</param>
		/// <param name="font">Font to use when drawing text.</param>
		/// <param name="textColor">Color to draw enabled text inside command.</param>
		/// <param name="baseColor">Base color to draw inside command.</param>
        /// <param name="hAlign">Horizontal alignment of string.</param>
        /// <param name="vAlign">Vertical alignment of string.</param>
        /// <param name="text">Test to draw inside command.</param>
        /// <param name="pushed">Is the button already checked.</param>
        public static void DrawCommandText(Graphics g, VisualStyle style, LayoutDirection direction,
										   ItemState state, bool enabled, Rectangle drawRect,
										   Font font, Color textColor, Color baseColor,
										   StringAlignment hAlign, StringAlignment vAlign,
                                           string text, bool pushed)
		{
			// Create default format settings
			using(StringFormat format = new StringFormat())
			{
				format.FormatFlags = StringFormatFlags.NoWrap;
                format.Alignment = hAlign;
                format.LineAlignment = vAlign;
				format.Trimming = StringTrimming.EllipsisCharacter;
				format.HotkeyPrefix = HotkeyPrefix.Show;

				// Do we need to draw vertical?
				if (direction == LayoutDirection.Vertical)
					format.FormatFlags |= StringFormatFlags.DirectionVertical;

				// If enabled then all styles draw the same
				if (enabled)
				{
					Color drawColor;

                    switch (style)
                    {
                        case VisualStyle.Office2007Blue:
                        case VisualStyle.Office2007Silver:
                        case VisualStyle.Office2007Black:
                            if ((state == ItemState.Normal) && !pushed)
                                drawColor = Office2007ColorTable.TabInactiveTextColor(style, Crownwood.DotNetMagic.Controls.OfficeStyle.Light);
                            else
                                drawColor = textColor;
                            break;
                        case VisualStyle.MediaPlayerBlue:
                        case VisualStyle.MediaPlayerOrange:
                        case VisualStyle.MediaPlayerPurple:
                            if ((state == ItemState.Normal) && !pushed)
                                drawColor = MediaPlayerColorTable.TabInactiveTextColor(style, Crownwood.DotNetMagic.Controls.MediaPlayerStyle.Light);
                            else
                                drawColor = Color.White;
                            break;
                        default:
                            drawColor = textColor;
                            break;
                    }

					// If Plain style when button is pressed then draw down and right
					if ((style == VisualStyle.Plain) && (state == ItemState.Pressed))
						drawRect.Offset(1, 1);
					
					using(SolidBrush fontBrush = new SolidBrush(drawColor))
						g.DrawString(text, font, fontBrush, drawRect, format);
				}
				else
				{
					// If not enabled, then draw style specific
					switch(style)
					{
						case VisualStyle.Plain:
							// Helper values used when drawing grayed text in plain style
							Rectangle drawDownRight = drawRect;
							drawDownRight.Offset(1,1);

							// Draw the text offset down and right
							g.DrawString(text, font, Brushes.White, drawDownRight, format);

							// Draw then text offset up and left
							using (SolidBrush grayBrush = new SolidBrush(SystemColors.GrayText))
								g.DrawString(text, font, grayBrush, drawRect, format);

							break;
						case VisualStyle.IDE2005:
						case VisualStyle.Office2003:
                        case VisualStyle.Office2007Blue:
                        case VisualStyle.Office2007Silver:
                        case VisualStyle.Office2007Black:
                        case VisualStyle.MediaPlayerBlue:
                        case VisualStyle.MediaPlayerOrange:
                        case VisualStyle.MediaPlayerPurple:
                            using (SolidBrush grayBrush = new SolidBrush(SystemColors.GrayText))
								g.DrawString(text, font, grayBrush, drawRect, format);

							break;
						default:
							// Should never happen!
							Debug.Assert(false);
							break;
					}
				}
			}
		}
										  
		/// <summary>
		/// Draw image inside given rectangle using appropriate appearance.
		/// </summary>
		/// <param name="g">Graphics reference needed for drawing.</param>
		/// <param name="style">Required visual style of button.</param>
		/// <param name="state">Visual state of button.</param>
		/// <param name="enabled">Is the command enabled.</param>
		/// <param name="drawRect">Rectangle that encloses drawing area.</param>
		/// <param name="baseColor">Color for drawing behind image.</param>
		/// <param name="image">Image to draw inside command.</param>
		/// <param name="imageAttr">ImageAttributes used to modify drawing of image.</param>
		/// <param name="staticButton">Should buttons be forced to stay static.</param>
		public static void DrawCommandImage(Graphics g, VisualStyle style, 
											ItemState state, bool enabled, Rectangle drawRect, 
											Color baseColor, CommandImage image,
											ImageAttributes imageAttr, bool staticButton)
		{
			// Image drawing depends on the style
			switch(style)
			{
				case VisualStyle.Plain:
					// If not enabled....
					if (!enabled)
					{
						Rectangle disabledRect = new Rectangle(drawRect.Left + 1, drawRect.Top + 1, drawRect.Width - 1, drawRect.Height - 1);

						// Draw the disabled image offset down and right
						DrawImageInCentre(g, disabledRect, image.ImageSize, image.WhiteImage, imageAttr);
						// Draw the image in the centre of the drawing rectangle, but clipped to it
						DrawImageInCentre(g, drawRect, image.ImageSize, image.GetDisabledImage(baseColor), imageAttr);
					}
					else
					{
						// Where to draw depends on state
						switch(state)
						{
							case ItemState.Normal:
							case ItemState.HotTrack:
							case ItemState.Open:
								// Draw normal image in the centre of the drawing rectangle, but clipped to it
								DrawImageInCentre(g, drawRect, image.ImageSize, image.Image, imageAttr);
								break;
							case ItemState.Pressed:
								Rectangle downRect = new Rectangle(drawRect.Left + 1, drawRect.Top + 1, drawRect.Width - 1, drawRect.Height - 1);

								// Draw normal image down and right to show its pressed down
								DrawImageInCentre(g, downRect, image.ImageSize, image.Image, imageAttr);
								break;
							default:
								// Should never happen!
								Debug.Assert(false);
								break;
						}
					}

					break;
				case VisualStyle.IDE2005:
                case VisualStyle.Office2003:
                case VisualStyle.Office2007Blue:
                case VisualStyle.Office2007Silver:
                case VisualStyle.Office2007Black:
                case VisualStyle.MediaPlayerBlue:
                case VisualStyle.MediaPlayerOrange:
                case VisualStyle.MediaPlayerPurple:
                    // If not enabled....
					if (!enabled)
					{
						// Draw the disabled image on its own
						DrawImageInCentre(g, drawRect, image.ImageSize, image.GetDisabledImage(baseColor), imageAttr);
					}
					else
					{
						switch(state)
						{
							case ItemState.Normal:
							case ItemState.Pressed:
							case ItemState.HotTrack:
							case ItemState.Open:
								// Draw the normal image on its own
								DrawImageInCentre(g, drawRect, image.ImageSize, image.Image, imageAttr);
								break;
							default:
								// Should never happen!
								Debug.Assert(false);
								break;
						}
					}
					break;
				default:
					// Should never happen!
					Debug.Assert(false);
					break;
			}
		}

		/// <summary>
		/// Draw an image in the centre of rectangle but clipped to it
		/// </summary>
		/// <param name="g">Graphics reference needed for drawing.</param>
		/// <param name="drawRect">Rectangle that encloses drawing area.</param>
		/// <param name="imageSize">Size of the image.</param>
		/// <param name="image">Image to draw.</param>
		/// <param name="imageAttr">ImageAttributes used to modify drawing of image</param>
		public static void DrawImageInCentre(Graphics g,
											 Rectangle drawRect,
											 Size imageSize,
											 Image image,
											 ImageAttributes imageAttr)
		{
			// Do we have an image to draw?
			if (image != null)
			{
				// Find the x and y offsets for drawing the image
				int xOffset = (drawRect.Width - imageSize.Width) / 2;
				int yOffset = (drawRect.Height - imageSize.Height) / 2;

				// Is the image smaller than the drawing rectangle?
				if ((drawRect.Width >= imageSize.Width) &&
					(drawRect.Height >= imageSize.Height))
				{
					// Calculate the destination rectangle for drawing
					Rectangle destRect = new Rectangle(drawRect.Left + xOffset, 
													   drawRect.Top + yOffset,
													   imageSize.Width,
													   imageSize.Height);
				
					// Then entire image offset into drawing rectangle
					if (imageAttr != null)
						g.DrawImage(image, destRect, 0, 0, imageSize.Width, imageSize.Height, GraphicsUnit.Pixel, imageAttr);
					else
						g.DrawImage(image, destRect, 0, 0, imageSize.Width, imageSize.Height, GraphicsUnit.Pixel);
				}
				else
				{
					// How much of the image needs to be removed from drawing
					int xShrink = imageSize.Width - drawRect.Width;
					int yShrink = imageSize.Height - drawRect.Height;

					// If image is wider than the drawing rectangle
					if (xShrink > 0) 
					{
						// Do not offset drawing into rectangle
						xOffset = 0;

						// Limit image width to draw to available drawing rectangle
						imageSize.Width = drawRect.Width;
					}
					
					// If image is taller than the drawing rectangle
					if (yShrink > 0) 
					{
						// Do not offset drawing into rectangle
						yOffset = 0;

						// Limit image width to draw to available drawing rectangle
						imageSize.Height = drawRect.Height;
					}

					// Calculate the destination rectangle for drawing
					Rectangle destRect = new Rectangle(drawRect.Left + xOffset, 
													   drawRect.Top + yOffset,
													   imageSize.Width,
													   imageSize.Height);

					// Draw the entire source image but shrunk to available space
					if (imageAttr != null)
						g.DrawImage(image, destRect, 0, 0, image.Size.Width, image.Size.Height, GraphicsUnit.Pixel, imageAttr);
					else
						g.DrawImage(image, destRect, 0, 0, image.Size.Width, image.Size.Height, GraphicsUnit.Pixel);
				}
			}
		}

		/// <summary>
		/// Draw a menu buton according to a variety of specified styles.
		/// </summary>
		/// <param name="g">Graphics reference needed for drawing.</param>
		/// <param name="style">Required visual style of button.</param>
		/// <param name="direction">Direction of flow button is in.</param>
		/// <param name="drawRect">Rectangle that encloses drawing area.</param>
		/// <param name="state">Visual state of button.</param>
		/// <param name="enabled">Is the command enabled.</param>
		/// <param name="edge">Which edge to draw text around the image.</param>
		/// <param name="font">Font to use when drawing text.</param>
		/// <param name="textColor">Color to draw enabled text inside command.</param>
		/// <param name="baseColor">Base color used for drawing.</param>
		/// <param name="text">Text to draw inside command.</param>
		/// <param name="image">Image to draw inside command.</param>
		/// <param name="trackBase">Base color used for tracking.</param>
		/// <param name="trackLight">Light color used for tracking.</param>
		/// <param name="trackLightLight">Very light color used for tracking.</param>
		/// <param name="trackBorder">Border color used for tracking.</param>
		/// <param name="openBase">Open color used for background in open mode.</param>
		/// <param name="openBorder">Open color used for border in open mode.</param>
		/// <param name="omitEdge">Which edge should not be drawn.</param>
		public static void DrawMenuTopCommand(Graphics g, VisualStyle style, LayoutDirection direction, 
											  Rectangle drawRect, ItemState state, bool enabled,
											  TextEdge edge, Font font, Color textColor,
											  Color baseColor, string text, CommandImage image,
											  Color trackBase, Color trackLight, Color trackLightLight,
											  Color trackBorder, Color openBase, Color openBorder,
											  OmitEdge omitEdge)
		{
			// Draw background parts of the command first
			DrawMenuTopCommandBack(g, style, direction, drawRect, state,
								   baseColor, trackBase, trackLight, trackLightLight,
								   trackBorder, openBase);

			// Draw inside contents on top of the background
			DrawButtonCommandInside(g, style, direction, drawRect, state, 
									enabled, edge, font, textColor, baseColor, 
									text, image, null, false, StringAlignment.Center,
                                    StringAlignment.Center, false);

			// Finally draw any outside parts
			DrawMenuTopCommandOutline(g, style, direction, drawRect, state,
									  baseColor, trackBase, trackLight, trackLightLight,
									  trackBorder, openBorder, omitEdge);
		}

		/// <summary>
		/// Draw background of a menu command as a top item.
		/// </summary>
		/// <param name="g">Graphics reference needed for drawing.</param>
		/// <param name="style">Required visual style of button.</param>
		/// <param name="direction">Direction of flow button is in.</param>
		/// <param name="drawRect">Rectangle that encloses drawing area.</param>
		/// <param name="state">Visual state of button.</param>
		/// <param name="baseColor">Base color used for drawing.</param>
		/// <param name="trackBase">Base color used for tracking.</param>
		/// <param name="trackLight">Light color used for tracking.</param>
		/// <param name="trackLightLight">Very light color used for tracking.</param>
		/// <param name="trackBorder">Border color used for tracking.</param>
		/// <param name="openBase">Open color used for background in open mode.</param>
		public static void DrawMenuTopCommandBack(Graphics g, VisualStyle style, LayoutDirection direction, 
													 Rectangle drawRect, ItemState state, Color baseColor,
													 Color trackBase, Color trackLight, Color trackLightLight,
													 Color trackBorder, Color openBase)
		{
			Rectangle rect = new Rectangle(drawRect.Left, drawRect.Top, drawRect.Width - 1, drawRect.Height - 1);
        
			// Draw background according to style
			switch(style)
			{
				case VisualStyle.Plain:
					// Draw background with back color
					using(SolidBrush backBrush = new SolidBrush(baseColor))
						g.FillRectangle(backBrush, drawRect);
					break;
				case VisualStyle.IDE2005:
				case VisualStyle.Office2003:
					// Otherwise draw according to state
					switch(state)
					{
						case ItemState.Normal:
							// Draw background with back color
							using(SolidBrush backBrush = new SolidBrush(baseColor))
								g.FillRectangle(backBrush, drawRect);
							break;
						case ItemState.HotTrack:
							using(SolidBrush trackBrush = new SolidBrush(trackLight))
								g.FillRectangle(trackBrush, drawRect);
							break;
						case ItemState.Pressed:
							using(SolidBrush trackBrush = new SolidBrush(trackBase))
								g.FillRectangle(trackBrush, drawRect);
							break;
						case ItemState.Open:
							using(SolidBrush openBrush = new SolidBrush(openBase))
								g.FillRectangle(openBrush, drawRect);
							break;
						default:
							// Should never happen!
							Debug.Assert(false);
							break;
					}
					break;
				default:
					// Should never happen!
					Debug.Assert(false);
					break;
			}
		}

		/// <summary>
		/// Draw a button outline in appropriate visual style.
		/// </summary>
		/// <param name="g">Graphics reference needed for drawing.</param>
		/// <param name="style">Required visual style of button.</param>
		/// <param name="direction">Direction of flow button is in.</param>
		/// <param name="drawRect">Rectangle that encloses drawing area.</param>
		/// <param name="state">Visual state of button.</param>
		/// <param name="baseColor">Base color used for drawing.</param>
		/// <param name="trackBase">Base color used for tracking.</param>
		/// <param name="trackLight">Light color used for tracking.</param>
		/// <param name="trackLightLight">Very light color used for tracking.</param>
		/// <param name="trackBorder">Border color used for tracking.</param>
		/// <param name="openBorder">OPen border color for open mode.</param>
		/// <param name="omitEdge">Which edge should not be drawn.</param>
		public static void DrawMenuTopCommandOutline(Graphics g, VisualStyle style, LayoutDirection direction, 
													Rectangle drawRect, ItemState state, Color baseColor,
													Color trackBase, Color trackLight, Color trackLightLight,
													Color trackBorder, Color openBorder, OmitEdge omitEdge)
		{
			Rectangle rect = new Rectangle(drawRect.Left, drawRect.Top, drawRect.Width - 1, drawRect.Height - 1);
        
			// Draw background according to style
			switch(style)
			{
				case VisualStyle.Plain:
					// Otherwise draw according to state
					switch(state)
					{
						case ItemState.Normal:
							// Do nothing, no border needed in normal state
							break;
						case ItemState.HotTrack:
							DrawPlainRaised(g, rect, baseColor);
							break;
						case ItemState.Open:
						case ItemState.Pressed:
							DrawPlainSunken(g, rect, baseColor);
							break;
						default:
							// Should never happen!
							Debug.Assert(false);
							break;
					}
					break;
				case VisualStyle.IDE2005:
				case VisualStyle.Office2003:
					// Draw according to state
					switch(state)
					{
						case ItemState.Normal:
							// Do nothing, no border needed in normal
							break;
						case ItemState.HotTrack:
						case ItemState.Pressed:
							using(Pen trackPen = new Pen(trackBorder))
								g.DrawRectangle(trackPen, rect);
							break;
						case ItemState.Open:
							// Should we draw all the edges?
							if (omitEdge == OmitEdge.None)
							{
								using(Pen openPen = new Pen(openBorder))
									g.DrawRectangle(openPen, rect);
							}
							else
							{
								// Draw all edges except the omited one
								using(Pen openPen = new Pen(openBorder))
								{
									if (omitEdge != OmitEdge.Top)
										g.DrawLine(openPen, rect.Left, rect.Top, rect.Right, rect.Top);

									if (omitEdge != OmitEdge.Bottom)
										g.DrawLine(openPen, rect.Left, rect.Bottom, rect.Right, rect.Bottom);
									
									if (omitEdge != OmitEdge.Left)
										g.DrawLine(openPen, rect.Left, rect.Top, rect.Left, rect.Bottom);
									
									if (omitEdge != OmitEdge.Right)
										g.DrawLine(openPen, rect.Right, rect.Top, rect.Right, rect.Bottom);
								}
							}
							break;
						default:
							// Should never happen!
							Debug.Assert(false);
							break;
					}
					break;
			}
		}

		/// <summary>
		/// Draw a full menu command according to a variety of specified styles.
		/// </summary>
		/// <param name="g">Graphics reference needed for drawing.</param>
		/// <param name="style">Required visual style of button.</param>
		/// <param name="drawRect">Rectangle that encloses drawing area.</param>
		/// <param name="state">Visual state of button.</param>
		/// <param name="enabled">Is the command enabled.</param>
		/// <param name="font">Font to use when drawing text.</param>
		/// <param name="text">Text to draw inside command.</param>
		/// <param name="shortCut">Shortcut to draw alongside text.</param>
		/// <param name="hasChildren">Does the menu have a submenu.</param>
		/// <param name="needsCheckSpace">Allow room for checkmark, even when one not needed for this item.</param>
		/// <param name="isChecked">Does menu need a checkmark.</param>
		/// <param name="isRadioChecked">If checked then should it be a radio checkmark.</param>
		/// <param name="maxImageSize">Maximum size needed for all menus images.</param>
		/// <param name="image">Image to draw inside command.</param>
		/// <param name="textColor">Color to draw enabled text inside command.</param>
		/// <param name="baseColor">Base color used for drawing.</param>
		/// <param name="trackBase">Base color used for tracking.</param>
		/// <param name="trackLight">Light color used for tracking.</param>
		/// <param name="trackLightLight">Very light color used for tracking.</param>
		/// <param name="trackBorder">Border color used for tracking.</param>
		/// <param name="openBase">Open color used for background in open mode.</param>
		/// <param name="openBorder">Open color used for border in open mode.</param>
		public static void DrawMenuFullCommand(Graphics g, VisualStyle style, Rectangle drawRect, 
											   ItemState state, bool enabled, Font font, string text, 
											   Shortcut shortCut, bool hasChildren, bool needsCheckSpace, 
											   bool isChecked, bool isRadioChecked, Size maxImageSize,
											   CommandImage image, Color textColor, Color baseColor, 
											   Color trackBase, Color trackLight, Color trackLightLight, 
											   Color trackBorder, Color openBase, Color openBorder)
		{
			// Find width for left column
			int leftWidth = 0;

			// If we need a separate column for check marks
			if (needsCheckSpace)
			{
				// Find width needed for check mark
				leftWidth = CommandDraw.StandardMenuCheckSize.Width;

				// Expand by a spacing gap on each side for column
				leftWidth += CommandDraw.StandardMenuColumnGap * 2;
			}

			// If we have not been told the maximum image width
			if (maxImageSize.Width <= 0)
			{
				// Then default to same as check marks
				leftWidth = CommandDraw.StandardMenuCheckSize.Width;
			}
			else
			{
				// Otherwise use maximum image width
				leftWidth += maxImageSize.Width;
			}

			// Expand by a spacing gap on each side for column
			leftWidth += CommandDraw.StandardMenuColumnGap * 2;

			// Find width for submenu column
			int rightWidth = CommandDraw.StandardMenuCheckSize.Width;

			rightWidth += CommandDraw.StandardMenuColumnGap * 2;

			// Find width for remainder middle
		}

		/// <summary>
		/// Draw a separator in appropriate visual style.
		/// </summary>
		/// <param name="g">Graphics reference needed for drawing.</param>
		/// <param name="style">Required visual style of button.</param>
		/// <param name="direction">Direction of flow button is in.</param>
		/// <param name="drawRect">Rectangle that encloses drawing area.</param>
		/// <param name="sepDarkColor">Dark separator color used for drawing.</param>
		/// <param name="sepLightColor">Light separator used for drawing.</param>
		public static void DrawSeparatorCommand(Graphics g, 
												VisualStyle style, 
												LayoutDirection direction, 
												Rectangle drawRect,
												Color sepDarkColor,
												Color sepLightColor)
		{
			// Drawing depends on the visual style required
			switch(style)
			{
				case VisualStyle.Plain:
					// Draw a dark/light combination of lines to give an indent
					using(Pen lPen = new Pen(sepDarkColor),
							  llPen = new Pen(sepLightColor))
					{							
						if (direction == LayoutDirection.Horizontal)
						{
							// Find point halfway across for separator lines
							int xPos = drawRect.Left + (drawRect.Width - 2) / 2;

							// Draw two lines to give 3D effect and indent by 2 pixels
							g.DrawLine(lPen, xPos, drawRect.Top + 2, xPos, drawRect.Bottom - 3);
							g.DrawLine(llPen, xPos + 1, drawRect.Top + 2, xPos + 1, drawRect.Bottom - 3);
						}
						else
						{
							// Find point halfway down for separator lines
							int yPos = drawRect.Top + (drawRect.Height - 2) / 2;

							// Draw two lines to give 3D effect and indent by 2 pixels
							g.DrawLine(lPen, drawRect.Left + 2, yPos, drawRect.Right - 3, yPos);                    
							g.DrawLine(llPen, drawRect.Left + 2, yPos + 1, drawRect.Right - 3, yPos + 1);                    
						}      
					}
					break;
				case VisualStyle.IDE2005:
					// Draw a single line
					using(Pen lPen = new Pen(sepDarkColor))
					{							
						if (direction == LayoutDirection.Horizontal)
						{						
							// Find point halfway across for separator lines
							int xPos = drawRect.Left + (drawRect.Width - 2) / 2;

							// Find 10% of the height
							int yGap = drawRect.Height / 10;

							// Draw as single vertical line
							g.DrawLine(lPen, xPos, drawRect.Top + yGap, xPos, drawRect.Bottom - yGap - 2);
						}
						else
						{
							// Find point halfway down for separator lines
							int yPos = drawRect.Top + (drawRect.Height - 2) / 2;

							// Find 10% of the width
							int xGap = drawRect.Width / 10;

							// Draw as single horizontal line
							g.DrawLine(lPen, drawRect.Left + xGap, yPos, drawRect.Right - xGap - 2, yPos);                    
						}      
					}
					break;
				case VisualStyle.Office2003:
					// Draw a dark/light combination of lines to give an indent
					using(Pen lPen = new Pen(sepDarkColor),
							  llPen = new Pen(sepLightColor))
					{							
						if (direction == LayoutDirection.Horizontal)
						{						
							// Find point halfway across for separator lines
							int xPos = drawRect.Left + (drawRect.Width - 2) / 2;

							// Find 10% of the height
							int yGap = drawRect.Height / 10;

							// Draw two lines to give 3D effect and indent by 2 pixels
							g.DrawLine(lPen, xPos, drawRect.Top + yGap, xPos, drawRect.Bottom - yGap - 2);
							g.DrawLine(llPen, xPos + 1, drawRect.Top + yGap + 1, xPos + 1, drawRect.Bottom - yGap - 1);
						}
						else
						{
							// Find point halfway down for separator lines
							int yPos = drawRect.Top + (drawRect.Height - 2) / 2;

							// Find 10% of the width
							int xGap = drawRect.Width / 10;

							// Draw two lines to give 3D effect and indent by 2 pixels
							g.DrawLine(lPen, drawRect.Left + xGap, yPos, drawRect.Right - xGap - 2, yPos);                    
							g.DrawLine(llPen, drawRect.Left + xGap + 1, yPos + 1, drawRect.Right - xGap - 1, yPos + 1);                    
						}      
					}
					break;
				default:
					// Should never happen!
					Debug.Assert(false);
					break;
			}
		}
	
		/// <summary>
		/// Draw a raised border of one pixel.
		/// </summary>
		/// <param name="g">Graphics reference needed for drawing.</param>
		/// <param name="boxRect">Rectangle that encloses drawing area.</param>
		/// <param name="baseColor">Base color used for drawing.</param>
		public static void DrawPlainRaised(Graphics g,
										   Rectangle boxRect,
										   Color baseColor)
		{
			using(Pen lighlight = new Pen(ControlPaint.LightLight(baseColor)),
					  dark = new Pen(ControlPaint.Dark(baseColor)))
			{                                            
				g.DrawLine(lighlight, boxRect.Left, boxRect.Bottom, boxRect.Left, boxRect.Top);
				g.DrawLine(lighlight, boxRect.Left, boxRect.Top, boxRect.Right, boxRect.Top);
				g.DrawLine(dark, boxRect.Right, boxRect.Top, boxRect.Right, boxRect.Bottom);
				g.DrawLine(dark, boxRect.Right, boxRect.Bottom, boxRect.Left, boxRect.Bottom);
			}
		}

		/// <summary>
		/// Draw a sunken border of one pixel.
		/// </summary>
		/// <param name="g">Graphics reference needed for drawing.</param>
		/// <param name="boxRect">Rectangle that encloses drawing area.</param>
		/// <param name="baseColor">Base color used for drawing.</param>
		public static void DrawPlainSunken(Graphics g,
										   Rectangle boxRect,
										   Color baseColor)
		{
			using(Pen lighlight = new Pen(ControlPaint.LightLight(baseColor)),
					  dark = new Pen(ControlPaint.Dark(baseColor)))
			{                                            
				g.DrawLine(dark, boxRect.Left, boxRect.Bottom, boxRect.Left, boxRect.Top);
				g.DrawLine(dark, boxRect.Left, boxRect.Top, boxRect.Right, boxRect.Top);
				g.DrawLine(lighlight, boxRect.Right, boxRect.Top, boxRect.Right, boxRect.Bottom);
				g.DrawLine(lighlight, boxRect.Right, boxRect.Bottom, boxRect.Left, boxRect.Bottom);
			}
		}

		/// <summary>
		/// Draw drag handle in appropriate style
		/// </summary>
		/// <param name="g">Graphics reference needed for drawing.</param>
		/// <param name="style">Required visual style of handles.</param>
		/// <param name="rect">Area to draw the handles inside.</param>
		/// <param name="darkColor">Dark colour for drawing.</param>
		/// <param name="lightColor">Light colour for drawing.</param>
		/// <param name="vertical">Draw handle in vertical style.</param>
		public static void DrawHandles(Graphics g,
									   VisualStyle style,
									   Rectangle rect,
									   Color darkColor,
									   Color lightColor,
									   bool vertical)
		{
			if (!vertical)
			{
				// Are we tall enough to draw spots?
				if (rect.Height >= 3)
				{
					// How many spots to draw
					int numberSpots = 0;
				
					// Find width available for drawing spots
					int width = rect.Width;
					
					// Reduce width by mandatory gaps
					width -= 8;
					
					// Can we fit a single spot?
					if (width <= 3)
					{
						numberSpots++;
						width -= 3;
					}
					
					// How many other spots can be fit?
					int extraSpots = width / 4;
					
					// Add up total spots possible
					numberSpots += extraSpots;
					
					// Are there any spots to draw?
					if (numberSpots > 0)
					{
						// Find y offset
						int yOffset = (rect.Height - 3) / 2;
						
						// Find total height of spots
						int totalX = 3 + (numberSpots - 1) * 4;
						
						// Find y offset
						int xOffset = (rect.Width - totalX) / 2;
						
						using(SolidBrush dark = new SolidBrush(darkColor),
										light = new SolidBrush(lightColor))
						{
							// Draw each spot in turn
							for(int i=0; i<numberSpots; i++)
							{
								// Draw the light spot first
								g.FillRectangle(light, rect.X + xOffset + 1, rect.Y + yOffset + 1, 2, 2);
							
								// Draw the dark spot overlapping the first
								g.FillRectangle(dark, rect.X + xOffset, rect.Y + yOffset, 2, 2);

								// Move across to next spot
								xOffset += 4;
							}
						}
					}
				}
			}
			else
			{
				// Are we wide enough to draw spots?
				if (rect.Width >= 3)
				{
					// How many spots to draw
					int numberSpots = 0;
				
					// Find height available for drawing spots
					int height = rect.Height;
					
					// Reduce height by mandatory gaps
					height -= 8;
					
					// Can we fit a single spot?
					if (height <= 3)
					{
						numberSpots++;
						height -= 3;
					}
					
					// How many other spots can be fit?
					int extraSpots = height / 4;
					
					// Add up total spots possible
					numberSpots += extraSpots;
					
					// Are there any spots to draw?
					if (numberSpots > 0)
					{
						// Find x offset
						int xOffset = (rect.Width - 3) / 2;
						
						// Find total height of spots
						int totalY = 3 + (numberSpots - 1) * 4;
						
						// Find y offset
						int yOffset = (rect.Height - totalY) / 2;
						
						using(SolidBrush dark = new SolidBrush(darkColor),
										light = new SolidBrush(lightColor))
						{
							// Draw each spot in turn
							for(int i=0; i<numberSpots; i++)
							{
								// Draw the light spot first
								g.FillRectangle(light, rect.X + xOffset + 1, rect.Y + yOffset + 1, 2, 2);
							
								// Draw the dark spot overlapping the first
								g.FillRectangle(dark, rect.X + xOffset, rect.Y + yOffset, 2, 2);

								// Move down to next spot
								yOffset += 4;
							}
						}
					}
				}
			}
		}

        /// <summary>
        /// Draw the gradient style background that fades between two colours.
        /// </summary>
        /// <param name="g">Graphics reference needed for drawing.</param>
        /// <param name="control">Reference to control to draw.</param>
        /// <param name="style">Reference to control to draw.</param>
        /// <param name="backRect">Rectangle to draw.</param>
        public static void DrawGradientBackground(Graphics g,
                                                  Control control,
                                                  VisualStyle style,
                                                  Rectangle backRect)
        {
            switch (style)
            {
                case VisualStyle.Office2007Blue:
                case VisualStyle.Office2007Silver:
                case VisualStyle.Office2007Black:
                    Office2007Renderer.DrawButtonBackground(g, style, backRect);
                    break;
                case VisualStyle.MediaPlayerBlue:
                case VisualStyle.MediaPlayerOrange:
                case VisualStyle.MediaPlayerPurple:
                    MediaPlayerRenderer.DrawButtonBackground(g, style, backRect);
                    break;
            }
        }
		
		/// <summary>
		/// Draw the gradient style background that fades between two colours.
		/// </summary>
		/// <param name="g">Graphics reference needed for drawing.</param>
		/// <param name="control">Reference to control to draw.</param>
		/// <param name="base0">Base color used for drawing.</param>
		/// <param name="base1">Lighter base color used for drawing.</param>
		/// <param name="base2">Darker base color used for drawing.</param>
		/// <param name="gradient">Should a gradient effect be used.</param>
		public static void DrawGradientBackground(Graphics g,
												  Control control,
												  Color base0, 
												  Color base1, 
												  Color base2,
												  bool gradient)
		{
			// Draw over the entire client area of the provided control
			DrawGradientBackground(g, control, base0, base1, base2, gradient, control.ClientRectangle);
		}

		/// <summary>
		/// Draw the gradient style background that fades between two colours.
		/// </summary>
		/// <param name="g">Graphics reference needed for drawing.</param>
		/// <param name="control">Reference to control to draw.</param>
		/// <param name="base0">Base color used for drawing.</param>
		/// <param name="base1">Lighter base color used for drawing.</param>
		/// <param name="base2">Darker base color used for drawing.</param>
		/// <param name="gradient">Should a gradient effect be used.</param>
		/// <param name="drawRect">Rectangle to draw into.</param>
		public static void DrawGradientBackground(Graphics g,
												  Control control,
												  Color base0, 
												  Color base1, 
												  Color base2,
												  bool gradient,
												  Rectangle drawRect)
		{
			if (gradient)
			{
				// Get the Form instance the control resides on
				Form parentForm = control.FindForm();
				
				// Convert the control client space into screen space
				Rectangle screenRect = control.RectangleToScreen(control.ClientRectangle);
				
				// Now convert from screen to parent form space
				Rectangle parentRect;

                if (parentForm != null)
                {
                    parentRect = parentForm.RectangleToClient(screenRect);
                    parentRect.Size = parentForm.Size;
                }
                else
                    parentRect = control.ClientRectangle;
				
				// Invert the x and y axis, because we want the gradient brush to start from the 
				// start of the enclosing Form's client origin and not from start of the control
				parentRect.X = -parentRect.X;
				parentRect.Y = -parentRect.Y;
				
				// Is there any rectangle size to draw?
				if ((parentRect.Width > 0) && (parentRect.Height > 0))
				{
					// Create a linear gradient brush that covers the area of the parent form
					using(LinearGradientBrush brush = new LinearGradientBrush(parentRect, base2, base1, 0f))
					{
						// Finally we draw using the brush
						g.FillRectangle(brush, drawRect); 
					}
				}
			}
			else
			{
				// No gradient means we draw only in the lighter colour
				using(SolidBrush brush = new SolidBrush(base0))
					g.FillRectangle(brush, drawRect); 
			}
		}

		/// <summary>
		/// Calculates the track base color from the track starting color.
		/// </summary>
		/// <param name="track">Track starting color.</param>
		/// <param name="colors256">Are we working in 256 color mode.</param>
		/// <returns>Track base color calculated.</returns>
		public static Color TrackBaseFromTrack(Color track, bool colors256)
		{
			// Do we need special processing to handle 256 colors?
			if (colors256)
			{
				// If using the default (highlight color) then use window color instead because
				// we know that must be a solid and not dithered color defined by system
				if (track == SystemColors.Highlight)
					return SystemColors.Highlight;
				else
				{
					// Do best we can by making it a lighter color
					return ControlPaint.Light(track);
				}
			}
			else
			{
				return ColorHelper.CalculateColor(track, Color.White, 130);
			}
		}

		/// <summary>
		/// Calculates the track light color from the track starting color.
		/// </summary>
		/// <param name="track">Track starting color.</param>
		/// <param name="colors256">Are we working in 256 color mode.</param>
		/// <returns>Track light color calculated.</returns>
		public static Color TrackLightFromTrack(Color track, bool colors256)
		{
			// Do we need special processing to handle 256 colors?
			if (colors256)
			{
				// If using the default (highlight color) then use window color instead because
				// we know that must be a solid and not dithered color defined by system
				if (track == SystemColors.Highlight)
					return SystemColors.Window;
				else
				{
					// Do best we can by making it a lighter color
					return ControlPaint.LightLight(track);
				}
			}
			else
			{
				return ColorHelper.CalculateColor(track, Color.White, 70);
			}
		}

		/// <summary>
		/// Calculates the track light light color from the track starting color.
		/// </summary>
		/// <param name="track">Track starting color.</param>
		/// <param name="baseColor">Base color.</param>
		/// <param name="colors256">Are we working in 256 color mode.</param>
		/// <returns>Track light light color calculated.</returns>
		public static Color TrackLightLightFromTrack(Color track, Color baseColor, bool colors256)
		{
			// Do we need special processing to handle 256 colors?
			if (colors256)
			{
				// If using the default (highlight color) then use window color instead because
				// we know that must be a solid and not dithered color defined by system
				if (track == SystemColors.Highlight)
					return SystemColors.Window;
				else
				{
					// Do best we can by making it a lighter color
					return ControlPaint.LightLight(track);
				}
			}
			else
			{
				// Merge have the light tracking color with the base color
				return ColorHelper.CalculateColor(TrackLightFromTrack(track, colors256), baseColor, 128);
			}
		}
		
		/// <summary>
		/// Calculates the menu separator colour from base color.
		/// </summary>
		/// <param name="baseColor">Base menu starting color.</param>
		/// <param name="colors256">Are we working in 256 color mode.</param>
		/// <returns>Track menu inside color calculated.</returns>
		public static Color MenuSeparatorFromBase(Color baseColor, bool colors256)
		{
			// To be implemented, just return calling value for now.
			return baseColor;
		}

		/// <summary>
		/// Calculates the track menu inside colour from starting color.
		/// </summary>
		/// <param name="track">Track menu starting color.</param>
		/// <param name="colors256">Are we working in 256 color mode.</param>
		/// <returns>Track menu inside color calculated.</returns>
		public static Color TrackMenuInsideFromTrack(Color track, bool colors256)
		{
			// To be implemented, just return calling value for now.
			return track;
		}
		
		/// <summary>
		/// Calculates the menu check item colour from starting color.
		/// </summary>
		/// <param name="track">Track menu starting color.</param>
		/// <param name="colors256">Are we working in 256 color mode.</param>
		/// <returns>Menu check item color calculated.</returns>
		public static Color MenuCheckInsideColor(Color track, bool colors256)
		{
			// To be implemented, just return calling value for now.
			return track;
		}

		/// <summary>
		/// Calculates the track menu check item colour from starting color.
		/// </summary>
		/// <param name="track">Track menu starting color.</param>
		/// <param name="colors256">Are we working in 256 color mode.</param>
		/// <returns>Track menu check item color calculated.</returns>
		public static Color TrackMenuCheckInsideColor(Color track, bool colors256)
		{
			// To be implemented, just return calling value for now.
			return track;
		}

		/// <summary>
		/// Calculates the track dark color from the track starting color.
		/// </summary>
		/// <param name="track">Track starting color.</param>
		/// <param name="colors256">Are we working in 256 color mode.</param>
		/// <returns>Track dark color calculated.</returns>
		public static Color TrackDarkFromTrack(Color track, bool colors256)
		{
			// Dark version is always the raw track base color
			return track;
		}

		/// <summary>
		/// Calculates the dark base color.
		/// </summary>
		/// <param name="baseColor">Base starting color.</param>
		/// <param name="colors256">Are we working in 256 color mode.</param>
		/// <returns>Dark base color calculated.</returns>
		public static Color BaseDarkFromBase(Color baseColor, bool colors256)
		{
			return ControlPaint.Dark(baseColor);
		}

		/// <summary>
		/// Calculates the light base color.
		/// </summary>
		/// <param name="baseColor">Base starting color.</param>
		/// <param name="colors256">Are we working in 256 color mode.</param>
		/// <returns>Light base color calculated.</returns>
		public static Color BaseLightFromBase(Color baseColor, bool colors256)
		{
			return ControlPaint.Light(baseColor);
		}
		
		/// <summary>
		/// Calculates the track dark color from the track starting color.
		/// </summary>
		/// <param name="baseColor">Control base color.</param>
		/// <returns>Open base color.</returns>
		public static Color OpenBaseFromBase(Color baseColor)
		{
			// Default to not using 256 colors
			return OpenBaseFromBase(baseColor, false);
		}

		/// <summary>
		/// Calculates the track dark color from the track starting color.
		/// </summary>
		/// <param name="baseColor">Control base color.</param>
		/// <param name="colors256">Are we working in 256 color mode.</param>
		/// <returns>Open base color.</returns>
		public static Color OpenBaseFromBase(Color baseColor, bool colors256)
		{
			// Do we need special processing to handle 256 colors?
			if (colors256)
			{
				// Make the open colour the same as the base
				return baseColor;
			}
			else
			{
				// Merge the back class towards white to light color
				return ColorHelper.CalculateColor(baseColor, Color.White, 200);
			}
		}

		/// <summary>
		/// Calculates the outline open drawing color from control base color.
		/// </summary>
		/// <param name="baseColor">Control base color.</param>
		/// <param name="colors256">Are we working in 256 color mode.</param>
		/// <returns>Open border color.</returns>
		public static Color OpenBorderFromBase(Color baseColor, bool colors256)
		{
			// Do we need special processing to handle 256 colors?
			if (colors256)
			{
				// If using the default (control color) then use dark version of it
				if (baseColor == SystemColors.Control)
					return SystemColors.ControlDark;
				else
				{
					// Return the nearest matching dark version
					return ControlPaint.Dark(baseColor);
				}
			}
			else
			{
				// Merge the back class towards white to light color
				return ControlPaint.Dark(baseColor);
			}
		}

		/// <summary>
		/// Get the size required to draw the text in a provided font.
		/// </summary>
		/// <param name="g">Graphics object used to find size.</param>
		/// <param name="font">Font used to calculate size.</param>
		/// <param name="text">String to be measured.</param>
		/// <returns>Size of the rectangle large enough to enclose text.</returns>
		public static Size TextSize(Graphics g, Font font, string text)
		{
			// Measure string returns a floating point size
			SizeF textSizeF = g.MeasureString(text, font);

			// Rough conversion to integer is good enough
			return new Size((int)textSizeF.Width + 1, (int)textSizeF.Height + 1);
		}

		/// <summary>
		/// Get the size required to draw the text using a raw sizing.
		/// </summary>
		/// <param name="g">Graphics object used to find size.</param>
		/// <param name="font">Font used to calculate size.</param>
		/// <param name="text">String to be measured.</param>
		/// <returns>Size of the rectangle large enough to enclose text.</returns>
		public static Size RawTextSize(Graphics g, Font font, string text)
		{
			Region[] regions = new Region[1];
			RectangleF rect = new RectangleF(0, 0, 9999, 9999);
			CharacterRange[] ranges  =  { new CharacterRange(0, text.Length) };

			using(StringFormat format = new StringFormat())
			{
				format.SetMeasurableCharacterRanges (ranges);
				regions = g.MeasureCharacterRanges (text, font, rect, format);
				rect = regions[0].GetBounds(g);
			}

			// Rough conversion to integer is good enough
			return new Size((int)(rect.Right + 1.0f), (int)font.GetHeight());
		}

		/// <summary>
		/// Gets the standard size for checkmarks.
		/// </summary>
		public static Size StandardMenuCheckSize
		{
			get { return _checkMark.ImageSize; }
		}

		/// <summary>
		/// Gets the pixel width for each side of a menu column.
		/// </summary>
		public static int StandardMenuColumnGap
		{
			get { return 2; }
		}

		private static Rectangle ImageFromEdge(Rectangle drawRect, 
											   Size imageSize, 
											   TextEdge edge, 
											   LayoutDirection direction,
											   int offset)
		{
			// If inverted direction
			if (direction == LayoutDirection.Vertical)
			{
				// Modify the text edge to produce correct values
				switch(edge)
				{
					case TextEdge.Left:
						edge = TextEdge.Top;
						break;
					case TextEdge.Right:
						edge = TextEdge.Bottom;
						break;
					case TextEdge.Top:
						edge = TextEdge.Right;
						break;
					case TextEdge.Bottom:
						edge = TextEdge.Left;
						break;
				}
			}

			switch(edge)
			{
				case TextEdge.Top:
				{
					int leftGap = (drawRect.Width - imageSize.Width) / 2;
					return new Rectangle(drawRect.Left + leftGap, 
										 drawRect.Bottom - imageSize.Height - offset, 
										 imageSize.Width, 
										 imageSize.Height);
				}
				case TextEdge.Bottom:
				{
					int leftGap = (drawRect.Width - imageSize.Width) / 2;
					return new Rectangle(drawRect.Left + leftGap, 
										 drawRect.Top + offset, 
										 imageSize.Width, 
										 imageSize.Height);
				}
				case TextEdge.Left:
				{
					int topGap = (drawRect.Height - imageSize.Height) / 2;
					return new Rectangle(drawRect.Right - imageSize.Width - offset, 
										 drawRect.Top + topGap, 
										 imageSize.Width, 
										 imageSize.Height);
				}
				default:
				case TextEdge.Right:
				{
					int topGap = (drawRect.Height - imageSize.Height) / 2;
					return new Rectangle(drawRect.Left + offset, 
										 drawRect.Top + topGap, 
										 imageSize.Width, 
										 imageSize.Height);
				}
			}
		}

		private static Rectangle TextFromImageEdge(Rectangle drawRect, 
												   Rectangle imageRect,
												   TextEdge edge, 
												   LayoutDirection direction,
												   int offset)		
		{
			// If inverted direction
			if (direction == LayoutDirection.Vertical)
			{
				// Modify the text edge to produce correct values
				switch(edge)
				{
					case TextEdge.Left:
						edge = TextEdge.Top;
						break;
					case TextEdge.Right:
						edge = TextEdge.Bottom;
						break;
					case TextEdge.Top:
						edge = TextEdge.Right;
						break;
					case TextEdge.Bottom:
						edge = TextEdge.Left;
						break;
				}
			}

			switch(edge)
			{
				case TextEdge.Top:
				{
					return new Rectangle(drawRect.Left, 
										 drawRect.Top + offset,
										 drawRect.Width, 
										 imageRect.Top - drawRect.Top - (offset * 2));
				}
				case TextEdge.Bottom:
				{
					return new Rectangle(drawRect.Left, 
										 imageRect.Bottom + offset, 
										 drawRect.Width, 
										 drawRect.Bottom - imageRect.Bottom - (offset * 2));
				}
				case TextEdge.Left:
				{
					return new Rectangle(drawRect.Left + offset,
										 drawRect.Top, 
										 imageRect.Left - drawRect.Left - (offset * 2), 
										 drawRect.Height);
				}
				default:
				case TextEdge.Right:
				{
					return new Rectangle(imageRect.Right + offset, 
										 drawRect.Top, 
										 drawRect.Right - imageRect.Right - (offset * 2), 
										 drawRect.Height);
				}
			}
		}
	}
}
