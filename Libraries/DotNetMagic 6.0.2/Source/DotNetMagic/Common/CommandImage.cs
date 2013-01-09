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
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Crownwood.DotNetMagic.Common
{
	/// <summary>
	/// Manage an individual source image.
	/// </summary>
	public class CommandImage : IDisposable
	{
		// Class constants
		private readonly Color _transparent = Color.FromArgb(0, 0, 0, 0);
		private readonly int EXTRA_OFFICE2003_WIDTH = 2;
		private readonly int EXTRA_OFFICE2003_HEIGHT = 2;
		private readonly int EXTRA_PLAIN_WIDTH = 1;
		private readonly int EXTRA_PLAIN_HEIGHT = 1;

		// Instance fields
		private Image _image;
		private Image _disabledImage;
		private Color _disabledBackColor;
		private Image _whiteImage;
		private Image _fadedImage;

		/// <summary>
		/// Initializes a new instance of the CommandImage class.
		/// </summary>
		public CommandImage()
		{
			// Default to no image at all
			_image = null;
			_disabledImage = null;
			_disabledBackColor = Color.Empty;
			_whiteImage = null;
			_fadedImage = null;
		}

		/// <summary>
		/// Dispose of any resources.
		/// </summary>
		public void Dispose()
		{
			if (_image != null)
				_image = null;

			if (_disabledImage != null)
			{
				_disabledImage.Dispose();
				_disabledImage = null;
			}

			if (_whiteImage != null)
			{
				_whiteImage.Dispose();
				_whiteImage = null;
			}

			if (_fadedImage != null)
			{
				_fadedImage.Dispose();
				_fadedImage = null;
			}
		}

		/// <summary>
		/// Initializes a new instance of the CommandImage class.
		/// </summary>
		/// <param name="image">Initial image.</param>
		public CommandImage(Image image)
		{
			// Store initial image
			_image = image;

			// Default to no cached extra images
			_disabledImage = null;
			_whiteImage = null;
			_fadedImage = null;
		}
		
		/// <summary>
		/// Gets a new empty command image.
		/// </summary>
		public static CommandImage Empty
		{
			get { return new CommandImage(); }
		}

		/// <summary>
		///  Gets access to the original image.
		/// </summary>
		public virtual Image Image
		{
			get { return _image; }
			
			set 
			{
				// Only interest in changes
				if (_image != value)
				{
					// Release existing images
					if (_image != null)
						_image = null;

					if (_disabledImage != null)
					{
						_disabledImage.Dispose();
						_disabledImage = null;
					}

					if (_whiteImage != null)
					{
						_whiteImage.Dispose();
						_whiteImage = null;
					}

					if (_fadedImage != null)
					{
						_fadedImage.Dispose();
						_fadedImage = null;
					}

					// Store new image
					_image = value;
				}
			}
		}

		/// <summary>
		/// Gets a disabled version of the original image.
		/// </summary>
		/// <param name="backColor">Background color for creating image.</param>
		/// <returns></returns>
		public virtual Image GetDisabledImage(Color backColor)
		{
			if ((_disabledImage == null) || (backColor != _disabledBackColor))
			{
				// Create a new bitmap based on the existing one
				Bitmap disabledImage = new Bitmap(_image.Width, _image.Height, PixelFormat.Format32bppArgb);

				// Create a graphics object from the bitmap so we can draw onto it
				Graphics g = Graphics.FromImage(disabledImage);

				// Draw the faded image onto the bitmap
				ControlPaint.DrawImageDisabled(g, _image, 0, 0, backColor);

				// Finished with the graphics class
				g.Dispose();

				// Cache back color used to create image
				_disabledBackColor = backColor;

				// Cache new image
				_disabledImage = disabledImage;
			}

			return _disabledImage;
		}
		
		/// <summary>
		/// Gets a white version of the original image.
		/// </summary>
		public virtual Image WhiteImage
		{
			get
			{
				if (_whiteImage == null)
				{
					// Create a new bitmap based on the existing one
					Bitmap whiteImage = new Bitmap(_image);

					// Cache image dimensions
					Size imageSize = ImageSize;

					// Replace all the non-transparent pixels with a disabled color
					for(int x = 0; x < imageSize.Width; x++)
						for(int y = 0; y < imageSize.Height; y++)
						{
							// Grab the color at x,y position
							Color pixel = whiteImage.GetPixel(x, y);

							// Only change opaque colors
							if (pixel != _transparent)
							{
								// Convert white to be transparent, otherwise disabled
								if ((pixel.R == 255) && (pixel.G == 255) && (pixel.B == 255) && (pixel.A == 255))
									whiteImage.SetPixel(x, y, _transparent);
								else
									whiteImage.SetPixel(x, y, Color.White);
							}
						}

					// Cache new image
					_whiteImage = whiteImage;
				}


				return _whiteImage;
			}
		}

		/// <summary>
		/// Gets a disabled version of the original image.
		/// </summary>
		public virtual Image FadedImage
		{
			get
			{
				if (_fadedImage == null)
				{
					// Create a new bitmap based on the existing one
					Bitmap fadedImage = new Bitmap(_image);

					// Cache image dimensions
					Size imageSize = ImageSize;

					// Replace all the non-transparent pixels with a disabled color
					for(int x = 0; x < imageSize.Width; x++)
						for(int y = 0; y < imageSize.Height; y++)
						{			
							// Get pixel raw color
							Color pixel = fadedImage.GetPixel(x, y);

							// Not interested in changing transparent pixels
							if (pixel != _transparent) 
							{
								// Calculate a slightly washed out color
								Color newPixel = Color.FromArgb((pixel.R + 76) - (((pixel.R + 32) / 64) * 19), 			
																(pixel.G + 76) - (((pixel.G + 32) / 64) * 19), 
																(pixel.B + 76) - (((pixel.B + 32) / 64) * 19));
											
								// Use new pixel color instead
								fadedImage.SetPixel(x, y, newPixel);
							}
						}

					// Cache new image
					_fadedImage = fadedImage;
				}


				return _fadedImage;
			}
		}

		/// <summary>
		/// Get the size of the image.
		/// </summary>
		public virtual Size ImageSize
		{
			get
			{
				if (Image == null)
					return Size.Empty;
				else
					return Image.Size;
			}
		}

		/// <summary>
		/// Gets the space needed to draw the image in different styles.
		/// </summary>
		/// <param name="style">Visual style required.</param>
		/// <returns>Space image needs.</returns>
		public virtual Size ImageSpace(VisualStyle style)
		{
			// Calculate the raw space of the image
			Size size = ImageSize;

			// Add extra spacing needed for when drawing in different states
			switch(style)
			{
				case VisualStyle.Office2003:
				case VisualStyle.IDE2005:
					size.Width += EXTRA_OFFICE2003_WIDTH;
					size.Height += EXTRA_OFFICE2003_HEIGHT;
					break;
				case VisualStyle.Plain:
				default:
					size.Width += EXTRA_PLAIN_WIDTH;
					size.Height += EXTRA_PLAIN_HEIGHT;
					break;
			}
			
			return size;
		}
		
		/// <summary>
		/// Create a gray scale version of provided image.
		/// </summary>
		/// <param name="source">Original image.</param>
		/// <returns>Gray scale version.</returns>
		public static Image CreateGrayScaleImage(Image source)
		{
			// Create a new bitmap based on the existing one
			Bitmap grayImage = new Bitmap(source);

			// Cache image dimensions
			Size imageSize = source.Size;

			// Replace all the non-transparent pixels with a disabled color
			for(int x = 0; x < grayImage.Width; x++)
			{
				for(int y = 0; y < grayImage.Height; y++)
				{
					// Grab the color at x,y position
					Color pixel = grayImage.GetPixel(x, y);

					// Calculate gray scale intensity from colors
					int intensity = (int)(((pixel.R) * 0.3f + 
										   (pixel.G) * 0.59f +
										   (pixel.B) * 0.11f) * 1.5f);

					// Range check
					if (intensity > 255) intensity = 255;

					// Put pixel back again
					grayImage.SetPixel(x, y, Color.FromArgb(pixel.A, intensity, intensity, intensity));
				}
			}
				
			return grayImage;
		}
	}
}
