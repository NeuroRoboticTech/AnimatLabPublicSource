/**
\file	StdBitmap.h

\brief	Declares the standard bitmap class.
**/

/* 
   Bitmap.h

   Copyright (C) 2002-2005 René Nyffenegger

   This source code is provided 'as-is', without any express or implied
   warranty. In no event will the author be held liable for any damages
   arising from the use of this software.

   Permission is granted to anyone to use this software for any purpose,
   including commercial applications, and to alter it and redistribute it
   freely, subject to the following restrictions:

   1. The origin of this source code must not be misrepresented; you must not
      claim that you wrote the original source code. If you use this source code
      in a product, an acknowledgment in the product documentation would be
      appreciated but is not required.

   2. Altered source versions must be plainly marked as such, and must not be
      misrepresented as being the original source code.

   3. This notice may not be removed or altered from any source distribution.

   René Nyffenegger rene.nyffenegger@adp-gmbh.ch
*/
namespace StdUtils
{

#if !defined( _BITMAP_H )
#define _BITMAP_H

/**
\brief	Standard bitmap. 

\author	dcofer
\date	5/4/2011
**/
class STD_UTILS_PORT CStdBitmap {
  public:

    /**
    \brief	Default constructor.
    
    \author	dcofer
    \date	5/4/2011
    **/
    CStdBitmap();

    /**
    \brief	Constructor.
    
    \author	dcofer
    \date	5/4/2011
    
    \param	file_name	Filename of the file. 
    **/
    CStdBitmap(std::string const& file_name);

	/**
	\brief	Constructor.
	
	\author	dcofer
	\date	5/4/2011
	
	\param	bmp	Handle of the bitmap. 
	**/
	CStdBitmap(HBITMAP bmp);

    /**
    \brief	HBITMAP casting operator.
    
    \author	dcofer
    \date	5/4/2011
    **/
    operator HBITMAP() const;

  protected:
    /// The memory device-context
    friend class MemoryDC;

    /// Handle of the bitmap
    HBITMAP bitmap_;
};

}				//StdUtils

#endif  // #if !defined( _ZLIBENGN_H )
