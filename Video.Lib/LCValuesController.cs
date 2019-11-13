//using HG.Infrastructure.Common;
using System.Collections.Generic;
//using Microsoft.Owin;
//using Owin;
using System.Web.Http;

namespace Video.Lib
{
    public class LCValuesController : ApiController
    {
        [HttpGet]
        public IEnumerable<TPointsModel> GetPoints()
        {
            using (var query = new DapperQuery(Enums.Core))
            {
                var sql = "SELECT p.Name,TagID,TagName,NodeIdx ParentIdx,e.Name [Type] FROM TPoints p left join EnumPointType e on p.TypeIdx = e.Idx";

                return query.QueryList<TPointsModel>(sql, null);
            }
        }

        [HttpGet]
        public IEnumerable<TPointsModel> GetPointsByNodeIdx(int id)
        {
            using (var query = new DapperQuery(Enums.Core))
            {
                var sql = @"SELECT p.Name,TagID,TagName,NodeIdx ParentIdx,e.Name [Type] FROM TPoints p left join EnumPointType e on p.TypeIdx = e.Idx
                            WHERE NodeIdx = @ParentIdx";

                return query.QueryList<TPointsModel>(sql, new { ParentIdx = id });
            }
        }

        [HttpGet]
        public IEnumerable<TNodesModel> GetNodes()
        {
            using (var query = new DapperQuery(Enums.Core))
            {
                var sql = @"select TypeLayerIdx Idx,TNodes.ParentIdx,TNodes.TagID,TNodes.TagName,TNodes.Name,TNodes.[Type],N'界面' WDName FROM TNodes";

                return query.QueryList<TNodesModel>(sql, null);
            }
        }

        [HttpGet]
        public IEnumerable<TSubSysNodes> GetSubSysNodes()
        {
            using (var query = new DapperQuery(Enums.Core))
            {
                var sql = @" select TSubSysNodes.Idx,TSubSysNodes.Name,N'界面' WDName,ParentIdx from TSubSysNodes";

                return query.QueryList<TSubSysNodes>(sql, null);
            }
        }

        [HttpGet]
        public IEnumerable<GetEncodersResponse> GetEncoders()
        {
            using (var query = new DapperQuery(Enums.Core))
            {
                var sql = "select e.EncoderGUID,e.Name,N'界面' DBHostName from TEncoder e order by e.ShowOrder";

                return query.QueryList<GetEncodersResponse>(sql, null);
            }
        }

        [HttpGet]
        public IEnumerable<TPointClassGroup> GetGroups()
        {
            using (var query = new DapperQuery(Enums.Core))
            {
                var sql = @"select Idx,Name,SysNodeIdx FROM TPointClassGroup";

                return query.QueryList<TPointClassGroup>(sql, null);
            }
        }


        [HttpGet]
        public IEnumerable<TPointClassGroup> GetGroupsBySubSysNodesIdx(int id)
        {
            using (var query = new DapperQuery(Enums.Core))
            {
                var sql = @"select Idx,Name FROM TPointClassGroup WHERE SysNodeIdx = @SysNodeIdx";

                return query.QueryList<TPointClassGroup>(sql, new { SysNodeIdx = id });
            }
        }

        [HttpGet]
        public IEnumerable<GetVideoByEncodeGUIDResponse> GetVideoByEncodeGUID(string id)
        {
            using (var query = new DapperQuery(Enums.Core))
            {
                var sql = @"select VideoGUID,Name from TDevice_Video where Valid = 1 and EncodeGUID = @EncodeGUID order by ShowOrder";

                return query.QueryList<GetVideoByEncodeGUIDResponse>(sql, new { EncodeGUID = id });
            }
        }



    }
}
