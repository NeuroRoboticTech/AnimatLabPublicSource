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
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Crownwood.DotNetMagic.Win32;

namespace Crownwood.DotNetMagic.Common
{
	/// <summary>
	/// Show drop indicators for possible drag and drop positions.
	/// </summary>
	public class DropIndicators : Form
	{
		// Class constants
		private const int MIDDLE_SIZE = 32;
		private const int MIDDLE_CIRCLE_OFFSET = 3;
		private const int DIAMOND_SIZE = 42;
		private const int DIAMOND_CUTOUT_INDENT = 12;
		private const int DIAMOND_CUTOUT_INSET = 10;
		private const int DIAMOND_CUTOUT_CENTRE = 20;
		private const int ARROWBOX_HEIGHT = 14;
		private const int ARROWBOX_HEIGHT_MID = 7;
		private const int ARROWBOX_WIDTH = 12;
		private const int ARROWBOX_WIDTH_HEADER = 4;
		private const int ARROWBOX_POINT_XOFFSET = 6;
		private const int ARROWBOX_POINT_YOFFSET = 5;
		private const int COLOR_LIGHT_CHANGE = 35;

		// Instance fields
		private bool _squares;
		private bool _showBack;
		private bool _showMiddle;
		private bool _showLeft;
		private bool _showRight;
		private bool _showTop;
		private bool _showBottom;
		private int _active;
        private VisualStyle _style;
		private Rectangle _rectMiddle;
		private Rectangle _rectLeft;
		private Rectangle _rectRight;
		private Rectangle _rectTop;
		private Rectangle _rectBottom;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Initialize a new instance of the DropIndicators class.
		/// </summary>
        /// <param name="style">Drawing style.</param>
        public DropIndicators(VisualStyle style)
		{
            // Cache the drawing style
            _style = style;

			// Default display state
			_squares = true;
			_showBack = true;
			_showMiddle = true;
			_showLeft = true;
			_showRight = true;
			_showTop = true;
			_showBottom = true;
			_active = 0;

			// Required for Windows Form Designer support
			InitializeComponent();
		}

		/// <summary>
		/// Initialize a new instance of the DropIndicators class.
		/// </summary>
		/// <param name="squares">Show as squares or diamonds.</param>
		/// <param name="showMiddle">Show middle hot area.</param>
		/// <param name="showLeft">Show left hot area.</param>
		/// <param name="showRight">Show right hot area.</param>
		/// <param name="showTop">Show top hot area.</param>
		/// <param name="showBottom">Show bottom hot area.</param>
        /// <param name="style">Drawing style.</param>
        public DropIndicators(bool squares, bool showMiddle,
							  bool showLeft, bool showRight,
							  bool showTop, bool showBottom,
                              VisualStyle style)
		{
            // Cache the drawing style
            _style = style;
            
            // Remember required display state
			_squares = squares;
			_showMiddle = showMiddle;
			_showLeft = showLeft;
			_showRight = showRight;
			_showTop = showTop;
			_showBottom = showBottom;
			_active = 0;

			int total = 0;

			// Count how many indicators needed
			if (_showMiddle) total++;
			if (_showLeft) total++;
			if (_showRight) total++;
			if (_showTop) total++;
			if (_showBottom) total++;

			if (!squares)
			{
				// If more than one then show all background
				if (total > 1)
					_showBack = true;
			}
			else
			{
				// If more than one indicator
				if (total > 1)
				{
					// If an indicator in horizontal and vertical direction, then
					// must draw the entire cross background, otherwise we leave
					// and let just the required direction be drawn
					if ((_showLeft || _showRight) && (_showTop || _showBottom))
						_showBack = true;
					else
						_showBack = false;
				}
				else
					_showBack = false;
			}

			// Required for Windows Form Designer support
			InitializeComponent();
		}

		/// <summary>
		/// Show the window relative to provided screen rectangle.
		/// </summary>
		/// <param name="screenRect">Screen rectangle.</param>
		public void ShowRelative(Rectangle screenRect)
		{
			// Find middle points
			int yMid = screenRect.Y + (screenRect.Height / 2);
			int xMid = screenRect.X + (screenRect.Width / 2);

			// If only showing the left indicator
			if (_showLeft && !_showRight && !_showMiddle && !_showTop && !_showBottom)
				Location = new Point(screenRect.Left + 10, yMid - (_squares ? 43 : 52));
			else if (!_showLeft && _showRight && !_showMiddle && !_showTop && !_showBottom)
				Location = new Point(screenRect.Right - (_squares ? 97 : 110), yMid - (_squares ? 43 : 52));
			else if (!_showLeft && !_showRight && !_showMiddle && _showTop && !_showBottom)
				Location = new Point(xMid - (_squares ? 43 : 50), screenRect.Top + 10);
			else if (!_showLeft && !_showRight && !_showMiddle && !_showTop && _showBottom)
				Location = new Point(xMid - (_squares ? 43 : 50), screenRect.Bottom - (_squares ? 97 : 110));
			else
				Location = new Point(xMid - (_squares ? 43 : 50), yMid - (_squares ? 43 : 52));

			// Show the window without activating it (i.e. do not take focus)
			User32.ShowWindow(this.Handle, (short)Win32.ShowWindowStyles.SW_SHOWNOACTIVATE);
		}

		/// <summary>
		/// Perofrm mouse hit testing against a screen point.
		/// </summary>
		/// <param name="screenPoint">Screen point.</param>
		/// <returns>Area that is active.</returns>
		public int ScreenMouseMove(Point screenPoint)
		{
			// Convert from screen to client coordinates
			Point pt = PointToClient(screenPoint);

			// Remember the current active value
			int activeBefore = _active;

			// Reset active back to nothing
			_active = 0;

			// Find new active area
			if (_showTop && _rectTop.Contains(pt))			_active = 3;
			if (_showBottom && _rectBottom.Contains(pt))	_active = 4;
			if (_showLeft && _rectLeft.Contains(pt))		_active = 1;
			if (_showRight && _rectRight.Contains(pt))		_active = 2;
			
			// Only consider the middle if the others do not match
			if ((_active == 0) && _showMiddle && _rectMiddle.Contains(pt))	
				_active = 5;

			// Do we need to update the display?
			if (_active != activeBefore)
				Invalidate();

			return _active;
		}

		/// <summary>
		/// Ensure the state is updated to reflect the mouse not being over the control.
		/// </summary>
		public void MouseReset()
		{
			// Do we need to update display?
			if (_active != 0)
			{
				_active = 0;
				Invalidate();
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// DropIndicators
			// 
			this.BackColor = System.Drawing.Color.Silver;
			this.ClientSize = new System.Drawing.Size(105, 105);
			this.ControlBox = false;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Location = new System.Drawing.Point(100, 200);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DropIndicators";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "DropIndicators";
			this.TransparencyKey = System.Drawing.Color.Silver;
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.DropIndicators_Paint);

		}
		#endregion

		private void DropIndicators_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if (_squares)
				DrawSquares(e.Graphics,
							_showBack,
							_showLeft, (_active == 1), 
							_showRight, (_active == 2), 
							_showTop, (_active == 3), 
							_showBottom, (_active == 4), 
							_showMiddle, (_active == 5));
			else
				DrawDiamonds(e.Graphics, 
							 _showBack,
							 _showLeft, (_active == 1), 
							 _showRight, (_active == 2), 
							 _showTop, (_active == 3), 
							 _showBottom, (_active == 4), 
							 _showMiddle, (_active == 5));
		}

		private void DrawSquares(Graphics g,
								 bool background, 
								 bool leftV, bool leftA, 
								 bool rightV, bool rightA,
								 bool topV, bool topA, 
								 bool bottomV, bool bottomA,
								 bool middleV, bool middleA)
		{
            Color activeColor;
            Color inactiveColor;

            switch (_style)
            {
                case VisualStyle.Office2007Blue:
                case VisualStyle.Office2007Silver:
                case VisualStyle.Office2007Black:
                    activeColor = Office2007ColorTable.IndicatorsActive(_style);
                    inactiveColor = Office2007ColorTable.IndicatorsInactive(_style);
                    break;
                case VisualStyle.MediaPlayerBlue:
                case VisualStyle.MediaPlayerOrange:
                case VisualStyle.MediaPlayerPurple:
                    activeColor = MediaPlayerColorTable.IndicatorsActive(_style);
                    inactiveColor = MediaPlayerColorTable.IndicatorsInactive(_style);
                    break;
                default:
                    activeColor = SystemColors.ActiveCaption;
                    inactiveColor = SystemColors.InactiveCaption;
                    break;
            }


			DrawSquaresBackground(g, background);
            if (leftV)      DrawSquaresLeft(g, activeColor, inactiveColor, leftA);
            if (rightV)     DrawSquaresRight(g, activeColor, inactiveColor, rightA);
            if (topV)       DrawSquaresTop(g, activeColor, inactiveColor, topA);
            if (bottomV)    DrawSquaresBottom(g, activeColor, inactiveColor, bottomA);
            if (middleV)    DrawSquaresMiddle(g, activeColor, inactiveColor, middleA);
		}
	
		private void DrawDiamonds(Graphics g,
								 bool background, 
								 bool leftV, bool leftA, 
								 bool rightV, bool rightA,
								 bool topV, bool topA, 
								 bool bottomV, bool bottomA,
								 bool middleV, bool middleA)
		{
			SmoothingMode temp = g.SmoothingMode;
			g.SmoothingMode = SmoothingMode.AntiAlias;

            Color active;
            Color inactive;
            Color arrow;

            switch (_style)
            {
                case VisualStyle.Office2007Blue:
                case VisualStyle.Office2007Silver:
                case VisualStyle.Office2007Black:
                    active = Office2007ColorTable.IndicatorsActive(_style);
                    inactive = Office2007ColorTable.IndicatorsInactive(_style);
                    arrow = active;
                    break;
                case VisualStyle.MediaPlayerBlue:
                case VisualStyle.MediaPlayerOrange:
                case VisualStyle.MediaPlayerPurple:
                    active = MediaPlayerColorTable.IndicatorsActive(_style);
                    inactive = MediaPlayerColorTable.IndicatorsInactive(_style);
                    arrow = active;
                    break;
                default:
                    active = SystemColors.ActiveCaption;
                    inactive = SystemColors.InactiveCaption;
                    arrow = SystemColors.ActiveCaptionText;
                    break;
            }
			
			if (background) DrawDiamondBackground(g, 0, 0);
            if (leftV)      DrawDiamondLeft(g, leftA ? active : inactive, 0, 31, arrow);
            if (rightV)     DrawDiamondRight(g, rightA ? active : inactive, 58, 31, arrow);
            if (topV)       DrawDiamondTop(g, topA ? active : inactive, 29, 0, arrow);
            if (bottomV)    DrawDiamondBottom(g, bottomA ? active : inactive, 29, 62, arrow);
            if (middleV)    DrawDiamondMiddle(g, middleA ? active : inactive, 34, 36, arrow);

			g.SmoothingMode = temp;
		}
						
		private void DrawSquaresBackground(Graphics g, bool background)
		{
			Color border = Color.FromArgb(181, 181, 181);
			Color start = Color.FromArgb(190, 190, 190);
			Color inside = Color.FromArgb(228, 228, 228);

			using(Pen borderPen = new Pen(border))
			{
				using(SolidBrush insideBrush = new SolidBrush(inside))
				{			
					using(LinearGradientBrush gradientLL = new LinearGradientBrush(new Rectangle(-1, -1, 5, 5), start, inside, 0f),
											  gradientTL = new LinearGradientBrush(new Rectangle(-1, 23, 5, 5), start, inside, 90f),
											  gradientCC = new LinearGradientBrush(new Rectangle(24, 25, 5, 5), start, inside, 45f),
											  gradientLT = new LinearGradientBrush(new Rectangle(28, -1, 5, 5), start, inside, 0f),
											  gradientML = new LinearGradientBrush(new Rectangle(22, -1, 5, 5), start, inside, 0f),
											  gradientMT = new LinearGradientBrush(new Rectangle(-1, 22, 5, 5), start, inside, 90f),
											  gradientTT = new LinearGradientBrush(new Rectangle(-1, -1, 5, 5), start, inside, 90f))
					{
						// Draw all the background cross?
						if (background)
						{
							// Create points for a polygon
							Point[] pts = new Point[]{new Point(0,  29), new Point(23, 29),
													  new Point(29, 23), new Point(29, 0),
													  new Point(57, 0),  new Point(57, 23),
													  new Point(63, 29), new Point(87, 29),
													  new Point(87, 57), new Point(63, 57),
													  new Point(57, 63), new Point(57, 87),
													  new Point(29, 87), new Point(29, 63),
													  new Point(23, 57), new Point(0,  57)};

							// Fill this area with a solid colour
							g.FillPolygon(insideBrush, pts);
				
							// Draw shadow at some of the box edges
							g.FillPolygon(gradientLL, new Point[]{new Point(1,57), new Point(1,30), new Point(4,33), new Point(4,57)});
							g.FillPolygon(gradientTL, new Point[]{new Point(1,30), new Point(25,30), new Point(27,33), new Point(3,33)});
							g.FillPolygon(gradientCC, new Point[]{new Point(23,30), new Point(30,23), new Point(33,26), new Point(26,33)});
							g.FillPolygon(gradientLT, new Point[]{new Point(30,1), new Point(30,24), new Point(33,26), new Point(33,4)});
							g.FillPolygon(gradientTT, new Point[]{new Point(30,1), new Point(57,1), new Point(57,4), new Point(33,4)});
							g.FillPolygon(gradientLT, new Point[]{new Point(30,63), new Point(30,87), new Point(33,87), new Point(33,66)});
							g.FillPolygon(gradientTL, new Point[]{new Point(63,30), new Point(87,30), new Point(87,33), new Point(66,33)});

							// Draw outline in darker colour
							g.DrawPolygon(borderPen, pts);
						}
						else if (_showLeft && _showRight)
						{
							// Create points for a polygon
							Point[] pts = new Point[]{new Point(0,  29), new Point(23, 29),
													  new Point(29, 23), new Point(57, 23),
													  new Point(63, 29), new Point(87, 29),
													  new Point(87, 57), new Point(63, 57),
													  new Point(57, 63), new Point(29, 63),
													  new Point(23, 57), new Point(0,  57)};

							// Fill this area with a solid colour
							g.FillPolygon(insideBrush, pts);

							// Draw shadow at some of the box edges
							g.FillPolygon(gradientLL, new Point[]{new Point(1,57), new Point(1,30), new Point(4,33), new Point(4,57)});
							g.FillPolygon(gradientTL, new Point[]{new Point(1,30), new Point(25,30), new Point(27,33), new Point(3,33)});
							g.FillPolygon(gradientCC, new Point[]{new Point(23,30), new Point(30,23), new Point(33,26), new Point(26,33)});
							g.FillPolygon(gradientMT, new Point[]{new Point(30,24), new Point(57,24), new Point(60,27), new Point(33,27)});
							g.FillPolygon(gradientTL, new Point[]{new Point(63,30), new Point(87,30), new Point(87,33), new Point(66,33)});

							// Draw outline in darker colour
							g.DrawPolygon(borderPen, pts);
						}
						else if (_showLeft)
						{
							// Only draw the background for the left square
							g.FillRectangle(insideBrush, 0, 29, 31, 28);
							g.DrawRectangle(borderPen, 0, 29, 31, 28);
						}
						else if (_showRight)
						{
							// Only draw the background for the right square
							g.FillRectangle(insideBrush, 56, 29, 31, 28);
							g.DrawRectangle(borderPen, 56, 29, 31, 28);
						}
						else if (_showTop && _showBottom)
						{
							// Create points for a polygon
							Point[] pts = new Point[]{new Point(23, 29), new Point(29, 23), 
													  new Point(29, 0),  new Point(57, 0),  
													  new Point(57, 23), new Point(63, 29), 
													  new Point(63, 57), new Point(57, 63), 
													  new Point(57, 87), new Point(29, 87), 
													  new Point(29, 63), new Point(23, 57)};

							// Fill this area with a solid colour
							g.FillPolygon(insideBrush, pts);
				
							g.FillPolygon(gradientLT, new Point[]{new Point(30,1), new Point(30,24), new Point(33,26), new Point(33,4)});
							g.FillPolygon(gradientTT, new Point[]{new Point(30,1), new Point(57,1), new Point(57,4), new Point(33,4)});
							g.FillPolygon(gradientCC, new Point[]{new Point(23,30), new Point(30,23), new Point(33,26), new Point(26,33)});
							g.FillPolygon(gradientML, new Point[]{new Point(24,57), new Point(24,30), new Point(27,33), new Point(27,60)});
							g.FillPolygon(gradientLT, new Point[]{new Point(30,63), new Point(30,87), new Point(33,87), new Point(33,66)});

							// Draw outline in darker colour
							g.DrawPolygon(borderPen, pts);
						}
						else if (_showTop)
						{
							// Only draw the background for the top square
							g.FillRectangle(insideBrush, 29, 0, 28, 31);
							g.DrawRectangle(borderPen, 29, 0, 28, 31);
						}
						else if (_showBottom)
						{
							// Only draw the background for the bottom square
							g.FillRectangle(insideBrush, 29, 56, 28, 31);
							g.DrawRectangle(borderPen, 29, 56, 28, 31);
						}
						else if (_showMiddle)
						{
							// Only draw the background for the middle square
							Point[] pts = new Point[]{new Point(23, 29), new Point(29, 23), 
													  new Point(57, 23), new Point(63, 29), 
													  new Point(63, 57), new Point(57, 63),
													  new Point(29, 63), new Point(23, 57)};

							g.FillPolygon(insideBrush, pts);
							g.DrawPolygon(borderPen, pts);
						}
					}
				}
			}
		}
		 
		private void DrawDiamondBackground(Graphics g, int x, int y)
		{
			// Create points for a polygon
			Point[] pts = new Point[]{new Point(x + 10, y + 52), 
									  new Point(x + 50, y + 12),
									  new Point(x + 90, y + 52),
									  new Point(x + 50, y + 92),
									  new Point(x + 10, y + 52)};
			
			// Fill this area with a solid colour
			g.FillPolygon(SystemBrushes.Control, pts);

			// Draw outline in darker colour
			g.DrawPolygon(SystemPens.ControlText, pts);
		}

		private void DrawSquaresLeft(Graphics g, 
                                     Color activeColor,
                                     Color inactiveColor,
                                     bool active)
		{
            Color borderColour = ControlPaint.Dark(activeColor);
			Color shadow1 = Color.FromArgb(190, 190, 190);
			Color shadow2 = Color.FromArgb(218, 218, 218);

			// Draw border around the window square
			using(Pen borderPen = new Pen(borderColour),
					  dashPen = new Pen(borderColour),
					  shadow1Pen = new Pen(shadow1),
					  shadow2Pen = new Pen(shadow2))
			{
				// Draw the caption area at top of window
				using(LinearGradientBrush middleBrush = new LinearGradientBrush(new Rectangle(4, 33, 23, 1), ControlPaint.LightLight(inactiveColor), activeColor, 0f),
										  bottomBrush = new LinearGradientBrush(new Rectangle(4, 34, 23, 1), ControlPaint.Light(activeColor), activeColor, 0f),
										  positionBrush = new LinearGradientBrush(new Rectangle(4, 35, 11, 1), Color.FromArgb(160, inactiveColor), Color.FromArgb(64, inactiveColor), 0f),
										  arrowBrush = new LinearGradientBrush(new Rectangle(18, 40, 5, 8), borderColour, Color.FromArgb(175, borderColour), 0f))
				{
					// Draw border
					g.DrawLine(borderPen, 4, 33, 4, 53);
					g.DrawLine(borderPen, 27, 33, 27, 53);
					g.DrawLine(borderPen, 4, 53, 27, 53);
					g.DrawLine(borderPen, 4, 33, 27, 33);

					// Draw shadows around right and bottom edges
					g.DrawLine(shadow1Pen, 5, 54, 28, 54);
					g.DrawLine(shadow1Pen, 28, 34, 28, 54);
					g.DrawLine(shadow2Pen, 6, 55, 29, 55);
					g.DrawLine(shadow2Pen, 29, 35, 29, 55);

					// Draw the caption area
					g.FillRectangle(middleBrush, 5, 34, 22, 1);
					g.FillRectangle(bottomBrush, 5, 35, 22, 1);
					
					// Draw client area
					g.FillRectangle(SystemBrushes.Window, 5, 36, 22, 17);

					// Draw docking edge indicator
					g.FillRectangle(positionBrush, 5, 36, 11, 17);

					// Draw a dashed line down the middle
					dashPen.DashStyle = DashStyle.Dot;
					g.DrawLine(dashPen, 15, 37, 15, 52);

					// Draw the direction arrow
					g.FillPolygon(arrowBrush, new Point[]{new Point(19, 44), new Point(23, 40), new Point(23, 48), new Point(19, 44)});
					
					// If active, then draw highlighted border
					if (active)
					{
						g.DrawLine(borderPen, 0, 29, 23, 29);
						g.DrawLine(borderPen, 0, 57, 23, 57);
						g.DrawLine(borderPen, 0, 29, 0, 57);
					}
				}
			}
			
			// Create the hot zone for the left, if not ready present
			if (_rectLeft.IsEmpty)
			{
				if (_showMiddle)
					_rectLeft = new Rectangle(0, 29, 29, 29);
				else
					_rectLeft = new Rectangle(0, 29, 32, 29);
			}
		}

        private void DrawSquaresRight(Graphics g,
                                      Color activeColor,
                                      Color inactiveColor,
                                      bool active)
		{
            Color borderColour = ControlPaint.Dark(activeColor);
			Color shadow1 = Color.FromArgb(190, 190, 190);
			Color shadow2 = Color.FromArgb(218, 218, 218);

			// Draw border around the window square
			using(Pen borderPen = new Pen(borderColour),
					  dashPen = new Pen(borderColour),
					  shadow1Pen = new Pen(shadow1),
					  shadow2Pen = new Pen(shadow2))
			{
				// Draw the caption area at top of window
				using(LinearGradientBrush middleBrush = new LinearGradientBrush(new Rectangle(60, 33, 23, 1), ControlPaint.LightLight(inactiveColor), activeColor, 0f),
										  bottomBrush = new LinearGradientBrush(new Rectangle(60, 34, 23, 1), ControlPaint.Light(activeColor), activeColor, 0f),
										  positionBrush = new LinearGradientBrush(new Rectangle(71, 35, 11, 1), Color.FromArgb(160, inactiveColor), Color.FromArgb(64, inactiveColor), 180f),
										  arrowBrush = new LinearGradientBrush(new Rectangle(68, 40, 5, 8), borderColour, Color.FromArgb(175, borderColour), 180f))
				{
					// Draw border
					g.DrawLine(borderPen, 60, 33, 60, 53);
					g.DrawLine(borderPen, 83, 33, 83, 53);
					g.DrawLine(borderPen, 60, 53, 83, 53);
					g.DrawLine(borderPen, 60, 33, 83, 33);

					// Draw shadows around right and bottom edges
					g.DrawLine(shadow1Pen, 61, 54, 84, 54);
					g.DrawLine(shadow1Pen, 84, 34, 84, 54);
					g.DrawLine(shadow2Pen, 62, 55, 85, 55);
					g.DrawLine(shadow2Pen, 85, 35, 85, 55);

					// Draw the caption area
					g.FillRectangle(middleBrush, 61, 34, 22, 1);
					g.FillRectangle(bottomBrush, 61, 35, 22, 1);
					
					// Draw client area
					g.FillRectangle(SystemBrushes.Window, 61, 36, 22, 17);

					// Draw docking edge indicator
					g.FillRectangle(positionBrush, 72, 36, 11, 17);

					// Draw a dashed line down the middle
					dashPen.DashStyle = DashStyle.Dot;
					g.DrawLine(dashPen, 72, 37, 72, 52);

					// Draw the direction arrow
					g.FillPolygon(arrowBrush, new Point[]{new Point(69, 44), new Point(65, 40), new Point(65, 48), new Point(69, 44)});

					// If active, then draw highlighted border
					if (active)
					{
						g.DrawLine(borderPen, 87, 29, 63, 29);
						g.DrawLine(borderPen, 87, 57, 63, 57);
						g.DrawLine(borderPen, 87, 29, 87, 57);
					}
				}
			}
			
			// Create the hot zone for the right, if not ready present
			if (_rectRight.IsEmpty)
			{
				if (_showMiddle)
					_rectRight = new Rectangle(59, 29, 29, 29);
				else
					_rectRight = new Rectangle(56, 29, 32, 29);
			}
		}

        private void DrawSquaresTop(Graphics g,
                                    Color activeColor,
                                    Color inactiveColor,
                                    bool active)
		{
            Color borderColour = ControlPaint.Dark(activeColor);
			Color shadow1 = Color.FromArgb(190, 190, 190);
			Color shadow2 = Color.FromArgb(218, 218, 218);

			// Draw border around the window square
			using(Pen borderPen = new Pen(borderColour),
					  dashPen = new Pen(borderColour),
					  shadow1Pen = new Pen(shadow1),
					  shadow2Pen = new Pen(shadow2))
			{
				// Draw the caption area at top of window
				using(LinearGradientBrush middleBrush = new LinearGradientBrush(new Rectangle(33, 5, 20, 1), ControlPaint.LightLight(inactiveColor), activeColor, 0f),
										  bottomBrush = new LinearGradientBrush(new Rectangle(33, 6, 20, 1), ControlPaint.Light(activeColor), activeColor, 0f),
										  positionBrush = new LinearGradientBrush(new Rectangle(34, 6, 19, 10), Color.FromArgb(160, inactiveColor), Color.FromArgb(64, inactiveColor), 90f),
										  arrowBrush = new LinearGradientBrush(new Rectangle(39, 40, 8, 4), borderColour, Color.FromArgb(175, borderColour), 90f))
				{
					// Draw border
					g.DrawLine(borderPen, 33, 4, 53, 4);
					g.DrawLine(borderPen, 53, 4, 53, 27);
					g.DrawLine(borderPen, 53, 27, 33, 27);
					g.DrawLine(borderPen, 33, 27, 33, 4);

					// Draw shadows around right and bottom edges
					g.DrawLine(shadow1Pen, 34, 28, 54, 28);
					g.DrawLine(shadow1Pen, 54, 5, 54, 28);
					g.DrawLine(shadow2Pen, 35, 29, 55, 29);
					g.DrawLine(shadow2Pen, 55, 6, 55, 29);

					// Draw the caption area
					g.FillRectangle(middleBrush, 34, 5, 19, 1);
					g.FillRectangle(bottomBrush, 34, 6, 19, 1);
					
					// Draw client area
					g.FillRectangle(SystemBrushes.Window, 34, 7, 19, 20);

					// Draw docking edge indicator
					g.FillRectangle(positionBrush, 34, 7, 19, 9);

					// Draw a dashed line down the middle
					dashPen.DashStyle = DashStyle.Dot;
					g.DrawLine(dashPen, 35, 15, 53, 15);

					// Draw the direction arrow
					g.FillPolygon(arrowBrush, new Point[]{new Point(43, 18), new Point(47, 23), new Point(39, 23), new Point(43, 18)});

					// If active, then draw highlighted border
					if (active)
					{
						g.DrawLine(borderPen, 29, 0, 29, 23);
						g.DrawLine(borderPen, 57, 0, 57, 23);
						g.DrawLine(borderPen, 29, 0, 57, 0);
					}
				}
			}

			// Create the hot zone for the top, if not ready present
			if (_rectTop.IsEmpty)
			{
				if (_showMiddle)
					_rectTop = new Rectangle(29, 0, 29, 29);
				else
					_rectTop = new Rectangle(29, 0, 29, 32);
			}
		}

        private void DrawSquaresBottom(Graphics g,
                                       Color activeColor,
                                       Color inactiveColor,
                                       bool active)
		{
            Color borderColour = ControlPaint.Dark(activeColor);
			Color shadow1 = Color.FromArgb(190, 190, 190);
			Color shadow2 = Color.FromArgb(218, 218, 218);

			// Draw border around the window square
			using(Pen borderPen = new Pen(borderColour),
					  dashPen = new Pen(borderColour),
					  shadow1Pen = new Pen(shadow1),
					  shadow2Pen = new Pen(shadow2))
			{
				// Draw the caption area at top of window
				using(LinearGradientBrush middleBrush = new LinearGradientBrush(new Rectangle(33, 61, 20, 1), ControlPaint.LightLight(inactiveColor), activeColor, 0f),
										  bottomBrush = new LinearGradientBrush(new Rectangle(33, 62, 20, 1), ControlPaint.Light(activeColor), activeColor, 0f),
										  positionBrush = new LinearGradientBrush(new Rectangle(34, 72, 19, 11), Color.FromArgb(160, inactiveColor), Color.FromArgb(64, inactiveColor), 270f),
										  arrowBrush = new LinearGradientBrush(new Rectangle(39, 66, 8, 4), borderColour, Color.FromArgb(175, borderColour), 270f))
				{
					// Draw border
					g.DrawLine(borderPen, 33, 60, 53, 60);
					g.DrawLine(borderPen, 53, 60, 53, 83);
					g.DrawLine(borderPen, 53, 83, 33, 83);
					g.DrawLine(borderPen, 33, 83, 33, 60);

					// Draw shadows around right and bottom edges
					g.DrawLine(shadow1Pen, 34, 84, 54, 84);
					g.DrawLine(shadow1Pen, 54, 61, 54, 84);
					g.DrawLine(shadow2Pen, 35, 85, 55, 85);
					g.DrawLine(shadow2Pen, 55, 61, 55, 85);

					// Draw the caption area
					g.FillRectangle(middleBrush, 34, 61, 19, 1);
					g.FillRectangle(bottomBrush, 34, 62, 19, 1);
					
					// Draw client area
					g.FillRectangle(SystemBrushes.Window, 34, 63, 19, 20);

					// Draw docking edge indicator
					g.FillRectangle(positionBrush, 34, 73, 19, 10);

					// Draw a dashed line down the middle
					dashPen.DashStyle = DashStyle.Dot;
					g.DrawLine(dashPen, 35, 73, 53, 73);

					// Draw the direction arrow
					g.FillPolygon(arrowBrush, new Point[]{new Point(43, 71), new Point(47, 67), new Point(40, 67), new Point(43, 71)});

					// If active, then draw highlighted border
					if (active)
					{
						g.DrawLine(borderPen, 29, 63, 29, 87);
						g.DrawLine(borderPen, 57, 63, 57, 87);
						g.DrawLine(borderPen, 29, 87, 57, 87);
					}
				}
			}

			// Create the hot zone for the bottom, if not ready present
			if (_rectBottom.IsEmpty)
			{
				if (_showMiddle)
					_rectBottom = new Rectangle(29, 59, 29, 29);
				else
					_rectBottom = new Rectangle(29, 56, 29, 31);
			}
		}

        private void DrawSquaresMiddle(Graphics g,
                                       Color activeColor,
                                       Color inactiveColor,
                                       bool active)
		{
            Color borderColour = ControlPaint.Dark(activeColor);
			Color shadow1 = Color.FromArgb(190, 190, 190);
			Color shadow2 = Color.FromArgb(218, 218, 218);

			// Draw border around the window square
			using(Pen borderPen = new Pen(borderColour),
					  dashPen = new Pen(borderColour),
					  shadow1Pen = new Pen(shadow1),
					  shadow2Pen = new Pen(shadow2))
			{
				// Draw the caption area at top of window
				using(LinearGradientBrush middleBrush = new LinearGradientBrush(new Rectangle(32, 34, 21, 1), ControlPaint.LightLight(inactiveColor), activeColor, 0f),
										  bottomBrush = new LinearGradientBrush(new Rectangle(32, 35, 21, 1), ControlPaint.Light(activeColor), activeColor, 0f))
				{
					// Draw border
					g.DrawLine(borderPen, 32, 32, 54, 32);
					g.DrawLine(borderPen, 32, 32, 32, 53);
					g.DrawLine(borderPen, 32, 53, 33, 54);
					g.DrawLine(borderPen, 33, 54, 41, 54);
					g.DrawLine(borderPen, 41, 54, 42, 52);
					g.DrawLine(borderPen, 42, 52, 42, 50);
					g.DrawLine(borderPen, 42, 50, 54, 50);
					g.DrawLine(borderPen, 54, 32, 54, 53);
					g.DrawLine(borderPen, 54, 53, 53, 54);
					g.DrawLine(borderPen, 53, 54, 49, 54);
					g.DrawLine(borderPen, 49, 54, 48, 53);
					g.DrawLine(borderPen, 48, 53, 48, 50);
					g.DrawLine(borderPen, 48, 53, 47, 54);
					g.DrawLine(borderPen, 47, 54, 43, 54);
					g.DrawLine(borderPen, 43, 54, 42, 53);

					// Draw the caption area
					g.FillRectangle(middleBrush, 33, 33, 21, 1);
					g.FillRectangle(bottomBrush, 33, 34, 21, 1);

					// Draw the client area
					g.FillRectangle(SystemBrushes.Window, 33, 35, 21, 15);
					g.FillRectangle(SystemBrushes.Window, 33, 50, 9, 3);
					g.FillRectangle(SystemBrushes.Window, 33, 53, 9, 1);
					g.FillRectangle(SystemBrushes.Window, 43, 51, 5, 3);
					g.FillRectangle(SystemBrushes.Window, 49, 51, 5, 3);

					// Fill the inner indicator area
					using(SolidBrush innerBrush = new SolidBrush(Color.FromArgb(64, inactiveColor)))
					{
						g.FillRectangle(innerBrush, 34, 36, 19, 13);
						g.FillRectangle(innerBrush, 34, 49, 7, 3);
						g.FillRectangle(innerBrush, 35, 52, 5, 1);
					}

					// Draw outline of the indicator area
					dashPen.DashStyle = DashStyle.Dot;
					g.DrawLine(dashPen, 34, 37, 34, 52);
					g.DrawLine(dashPen, 35, 52, 40, 52);
					g.DrawLine(dashPen, 40, 51, 40, 49);
					g.DrawLine(dashPen, 40, 51, 40, 48);
					g.DrawLine(dashPen, 41, 48, 53, 48);
					g.DrawLine(dashPen, 52, 47, 52, 36);
					g.DrawLine(dashPen, 35, 36, 52, 36);

					// Draw right han side shadow
					g.DrawLine(shadow1Pen, 55, 33, 55, 53);
					g.DrawLine(shadow2Pen, 56, 34, 56, 53);
					g.DrawLine(shadow1Pen, 33, 55, 53, 55);
					g.DrawLine(shadow1Pen, 53, 55, 55, 53);
					g.DrawLine(shadow2Pen, 34, 56, 53, 56);
					g.DrawLine(shadow2Pen, 53, 56, 56, 53);
				}

				// If active, then draw highlighted border
				if (active)
				{
					g.DrawLine(borderPen, 23, 29, 29, 23);
					g.DrawLine(borderPen, 57, 23, 63, 29);
					g.DrawLine(borderPen, 63, 57, 57, 63);
					g.DrawLine(borderPen, 23, 57, 29, 63);
				}
			}

			// Create the hot zone for the middle, if not ready present
			if (_rectMiddle.IsEmpty)
				_rectMiddle = new Rectangle(23, 23, 40, 40);
		}

        private void DrawDiamondMiddle(Graphics g, Color baseColor, int x, int y, Color fillColor)
		{
            using (SolidBrush fillBrush = new SolidBrush(Color.FromArgb(128, fillColor)))
			{
				DrawMiddleCircle(g, baseColor, x, y, MIDDLE_SIZE, MIDDLE_SIZE);
				DrawMiddleArrow(g, Pens.White, fillBrush, 
								x+(MIDDLE_SIZE-ARROWBOX_HEIGHT)/2,
								y+(MIDDLE_SIZE-ARROWBOX_HEIGHT)/2);
			}
		}
 
		private void DrawDiamondLeft(Graphics g, Color baseColor, int x, int y, Color fillColor)
		{
            using (SolidBrush fillBrush = new SolidBrush(Color.FromArgb(128, fillColor)))
			{
				DrawLeftDiamond(g, baseColor, x, y, DIAMOND_SIZE, DIAMOND_SIZE);
				DrawLeftArrow(g, Pens.White, fillBrush, 
							  x-1+(DIAMOND_SIZE-ARROWBOX_HEIGHT)/2,
							  y+(DIAMOND_SIZE-ARROWBOX_HEIGHT)/2);
			}
		}

        private void DrawDiamondRight(Graphics g, Color baseColor, int x, int y, Color fillColor)
		{
            using (SolidBrush fillBrush = new SolidBrush(Color.FromArgb(128, fillColor)))
			{
				DrawRightDiamond(g, baseColor, x, y, DIAMOND_SIZE, DIAMOND_SIZE);
				DrawRightArrow(g, Pens.White, fillBrush, 
							   x+3+(DIAMOND_SIZE-ARROWBOX_HEIGHT)/2,
							   y+(DIAMOND_SIZE-ARROWBOX_HEIGHT)/2);
			}
		}

        private void DrawDiamondTop(Graphics g, Color baseColor, int x, int y, Color fillColor)
		{
            using (SolidBrush fillBrush = new SolidBrush(Color.FromArgb(128, fillColor)))
			{
				DrawTopDiamond(g, baseColor, x, y, DIAMOND_SIZE, DIAMOND_SIZE);
				DrawTopArrow(g, Pens.White, fillBrush, 
							 x+(DIAMOND_SIZE-ARROWBOX_HEIGHT)/2,
							 y+(DIAMOND_SIZE-ARROWBOX_HEIGHT)/2);
			}
		}

        private void DrawDiamondBottom(Graphics g, Color baseColor, int x, int y, Color fillColor)
		{
            using (SolidBrush fillBrush = new SolidBrush(Color.FromArgb(128, fillColor)))
			{
				DrawBottomDiamond(g, baseColor, x, y, DIAMOND_SIZE, DIAMOND_SIZE);
				DrawBottomArrow(g, Pens.White, fillBrush, 
							    x+(DIAMOND_SIZE-ARROWBOX_HEIGHT)/2,
							    y+2+(DIAMOND_SIZE-ARROWBOX_HEIGHT)/2);
			}
		}

		private void DrawMiddleCircle(Graphics g, Color baseColor,
									  int x, int y, int width, int height)
		{
			// Create lighter and darker version of base color
			Color light = Color.FromArgb(Math.Min(255, (int)(baseColor.R + COLOR_LIGHT_CHANGE)),
										 Math.Min(255, (int)(baseColor.G + COLOR_LIGHT_CHANGE)),
										 Math.Min(255, (int)(baseColor.B + COLOR_LIGHT_CHANGE)));

			Color dark = Color.FromArgb(Math.Max(0, (int)(baseColor.R - COLOR_LIGHT_CHANGE)),
										Math.Max(0, (int)(baseColor.G - COLOR_LIGHT_CHANGE)),
										Math.Max(0, (int)(baseColor.B - COLOR_LIGHT_CHANGE)));

			// Create light to dark gradient brush
			using(LinearGradientBrush gradient = new LinearGradientBrush(new Rectangle(x, y, width, height), light, dark, 45f))
				g.FillEllipse(gradient, x, y, width, height);

			// Fill the inside of the circle so it looks flat
			using(SolidBrush solid = new SolidBrush(baseColor))
				g.FillEllipse(solid, x+MIDDLE_CIRCLE_OFFSET, y+MIDDLE_CIRCLE_OFFSET,
									 width-MIDDLE_CIRCLE_OFFSET*2,
									 height-MIDDLE_CIRCLE_OFFSET*2);

			// Draw a smooth outline around the circle
			using(Pen outline = new Pen(baseColor))
				g.DrawEllipse(outline, x, y, width, height);

			// Create the hot zone for the middle, if not ready present
			if (_rectMiddle.IsEmpty)
				_rectMiddle = new Rectangle(x-ARROWBOX_WIDTH, y-ARROWBOX_WIDTH, 
											width+ARROWBOX_WIDTH*2, 
											height+ARROWBOX_WIDTH*2);
		}

		private void DrawLeftDiamond(Graphics g, Color baseColor,
									 int x, int y, int width, int height)
		{
			// Create lighter and darker version of base color
			Color light = Color.FromArgb(Math.Min(255, (int)(baseColor.R + COLOR_LIGHT_CHANGE)),
										 Math.Min(255, (int)(baseColor.G + COLOR_LIGHT_CHANGE)),
										 Math.Min(255, (int)(baseColor.B + COLOR_LIGHT_CHANGE)));

			Color dark = Color.FromArgb(Math.Max(0, (int)(baseColor.R - COLOR_LIGHT_CHANGE)),
										Math.Max(0, (int)(baseColor.G - COLOR_LIGHT_CHANGE)),
										Math.Max(0, (int)(baseColor.B - COLOR_LIGHT_CHANGE)));

			// Find halfway down point
			int yHalf = height / 2;
			int xHalf = width / 2;

			// Create path for the unusual shape
			using(GraphicsPath path = new GraphicsPath())
			{
				// Draw the outline into the region
				path.AddLine(x, y+yHalf+1, x, y+yHalf);
				path.AddLine(x, y+yHalf, x+xHalf, y);
				path.AddLine(x+xHalf, y, x+width-DIAMOND_CUTOUT_INSET, y+yHalf-DIAMOND_CUTOUT_INSET);
				path.AddArc(x+width-DIAMOND_CUTOUT_INDENT, y, DIAMOND_SIZE, DIAMOND_SIZE, -155, -52);
				path.AddLine(x+width-DIAMOND_CUTOUT_INSET, y+yHalf+1+DIAMOND_CUTOUT_INSET, x+xHalf, y+height+1);
				path.AddLine(x+xHalf, y+height+1, x, y+yHalf+1);

				// Fill the inside of the circle so it looks flat
				using(SolidBrush solid = new SolidBrush(baseColor))
					g.FillPath(solid, path);

				// Create light and dark gradient brushes
				using(LinearGradientBrush gradientL = new LinearGradientBrush(new Rectangle(x-1, y-1, 4, 4), light, baseColor, 45f),
										  gradientD = new LinearGradientBrush(new Rectangle(x+width+1, y+height+1, 5, 5), baseColor, dark, 45f))
				{
					// Create path for the unusual shape
					using(GraphicsPath lightPatch = new GraphicsPath(),
									   darkPatch = new GraphicsPath())
					{
						// Create a lighter patch at the top left edge
						lightPatch.AddLine(x, y+yHalf+1, x, y+yHalf);
						lightPatch.AddLine(x, y+yHalf, x+xHalf, y);
						lightPatch.AddLine(x+xHalf, y, x+xHalf+3, y+4);
						lightPatch.AddLine(x+xHalf+4, y+4, x+4, y+yHalf+4);

						// Create a darker path at the bottom right edge
						darkPatch.AddLine(x+xHalf, y+height, x+width-DIAMOND_CUTOUT_INSET, y+yHalf+1+DIAMOND_CUTOUT_INSET);
						darkPatch.AddLine(x+width-DIAMOND_CUTOUT_INSET, y+yHalf+1+DIAMOND_CUTOUT_INSET, x+width-DIAMOND_CUTOUT_INSET-2, y+yHalf+1+DIAMOND_CUTOUT_INSET-6);
						darkPatch.AddLine(x+width-DIAMOND_CUTOUT_INSET-2, y+yHalf+1+DIAMOND_CUTOUT_INSET-6, x+xHalf-4, y+height-3);
						darkPatch.AddLine(x+xHalf-4, y+height-3, x+xHalf, y+height);

						g.FillPath(gradientL, lightPatch);
						g.FillPath(gradientD, darkPatch);
					}
				}

				// Finally draw a smooth outline around area
				DrawPath(g, baseColor, path);

				// Create the hot zone for the middle, if not ready present
				if (_rectLeft.IsEmpty)
				{
					RectangleF rawRect = path.GetBounds();
					_rectLeft = new Rectangle((int)rawRect.Left,
											  (int)rawRect.Top,
											  (int)rawRect.Width,
											  (int)rawRect.Height);
				}
			}
		}

		private void DrawRightDiamond(Graphics g, Color baseColor,
									  int x, int y, int width, int height)
		{
			// Create lighter and darker version of base color
			Color light = Color.FromArgb(Math.Min(255, (int)(baseColor.R + COLOR_LIGHT_CHANGE)),
										 Math.Min(255, (int)(baseColor.G + COLOR_LIGHT_CHANGE)),
										 Math.Min(255, (int)(baseColor.B + COLOR_LIGHT_CHANGE)));

			Color dark = Color.FromArgb(Math.Max(0, (int)(baseColor.R - COLOR_LIGHT_CHANGE)),
										Math.Max(0, (int)(baseColor.G - COLOR_LIGHT_CHANGE)),
										Math.Max(0, (int)(baseColor.B - COLOR_LIGHT_CHANGE)));

			// Find halfway down point
			int yHalf = height / 2;
			int xHalf = width / 2;

			// Create path for the unusual shape
			using(GraphicsPath path = new GraphicsPath())
			{
				// Draw the outline into the region
				path.AddLine(x+width, y+yHalf+1, x+width, y+yHalf);
				path.AddLine(x+width, y+yHalf, x+width-xHalf, y);
				path.AddLine(x+width-xHalf, y, x+DIAMOND_CUTOUT_INSET, y+yHalf-DIAMOND_CUTOUT_INSET);
				path.AddArc(x+DIAMOND_CUTOUT_INDENT-DIAMOND_SIZE, y, DIAMOND_SIZE, DIAMOND_SIZE, -27, 52);
				path.AddLine(x+DIAMOND_CUTOUT_INSET, y+yHalf+1+DIAMOND_CUTOUT_INSET, x+width-xHalf, y+height);
				path.AddLine(x+width-xHalf, y+height+1, x+width, y+yHalf+1);

				// Fill the inside of the circle so it looks flat
				using(SolidBrush solid = new SolidBrush(baseColor))
					g.FillPath(solid, path);

				// Create light and dark gradient brushes
				using(LinearGradientBrush gradientL = new LinearGradientBrush(new Rectangle(x-1, y-1, 4, 4), light, baseColor, 45f),
										  gradientD = new LinearGradientBrush(new Rectangle(x+width+1, y+height+1, 5, 5), baseColor, dark, 45f))
				{
					// Create path for the unusual shape
					using(GraphicsPath lightPatch = new GraphicsPath(),
									   darkPatch = new GraphicsPath())
					{
						// Create a lighter patch at the top left edge
						lightPatch.AddLine(x+DIAMOND_CUTOUT_INSET, y+yHalf-DIAMOND_CUTOUT_INSET, x+width-xHalf, y);
						lightPatch.AddLine(x+width-xHalf, y, x+width-xHalf+4, y+4);
						lightPatch.AddLine(x+width-xHalf+4, y+4, x+DIAMOND_CUTOUT_INSET+3, y+yHalf-DIAMOND_CUTOUT_INSET+4);
						lightPatch.AddLine(x+DIAMOND_CUTOUT_INSET+3, y+yHalf-DIAMOND_CUTOUT_INSET+4, x+DIAMOND_CUTOUT_INSET, y+yHalf-DIAMOND_CUTOUT_INSET);

						// Create a darker path at the bottom right edge
						darkPatch.AddLine(x+width-xHalf, y+height, x+width, y+yHalf+1);
						darkPatch.AddLine(x+width, y+yHalf+1, x+width-4, y+yHalf-3);
						darkPatch.AddLine(x+width-4, y+yHalf-3, x+width-xHalf-4, y+height-4);
						darkPatch.AddLine(x+width-xHalf-4, y+height-4, x+width-xHalf, y+height);

						g.FillPath(gradientL, lightPatch);
						g.FillPath(gradientD, darkPatch);
					}
				}

				// Finally draw a smooth outline around area
				DrawPath(g, baseColor, path);

				// Create the hot zone for the right, if not ready present
				if (_rectRight.IsEmpty)
				{
					RectangleF rawRect = path.GetBounds();
					_rectRight = new Rectangle((int)rawRect.Left,
											   (int)rawRect.Top,
											   (int)rawRect.Width,
											   (int)rawRect.Height);
				}
			}
		}

		private void DrawTopDiamond(Graphics g, Color baseColor,
								    int x, int y, int width, int height)
		{
			// Create lighter and darker version of base color
			Color light = Color.FromArgb(Math.Min(255, (int)(baseColor.R + COLOR_LIGHT_CHANGE)),
										 Math.Min(255, (int)(baseColor.G + COLOR_LIGHT_CHANGE)),
										 Math.Min(255, (int)(baseColor.B + COLOR_LIGHT_CHANGE)));

			Color dark = Color.FromArgb(Math.Max(0, (int)(baseColor.R - COLOR_LIGHT_CHANGE)),
										Math.Max(0, (int)(baseColor.G - COLOR_LIGHT_CHANGE)),
										Math.Max(0, (int)(baseColor.B - COLOR_LIGHT_CHANGE)));

			// Find halfway down point
			int yHalf = height / 2;
			int xHalf = width / 2;

			// Create path for the unusual shape
			using(GraphicsPath path = new GraphicsPath())
			{
				// Draw the outline into the region
				path.AddLine(x+xHalf-1, y+1, x+xHalf, y+1);
				path.AddLine(x+xHalf+1, y+1, x+width, y+yHalf);
				path.AddLine(x+width, y+yHalf, x+width-DIAMOND_CUTOUT_INSET, y+height-DIAMOND_CUTOUT_INSET);
				path.AddArc(x, y+height-DIAMOND_CUTOUT_INSET, DIAMOND_SIZE, DIAMOND_SIZE, -65, -52);
				path.AddLine(x+DIAMOND_CUTOUT_INSET, y+height-DIAMOND_CUTOUT_INSET, x, y+yHalf);
				path.AddLine(x, y+yHalf, x+xHalf-1, y+1);

				// Fill the inside of the circle so it looks flat
				using(SolidBrush solid = new SolidBrush(baseColor))
					g.FillPath(solid, path);

				// Create light and dark gradient brushes
				using(LinearGradientBrush gradientL = new LinearGradientBrush(new Rectangle(x-1, y-1, 4, 4), light, baseColor, 45f),
										  gradientD = new LinearGradientBrush(new Rectangle(x+width+1, y+height+1, 5, 5), baseColor, dark, 45f))
				{
					// Create path for the unusual shape
					using(GraphicsPath lightPatch = new GraphicsPath(),
									   darkPatch = new GraphicsPath())
					{
						// Create a lighter patch at the top left edge
						lightPatch.AddLine(x, y+yHalf+1, x, y+yHalf);
						lightPatch.AddLine(x, y+yHalf, x+xHalf, y);
						lightPatch.AddLine(x+xHalf, y, x+xHalf+3, y+4);
						lightPatch.AddLine(x+xHalf+4, y+4, x+4, y+yHalf+4);

						// Create a darker path at the bottom right edge
						darkPatch.AddLine(x+width, y+yHalf, x+width-DIAMOND_CUTOUT_INSET-3, y+height-DIAMOND_CUTOUT_INSET+3);
						darkPatch.AddLine(x+width-DIAMOND_CUTOUT_INSET-3, y+height-DIAMOND_CUTOUT_INSET+3, x+width-DIAMOND_CUTOUT_INSET-7, y+height-DIAMOND_CUTOUT_INSET-1);
						darkPatch.AddLine(x+width-DIAMOND_CUTOUT_INSET-7, y+height-DIAMOND_CUTOUT_INSET-1, x+width-4, y+yHalf-4);
						darkPatch.AddLine(x+width-4, y+yHalf-4, x+width, y+yHalf);

						g.FillPath(gradientL, lightPatch);
						g.FillPath(gradientD, darkPatch);
					}
				}

				// Finally draw a smooth outline around area
				DrawPath(g, baseColor, path);

				// Create the hot zone for the top, if not ready present
				if (_rectTop.IsEmpty)
				{
					RectangleF rawRect = path.GetBounds();
					_rectTop = new Rectangle((int)rawRect.Left,
											 (int)rawRect.Top,
											 (int)rawRect.Width,
											 (int)rawRect.Height);
				}
			}
		}

		private void DrawBottomDiamond(Graphics g, Color baseColor,
								       int x, int y, int width, int height)
		{
			// Create lighter and darker version of base color
			Color light = Color.FromArgb(Math.Min(255, (int)(baseColor.R + COLOR_LIGHT_CHANGE)),
										 Math.Min(255, (int)(baseColor.G + COLOR_LIGHT_CHANGE)),
										 Math.Min(255, (int)(baseColor.B + COLOR_LIGHT_CHANGE)));

			Color dark = Color.FromArgb(Math.Max(0, (int)(baseColor.R - COLOR_LIGHT_CHANGE)),
										Math.Max(0, (int)(baseColor.G - COLOR_LIGHT_CHANGE)),
										Math.Max(0, (int)(baseColor.B - COLOR_LIGHT_CHANGE)));

			// Find halfway down point
			int yHalf = height / 2;
			int xHalf = width / 2;

			// Create path for the unusual shape
			using(GraphicsPath path = new GraphicsPath())
			{
				// Draw the outline into the region
				path.AddLine(x+xHalf-1, y+height-1, x+xHalf, y+height-1);
				path.AddLine(x+xHalf+1, y+height-1, x+width, y+height-yHalf);
				path.AddLine(x+width, y+height-yHalf, x+width-DIAMOND_CUTOUT_INSET, y+DIAMOND_CUTOUT_INSET);
				path.AddArc(x+1, y-DIAMOND_SIZE+DIAMOND_CUTOUT_INSET, DIAMOND_SIZE, DIAMOND_SIZE, 65, 52);
				path.AddLine(x+DIAMOND_CUTOUT_INSET, y+DIAMOND_CUTOUT_INSET, x, y+height-yHalf);
				path.AddLine(x, y+height-yHalf, x+xHalf-1, y+height-1);

				// Fill the inside of the circle so it looks flat
				using(SolidBrush solid = new SolidBrush(baseColor))
					g.FillPath(solid, path);

				// Create light and dark gradient brushes
				using(LinearGradientBrush gradientL = new LinearGradientBrush(new Rectangle(x-1, y-1, 4, 4), light, baseColor, 45f),
										  gradientD = new LinearGradientBrush(new Rectangle(x+width, y+height, 5, 5), baseColor, dark, 45f))
				{
					// Create path for the unusual shape
					using(GraphicsPath lightPatch = new GraphicsPath(),
									   darkPatch = new GraphicsPath())
					{
						// Create a lighter patch at the top left edge
						lightPatch.AddLine(x, y+height-yHalf, x+DIAMOND_CUTOUT_INSET+3, y+DIAMOND_CUTOUT_INSET-1);
						lightPatch.AddLine(x+DIAMOND_CUTOUT_INSET+3, y+DIAMOND_CUTOUT_INSET-1, x+DIAMOND_CUTOUT_INSET+7, y+DIAMOND_CUTOUT_INSET+2);
						lightPatch.AddLine(x+DIAMOND_CUTOUT_INSET+7, y+DIAMOND_CUTOUT_INSET+2, x+4, y+height-yHalf+4);
						lightPatch.AddLine(x+4, y+height-yHalf+4, x, y+height-yHalf);

						// Create a darker path at the bottom right edge
						darkPatch.AddLine(x+xHalf, y+height-1, x+width, y+height-yHalf);
						darkPatch.AddLine(x+width, y+height-yHalf, x+width-4, y+height-yHalf-4);
						darkPatch.AddLine(x+width-4, y+height-yHalf-4, x+xHalf-4, y+height-4);
						darkPatch.AddLine(x+xHalf-4, y+height-4, x+xHalf, y+height-1);
						
						g.FillPath(gradientL, lightPatch);
						g.FillPath(gradientD, darkPatch);
					}
				}

				// Finally draw a smooth outline around area
				DrawPath(g, baseColor, path);

				// Create the hot zone for the bottom, if not ready present
				if (_rectBottom.IsEmpty)
				{
					RectangleF rawRect = path.GetBounds();
					_rectBottom = new Rectangle((int)rawRect.Left,
												(int)rawRect.Top,
												(int)rawRect.Width,
												(int)rawRect.Height);
				}
			}
		}

		private void DrawLeftArrow(Graphics g, Pen borderPen, 
								   Brush fillBrush, int x, int y)
		{
			// Draw rectangle around whole arrow box
			g.DrawRectangle(borderPen, x, y, ARROWBOX_WIDTH, ARROWBOX_HEIGHT);
			
			// Draw and fill the header section of the box
			g.DrawRectangle(borderPen, x, y, ARROWBOX_WIDTH_HEADER, ARROWBOX_HEIGHT);
			g.FillRectangle(fillBrush, x+1, y+1, ARROWBOX_WIDTH_HEADER-1, ARROWBOX_HEIGHT-1);
			
			// Draw the arrow itself using a series of lines
			g.DrawLine(borderPen, x+ARROWBOX_POINT_XOFFSET, y+ARROWBOX_POINT_YOFFSET+2, x+ARROWBOX_POINT_XOFFSET+4, y+ARROWBOX_POINT_YOFFSET+2);
			g.DrawLine(borderPen, x+ARROWBOX_POINT_XOFFSET+1, y+ARROWBOX_POINT_YOFFSET+1, x+ARROWBOX_POINT_XOFFSET+2, y+ARROWBOX_POINT_YOFFSET+1);
			g.DrawLine(borderPen, x+ARROWBOX_POINT_XOFFSET+1, y+ARROWBOX_POINT_YOFFSET+3, x+ARROWBOX_POINT_XOFFSET+2, y+ARROWBOX_POINT_YOFFSET+3);
			g.DrawLine(borderPen, x+ARROWBOX_POINT_XOFFSET+2, y+ARROWBOX_POINT_YOFFSET, x+ARROWBOX_POINT_XOFFSET+2, y+ARROWBOX_POINT_YOFFSET+4);
		}

		private void DrawRightArrow(Graphics g, Pen borderPen, 
								    Brush fillBrush, int x, int y)
		{
			// Draw rectangle around whole arrow box
			g.DrawRectangle(borderPen, x, y, ARROWBOX_WIDTH, ARROWBOX_HEIGHT);
			
			// Draw and fill the header section of the box
			g.DrawRectangle(borderPen, x+ARROWBOX_WIDTH-ARROWBOX_WIDTH_HEADER, y, ARROWBOX_WIDTH_HEADER, ARROWBOX_HEIGHT);
			g.FillRectangle(fillBrush, x+ARROWBOX_WIDTH-ARROWBOX_WIDTH_HEADER+1, y+1, ARROWBOX_WIDTH_HEADER-1, ARROWBOX_HEIGHT-1);
			
			// Draw the arrow itself using a series of lines
			g.DrawLine(borderPen, x+ARROWBOX_WIDTH-ARROWBOX_POINT_XOFFSET, y+ARROWBOX_POINT_YOFFSET+2, x+ARROWBOX_WIDTH-ARROWBOX_POINT_XOFFSET-4, y+ARROWBOX_POINT_YOFFSET+2);
			g.DrawLine(borderPen, x+ARROWBOX_WIDTH-ARROWBOX_POINT_XOFFSET-1, y+ARROWBOX_POINT_YOFFSET+1, x+ARROWBOX_WIDTH-ARROWBOX_POINT_XOFFSET-2, y+ARROWBOX_POINT_YOFFSET+1);
			g.DrawLine(borderPen, x+ARROWBOX_WIDTH-ARROWBOX_POINT_XOFFSET-1, y+ARROWBOX_POINT_YOFFSET+3, x+ARROWBOX_WIDTH-ARROWBOX_POINT_XOFFSET-2, y+ARROWBOX_POINT_YOFFSET+3);
			g.DrawLine(borderPen, x+ARROWBOX_WIDTH-ARROWBOX_POINT_XOFFSET-2, y+ARROWBOX_POINT_YOFFSET, x+ARROWBOX_WIDTH-ARROWBOX_POINT_XOFFSET-2, y+ARROWBOX_POINT_YOFFSET+4);
		}

		private void DrawTopArrow(Graphics g, Pen borderPen, 
								  Brush fillBrush, int x, int y)
		{
			// Draw rectangle around whole arrow box
			g.DrawRectangle(borderPen, x, y, ARROWBOX_HEIGHT, ARROWBOX_WIDTH);
			
			// Draw and fill the header section of the box
			g.DrawRectangle(borderPen, x, y, ARROWBOX_HEIGHT, ARROWBOX_WIDTH_HEADER);
			g.FillRectangle(fillBrush, x+1, y+1, ARROWBOX_HEIGHT-1, ARROWBOX_WIDTH_HEADER-1);
			
			// Draw the arrow itself using a series of lines
			g.DrawLine(borderPen, x+ARROWBOX_POINT_YOFFSET+2, y+ARROWBOX_POINT_XOFFSET, x+ARROWBOX_POINT_YOFFSET+2, y+ARROWBOX_POINT_XOFFSET+4);
			g.DrawLine(borderPen, x+ARROWBOX_POINT_YOFFSET+1, y+ARROWBOX_POINT_XOFFSET+1, x+ARROWBOX_POINT_YOFFSET+1, y+ARROWBOX_POINT_XOFFSET+2);
			g.DrawLine(borderPen, x+ARROWBOX_POINT_YOFFSET+3, y+ARROWBOX_POINT_XOFFSET+1, x+ARROWBOX_POINT_YOFFSET+3, y+ARROWBOX_POINT_XOFFSET+2);
			g.DrawLine(borderPen, x+ARROWBOX_POINT_YOFFSET, y+ARROWBOX_POINT_XOFFSET+2, x+ARROWBOX_POINT_YOFFSET+4, y+ARROWBOX_POINT_XOFFSET+2);
		}

		private void DrawBottomArrow(Graphics g, Pen borderPen, 
									 Brush fillBrush, int x, int y)
		{
			// Draw rectangle around whole arrow box
			g.DrawRectangle(borderPen, x, y, ARROWBOX_HEIGHT, ARROWBOX_WIDTH);
			
			// Draw and fill the header section of the box
			g.DrawRectangle(borderPen, x, y+ARROWBOX_WIDTH-ARROWBOX_WIDTH_HEADER, ARROWBOX_HEIGHT, ARROWBOX_WIDTH_HEADER);
			g.FillRectangle(fillBrush, x+1, y+ARROWBOX_WIDTH-ARROWBOX_WIDTH_HEADER+1, ARROWBOX_HEIGHT-1, ARROWBOX_WIDTH_HEADER-1);
			
			// Draw the arrow itself using a series of lines
			g.DrawLine(borderPen, x+ARROWBOX_POINT_YOFFSET+2, y+ARROWBOX_WIDTH-ARROWBOX_POINT_XOFFSET, x+ARROWBOX_POINT_YOFFSET+2, y+ARROWBOX_WIDTH-ARROWBOX_POINT_XOFFSET-4);
			g.DrawLine(borderPen, x+ARROWBOX_POINT_YOFFSET+1, y+ARROWBOX_WIDTH-ARROWBOX_POINT_XOFFSET-1, x+ARROWBOX_POINT_YOFFSET+1, y+ARROWBOX_WIDTH-ARROWBOX_POINT_XOFFSET-2);
			g.DrawLine(borderPen, x+ARROWBOX_POINT_YOFFSET+3, y+ARROWBOX_WIDTH-ARROWBOX_POINT_XOFFSET-1, x+ARROWBOX_POINT_YOFFSET+3, y+ARROWBOX_WIDTH-ARROWBOX_POINT_XOFFSET-2);
			g.DrawLine(borderPen, x+ARROWBOX_POINT_YOFFSET, y+ARROWBOX_WIDTH-ARROWBOX_POINT_XOFFSET-2, x+ARROWBOX_POINT_YOFFSET+4, y+ARROWBOX_WIDTH-ARROWBOX_POINT_XOFFSET-2);
		}

		private void DrawMiddleArrow(Graphics g, Pen borderPen, 
									 Brush fillBrush, int x, int y)
		{
			// Draw lines to create the tabs outline
			g.DrawLine(borderPen, x, y, x, y+ARROWBOX_HEIGHT-1);
			g.DrawLine(borderPen, x, y+ARROWBOX_HEIGHT-1, x+1, y+ARROWBOX_HEIGHT);
			g.DrawLine(borderPen, x+1, y+ARROWBOX_HEIGHT, x+ARROWBOX_HEIGHT_MID-1, y+ARROWBOX_HEIGHT);
			g.DrawLine(borderPen, x+ARROWBOX_HEIGHT_MID-1, y+ARROWBOX_HEIGHT, x+ARROWBOX_HEIGHT_MID, y+ARROWBOX_HEIGHT-1);
			g.DrawLine(borderPen, x+ARROWBOX_HEIGHT_MID, y+ARROWBOX_HEIGHT-1, x+ARROWBOX_HEIGHT_MID+1, y+ARROWBOX_HEIGHT);
			g.DrawLine(borderPen, x+ARROWBOX_HEIGHT_MID+1, y+ARROWBOX_HEIGHT, x+ARROWBOX_HEIGHT-1, y+ARROWBOX_HEIGHT);
			g.DrawLine(borderPen, x+ARROWBOX_HEIGHT-1, y+ARROWBOX_HEIGHT, x+ARROWBOX_HEIGHT, y+ARROWBOX_HEIGHT-1);
			g.DrawLine(borderPen, x+ARROWBOX_HEIGHT, y+ARROWBOX_HEIGHT-1, x+ARROWBOX_HEIGHT, y);
			g.DrawLine(borderPen, x+ARROWBOX_HEIGHT, y, x, y);

			// Create empty tab cut off
			g.DrawLine(borderPen, x+ARROWBOX_HEIGHT_MID, y+ARROWBOX_HEIGHT-1, x+ARROWBOX_HEIGHT_MID, y+ARROWBOX_HEIGHT-3);
			g.DrawLine(borderPen, x+ARROWBOX_HEIGHT_MID, y+ARROWBOX_HEIGHT-3, x+ARROWBOX_HEIGHT, y+ARROWBOX_HEIGHT-3);

			// Fill in the solid area
			g.FillRectangle(fillBrush, x+1, y+1, ARROWBOX_HEIGHT-1, ARROWBOX_HEIGHT-4);
			g.FillRectangle(fillBrush, x+1, y+ARROWBOX_HEIGHT-3, ARROWBOX_HEIGHT_MID-1, 3);
		}

		private void DrawPath(Graphics g, Color baseColor, GraphicsPath path)
		{
			// Draw a smooth outline around the circle
			using(Pen outline = new Pen(baseColor))
				g.DrawPath(outline, path);
		}
	}
}
