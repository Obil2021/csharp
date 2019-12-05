using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanJing.VOD.TencentResult
{
    public class SourceInfo
    {
        public string SourceType { get; set; }
        public string SourceContext { get; set; }
    }

    public class BasicInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime ExpireTime { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public string ClassPath { get; set; }
        public string CoverUrl { get; set; }
        public string Type { get; set; }
        public string MediaUrl { get; set; }
        public List<object> TagSet { get; set; }
        public SourceInfo SourceInfo { get; set; }
        public string StorageRegion { get; set; }
    }

    public class MediaInfoSet
    {
        public string FileId { get; set; }
        public BasicInfo BasicInfo { get; set; }
        public object MetaData { get; set; }
        public object TranscodeInfo { get; set; }
        public object AdaptiveDynamicStreamingInfo { get; set; }
        public object AnimatedGraphicsInfo { get; set; }
        public object SampleSnapshotInfo { get; set; }
        public object ImageSpriteInfo { get; set; }
        public object SnapshotByTimeOffsetInfo { get; set; }
        public object KeyFrameDescInfo { get; set; }
    }

    public class SelectBasicInfoResponse
    {
        public string RequestId { get; set; }
        public List<MediaInfoSet> MediaInfoSet { get; set; }
        public List<string> NotExistFileIdSet { get; set; }
    }

    public class SelectBasicInfoResult
    {
        public SelectBasicInfoResponse Response { get; set; }
    }
}
