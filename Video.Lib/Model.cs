using System;

namespace Video.Lib
{
    public class TPointsModel
    {
        public string Name { get; set; }
        public int TagID { get; set; }
        public string TagName { get; set; }
        public int ParentIdx { get; set; }
        public string Type { get; set; }
    }

    public class TNodesModel
    {
        public int Idx { get; set; }
        public int ParentIdx { get; set; }
        public int TagID { get; set; }
        public string TagName { get; set; }
        public string Name { get; set; } 
        public string Type { get; set; }
        public string WDName { get; set; }
    }

    public class TSubSysNodes
    {
        public int Idx { get; set; }
        public int ParentIdx { get; set; }
        public string Name { get; set; }
        public string WDName { get; set; }
    }

    public class TPointClassGroup
    {
        public int Idx { get; set; }
        public string Name { get; set; }
        public int SysNodeIdx { get; set; }
    }

    public class GetEncodersResponse
    {
        /// <summary>编码器ID</summary>	
        public Guid EncoderGUID { get; set; }

        /// <summary>名称</summary>
        public string Name { get; set; }

        /// <summary>数据中心名称</summary>
        public string DBHostName { get; set; }
    }

    public class GetVideoByEncodeGUIDResponse
    {
        /// <summary>视频ID</summary>	
        public Guid VideoGUID { get; set; }

        /// <summary>名称</summary>
        public string Name { get; set; }
    }
}
