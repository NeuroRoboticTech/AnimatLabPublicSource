/*=======================================================================
  Copyright (C) Lyquidity Solutions Limited.  All rights reserved.
 
  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
  PARTICULAR PURPOSE.

  LYQUIDITY SOLUTIONS LIMITED DOES NOT IMPOSE ANY LIMITATION ON THE
  USE OF THIS CODE AND IT AN BE USED IN COMMERCIAL APPLICATIONS.  LYQUIDIY
  ACCEPTS NO RESPONSIBLY FOR ANY LIABILTY WHATEVER AND WILL NOT PROVIDE
  ANY SUPPORT TO USER OR THEIR CLIENTS.
=======================================================================*/

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;
using AnimatGuiCtrls.Collections;

namespace AnimatGuiCtrls.Controls
{
	#region Enumerations

	public enum enumOrientation
	{
		orHorizontal = 0,
		orVertical = 1
	}

	public enum enumScaleMode
	{
		smPoints = 0,
		smPixels = 1,
		smCentimetres = 2,
		smInches = 3
	}

	public enum enumRulerAlignment
	{
		raTopOrLeft,
		raMiddle,
		raBottomOrRight
	}

	public enum enumTimeScale
	{
		MilliSeconds = 0,
		Seconds = 1,
		Minutes = 2,
		Hours = 3
	}

	internal enum Msg
	{
		WM_MOUSEMOVE              = 0x0200,
		WM_MOUSELEAVE             = 0x02A3,
		WM_NCMOUSELEAVE           = 0x02A2,
	}

	#endregion

	/// <summary>
	/// Summary description for TimeRuler.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(TimeRuler), "Ruler.bmp")]
	public class TimeRuler : System.Windows.Forms.Control, IMessageFilter
	{

		#region Internal Variables

		private float					_Scale;
		private int					_ScaleStartValue;
		private bool				_bDrawLine = false;
		//private bool				_bInControl = false;
		private int					_iMousePosition	= 1;
		private int					_iOldMousePosition = -1;
		private Bitmap				_Bitmap = null;
		private long				_lStartMillisecond = 0;
		private long				_lEndMillisecond = 10000;
		private long				_lCurrentMillisecond = 0;
		private long				_lActualMillisecond = 0;
		private int					_iProgressBarScale = 40;
		private int					_iHeaderOffset = 15;
		private int					_iSideOffset = 5;
		private enumTimeScale		_TimeScale = enumTimeScale.MilliSeconds;
		private bool				_bAutomaticTimeScale = true;
		private bool				_bAutomaticOrientation = true;
		private int[]				_aryIntervals = new int[16] { 1, 2, 5, 10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000, 20000, 50000, 100000 };

		protected KeyFrameCollection _aryKeyFrames;

		private bool				_bInsideDraw						= false;
		private KeyFrame		_MovingFrame						= null;
		private KeyFrame		_CurrentFrame						= null;
		private KeyFrame		_SelectedFrame						= null;

		private int					_iDisplaySize						= 0;

		private bool				_bAllowKeyFrameSelection				= true;

		private System.Drawing.Color _CurrentTimeColor = Color.Blue;
		private System.Drawing.Color _ActualTimeColor = Color.LightBlue;

		#endregion

		#region Property variable

		private enumOrientation		_Orientation;
		private enumScaleMode		_ScaleMode;
		private enumRulerAlignment	_RulerAlignment     = enumRulerAlignment.raBottomOrRight;
		private Border3DStyle		_i3DBorderStyle     = Border3DStyle.Etched;
		private int					_iMajorInterval     = 100;
		private int					_iNumberOfDivisions = 10;
		private int					_DivisionMarkFactor = 4;
		private int					_MiddleMarkFactor	= 3;
		private double				_ZoomFactor         = 1;
		private double				_StartValue			= 0;
		private bool				_bMouseTrackingOn   = false;
		private bool				_VerticalNumbers	= true;

		#endregion

		#region Event Arguments

		public class ScaleModeChangedEventArgs : EventArgs
		{
			public enumScaleMode Mode;

			public ScaleModeChangedEventArgs(enumScaleMode Mode) : base()
			{
				this.Mode = Mode;
			}
		}

		public class HooverValueEventArgs : EventArgs
		{
			public double Value;

			public HooverValueEventArgs(double Value) : base()
			{
				this.Value = Value;
			}
		}

		public class KeyFrameEventArgs : EventArgs
		{
			public KeyFrame SelectedKeyFrame;

			public KeyFrameEventArgs(KeyFrame Frame) : base()
			{
				this.SelectedKeyFrame = Frame;
			}
		}

		#endregion

		#region Delegates

		public delegate void ScaleModeChangedEvent(object sender, ScaleModeChangedEventArgs e);
		public delegate void HooverValueEvent(object sender, HooverValueEventArgs e);
		public delegate void KeyFrameSelectedEvent(object sender, KeyFrameEventArgs e);
		public delegate void KeyFrameAddedEvent(object sender, KeyFrameEventArgs e);
		public delegate void KeyFrameRemovedEvent(object sender, KeyFrameEventArgs e);
		public delegate void KeyFrameMovedEvent(object sender, KeyFrameEventArgs e);
		public delegate void KeyFrameMovingEvent(object sender, KeyFrameEventArgs e);
		public delegate void CurrentFrameMovedEvent(object sender, KeyFrameEventArgs e);

		#endregion

		#region Events

		public event ScaleModeChangedEvent ScaleModeChanged;
		public event HooverValueEvent HooverValue;
		public event KeyFrameSelectedEvent KeyFrameSelected;
		public event KeyFrameAddedEvent KeyFrameAdded;
		public event KeyFrameRemovedEvent KeyFrameRemoved;
		public event KeyFrameMovedEvent KeyFrameMoved;
		public event KeyFrameMovingEvent KeyFrameMoving;
		public event CurrentFrameMovedEvent CurrentFrameMoved;

		#endregion

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#region Constrcutors/Destructors

		public TimeRuler()
		{
			base.BackColor = System.Drawing.Color.White;
			base.ForeColor = System.Drawing.Color.Black;


			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			_aryKeyFrames = new KeyFrameCollection(this);
			ScaleMode = enumScaleMode.smPixels;

		}

		#endregion

		#region Methods

		private Point GetMousePosition()
		{
			int X = 0;
			int Y = 0;

			Point pointScreen = Control.MousePosition;
			Point pointClientOrigin = new Point(X, Y);
			pointClientOrigin = PointToScreen(pointClientOrigin);

			Point p = new Point( (pointScreen.X-pointClientOrigin.X), (pointScreen.Y-pointClientOrigin.Y) );
			return p;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool PreFilterMessage(ref Message m)
		{
			return false;
		}

//		[EditorBrowsable(EditorBrowsableState.Never)]
//		public bool PreFilterMessage(ref Message m)
//		{
//			if (!this._bMouseTrackingOn) return false;
//
//			if (m.Msg == (int)Msg.WM_MOUSEMOVE)
//			{
//				int X = 0;
//				int Y = 0;
//
//				// The mouse coordinate are measured in screen coordinates because thats what 
//				// Control.MousePosition returns.  The Message,LParam value is not used because
//				// it returns the mouse position relative to the control the mouse is over. 
//				// Chalk and cheese.
//
//				Point pointScreen = Control.MousePosition;
//
//				// Get the origin of this control in screen coordinates so that later we can 
//				// compare it against the mouse point to determine it we've hit this control.
//
//				Point pointClientOrigin = new Point(X, Y);
//				pointClientOrigin = PointToScreen(pointClientOrigin);
//
//				// ...workout the position of the mouse relative to the 
//				X = pointScreen.X-pointClientOrigin.X;
//				Y = pointScreen.Y-pointClientOrigin.Y;
//				//Debug.WriteLine("MousePos: (" + X.ToString() + ", " + Y.ToString() + ") Position: " + CalculateMillisecondValue(_iMousePosition).ToString());
//
//				// Determine whether the mouse is within the bounds of the control itself
//				_bInControl = (this.ClientRectangle.Contains(new Point(X, Y)));
//
//				//Only do this stuff if the mouse is inside this control.
//				if(_bInControl)
//				{
//					_bDrawLine = false;
//					_bInControl = false;
//
//					HooverValueEventArgs eHoover = null;
//
//					// Work out whether the mouse is within the Y-axis bounds of a vertital ruler or 
//					// within the X-axis bounds of a horizontal ruler
//
//					_bDrawLine = (pointScreen.X >= (pointClientOrigin.X + _iSideOffset)) && 
//						(pointScreen.X <= (pointClientOrigin.X + _iSideOffset + _iDisplaySize)) && 
//						(pointScreen.Y >= (pointClientOrigin.Y + _iSideOffset)) && 
//						(pointScreen.Y <= (pointClientOrigin.Y + _iSideOffset + _iDisplaySize));
//
//					// If the mouse is in valid position...
//					if (_bDrawLine)
//					{
//						// Make the relative mouse position available in pixel relative to this control's origin
//						ChangeMousePosition((this.Orientation == enumOrientation.orHorizontal) ? X : Y);
//						eHoover = new HooverValueEventArgs(CalculateMillisecondValue(_iMousePosition));
//
//					} 
//					else
//					{
//						//Debug.WriteLine("MousePos: -1");
//						ChangeMousePosition(-1);
//						eHoover = new HooverValueEventArgs(_iMousePosition);
//					}
//
//					// Paint directly by calling the OnPaint() method.  This way the background is not 
//					// hosed by the call to Invalidate() so paining occurs without the hint of a flicker
//					PaintEventArgs e = null;
//					try
//					{
//						e = new PaintEventArgs(this.CreateGraphics(), this.ClientRectangle);
//						OnPaint(e);
//					}
//					finally
//					{
//						e.Graphics.Dispose();
//					}
//
//					OnHooverValue(eHoover);
//				}
//			}
//
//			if ((m.Msg == (int)Msg.WM_MOUSELEAVE) || 
//				(m.Msg == (int)Msg.WM_NCMOUSELEAVE))
//			{
//				_bDrawLine = false;
//				PaintEventArgs paintArgs = null;
//				try
//				{
//					paintArgs = new PaintEventArgs(this.CreateGraphics(), this.ClientRectangle);
//					this.OnPaint(paintArgs);
//				}
//				finally
//				{
//					paintArgs.Graphics.Dispose();
//				}
//			}
//
//			return false;  // Whether or not the message is filtered
//		}


		public double PixelToScaleValue(int iOffset)
		{
			return this.CalculateMillisecondValue(iOffset);
		}

		public int ScaleValueToPixel(double nScaleValue)
		{
			return CalculateMillisecondPixel(nScaleValue);
		}

		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// RulerControl
			// 
			this.Name = "TimeRuler";
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RulerControl_MouseDown);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RulerControl_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.RulerControl_MouseUp);
			this.DoubleClick += new System.EventHandler(this.RulerControl_DoubleClick);
		}
		#endregion

		#region Overrides

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize (e);

			// Take private resize actions here
			_Bitmap = null;
			this.Invalidate();
		}

		public override void Refresh()
		{
			base.Refresh ();
			this.Invalidate();
		}


		[Description("Draws the ruler marks in the scale requested.")]
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);
			DrawControl(e.Graphics);
		}


		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed (e);
			try
			{
				if (_bMouseTrackingOn) Application.RemoveMessageFilter(this);
			} 
			catch {}
		}

		#endregion

		#region Event Handlers

		private void RulerControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
				//Debug.WriteLine("MouseDown: (" + e.X.ToString() + ", " + e.Y.ToString() + ")");

				if (e.Button.Equals(MouseButtons.Left) && _bAllowKeyFrameSelection) 
				{
					SelectedKeyFrame = _aryKeyFrames.IsHandleClick(e);

					if(_SelectedFrame != null && _SelectedFrame.CanBeMoved(this))
					{
						_MovingFrame = _SelectedFrame;
						_MovingFrame.StartMove(e, this);
					}
				}
			}
			catch {}

		}

		private void RulerControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
				if(_MovingFrame != null) 
				{
					_MovingFrame.MoveFrame(e, this);
					OnKeyFrameMoving(_MovingFrame);
				}
			}
			catch {}
		}

		private void RulerControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
				//Debug.WriteLine("MouseUp: (" + e.X.ToString() + ", " + e.Y.ToString() + ")");
				if(_MovingFrame != null) 
				{
					_MovingFrame.EndMove(e, this);
					OnKeyFrameMoved(_MovingFrame);
					_MovingFrame = null;
				}
			}
			catch {}

			EventArgs eClick = new EventArgs();
			this.OnClick(eClick);
		}

		private void RulerControl_DoubleClick(object sender, System.EventArgs e)
		{

			Point pMousePos = GetMousePosition();
			if( (Control.ModifierKeys & Keys.Control) == Keys.Control )
			{
				if( (Control.ModifierKeys & Keys.Shift) == Keys.Shift )
					RemoveVideoFrame(pMousePos);
				else
					AddVideoFrame(pMousePos);
			}
			else
			{
				if( (Control.ModifierKeys & Keys.Shift) == Keys.Shift )
					RemoveKeyFrame(pMousePos);
				else
					AddKeyFrame(pMousePos);
			}

		}

		internal void OnHooverValue(HooverValueEventArgs e)
		{
			if (HooverValue != null) HooverValue(this, e);
		}

		internal void OnScaleModeChanged(ScaleModeChangedEventArgs e)
		{
			if (ScaleModeChanged != null) ScaleModeChanged(this, e);
		}
		
		internal void OnKeyFrameSelected(KeyFrame frameSelected)
		{
			KeyFrameEventArgs e = new KeyFrameEventArgs(frameSelected);
			if (KeyFrameSelected != null) KeyFrameSelected(this, e);
		}
		
		internal void OnKeyFrameAdded(KeyFrame frameSelected)
		{
			KeyFrameEventArgs e = new KeyFrameEventArgs(frameSelected);
			if (KeyFrameAdded != null) KeyFrameAdded(this, e);
		}
		
		internal void OnKeyFrameRemoved(KeyFrame frameSelected)
		{
			KeyFrameEventArgs e = new KeyFrameEventArgs(frameSelected);
			if (KeyFrameRemoved != null) KeyFrameRemoved(this, e);
		}
		
		internal void OnKeyFrameMoved(KeyFrame frameSelected)
		{
			KeyFrameEventArgs e = new KeyFrameEventArgs(frameSelected);
			if (KeyFrameMoved != null) KeyFrameMoved(this, e);
		}
		
		internal void OnKeyFrameMoving(KeyFrame frameSelected)
		{
			KeyFrameEventArgs e = new KeyFrameEventArgs(frameSelected);
			if (KeyFrameMoving != null) KeyFrameMoving(this, e);
		}
		
		internal void OnCurrentFrameMoved(KeyFrame frameSelected)
		{
			KeyFrameEventArgs e = new KeyFrameEventArgs(frameSelected);
			if (CurrentFrameMoved != null) CurrentFrameMoved(this, e);
		}

		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter (e);
			_bDrawLine = false;
			Invalidate();
		}

		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave (e);
			Invalidate();
		}

		private void ContextMenu_Popup(object sender, EventArgs e)
		{
			//System.Diagnostics.Debug.WriteLine("Popup");
		}

		#endregion

		#region Properties

		internal int HeaderOffset
		{
			get
			{
				return _iHeaderOffset;
			}
		}

		internal int SideOffset
		{
			get
			{
				return _iSideOffset;
			}
		}

		internal int DisplaySize
		{
			get
			{
				return _iDisplaySize;
			}
		}

		public float ScaleOut
		{
			get
			{
				return _Scale;
			}
		}

		[
		DefaultValue(typeof(Border3DStyle),"Etched"),
		Description("The border style use the Windows.Forms.Border3DStyle type"),
		Category("Ruler"),
		]
		public Border3DStyle BorderStyle
		{
			get
			{
				return _i3DBorderStyle;
			}
			set
			{
				_i3DBorderStyle = value;
				RedrawBitmap();
			}
		}

		[Description("Horizontal or vertical layout")]
		[Category("Ruler")]
		public enumOrientation Orientation
		{ 
			get { return _Orientation; }
			set 
			{
				_Orientation = value;
				RedrawBitmap();
			}
		}

		[Description("Determines whether or not the user can select keyframes")]
		[Category("Ruler")]
		public bool AllowKeyFrameSelection
		{ 
			get { return _bAllowKeyFrameSelection; }
			set 
			{
				_bAllowKeyFrameSelection = value;
				
				if(!_bAllowKeyFrameSelection)
					SelectedKeyFrame = null;
			}
		}

		[Description("The second value where the ruler should start.  Default is zero.")]
		[Category("Ruler")]
		public long StartMillisecond
		{
			get { return _lStartMillisecond; }
			set 
			{
				if(value<0)
					throw new System.Exception("The starting time can not be less than zero.");

				_StartValue = (double) value;
				_lStartMillisecond = value;
				_ScaleStartValue = Convert.ToInt32(value * _Scale / _iMajorInterval);  // Convert value to pixels
				RedrawBitmap();
			}
		}

		[Description("The second value where the ruler should end.  Default is 500.")]
		[Category("Ruler")]
		public long EndMillisecond
		{
			get { return _lEndMillisecond; }
			set 
			{
				if(value < 0)
					throw new System.Exception("The ending time can not be less than zero.");

				if(value < _lStartMillisecond)
					throw new System.Exception("The ending time can not be less than the starting time.");

				_lEndMillisecond = value;
				RedrawBitmap();
			}
		}

		[Description("This displays the progress bar up to this value of the current second.")]
		[Category("Ruler")]
		public long CurrentMillisecond
		{
			get { return _lCurrentMillisecond; }
			set 
			{
				if(value < 0)
					throw new System.Exception("The current second can not be less than zero.");

				_lCurrentMillisecond = value;

				if(_lCurrentMillisecond > _lActualMillisecond)
					_lActualMillisecond = _lCurrentMillisecond;

				if(_CurrentFrame != null)
					_CurrentFrame.SetTimes(value, value);

				if(_lCurrentMillisecond > (_lEndMillisecond*0.7) )
					_lEndMillisecond = (long) (_lCurrentMillisecond * 1.4);

				RedrawBitmap();
			}
		}

		[Description("This gets a string display of the current time.")]
		[Category("Ruler")]
		public String CurrentTime
		{
			get
			{
				String strVal = string.Format("%4.4f",  _lCurrentMillisecond/ (float) GetMillisecondScale(_TimeScale));
				strVal+= GetTimeScaleAbbreviation(_TimeScale);
				return strVal;
			}
		}

		[Description("This is the actual value of how much time has elapsed. The current time property can be moved back into the past.")]
		[Category("Ruler")]
		public long ActualMillisecond
		{
			get { return _lActualMillisecond; }
			set 
			{
				if(value < 0)
					throw new System.Exception("The current second can not be less than zero.");

				if(value < _lCurrentMillisecond)
					throw new System.Exception("The actual second can not be less than current second.");

				_lActualMillisecond = value;
				RedrawBitmap();
			}
		}

		[Description("This is the time scale that is to used to draw the ruler. It is defaulted to seconds." + 
			 "If AutomaticTimeScale is true then this is the scale that is currently being used.")]
		[Category("Ruler")]
		public enumTimeScale TimeScale
		{
			get { return _TimeScale; }
			set 
			{
				if(!_bAutomaticTimeScale)
				{
					_TimeScale = value;
					RedrawBitmap();
				}
			}
		}

		[Description("If this is true then the scale for the time will be automatically calculated based on the number of milliseconds.")]
		[Category("Ruler")]
		public bool AutomaticTimeScale
		{
			get { return _bAutomaticTimeScale; }
			set 
			{
				_bAutomaticTimeScale = value;
			}
		}

		[Description("If this is true then the orientation of the ruler will change based on the relative ratio's of the size of the control.")]
		[Category("Ruler")]
		public bool AutomaticOrientation
		{
			get { return _bAutomaticOrientation; }
			set 
			{
				_bAutomaticOrientation = value;
			}
		}

		public int MillisecondScale
		{
			get 
			{return GetMillisecondScale(_TimeScale);}
		}

		[Description("This sets the color of the progress bar for the current time.")]
		[Category("Ruler")]
		public System.Drawing.Color CurrentTimeColor
		{
			get { return _CurrentTimeColor; }
			set 
			{
				_CurrentTimeColor = value;
				RedrawBitmap();
			}
		}


		[Description("This sets the color of the progress bar for the actual time.")]
		[Category("Ruler")]
		public System.Drawing.Color ActualTimeColor
		{
			get { return _ActualTimeColor; }
			set 
			{
				_ActualTimeColor = value;
				RedrawBitmap();
			}
		}

		[Description("This determines the percentage width of the progress bar. The default is 40%")]
		[Category("Ruler")]
		public int ProgressBarScale
		{
			get { return _iProgressBarScale; }
			set 
			{
				if(value < 1)
					throw new System.Exception("The progress bar scale must be at least 1%.");
				if(value > 100)
					throw new System.Exception("The progress bar scale must be less than or equal to 100%.");

				_iProgressBarScale = value;
				RedrawBitmap();
			}
		}

		public KeyFrameCollection KeyFrames
		{
			get { return _aryKeyFrames; }
			
			set 
			{
				_aryKeyFrames.Clear();
				_aryKeyFrames = value;	
				RedrawBitmap();
			}
		}

		[Description("This is the currently selected keyframe.")]
		[Category("Ruler")]
		public KeyFrame SelectedKeyFrame
		{
			get { return _SelectedFrame; }
			set 
			{
				if(_SelectedFrame != null)
				{
					_SelectedFrame.Selected = false;
					_SelectedFrame = null;
				}

				_SelectedFrame = value;

				if(_SelectedFrame != null)
					_SelectedFrame.Selected = true;

				OnKeyFrameSelected(_SelectedFrame);

				RedrawBitmap();
			}
		}

		[Description("The ruler scale to use")]
		[Category("Ruler")]
		private enumScaleMode ScaleMode
		{
			get { return _ScaleMode; }
			set 
			{
				enumScaleMode iOldScaleMode = _ScaleMode;
				_ScaleMode = value;    

				if (_iMajorInterval == DefaultMajorInterval(iOldScaleMode))
				{
					// Set the default Scale and MajorInterval value
					_Scale = DefaultScale(_ScaleMode);
					_iMajorInterval = DefaultMajorInterval(_ScaleMode);

				} 
				else
				{
					MajorInterval = _iMajorInterval;
				}

				// Use the current start value (if there is one)
				this.StartMillisecond = (int) this._StartValue;

				ScaleModeChangedEventArgs e = new ScaleModeChangedEventArgs(value);
				this.OnScaleModeChanged(e);
			}
		}

		[Description("The value of the major interval.  When displaying inches, 1 is a typical value.  When displaying Points, 36 or 72 might good values.")]
		[Category("Ruler")]
		public int MajorInterval
		{
			get { return _iMajorInterval; }
			set 
			{ 
				if (value <=0) throw new Exception("The major interval value cannot be less than one");
				_iMajorInterval = value;
				_Scale = ((float) DefaultScale1(_ScaleMode, _ZoomFactor) * _iMajorInterval) / DefaultMajorInterval(_ScaleMode);
				//Debug.WriteLine("DefaultScale: " + DefaultScale(_ScaleMode).ToString() + "  _iMajorInterval: " +
				//								_iMajorInterval.ToString() + "  DefaultMajorInterval: " +  DefaultMajorInterval(_ScaleMode).ToString() + 
				//								"  Scale: " + _Scale.ToString());

				RedrawBitmap();
			}
		}

		[Description("How many divisions should be shown between each major interval")]
		[Category("Ruler")]
		public int Divisions
		{
			get { return _iNumberOfDivisions; }
			set 
			{
				if (value <=0) throw new Exception("The number of divisions cannot be less than one");
				_iNumberOfDivisions = value;
				RedrawBitmap();
			}
		}

		[Description("The height or width of this control multiplied by the reciprocal of this number will be used to compute the height of the non-middle division marks.")]
		[Category("Ruler")]
		public int DivisionMarkFactor
		{
			get { return _DivisionMarkFactor; }
			set 
			{ 
				if (value <=0) throw new Exception("The Division Mark Factor cannot be less than one");
				_DivisionMarkFactor = value;
				RedrawBitmap();
			}
		}

		[Description("The height or width of this control multiplied by the reciprocal of this number will be used to compute the height of the middle division mark.")]
		[Category("Ruler")]
		public int MiddleMarkFactor
		{
			get { return _MiddleMarkFactor; }
			set
			{
				if (value <=0) throw new Exception("The Middle Mark Factor cannot be less than one");
				_MiddleMarkFactor = value;
				RedrawBitmap();
			}
		}

		[Description("The value of the current mouse position expressed in unit of the scale set (centimetres, inches, etc.")]
		[Category("Ruler")]
		public double ScaleValue
		{
			get {return CalculateValue(_iMousePosition); }
		}

		[Description("TRUE if a line is displayed to track the current position of the mouse and events are generated as the mouse moves.")]
		[Category("Ruler")]
		public bool MouseTrackingOn
		{
			get { return _bMouseTrackingOn; }
			set 
			{ 
				if (value == _bMouseTrackingOn) return;
				
				if (value)
				{
					// Tracking is being enabled so add the message filter hook
					Application.AddMessageFilter(this);
				}
				else
				{
					// Tracking is being disabled so remove the message filter hook
					Application.RemoveMessageFilter(this);
					ChangeMousePosition(-1);
				}

				_bMouseTrackingOn = value;

				RedrawBitmap();
			}
		}

		[Description("The font used to display the division number")]
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
				RedrawBitmap();
			}
		}

		[Description("Return the mouse position as number of pixels from the top or left of the control.  -1 means that the mouse is positioned before or after the control.")]
		[Category("Ruler")]
		public int MouseLocation
		{
			get { return _iMousePosition; }
		}

		[DefaultValue(typeof(Color), "ControlDarkDark")]
		[Description("The color used to lines and numbers on the ruler")]
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
				RedrawBitmap();
			}
		}

		[DefaultValue(typeof(Color), "White")]
		[Description("The color used to paint the background of the ruler")]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
				RedrawBitmap();
			}
		}


		[Description("")]
		[Category("Ruler")]
		public bool VerticalNumbers
		{
			get { return _VerticalNumbers; }
			set
			{
				_VerticalNumbers = value;
				RedrawBitmap();
			}
		}

		private double ZoomFactor
		{
			get { return _ZoomFactor; }
			set 
			{
				//if ((value < 0) || (value > 50)) throw new Exception("Zoom factor can be between 50% and 200%");
				if (_ZoomFactor == value) return;
				_ZoomFactor = value;
				this.ScaleMode = _ScaleMode;
				RedrawBitmap();
			}
		}

		[Description("Determines how the ruler markings are displayed")]
		[Category("Ruler")]
		public enumRulerAlignment RulerAlignment
		{
			get { return _RulerAlignment; }
			set 
			{
				if (_RulerAlignment == value) return;
				_RulerAlignment = value;
				RedrawBitmap();
			}
		}


		#endregion

		#region Private functions

		public void RedrawBitmap()
		{
			if(!_bInsideDraw)
			{
				_Bitmap = null;
				Invalidate();
			}
		}

		private int GetMillisecondScale(enumTimeScale timeScale)
		{
			int iScale = 1;
			switch(timeScale)
			{
				case enumTimeScale.MilliSeconds:
					iScale = 1;
					break;
				case enumTimeScale.Seconds:
					iScale = 1000;
					break;
				case enumTimeScale.Minutes:
					iScale = 60000;
					break;
				case enumTimeScale.Hours:
					iScale = 3600000;
					break;
			}				
			return iScale; 
		}

		private string GetTimeScaleAbbreviation(enumTimeScale timeScale)
		{
			string strText="";
			switch(timeScale)
			{
				case enumTimeScale.MilliSeconds:
					strText = "(ms)";
					break;
				case enumTimeScale.Seconds:
					strText = "(s)";
					break;
				case enumTimeScale.Minutes:
					strText = "(m)";
					break;
				case enumTimeScale.Hours:
					strText = "(h)";
					break;
			}				
			return strText; 
		}

		private double CalculateValue(int iOffset)
		{
			if (iOffset < 0) return 0;

			double nValue = ((double)iOffset-Start()) / (double)_Scale * (double)_iMajorInterval;
			return nValue + this._StartValue;
		}

		private long CalculateMillisecondValue(int iOffset)
		{
			double dblVal = CalculateValue(iOffset);
			dblVal = dblVal * GetMillisecondScale(this._TimeScale);
			return (long) dblVal;
		}

		[Description("May not return zero even when a -ve scale number is given as the returned value needs to allow for the border thickness")]
		private int CalculatePixel(double nScaleValue)
		{

			double nValue = nScaleValue - this._StartValue;
			if (nValue < 0) return Start();  // Start is the offset to the actual display area to allow for the border (if any)

			int iOffset = Convert.ToInt32(nValue / (double)_iMajorInterval * (double)_Scale);

			return iOffset + Start();
		}

		private int CalculateMillisecondPixel(double nScaleValue)
		{
			return CalculatePixel((double) nScaleValue/MillisecondScale);
		}

		public void RenderTrackLine(Graphics g)
		{
			if (_bMouseTrackingOn & _bDrawLine)
			{
				int iOffset = Offset();

				// Optionally render Mouse tracking line
				switch(Orientation)
				{
					case enumOrientation.orHorizontal:
						Line(g, _iMousePosition, iOffset, _iMousePosition, Height - iOffset);
						break;
					case enumOrientation.orVertical:
						Line (g, iOffset, _iMousePosition, Width - iOffset, _iMousePosition);
						break;
				}
			}
		}

		public void Clear()
		{
			_aryKeyFrames.Clear();
			ResetTime();
			RedrawBitmap();
		}

		public void ResetTime()
		{
			_lCurrentMillisecond = 0;
			_lActualMillisecond = 0;
			if(_CurrentFrame != null)
				_CurrentFrame.SetTimes(0, 0);
			RedrawBitmap();
		}

		private void RecalculateDivisions()
		{
			if(_bAutomaticTimeScale)
			{
				int iSize = (this.Orientation == enumOrientation.orHorizontal) ? Width : Height;
				int iOptimumDivisions;
				
				if(iSize>100)
					iOptimumDivisions = iSize/100;
				else
					iOptimumDivisions = 1;

				int iOptimumScale = iSize/iOptimumDivisions;
				int iScale=0, iDiff=0, iInterval=0;
				int iMinDiff = -1, iMinInterval = 1;

				for(int iIndex=0; iIndex<_aryIntervals.Length; iIndex++)
				{
					iInterval = _aryIntervals[iIndex];
					iScale = (DefaultScale(_ScaleMode, _ZoomFactor) * iInterval) / DefaultMajorInterval(_ScaleMode);
					iDiff = Math.Abs(iOptimumScale - iScale);
					
					if(iDiff < iMinDiff || iMinDiff == -1)
					{
						iMinDiff = iDiff;
						iMinInterval = iInterval;
					}
				}
				
				if(iMinInterval > 0) MajorInterval = iMinInterval;
				iInterval = (int) _Scale;
			}
		}

		private double CalculateZoomFactor(int iSize, enumTimeScale timeScale)
		{
			double dblZoomFactor = ((float) iSize / (float) ((float) (_lEndMillisecond - _lStartMillisecond)/GetMillisecondScale(timeScale)));
			if(dblZoomFactor > 10000) dblZoomFactor = 10000;
			if(dblZoomFactor < 0.00001) dblZoomFactor = 0.00001;
			return dblZoomFactor;
		}

		private void RecalculateTimeScale()
		{
			//Lets do the calculation for all of the possible time scales and see which one fits bets		
			if(_bAutomaticTimeScale)
			{
				int iSize = (this.Orientation == enumOrientation.orHorizontal) ? Width : Height;
				int iOptimumScale = iSize/10;

				double dblZoomFactor = CalculateZoomFactor(iSize, enumTimeScale.MilliSeconds);
				int iMilliScale = DefaultScale(dblZoomFactor) / DefaultMajorInterval();
				int iMilliDiff = Math.Abs(iOptimumScale - iMilliScale);

				dblZoomFactor = CalculateZoomFactor(iSize, enumTimeScale.Seconds);
				int iSecScale = DefaultScale(dblZoomFactor) / DefaultMajorInterval();
				int iSecDiff = Math.Abs(iOptimumScale - iSecScale);

				dblZoomFactor = CalculateZoomFactor(iSize, enumTimeScale.Minutes);
				int iMinScale = DefaultScale(dblZoomFactor) / DefaultMajorInterval();
				int iMinDiff = Math.Abs(iOptimumScale - iMinScale);

				dblZoomFactor = CalculateZoomFactor(iSize, enumTimeScale.Hours);
				int iHourScale = DefaultScale(dblZoomFactor) / DefaultMajorInterval();
				int iHourDiff = Math.Abs(iOptimumScale - iHourScale);

				if( (iSecDiff < iMilliDiff) && (iSecDiff < iMinDiff) && (iSecDiff < iHourDiff) )
					_TimeScale = enumTimeScale.Seconds;
				else if( (iMinDiff < iMilliDiff) && (iMinDiff < iSecDiff) && (iMinDiff < iHourDiff) )
					_TimeScale = enumTimeScale.Minutes;
				else if( (iHourDiff < iMilliDiff) && (iHourDiff < iSecDiff) && (iHourDiff < iMinDiff) )
					_TimeScale = enumTimeScale.Hours;
				else
					_TimeScale = enumTimeScale.MilliSeconds;
					
				RecalculateZoom();
				RecalculateDivisions();
			}
		}
		
		private void RecalculateZoom()
		{
			if(_bAutomaticOrientation)
			{
				if(Height > (Width*2) && this._Orientation == enumOrientation.orHorizontal)
					this.Orientation = enumOrientation.orVertical;
				else if(Width > (Height*2) && this._Orientation == enumOrientation.orVertical)
					this.Orientation = enumOrientation.orHorizontal;
			}


			_iDisplaySize = (this.Orientation == enumOrientation.orHorizontal) ? Width : Height;
			_iDisplaySize = _iDisplaySize - (_iSideOffset* 3);
			
			if(_iDisplaySize > 0)
				ZoomFactor = ((float) _iDisplaySize / (float) ((float) (_lEndMillisecond - _lStartMillisecond)/MillisecondScale));
		}
		

		private void DrawControl(Graphics graphics)
		{
			if(this.Width == 0 || this.Height == 0)
				return;

			_bInsideDraw = true;
			Graphics g = null;

			if(_CurrentFrame == null)
			{
				_CurrentFrame = new KeyFrameCurrent(_lCurrentMillisecond);
				_aryKeyFrames.Add(_CurrentFrame, true);
			}

			RecalculateZoom();
			RecalculateTimeScale();

			if (_Bitmap == null)
			{

				//Debug.WriteLine("TimeRuler Size: (" + this.Width.ToString() + ", " + this.Height.ToString() + ")");

				// Create a bitmap
				_Bitmap = new Bitmap(this.Width, this.Height);

				g = Graphics.FromImage(_Bitmap);

				try
				{
					// Wash the background with BackColor
					//Lets wash the whole thing out with the parent back color
					g.FillRectangle(new SolidBrush(this.Parent.BackColor), 0, 0, _Bitmap.Width, _Bitmap.Height);

					//Lets only draw the ruler portion of this in the control back color.
					if (this.Orientation == enumOrientation.orHorizontal)
					{
						int iHeight = _Bitmap.Height - _iHeaderOffset;
						g.FillRectangle(new SolidBrush(this.BackColor), _iSideOffset, _iHeaderOffset, _iDisplaySize, iHeight);
						Line(g, _iSideOffset, _iHeaderOffset, (_iDisplaySize + _iSideOffset), _iHeaderOffset);
					}
					else
					{
						int iWidth = _Bitmap.Width - _iHeaderOffset;
						g.FillRectangle(new SolidBrush(this.BackColor), _iHeaderOffset, _iSideOffset, iWidth, _iDisplaySize);
						Line(g, _iHeaderOffset, _iSideOffset, _iHeaderOffset, (_iDisplaySize + _iSideOffset));
					}

					DrawTimeScale(g);
					DrawProgressBars(g);
					_aryKeyFrames.Draw(g);

					// Paint the lines on the image
					int iScale = (int) _Scale;
					int intScale = (int) _Scale;

					int iStart = Start();
					int iEnd = iStart + _iDisplaySize + 1;
					float fltStart = (float) iStart;
					float fltEnd = (float) iEnd;

					int j;
					for(float fltJ = fltStart; fltJ <= fltEnd; fltJ += _Scale)
					{
						j = (int) fltJ;
						int iLeft = (int) _Scale;  // Make an assumption that we're starting at zero or on a major increment
						int jOffset = j+_ScaleStartValue;

						if (_RulerAlignment != enumRulerAlignment.raMiddle)
						{
							if (this.Orientation == enumOrientation.orHorizontal)
								Line(g, j, _iHeaderOffset, j, Height);
							else
								Line (g, _iHeaderOffset, j, Width, j);
						}

						iLeft = intScale;     // Set the for loop increment

						iScale = iLeft;

						int iValue = (((jOffset-iStart)/intScale)+1) * _iMajorInterval;
						DrawValue(g, iValue, j - iStart, iScale);

						int iUsed = 0;

						//Draw small lines
						for(int i = 0; i < _iNumberOfDivisions; i++)
						{
							int iX = Convert.ToInt32(Math.Round((double)(_Scale-iUsed)/(double)(_iNumberOfDivisions - i),0)); // Use a spreading algorithm rather that using expensive floating point numbers
							iUsed += iX;

							if (iUsed >= (intScale-iLeft))
							{
								iX = iUsed+j-(intScale-iLeft);

								// Is it an even number and, if so, is it the middle value?
								bool bMiddleMark = ((_iNumberOfDivisions & 0x1) == 0) & (i+1==_iNumberOfDivisions/2);
								bool bShowMiddleMark = bMiddleMark;
								bool bLastDivisionMark = (i+1 == _iNumberOfDivisions);
								bool bLastAlignMiddleDivisionMark =  bLastDivisionMark & (_RulerAlignment == enumRulerAlignment.raMiddle);
								bool bShowDivisionMark = !bMiddleMark & !bLastAlignMiddleDivisionMark;

								if( _RulerAlignment == enumRulerAlignment.raMiddle || !bLastDivisionMark )
								{
									if (bShowMiddleMark)
									{
										DivisionMark(g, iX, _MiddleMarkFactor);  // Height or Width will be 1/3
									} 
									else if (bShowDivisionMark)
									{
										DivisionMark(g, iX, _DivisionMarkFactor);  // Height or Width will be 1/5
									}
								}
							}
						}
					}
					
					if (_i3DBorderStyle != Border3DStyle.Flat)
						ControlPaint.DrawBorder3D(g, this.ClientRectangle, this._i3DBorderStyle );

				}
				catch(Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex.Message);
				}
				finally 
				{
					g.Dispose();
				}
			}

			g = graphics;

			try
			{

				// Always draw the bitmap
				g.DrawImage(_Bitmap, this.ClientRectangle);

				RenderTrackLine(g);
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
			finally
			{
				_bInsideDraw = false;
				GC.Collect();
			}

		}

		private void DrawProgressBars(Graphics g)
		{
			DrawProgressBar(g, _lActualMillisecond, new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.ForwardDiagonal, _CurrentTimeColor, _ActualTimeColor));
			DrawProgressBar(g, _lCurrentMillisecond, new SolidBrush(_CurrentTimeColor));
		}


		private void DrawProgressBar(Graphics g, long lMillisecond, System.Drawing.Brush barBrush)
		{
			float fltProgressBarScale = (float)_iProgressBarScale / 100;

			int iProgressLength = CalculatePixel(((double) lMillisecond/MillisecondScale)) - _iSideOffset;
			int iBmpSize = (this.Orientation == enumOrientation.orHorizontal) ? _Bitmap.Height : _Bitmap.Width;
			iBmpSize = iBmpSize - _iHeaderOffset;

			int iTop = (int) ((iBmpSize - (iBmpSize * fltProgressBarScale))/2) + _iHeaderOffset;
			int iBarThickness = (int) (iBmpSize * fltProgressBarScale);

			if(this.Orientation == enumOrientation.orHorizontal)
				g.FillRectangle(barBrush, _iSideOffset, iTop, iProgressLength, iBarThickness);
			else
				g.FillRectangle(barBrush, iTop, _iSideOffset, iBarThickness, iProgressLength);
		}

		private void DivisionMark(Graphics g, int iPosition, int iProportion)
		{
			// This function is affected by the RulerAlignment setting

			int iMarkStart = 0, iMarkEnd = 0;

			if (this.Orientation == enumOrientation.orHorizontal)
			{

				switch(_RulerAlignment)
				{
					case enumRulerAlignment.raBottomOrRight:
					{
						iMarkStart = Height - (Height - _iHeaderOffset)/iProportion;
						iMarkEnd = Height;
						break;
					}
					case enumRulerAlignment.raMiddle:
					{
						iMarkStart = ((Height - ((Height - _iHeaderOffset)/iProportion) + _iHeaderOffset)/2) - 1;
						iMarkEnd = (iMarkStart + (Height - _iHeaderOffset)/iProportion);
						break;
					}
					case enumRulerAlignment.raTopOrLeft:
					{
						iMarkStart = _iHeaderOffset;
						iMarkEnd = iMarkStart + (Height + _iHeaderOffset)/iProportion;
						break;
					}
				}

				Line(g, iPosition, iMarkStart, iPosition, iMarkEnd);
			}
			else
			{

				switch(_RulerAlignment)
				{
					case enumRulerAlignment.raBottomOrRight:
					{
						iMarkStart = Width - (Width - _iHeaderOffset)/iProportion;
						iMarkEnd = Width;
						break;
					}
					case enumRulerAlignment.raMiddle:
					{
						iMarkStart = ((Width - ((Width - _iHeaderOffset)/iProportion) + _iHeaderOffset)/2) - 1;
						iMarkEnd = (iMarkStart + (Width - _iHeaderOffset)/iProportion);
						break;
					}
					case enumRulerAlignment.raTopOrLeft:
					{
						iMarkStart = _iHeaderOffset;
						iMarkEnd = iMarkStart + (Width + _iHeaderOffset)/iProportion;
						break;
					}
				}

				Line(g, iMarkStart, iPosition, iMarkEnd, iPosition);
			}
		}

		private void DrawTimeScale(Graphics g)
		{
			StringFormat format = new StringFormat(StringFormatFlags.MeasureTrailingSpaces);
			if (_VerticalNumbers)
				format.FormatFlags |= StringFormatFlags.DirectionVertical;

			int iSpaceAvailable;
			//Debug.WriteLine("Scale: " +  _Scale.ToString());
			if (this.Orientation == enumOrientation.orHorizontal)
				iSpaceAvailable = (int) (_Scale*2/3)-2;
			else
				iSpaceAvailable = _Bitmap.Width - _iHeaderOffset - 2;

			string strText = _TimeScale.ToString();
			SizeF size = g.MeasureString(strText, this.Font);
			
			if(size.Width > iSpaceAvailable || 
				this.Orientation == enumOrientation.orVertical)
			{
				strText = GetTimeScaleAbbreviation(_TimeScale);
				size = g.MeasureString(strText, this.Font);
			}

			Point drawingPoint = new Point(0, 0);
			int iX = 0;
			int iY = 0;

			if (this.Orientation == enumOrientation.orHorizontal)
			{
				switch(_RulerAlignment)
				{
					case enumRulerAlignment.raBottomOrRight:
					{
						iX = 7;
						iY = _iHeaderOffset + 2;
						break;
					}
					case enumRulerAlignment.raMiddle:
					{
						iX = 7;
						iY = _iHeaderOffset + 2;
						break;
					}
					case enumRulerAlignment.raTopOrLeft:
					{
						iX = 7;
						iY = Height - (int)size.Height - 2;
						break;
					}
				}

				drawingPoint = new Point(iX, iY);
				//g.DrawString(strText, this.Font, new SolidBrush(this.ForeColor), drawingPoint, format);
			}
			else
			{
				switch(_RulerAlignment)
				{
					case enumRulerAlignment.raBottomOrRight:
					{
						iX = _iHeaderOffset + 2;
						iY = 7;
						break;
					}
					case enumRulerAlignment.raMiddle:
					{
						iX = _iHeaderOffset + 2;
						iY = 7;
						break;
					}
					case enumRulerAlignment.raTopOrLeft:
					{
						iX = Width - 2 - (int)size.Width;
						iY = 7;
						break;
					}
				}

				drawingPoint = new Point(iX, iY);
			}

			// The drawstring function is common to all operations
			g.DrawString(strText, this.Font, new SolidBrush(this.ForeColor), drawingPoint, format);
		}

		private void DrawValue(Graphics g, int iValue, int iPosition, int iSpaceAvailable)
		{

			// The sizing operation is common to all options
			StringFormat format = new StringFormat(StringFormatFlags.MeasureTrailingSpaces);
			if (_VerticalNumbers)
				format.FormatFlags |= StringFormatFlags.DirectionVertical;
			
			SizeF size = g.MeasureString((iValue).ToString(), this.Font, iSpaceAvailable, format);

			Point drawingPoint;
			int iX = 0;
			int iY = 0;

			if (this.Orientation == enumOrientation.orHorizontal)
			{
				switch(_RulerAlignment)
				{
					case enumRulerAlignment.raBottomOrRight:
					{
						iX = iPosition + iSpaceAvailable - (int)size.Width - 2;
						iY = _iHeaderOffset + 2;
						break;
					}
					case enumRulerAlignment.raMiddle:
					{
						iX = iPosition + iSpaceAvailable - (int)size.Width/2;
						iY = _iHeaderOffset + (Height - _iHeaderOffset - (int)size.Height)/2 - 2;
						break;
					}
					case enumRulerAlignment.raTopOrLeft:
					{
						iX = iPosition + iSpaceAvailable - (int)size.Width - 2;
						iY = Height - (int)size.Height - 2;
						break;
					}
				}

				drawingPoint = new Point(iX, iY);
			}
			else
			{
				switch(_RulerAlignment)
				{
					case enumRulerAlignment.raBottomOrRight:
					{
						iX = _iHeaderOffset + 2;
						iY = iPosition + iSpaceAvailable - (int)size.Height - 2;
						break;
					}
					case enumRulerAlignment.raMiddle:
					{
						iX = _iHeaderOffset + (Width - _iHeaderOffset - (int)size.Width)/2 - 2;
						iY = iPosition + iSpaceAvailable - (int)size.Height/2;
						break;
					}
					case enumRulerAlignment.raTopOrLeft:
					{
						iX = Width - (_iHeaderOffset + 2) - (int)size.Width;
						iY = iPosition + iSpaceAvailable - (int)size.Height - 2;
						break;
					}
				}

				drawingPoint = new Point(iX, iY);
			}

			// The drawstring function is common to all operations

			g.DrawString(iValue.ToString(), this.Font, new SolidBrush(this.ForeColor), drawingPoint, format);
		}

		private void Line(Graphics g, int x1, int y1, int x2, int y2)
		{
			g.DrawLine(new Pen(new SolidBrush(this.ForeColor)), x1, y1, x2, y2);
		}

		private int DefaultScale()
		{return DefaultScale(_ScaleMode);}

		private int DefaultScale(enumScaleMode iScaleMode)
		{return DefaultScale(iScaleMode, _ZoomFactor);}

		private int DefaultScale(double dblZoomFactor)
		{return DefaultScale(_ScaleMode, dblZoomFactor);}

		private int DefaultScale(enumScaleMode iScaleMode, double dblZoomFactor)
		{
			int iScale = 100;

			// Set scaling
			switch(iScaleMode)
			{
					// Determines the *relative* proportions of each scale
				case enumScaleMode.smPoints:
					iScale = 96;
					break;
				case enumScaleMode.smPixels:
					iScale = 100;
					break;
				case enumScaleMode.smCentimetres:
					iScale = 38;
					break;
				case enumScaleMode.smInches:
					iScale = 96;
					break;
			}

			return Convert.ToInt32((double)iScale * dblZoomFactor);
		}

		private float DefaultScale1(enumScaleMode iScaleMode, double dblZoomFactor)
		{
			int iScale = 100;

			// Set scaling
			switch(iScaleMode)
			{
					// Determines the *relative* proportions of each scale
				case enumScaleMode.smPoints:
					iScale = 96;
					break;
				case enumScaleMode.smPixels:
					iScale = 100;
					break;
				case enumScaleMode.smCentimetres:
					iScale = 38;
					break;
				case enumScaleMode.smInches:
					iScale = 96;
					break;
			}

			return (float) (iScale * dblZoomFactor);
		}

		private int DefaultMajorInterval()
		{return DefaultMajorInterval(_ScaleMode);}

		private int DefaultMajorInterval(enumScaleMode iScaleMode)
		{
			int iInterval = 10;

			// Set scaling
			switch(iScaleMode)
			{
					// Determines the *relative* proportions of each scale
				case enumScaleMode.smPoints:
					iInterval = 72;
					break;
				case enumScaleMode.smPixels:
					iInterval = 100;
					break;
				case enumScaleMode.smCentimetres:
					iInterval = 1;
					break;
				case enumScaleMode.smInches:
					iInterval = 1;
					break;
			}

			return iInterval;
		}

		private int Offset()
		{
			int iOffset = 0;

			switch(this._i3DBorderStyle)
			{
				case Border3DStyle.Flat: iOffset = 0; break;
				case Border3DStyle.Adjust: iOffset = 0; break;
				case Border3DStyle.Sunken: iOffset = 2; break;
				case Border3DStyle.Bump: iOffset = 2; break;
				case Border3DStyle.Etched: iOffset = 2; break;
				case Border3DStyle.Raised: iOffset = 2; break;
				case Border3DStyle.RaisedInner: iOffset = 1; break;
				case Border3DStyle.RaisedOuter: iOffset = 1; break;
				case Border3DStyle.SunkenInner: iOffset = 1; break;
				case Border3DStyle.SunkenOuter: iOffset = 1; break;
				default: iOffset = 0; break;
			}

			return iOffset;
		}

		private int Start()
		{
			int iStart = 0;

			switch(this._i3DBorderStyle)
			{
				case Border3DStyle.Flat: iStart = 0; break;
				case Border3DStyle.Adjust: iStart = 0; break;
				case Border3DStyle.Sunken: iStart = 1; break;
				case Border3DStyle.Bump: iStart = 1; break;
				case Border3DStyle.Etched: iStart = 1; break;
				case Border3DStyle.Raised: iStart = 1; break;
				case Border3DStyle.RaisedInner: iStart = 0; break;
				case Border3DStyle.RaisedOuter: iStart = 0; break;
				case Border3DStyle.SunkenInner: iStart = 0; break;
				case Border3DStyle.SunkenOuter: iStart = 0; break;
				default: iStart = 0; break;
			}
			return (iStart + _iSideOffset);
		}

		private void ChangeMousePosition(int iNewPosition)
		{
			this._iOldMousePosition = this._iMousePosition;
			this._iMousePosition = iNewPosition;
		}

		private void AddKeyFrame(Point pMousePos)
		{
			try
			{
				int iMousePosition = ((this.Orientation == enumOrientation.orHorizontal) ? pMousePos.X : pMousePos.Y);
				long lMillisecond = CalculateMillisecondValue(iMousePosition);

				if( (lMillisecond >= _lStartMillisecond) && (lMillisecond <= _lEndMillisecond) )
				{
					if(lMillisecond <= _lCurrentMillisecond)
						throw new System.Exception("You can not add new keyframes that occur before the current simulation time.");

					KeyFrame keyFrame = new KeyFrame(lMillisecond);
					this.KeyFrames.Add(keyFrame);

					OnKeyFrameAdded(keyFrame);

					SelectedKeyFrame = keyFrame;
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void RemoveKeyFrame(Point pMousePos)
		{
			try
			{
				int iMousePosition = ((this.Orientation == enumOrientation.orHorizontal) ? pMousePos.X : pMousePos.Y);
				long lMillisecond = CalculateMillisecondValue(iMousePosition);
				
				if(lMillisecond <= _lCurrentMillisecond)
					throw new System.Exception("You can not remove keyframes that occur before the current simulation time.");

				KeyFrame keyFrame = _aryKeyFrames.RemoveClosest(KeyFrame.enumKeyFrameType.Snapshot, lMillisecond);

				if(keyFrame != null)
					OnKeyFrameRemoved(keyFrame);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void AddVideoFrame(Point pMousePos)
		{
			try
			{
				int iMousePosition = ((this.Orientation == enumOrientation.orHorizontal) ? pMousePos.X : pMousePos.Y);
				long lMillisecond = CalculateMillisecondValue(iMousePosition);

				if( (lMillisecond >= _lStartMillisecond) && (lMillisecond <= _lEndMillisecond) )
				{
					if(lMillisecond <= _lCurrentMillisecond)
						throw new System.Exception("You can not add new keyframes that occur before the current simulation time.");

					KeyFrame keyFrame = new KeyFrameVideoRange(lMillisecond, lMillisecond + 200);
					this.KeyFrames.Add(keyFrame);

					OnKeyFrameAdded(keyFrame);

					SelectedKeyFrame = keyFrame;
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void RemoveVideoFrame(Point pMousePos)
		{
			try
			{
				int iMousePosition = ((this.Orientation == enumOrientation.orHorizontal) ? pMousePos.X : pMousePos.Y);
				long lMillisecond = CalculateMillisecondValue(iMousePosition);
				
				KeyFrame keyFrame = _aryKeyFrames.RemoveClosest(KeyFrame.enumKeyFrameType.Video, lMillisecond);

				if(keyFrame != null)
					OnKeyFrameRemoved(keyFrame);
			}
			catch(Exception ex)
			{
			MessageBox.Show(ex.Message);
			}
		}

	}

	#endregion

}