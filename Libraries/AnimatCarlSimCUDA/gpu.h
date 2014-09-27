/*
  Copyright (c) 2007 A. Arnold and J. A. van Meel, FOM institute
  AMOLF, Amsterdam; all rights reserved unless otherwise stated.

  This program is free software; you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation; either version 2 of the License, or
  (at your option) any later version.

  In addition to the regulations of the GNU General Public License,
  publications and communications based in parts on this program or on
  parts of this program are required to cite the article
  "Harvesting graphics power for MD simulations"
  by J.A. van Meel, A. Arnold, D. Frenkel, S. F. Portegies Zwart and
  R. G. Belleman, arXiv:0709.3225.

  This program is distributed in the hope that it will be useful, but
  WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
  General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program; if not, write to the Free Software
  Foundation, Inc., 59 Temple Place, Suite 330, Boston,
  MA 02111-1307 USA
*/
#ifndef _GPU_H_
#define _GPU_H_
#include <cstdio>
#include <cstring>


//#if __CUDA3__
//    // includes, library
//    #include "cudpp/cudpp.h"
//
//    // includes, project
//    #include "cutil_inline.h"
//    #include "cutil_math.h"
//#elif __CUDA5__
    #include <helper_cuda.h>
//#endif

#include "CUDAVersionControl.h"

/// initialize the GPU
void GPU_init();

#endif
