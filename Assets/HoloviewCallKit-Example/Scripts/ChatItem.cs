using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Holoview.Chat
{
    public class ChatItem : MonoBehaviour
    {

        public struct VideoData
        {
            public byte[] data;
            public int With;
            public int Height;
        }
        public struct VideoDataYuv
        {
            public int With;
            public int Height;
            public byte[] ydata;
            public int ystride;
            public byte[] udata;
            public int ustride;
            public byte[] vdata;
            public int vstride;
        }
        public long UserID;
        public Texture2D VideoTexture;
        public Queue<VideoData> VideoQueue = new Queue<VideoData>();

        public int TexWith = 1280;
        public int TexHeight = 720;
        public MeshRenderer render;

        private AudioSource testSource;

        public Texture2D DefaultTexture;
        //public Material mat; 
        private void Start()
        {
            //render.material.mainTexture = DefaultTexture;
        }

        void FixedUpdate()
        {
            if (VideoQueue.Count > 0)
            {
                try
                {
                    VideoData vd = VideoQueue.Dequeue();
                    if (VideoTexture == null || vd.With != TexWith || vd.Height != TexHeight)
                    {
                        TexWith = vd.With;
                        TexHeight = vd.Height;
                        VideoTexture = new Texture2D(TexWith, TexHeight, TextureFormat.BGRA32, false);

                        Debug.Log("new  TexWith: " + TexWith + " TexHeight:" + TexHeight);
                    }
                    else
                    {
                        //Debug.Log("old  TexWith: " + TexWith + " TexHeight:" + TexHeight);
                    }

                    VideoTexture.LoadRawTextureData(vd.data);
                    VideoTexture.Apply();
                    render.material.mainTexture = VideoTexture;

                    vd.data = null;
                    //mat.mainTexture = VideoTexture;

                }
                catch (System.Exception e)
                {
                    print("视频渲染出错:" + e.Message);
                }

            }



        }

        void Update()
        {

        }

    }
}

