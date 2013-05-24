#region Using Directives
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;
using System.Threading;
#endregion

namespace AnimatGuiCtrls.Forms
{
    public delegate void DelegateCloseSplash();

    public class SplashForm : Form
    {
        //private string m_strText = "This is a test string\r\nSecond row";
        //private System.Drawing.Font m_Font = new System.Drawing.Font("Arial", 16);
        //private PointF m_TextPos = new PointF(10, 10);
        //private System.Drawing.Color m_TextColor = System.Drawing.Color.Black;

        #region Constructor
        public SplashForm(Bitmap bmpFile, Color col, string strText, System.Drawing.Font fontText, PointF TextPos, System.Drawing.Color TextColor)
        {
            // ====================================================================================
            // Setup the form
            // ==================================================================================== 
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this.TopMost = true;

            // make form transparent
            this.TransparencyKey = this.BackColor;

            m_strText = strText;
            m_Font = fontText;
            m_TextPos = TextPos;
            m_TextColor = TextColor;

            // tie up the events
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SplashForm_KeyUp);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SplashForm_Paint);
            this.MouseDown += new MouseEventHandler(SplashForm_MouseClick);

            // load and make the bitmap transparent
            m_bmp = bmpFile;

            if(m_bmp == null)
                throw new Exception("Failed to load the bitmap file");
//						if(col != null)
//	            m_bmp.MakeTransparent(col);

            // resize the form to the size of the iamge
            this.Width = m_bmp.Width;
            this.Height = m_bmp.Height;

            // center the form
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            // thread handling
            m_delegateClose = new DelegateCloseSplash(InternalCloseSplash);
        }
        #endregion // Constructor

        #region Public methods
        // this can be used for About dialogs
        public static void ShowModal(Bitmap imageFile, Color col, string strText, System.Drawing.Font fontText, PointF TextPos, System.Drawing.Color TextColor)
        {
            m_imageFile = imageFile;
            m_transColor = col;
            m_strText = strText;
            m_Font = fontText;
            m_TextPos = TextPos;
            m_TextColor = TextColor;

            MySplashThreadFunc();
        }
        // Call this method with the image file path and the color 
        // in the image to be rendered transparent
        public static void StartSplash(Bitmap imageFile, Color col, string strText, System.Drawing.Font fontText, PointF TextPos, System.Drawing.Color TextColor)
        {
            m_imageFile = imageFile;
            m_transColor = col;
            m_strText = strText;
            m_Font = fontText;
            m_TextPos = TextPos;
            m_TextColor = TextColor;

            // Create and Start the splash thread
            Thread InstanceCaller = new Thread(new ThreadStart(MySplashThreadFunc));
            InstanceCaller.Start();
        }

        // Call this at the end of your apps initialization to close the splash screen
        public static void CloseSplash()
        {
            if(m_instance != null)
                m_instance.Invoke(m_instance.m_delegateClose);

        }
        #endregion // Public methods

        #region Dispose
        protected override void Dispose( bool disposing )
        {
            m_bmp.Dispose();
            base.Dispose( disposing );
            m_instance = null;
        }
        #endregion // Dispose

        #region Threading code
        // ultimately this is called for closing the splash window
        void InternalCloseSplash()
        {
						if(m_instance != null)
							m_instance.TopMost = false;

						this.Close();
            this.Dispose();
        }
        // this is called by the new thread to show the splash screen
        private static void MySplashThreadFunc()
        {
            m_instance = new SplashForm(m_imageFile, m_transColor, m_strText, m_Font, m_TextPos, m_TextColor);
            m_instance.TopMost = false;
            m_instance.ShowDialog();
        }
        #endregion // Multithreading code

        #region Event Handlers

        void SplashForm_MouseClick(object sender, MouseEventArgs e)
        {
            this.InternalCloseSplash();
        }

        private void SplashForm_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            e.Graphics.DrawImage(m_bmp, 0,0);

            if(m_strText.Length > 0)
                e.Graphics.DrawString(m_strText, m_Font, new System.Drawing.SolidBrush(m_TextColor), m_TextPos);
        }

        private void SplashForm_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
                this.InternalCloseSplash();
        }
        #endregion // Event Handlers

        #region Private variables
        private static SplashForm m_instance;
        private static Bitmap m_imageFile;
        private static Color m_transColor;
        private Bitmap m_bmp;
        private static string m_strText = "";
        private static System.Drawing.Font m_Font = new System.Drawing.Font("Arial", 16);
        private static PointF m_TextPos = new PointF(10, 10);
        private static System.Drawing.Color m_TextColor = System.Drawing.Color.Black;
        private DelegateCloseSplash m_delegateClose;
        #endregion
    }
}
