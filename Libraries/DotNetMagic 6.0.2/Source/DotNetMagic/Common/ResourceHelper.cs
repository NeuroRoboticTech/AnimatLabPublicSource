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
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Crownwood.DotNetMagic.Common
{
	/// <summary>
	/// Collection of static helper methods for loading embedded resources.
	/// </summary>
    public sealed class ResourceHelper
    {
		// Prevent instance from being created.
		private ResourceHelper() {}
    
		/// <summary>
		/// Load the named cursor from assembly.
		/// </summary>
		/// <param name="assemblyType">Type that resides in required assembly.</param>
		/// <param name="cursorName">Name of cursor resource in the assembly.</param>
		/// <returns>New Cursor instance.</returns>
        public static Cursor LoadCursor(Type assemblyType, string cursorName)
        {
            // Get the assembly that contains the bitmap resource
            Assembly myAssembly = Assembly.GetAssembly(assemblyType);

            // Get the resource stream containing the images
            Stream cursorStream = myAssembly.GetManifestResourceStream(cursorName);

            // Load the Icon from the stream
            Cursor cursor = new Cursor(cursorStream);
            
            // Must remember to close down the stream
            cursorStream.Close();
            
            return cursor;
        }
    
		/// <summary>
		/// Load the named icon from assembly.
		/// </summary>
		/// <param name="assemblyType">Type that resides in required assembly.</param>
		/// <param name="iconName">Name of icon resource in the assembly.</param>
		/// <returns>New Icon instance.</returns>
        public static Icon LoadIcon(Type assemblyType, string iconName)
        {
            // Get the assembly that contains the bitmap resource
            Assembly myAssembly = Assembly.GetAssembly(assemblyType);

            // Get the resource stream containing the images
            Stream iconStream = myAssembly.GetManifestResourceStream(iconName);

            // Load the Icon from the stream
            Icon icon = new Icon(iconStream);
            
			// Must remember to close down the stream
			iconStream.Close();
			
			return icon;
        }

		/// <summary>
		/// Load the named icon from assembly.
		/// </summary>
		/// <param name="assemblyType">Type that resides in required assembly.</param>
		/// <param name="iconName">Name of icon resource in the assembly.</param>
		/// <param name="iconSize">Which icon size is required from those available.</param>
		/// <returns>New Icon instance.</returns>
        public static Icon LoadIcon(Type assemblyType, string iconName, Size iconSize)
        {
            // Load the entire Icon requested (may include several different Icon sizes)
            Icon rawIcon = LoadIcon(assemblyType, iconName);
			
            // Create and return a new Icon that only contains the requested size
            return new Icon(rawIcon, iconSize); 
        }

		/// <summary>
		/// Load the named bitmap from assembly.
		/// </summary>
		/// <param name="assemblyType">Type that resides in required assembly.</param>
		/// <param name="imageName">Name of bitmap resource in the assembly.</param>
		/// <returns>New Bitmap instance.</returns>
        public static Bitmap LoadBitmap(Type assemblyType, string imageName)
        {
            return LoadBitmap(assemblyType, imageName, false, new Point(0,0));
        }

		/// <summary>
		/// Load the named bitmap from assembly.
		/// </summary>
		/// <param name="assemblyType">Type that resides in required assembly.</param>
		/// <param name="imageName">Name of bitmap resource in the assembly.</param>
		/// <param name="transparentPixel">Pixel to use for defining transparent color.</param>
		/// <returns>New Bitmap instance.</returns>
        public static Bitmap LoadBitmap(Type assemblyType, string imageName, Point transparentPixel)
        {
            return LoadBitmap(assemblyType, imageName, true, transparentPixel);
        }

		/// <summary>
		/// Load the named bitmap and create image strip from it.
		/// </summary>
		/// <param name="assemblyType">Type that resides in required assembly.</param>
		/// <param name="imageName">Name of bitmap resource in the assembly.</param>
		/// <param name="imageSize">Size of each individual image in strip.</param>
		/// <returns>New ImageList instance.</returns>
        public static ImageList LoadBitmapStrip(Type assemblyType, string imageName, Size imageSize)
        {
            return LoadBitmapStrip(assemblyType, imageName, imageSize, false, new Point(0,0));
        }

		/// <summary>
		/// Load the named bitmap and create image strip from it.
		/// </summary>
		/// <param name="assemblyType">Type that resides in required assembly.</param>
		/// <param name="imageName">Name of bitmap resource in the assembly.</param>
		/// <param name="imageSize">Size of each individual image in strip.</param>
		/// <param name="transparentPixel">Pixel to use for defining transparent color.</param>
		/// <returns>New ImageList instance.</returns>
        public static ImageList LoadBitmapStrip(Type assemblyType, 
                                                string imageName, 
                                                Size imageSize,
                                                Point transparentPixel)
        {
            return LoadBitmapStrip(assemblyType, imageName, imageSize, true, transparentPixel);
        }

        private static Bitmap LoadBitmap(Type assemblyType, 
                                         string imageName, 
                                         bool makeTransparent, 
                                         Point transparentPixel)
        {
            // Get the assembly that contains the bitmap resource
            Assembly myAssembly = Assembly.GetAssembly(assemblyType);

            // Get the resource stream containing the images
            Stream imageStream = myAssembly.GetManifestResourceStream(imageName);

            // Load the bitmap from stream
            Bitmap image = new Bitmap(imageStream, true);

            if (makeTransparent)
            {
                Color backColor = image.GetPixel(transparentPixel.X, transparentPixel.Y);
    
                // Make backColor transparent for Bitmap
                image.MakeTransparent(backColor);
            }
            
			// Must remember to close down the stream
			imageStream.Close();
			    
            return image;
        }

        private static ImageList LoadBitmapStrip(Type assemblyType, 
                                                 string imageName, 
                                                 Size imageSize,
                                                 bool makeTransparent,
                                                 Point transparentPixel)
        {
            // Create storage for bitmap strip
            ImageList images = new ImageList();

            // Define the size of images we supply
            images.ImageSize = imageSize;

            // Get the assembly that contains the bitmap resource
            Assembly myAssembly = Assembly.GetAssembly(assemblyType);

            // Get the resource stream containing the images
            Stream imageStream = myAssembly.GetManifestResourceStream(imageName);

            // Load the bitmap strip from resource
            Bitmap pics = new Bitmap(imageStream, true);

            if (makeTransparent)
            {
                Color backColor = pics.GetPixel(transparentPixel.X, transparentPixel.Y);
    
                // Make backColor transparent for Bitmap
                pics.MakeTransparent(backColor);
            }
			    
            // Load them all !
            images.Images.AddStrip(pics);

			// Must remember to close down the stream
			imageStream.Close();

            return images;
        }
    }
}
