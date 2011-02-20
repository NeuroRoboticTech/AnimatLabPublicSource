
#if !defined( _AVI_H )
#define _AVI_H


class STD_UTILS_PORT CStdAvi {
  public:
    // Avi - use constructor to start the creation of the avi file.
    // The period is the number of ms between each bitmap frame.
    // The waveformat can be null if you're not going to add any audio,
    // or if you're going to add audio from a file.
    CStdAvi(std::string const& file_name, int frameperiod, const WAVEFORMATEX* wfx);
   ~CStdAvi();

    // add_frame - adds this bitmap to the AVI file. hbm must point be a DIBSection.
    // It is the callers responsibility to free the hbm.
    // AddAviAudio - adds this junk of audio. The format of audio was as specified in the
    // wfx parameter to CreateAVI. This fails if NULL was given.
    // Both return S_OK if okay, otherwise one of the AVI errors.
    HRESULT add_frame(CStdBitmap const&);

    HRESULT add_audio(void* dat, unsigned long numbytes);

    // compression - allows compression of the video. If compression is desired,
    // then this function must have been called before any bitmap frames had been added.
    // The bitmap hbm must be a DIBSection (so that Avi knows what format/size you're giving it),
    // but won't actually be added to the movie.
    // This function can display a dialog box to let the user choose compression. In this case,
    // set ShowDialog to true and specify the parent window. If opts is non-NULL and its
    // dwFlags property includes AVICOMPRESSF_VALID, then opts will be used to give initial
    // values to the dialog. If opts is non-NULL then the chosen options will be placed in it.
    // This function can also be used to choose a compression without a dialog box. In this
    // case, set ShowDialog to false, and hparent is ignored, and the compression specified
    // in 'opts' is used, and there's no need to call GotAviVideoCompression afterwards.
    HRESULT compression(CStdBitmap const&, AVICOMPRESSOPTIONS *opts, bool ShowDialog, HWND hparent);

    // add_wav - a convenient way to add an entire wave file to the Avi.
    // The wav file may be in in memory (in which case flags=SND_MEMORY)
    // or a file on disk (in which case flags=SND_FILENAME).
    // This function requires that either a null WAVEFORMATEX was passed to CreateAvi,
    // or that the wave file now being added has the same format as was
    // added earlier.
    HRESULT add_wav(const char* src, DWORD flags);

	HRESULT AddWindowFrame(HWND hWnd, BOOL bSaveImage, string strFile);

	AVICOMPRESSOPTIONS m_aviOpts;
	BOOL m_bFirstFrame;

  private:
    HANDLE* avi_;
};

unsigned int Std_FormatAviMessage(HRESULT code, char *buf,unsigned int len);
// FormatAviMessage - given an error code, formats it as a string.
// It returns the length of the error message. If buf/len points
// to a real buffer, then it also writes as much as possible into there.


#endif  // #if !defined( _ZLIBENGN_H )
