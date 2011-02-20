<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<!-- saved from url=(0074)http://www.math.keio.ac.jp/~nisimura/random/old/19991028/real1/mt19937-1.c -->
<HTML><HEAD>
<META http-equiv=Content-Type content="text/html; charset=euc-jp">
<META content="MSHTML 6.00.2800.1106" name=GENERATOR></HEAD>
<BODY><PRE>/* A C-program for MT19937: Real number version  (1998/4/6)    */
/*   genrand() generates one pseudorandom real number (double) */
/* which is uniformly distributed on [0,1]-interval, for each  */
/* call. sgenrand(seed) set initial values to the working area */
/* of 624 words. Before genrand(), sgenrand(seed) must be      */
/* called once. (seed is any 32-bit integer except for 0).     */
/* Integer generator is obtained by modifying two lines.       */
/*   Coded by Takuji Nishimura, considering the suggestions by */
/* Topher Cooper and Marc Rieffel in July-Aug. 1997.           */

/* This library is free software; you can redistribute it and/or   */
/* modify it under the terms of the GNU Library General Public     */
/* License as published by the Free Software Foundation; either    */
/* version 2 of the License, or (at your option) any later         */
/* version.                                                        */
/* This library is distributed in the hope that it will be useful, */
/* but WITHOUT ANY WARRANTY; without even the implied warranty of  */
/* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.            */
/* See the GNU Library General Public License for more details.    */
/* You should have received a copy of the GNU Library General      */
/* Public License along with this library; if not, write to the    */
/* Free Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA   */ 
/* 02111-1307  USA                                                 */

/* Copyright (C) 1997 Makoto Matsumoto and Takuji Nishimura.       */
/* When you use this, send an email to: matumoto@math.keio.ac.jp   */
/* with an appropriate reference to your work.                     */

/* REFERENCE                                                       */
/* M. Matsumoto and T. Nishimura,                                  */
/* "Mersenne Twister: A 623-Dimensionally Equidistributed Uniform  */
/* Pseudo-Random Number Generator",                                */
/* ACM Transactions on Modeling and Computer Simulation,           */
/* Vol. 8, No. 1, January 1998, pp 3--30.                          */

#include&lt;stdio.h&gt;

/* Period parameters */  
#define N 624
#define M 397
#define MATRIX_A 0x9908b0df   /* constant vector a */
#define UPPER_MASK 0x80000000 /* most significant w-r bits */
#define LOWER_MASK 0x7fffffff /* least significant r bits */

/* Tempering parameters */   
#define TEMPERING_MASK_B 0x9d2c5680
#define TEMPERING_MASK_C 0xefc60000
#define TEMPERING_SHIFT_U(y)  (y &gt;&gt; 11)
#define TEMPERING_SHIFT_S(y)  (y &lt;&lt; 7)
#define TEMPERING_SHIFT_T(y)  (y &lt;&lt; 15)
#define TEMPERING_SHIFT_L(y)  (y &gt;&gt; 18)

static unsigned long mt[N]; /* the array for the state vector  */
static int mti=N+1; /* mti==N+1 means mt[N] is not initialized */

/* initializing the array with a NONZERO seed */
void
sgenrand(seed)
    unsigned long seed;	
{
    /* setting initial seeds to mt[N] using         */
    /* the generator Line 25 of Table 1 in          */
    /* [KNUTH 1981, The Art of Computer Programming */
    /*    Vol. 2 (2nd Ed.), pp102]                  */
    mt[0]= seed &amp; 0xffffffff;
    for (mti=1; mti&lt;N; mti++)
        mt[mti] = (69069 * mt[mti-1]) &amp; 0xffffffff;
}

double /* generating reals */
/* unsigned long */ /* for integer generation */
genrand()
{
    unsigned long y;
    static unsigned long mag01[2]={0x0, MATRIX_A};
    /* mag01[x] = x * MATRIX_A  for x=0,1 */

    if (mti &gt;= N) { /* generate N words at one time */
        int kk;

        if (mti == N+1)   /* if sgenrand() has not been called, */
            sgenrand(4357); /* a default initial seed is used   */

        for (kk=0;kk&lt;N-M;kk++) {
            y = (mt[kk]&amp;UPPER_MASK)|(mt[kk+1]&amp;LOWER_MASK);
            mt[kk] = mt[kk+M] ^ (y &gt;&gt; 1) ^ mag01[y &amp; 0x1];
        }
        for (;kk&lt;N-1;kk++) {
            y = (mt[kk]&amp;UPPER_MASK)|(mt[kk+1]&amp;LOWER_MASK);
            mt[kk] = mt[kk+(M-N)] ^ (y &gt;&gt; 1) ^ mag01[y &amp; 0x1];
        }
        y = (mt[N-1]&amp;UPPER_MASK)|(mt[0]&amp;LOWER_MASK);
        mt[N-1] = mt[M-1] ^ (y &gt;&gt; 1) ^ mag01[y &amp; 0x1];

        mti = 0;
    }
  
    y = mt[mti++];
    y ^= TEMPERING_SHIFT_U(y);
    y ^= TEMPERING_SHIFT_S(y) &amp; TEMPERING_MASK_B;
    y ^= TEMPERING_SHIFT_T(y) &amp; TEMPERING_MASK_C;
    y ^= TEMPERING_SHIFT_L(y);

    return ( (double)y * 2.3283064370807974e-10 ); /* reals */
    /* return y; */ /* for integer generation */
}

/* this main() outputs first 1000 generated numbers  */
main()
{ 
    int j;

    sgenrand(4357); /* any nonzero integer can be used as a seed */
    for (j=0; j&lt;1000; j++) {
        printf("%10.8f ", genrand());
        if (j%8==7) printf("\n");
    }
    printf("\n");
}
</PRE></BODY></HTML>
