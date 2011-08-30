using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace AnimatGuiCtrls.Controls
{

	#region DataGridTextBoxColumn
	//**********************************************************************************************
	//	DataGridTextBoxColumn
	//**********************************************************************************************
	public class DataGridComboBoxColumn : DataGridColumnStyle { //DataGridTextBoxColumn {
		private	DataGridComboBox	combobox;
		private	bool					edit;

		//-------------------------------------------------------------------------------------------
		//	Constructors and destructors
		//-------------------------------------------------------------------------------------------
		public DataGridComboBoxColumn() {
			combobox										= new DataGridComboBox();
			combobox.Visible							= false;
			combobox.DropDownStyle					= ComboBoxStyle.DropDownList;
			combobox.Leave								+= new EventHandler(ComboHide);
			combobox.SelectionChangeCommitted	+= new EventHandler(ComboStartEditing);
			edit											= false;
		} // DataGridComboBoxColumn

		//-------------------------------------------------------------------------------------------
		//	Properties
		//-------------------------------------------------------------------------------------------
		public ComboBox comboBox {
			get {
				return combobox;
			}
		} // comboBox

		//-------------------------------------------------------------------------------------------
		//	ComboBox event handlers
		//-------------------------------------------------------------------------------------------
		private void ComboHide(object sender, EventArgs e) {
			// When the ComboBox looses focus, then simply hide it.
			combobox.Hide();
		} // ComboHide

		private void ComboStartEditing(object sender, EventArgs e) {
			// Enter edit mode.
			edit = true;
			base.ColumnStartedEditing((Control)sender);
		} // ComboStartEditing

		//-------------------------------------------------------------------------------------------
		//	Override DataGridColumnStyle
		//-------------------------------------------------------------------------------------------
		protected override void SetDataGridInColumn(DataGrid value) {
			// Add the ComboBox to the DataGrids controls collection.
			// This ensures correct DataGrid scrolling.
			value.Controls.Add(combobox);
			base.SetDataGridInColumn(value);
		} // SetDataGridInColumn

		protected override void Abort(int rowNum) {
			// Abort edit mode, discard changes and hide the ComboBox.
			edit = false;
			Invalidate();
			combobox.Hide();
		} // Abort

		protected override void Edit(System.Windows.Forms.CurrencyManager source, int rowNum, System.Drawing.Rectangle bounds, bool readOnly, string instantText, bool cellIsVisible) {
			// Setup the ComboBox for action.
			// This includes positioning the ComboBox and showing it.
			// Also select the correct item in the ComboBox before it is shown.
			combobox.Parent			= this.DataGridTableStyle.DataGrid;
			combobox.Bounds			= bounds;
			combobox.Size				= new Size(this.Width, this.comboBox.Height);
			comboBox.SelectedValue	= base.GetColumnValueAtRow(source, rowNum).ToString();
			combobox.Visible			= (cellIsVisible == true) && (readOnly == false);
			combobox.BringToFront();
			combobox.Focus();	
		} // Edit

		protected override bool Commit(System.Windows.Forms.CurrencyManager source, int rowNum) {
			// Commit the selected value from the ComboBox to the DataGrid.
			if (edit == true) {
				edit = false;
				this.SetColumnValueAtRow(source, rowNum, combobox.SelectedValue);
			}

			return true;
		} // Commit

		protected override object GetColumnValueAtRow(System.Windows.Forms.CurrencyManager source, int rowNum) {
			// Return the display text associated with the data, insted of the
			// data from the DataGrid datasource.
			return combobox.GetDisplayText(base.GetColumnValueAtRow(source, rowNum));
		} // GetColumnValueAtRow

		protected override void SetColumnValueAtRow(CurrencyManager source, int rowNum, object value) {
			// Save the data (value) to the DataGrid datasource.
			// I try a few different types, because I often uses GUIDs as keys in my
			// data.

			// String.
			try {
				base.SetColumnValueAtRow(source, rowNum, value.ToString());
				return;
			} catch {}

			// Guid.
			try {
				base.SetColumnValueAtRow(source, rowNum, new Guid(value.ToString()));
				return;
			} catch {}

			// Object (default).
			base.SetColumnValueAtRow(source, rowNum, value);
		} // SetColumnValueAtRow

		protected override int GetMinimumHeight() {
			// Return the ComboBox preferred height, plus a few pixels.
			return combobox.PreferredHeight + 2;
		} // GetMinimumHeight
		
		protected override int GetPreferredHeight(Graphics g, object val) {
			// Return the font height, plus a few pixels.
			return FontHeight + 2;
		} // GetPreferredHeight

		protected override Size GetPreferredSize(Graphics g, object val) {
			// Return the preferred width.
			// Iterate through all display texts in the dropdown, and measure each
			// text width.
			int		widest		= 0;
			SizeF		stringSize	= new SizeF(0, 0);
			foreach (string text in combobox.GetDisplayText()) {
				stringSize	= g.MeasureString(text, base.DataGridTableStyle.DataGrid.Font);
				if (stringSize.Width > widest) {
					widest = (int)Math.Ceiling(stringSize.Width);
				}
			}

			return new Size(widest + 25, combobox.PreferredHeight + 2);
		} // GetPreferredSize

		protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum) {
			Paint(g, bounds, source, rowNum, false);
		} // Paint

		protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, bool alignToRight) {
			string			text				= GetColumnValueAtRow(source, rowNum).ToString();
			Brush				backBrush		= new SolidBrush(base.DataGridTableStyle.BackColor);
			Brush				foreBrush		= new SolidBrush(base.DataGridTableStyle.ForeColor);
			Rectangle		rect				= bounds;
			StringFormat	format			= new StringFormat();

			// Handle that the row can be selected.
			if (base.DataGridTableStyle.DataGrid.IsSelected(rowNum) == true) {
				backBrush		= new SolidBrush(base.DataGridTableStyle.SelectionBackColor);
				foreBrush		= new SolidBrush(base.DataGridTableStyle.SelectionForeColor);
			}

			// Handle align to right.
			if (alignToRight == true) {
				format.FormatFlags	= StringFormatFlags.DirectionRightToLeft;
			}

			// Handle alignment.
			switch (this.Alignment) {
				case HorizontalAlignment.Left:
					format.Alignment	= StringAlignment.Near;
					break;
				case HorizontalAlignment.Right:
					format.Alignment	= StringAlignment.Far;
					break;
				case HorizontalAlignment.Center:
					format.Alignment	= StringAlignment.Center;
					break;
			}

			// Paint.
			format.FormatFlags		= StringFormatFlags.NoWrap;
			g.FillRectangle(backBrush, rect);
			rect.Offset(0, 2);
			rect.Height -= 2;
			g.DrawString(text, this.DataGridTableStyle.DataGrid.Font, foreBrush, rect, format);
			format.Dispose();
		} // PaintText

	} // DataGridComboBoxColumn
	#endregion

	#region DataGridComboBox
	//**********************************************************************************************
	//	DataGridComboBox
	//**********************************************************************************************
	public class DataGridComboBox : ComboBox {
		private const int WM_KEYUP = 0x101;

		protected override void WndProc(ref System.Windows.Forms.Message message) {
			// Ignore keyup to avoid problem with tabbing and dropdown list.
			if (message.Msg == WM_KEYUP) {
				return;
			}

			base.WndProc(ref message);
		} // WndProc

		public string GetValueText(int index) {
			// Validate the index.
			if ((index < 0) && (index >= base.Items.Count))
				throw new IndexOutOfRangeException("Invalid index.");

			// Get the text.
			string	text			= string.Empty;
			int		memIndex		= -1;
			try {
				base.BeginUpdate();
				memIndex					= base.SelectedIndex;
				base.SelectedIndex	= index;
				text						= base.SelectedValue.ToString();
				base.SelectedIndex	= memIndex;
			} catch {
			} finally {
				base.EndUpdate();
			}

			return text;
		} // GetValueText

		public string GetDisplayText(int index) {
			// Validate the index.
			if ((index < 0) && (index >= base.Items.Count))
				throw new IndexOutOfRangeException("Invalid index.");

			// Get the text.
			string	text			= string.Empty;
			int		memIndex		= -1;
			try {
				base.BeginUpdate();
				memIndex					= base.SelectedIndex;
				base.SelectedIndex	= index;
				text						= base.SelectedItem.ToString();
				base.SelectedIndex	= memIndex;
			} catch {
			} finally {
				base.EndUpdate();
			}

			return text;
		} // GetDisplayText

		public string GetDisplayText(object value) {
			// Get the text.
			string	text			= string.Empty;
			int		memIndex		= -1;
			try {
				base.BeginUpdate();
				memIndex					= base.SelectedIndex;
				base.SelectedValue	= value.ToString();
				text						= base.SelectedItem.ToString();
				base.SelectedIndex	= memIndex;
			} catch {
			} finally {
				base.EndUpdate();
			}

			return text;
		} // GetDisplayText

		public string[] GetDisplayText() {
			// Get the text.
			string[]	text			= new string[base.Items.Count];
			int		memIndex		= -1;
			try {
				base.BeginUpdate();
				memIndex					= base.SelectedIndex;
				for (int index = 0; index < base.Items.Count; index++) {
					base.SelectedIndex	= index;
					text[index]				= base.SelectedItem.ToString();
				}
				base.SelectedIndex	= memIndex;
			} catch {
			} finally {
				base.EndUpdate();
			}

			return text;
		} // GetDisplayText

	} // DataGridComboBox
	#endregion

} // DataGridCombo