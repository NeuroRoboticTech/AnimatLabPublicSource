// A ComboBox with a TreeView Drop-Down
// Bradley Smith - 2010/11/04

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;
using System.Windows.Forms.VisualStyles;

namespace AnimatGuiCtrls.Controls
{

    /// <summary>
    /// Abstract base class for a control which behaves like a dropdown but does not contain 
    /// logic for displaying a popup window.
    /// </summary>
    [Designer(typeof(DropDownControlDesigner))]
    public abstract class DropDownControlBase : Control
    {

        const int CONTROL_HEIGHT = 7;
        const int DROPDOWNBUTTON_WIDTH = 17;

        private bool drawWithVisualStyles;
        private Rectangle dropDownButtonBounds;
        private bool droppedDown;

        /// <summary>
        /// Determines whether to draw the control with visual styles.
        /// </summary>
        [DefaultValue(true), Description("Determines whether to draw the control with visual styles."), Category("Appearance")]
        public bool DrawWithVisualStyles
        {
            get { return drawWithVisualStyles; }
            set
            {
                drawWithVisualStyles = value;
                Invalidate();
            }
        }
        /// <summary>
        /// Opens or closes the dropdown portion of the control.
        /// </summary>
        [Browsable(false)]
        public virtual bool DroppedDown
        {
            get { return droppedDown; }
            set
            {
                droppedDown = value;
                Invalidate();
            }
        }
        /// <summary>
        /// Gets or sets the background color to use for this control.
        /// </summary>
        [DefaultValue(typeof(Color), "Window")]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }
        /// <summary>
        /// Hides the BackgroundImage property on the designer.
        /// </summary>
        [Browsable(false)]
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = value;
            }
        }
        /// <summary>
        /// Hides the BackgroundImageLayout property on the designer.
        /// </summary>
        [Browsable(false)]
        public override ImageLayout BackgroundImageLayout
        {
            get
            {
                return base.BackgroundImageLayout;
            }
            set
            {
                base.BackgroundImageLayout = value;
            }
        }

        /// <summary>
        /// Fired when the user clicks the dropdown button at the right edge of the control.
        /// </summary>
        [Description("Occurs when the user clicks the dropdown button at the right edge of the control.")]
        protected event EventHandler DropDownButtonClick;
        /// <summary>
        /// Fired when the drop-down portion of the control is displayed.
        /// </summary>
        [Description("Occurs when the drop-down portion of the control is displayed.")]
        public event EventHandler DropDown;
        /// <summary>
        /// Fired when the drop-down portion of the control is closed.
        /// </summary>
        [Description("Occurs when the drop-down portion of the control is closed.")]
        public event EventHandler DropDownClosed;
        /// <summary>
        /// Fired when the content of the editable portion of the control is painted.
        /// </summary>
        [Description("Occurs when the content of the editable portion of the control is painted.")]
        public event EventHandler<DropDownPaintEventArgs> PaintContent;

        /// <summary>
        /// Creates a new instance of DropDownControlBase.
        /// </summary>
        public DropDownControlBase()
        {
            // control styles
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Selectable, true);
            SetStyle(ControlStyles.StandardClick, true);
            SetStyle(ControlStyles.UserPaint, true);

            // default values
            drawWithVisualStyles = true;
            BackColor = SystemColors.Window;
        }

        /// <summary>
        /// Gets the bounds of the textbox portion of the control by subtracting the dropdown button bounds from the client rectangle.
        /// </summary>
        /// <returns></returns>
        private Rectangle GetTextBoxBounds()
        {
            return new Rectangle(0, 0, dropDownButtonBounds.Left, ClientRectangle.Height);
        }

        /// <summary>
        /// Determines the state in which to render the textbox portion of the control (when using visual styles).
        /// </summary>
        /// <returns></returns>
        private ComboBoxState GetTextBoxState()
        {
            if (!Enabled)
                return ComboBoxState.Disabled;
            else if (Focused || ClientRectangle.Contains(PointToClient(Cursor.Position)))
                return ComboBoxState.Hot;
            else
                return ComboBoxState.Normal;
        }

        /// <summary>
        /// Determines the state in which to render the dropdown button portion of the control (when using visual styles).
        /// </summary>
        /// <returns></returns>
        private ComboBoxState GetDropDownButtonState()
        {
            if (!Enabled)
                return ComboBoxState.Disabled;
            else if (droppedDown || dropDownButtonBounds.Contains(PointToClient(Cursor.Position)))
                return (droppedDown || ((MouseButtons & MouseButtons.Left) == MouseButtons.Left)) ? ComboBoxState.Pressed : ComboBoxState.Hot;
            else
                return ComboBoxState.Normal;
        }

        /// <summary>
        /// Determines the state in which to render the dropdown button portion of the control (when not using visual styles).
        /// </summary>
        /// <returns></returns>
        private ButtonState GetPlainButtonState()
        {
            if (!Enabled)
                return ButtonState.Inactive;
            else if (droppedDown || (dropDownButtonBounds.Contains(PointToClient(Cursor.Position)) && ((MouseButtons & MouseButtons.Left) == MouseButtons.Left)))
                return ButtonState.Pushed;
            else
                return ButtonState.Normal;
        }

        /// <summary>
        /// Registers the arrow keys as input keys.
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                case Keys.PageDown:
                case Keys.PageUp:
                case Keys.Home:
                case Keys.End:
                case Keys.Enter:
                    return true;
                default:
                    return base.IsInputKey(keyData);
            }
        }

        /// <summary>
        /// Raised the DropDownButtonClick event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDropDownButtonClick(EventArgs e)
        {
            if (DropDownButtonClick != null) DropDownButtonClick(this, e);
        }

        /// <summary>
        /// Raises the DropDown event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDropDown(EventArgs e)
        {
            if (DropDown != null) DropDown(this, e);
        }

        /// <summary>
        /// Raises the DropDownClosed event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDropDownClosed(EventArgs e)
        {
            if (DropDownClosed != null) DropDownClosed(this, e);
        }

        /// <summary>
        /// Recalculates the fixed height of the control when the font changes.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            SetHeight();
        }

        /// <summary>
        /// Repaints the focus rectangle when focus changes.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            if (ShowFocusCues) Invalidate();
        }

        /// <summary>
        /// Repaints the focus rectangle when focus changes.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (ShowFocusCues) Invalidate();
        }

        /// <summary>
        /// Paints the control.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (drawWithVisualStyles && ComboBoxRenderer.IsSupported)
            {
                // draw using the visual style renderer
                ComboBoxRenderer.DrawTextBox(e.Graphics, ClientRectangle, GetTextBoxState());
                ComboBoxRenderer.DrawDropDownButton(e.Graphics, dropDownButtonBounds, GetDropDownButtonState());
            }
            else
            {
                // draw using the legacy technique
                Rectangle borderRect = ClientRectangle;
                borderRect.Height++;
                e.Graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);
                ControlPaint.DrawBorder3D(e.Graphics, borderRect);
                ControlPaint.DrawComboButton(e.Graphics, dropDownButtonBounds, GetPlainButtonState());
            }

            OnPaintContent(new DropDownPaintEventArgs(e.Graphics, e.ClipRectangle, GetTextBoxBounds()));
        }

        /// <summary>
        /// Paints the content in the editable portion of the control, providing additional measurements and operations.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPaintContent(DropDownPaintEventArgs e)
        {
            if (PaintContent != null) PaintContent(this, e);
        }

        /// <summary>
        /// Repaints the control when the mouse enters its bounds.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            Invalidate();
        }

        /// <summary>
        /// Repaints the control when the mouse leaves its bounds.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            Invalidate();
        }

        /// <summary>
        /// Repaints the control when a mouse button is pressed.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Focus();
            Invalidate(dropDownButtonBounds);
        }

        /// <summary>
        /// Repaints the control when a mouse button is released.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Invalidate(dropDownButtonBounds);
        }

        /// <summary>
        /// Repaints the control when the mouse is moved over the control.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Invalidate(dropDownButtonBounds);
        }

        /// <summary>
        /// Determines when to raise the DropDownButtonClick event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (dropDownButtonBounds.Contains(e.Location)) OnDropDownButtonClick(e);
        }

        /// <summary>
        /// Recalculates the bounds for the dropdown button when the control's size changes.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            dropDownButtonBounds = new Rectangle(ClientSize.Width - DROPDOWNBUTTON_WIDTH, 0, DROPDOWNBUTTON_WIDTH, ClientSize.Height);
        }

        /// <summary>
        /// Sets the fixed height of the control, based on the font size.
        /// </summary>
        private void SetHeight()
        {
            Height = CONTROL_HEIGHT + Font.Height;
        }
    }

    /// <summary>
    /// EventArgs class for the 
    /// </summary>
    public class DropDownPaintEventArgs : PaintEventArgs
    {

        /// <summary>
        /// Gets the display rectangle for the editable portion of the control.
        /// </summary>
        public Rectangle Bounds
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a new instance of the DropDownPaintEventArgs class.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="clipRect"></param>
        /// <param name="bounds"></param>
        public DropDownPaintEventArgs(Graphics graphics, Rectangle clipRect, Rectangle bounds)
            : base(graphics, clipRect)
        {
            Bounds = bounds;
        }

        /// <summary>
        /// Draws a focus rectangle on the editable portion of the control.
        /// </summary>
        public void DrawFocusRectangle()
        {
            Rectangle focus = Bounds;
            focus.Inflate(-2, -2);
            focus.Width++;
            ControlPaint.DrawFocusRectangle(Graphics, focus);
        }
    }

    /// <summary>
    /// Designer for DropDownControlBase
    /// </summary>
    public class DropDownControlDesigner : ControlDesigner
    {

        /// <summary>
        /// Ensures that this control can only be sized horizontally.
        /// </summary>
        public override SelectionRules SelectionRules
        {
            get
            {
                return base.SelectionRules & ~SelectionRules.BottomSizeable & ~SelectionRules.TopSizeable;
            }
        }

        /// <summary>
        /// Gets a list containing the four main alignment points plus the baseline for the text.
        /// </summary>
        public override IList SnapLines
        {
            get
            {
                IList snapLines = base.SnapLines;
                snapLines.Add(new SnapLine(SnapLineType.Baseline, Control.Height / 2 - (int)Control.Font.Size / 2 + (int)Control.Font.Size));
                return snapLines;
            }
        }
    }

}
