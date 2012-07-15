
#ifndef HEADER_GUARD_OSGFFMPEG_FFMPEG_IMAGE_STREAM_H
#define HEADER_GUARD_OSGFFMPEG_FFMPEG_IMAGE_STREAM_H

#include <osg/ImageStream>

#include <OpenThreads/Condition>
#include <OpenThreads/Thread>

#include "FFmpegDecoder.hpp"
#include "MessageQueue.hpp"

namespace osgFFmpeg
{

    template <class T>
    class MessageQueue;

    class FFmpegImageStream : public osg::ImageStream, public OpenThreads::Thread
    {
    public:

        FFmpegImageStream();
        FFmpegImageStream(const FFmpegImageStream & image, const osg::CopyOp & copyop = osg::CopyOp::SHALLOW_COPY);

        META_Object(osgFFmpeg, FFmpegImageStream);

        bool open(const std::string & filename);

        virtual void play();
        virtual void pause();
        virtual void rewind();
        virtual void seek(double time);
        virtual void quit(bool waitForThreadToExit = true);

        virtual void setVolume(float volume);
        virtual float getVolume() const;

        virtual double getLength() const;
        virtual double getReferenceTime () const; 
        virtual double getFrameRate() const;

        virtual bool isImageTranslucent() const;

    private:

        enum Command
        {
            CMD_PLAY,
            CMD_PAUSE,
            CMD_STOP,
            CMD_REWIND,
            CMD_SEEK
        };

        typedef MessageQueue<Command> CommandQueue;
        typedef OpenThreads::Mutex Mutex;
        typedef OpenThreads::Condition Condition;

        virtual ~FFmpegImageStream();
        virtual void run();
        virtual void applyLoopingMode();

        bool handleCommand(Command cmd);

        void cmdPlay();
        void cmdPause();
        void cmdRewind();
        void cmdSeek(double time);

        static void publishNewFrame(const FFmpegDecoderVideo &, void * user_data);

        osg::ref_ptr<FFmpegDecoder>    m_decoder;
        CommandQueue *    m_commands;

        Mutex            m_mutex;
        Condition        m_frame_published_cond;
        bool             m_frame_published_flag;
        double           m_seek_time;
    };

}



#endif // HEADER_GUARD_OSGFFMPEG_FFMPEG_IMAGE_STREAM_H
