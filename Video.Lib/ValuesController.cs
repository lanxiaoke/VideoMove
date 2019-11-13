using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Video.Lib
{
    public static class Helper
    {
        public static string ReadFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                StreamReader sr = new StreamReader(filePath, Encoding.UTF8);
                string str = sr.ReadToEnd();
                sr.Close();
                return str;
            }

            return string.Empty;
        }

        public static int ToInt(this string value, int defaultValue = 0)
        {
            int temp;
            if (Int32.TryParse(value, out temp))
            {
                return temp;
            }

            return defaultValue;
        }


        public static bool ToBool(this string value, bool defaultValue = false)
        {
            bool temp;
            if (bool.TryParse(value, out temp))
            {
                return temp;
            }

            return defaultValue;
        }
    }
    public class LoadPageReq
    {
        public int uid { get; set; }
        public int page { get; set; }
        public int size { get; set; }
    }
    public class DownloadArg
    {
        public string Body { get; set; }
    }

    public class ValuesController : ApiController
    {
        private string baseUrl = @"D:\\video\";
        private readonly static StringBuilder log = new StringBuilder();

        private static ModelMulti lastModelMulti = new ModelMulti();
        private static ModelSingle lastModelSingle = new ModelSingle();
        private static string downloadItem = string.Empty;


        [HttpPost]
        public ModelMulti LoadPage([FromBody]LoadPageReq arg)
        {
            var url = string.Format("https://space.bilibili.com/ajax/member/getSubmitVideos?mid={0}&pagesize={1}&tid=0&page={2}&keyword=&order=pubdate", arg.uid, arg.size, arg.page);
            var jsonContent = httpClient.GetWebClient(url);
            lastModelMulti = httpClient.Deserialize<ModelMulti>(jsonContent);

            return lastModelMulti;
        }

        [HttpGet]
        public ModelSingle LoadSingle(int id)
        {
            var url = string.Format(" https://api.bilibili.com/x/web-interface/view?aid={0}", id);
            var jsonContent = httpClient.GetWebClient(url);
            lastModelSingle = httpClient.Deserialize<ModelSingle>(jsonContent);

            return lastModelSingle;
        }


        [HttpPost]
        public void DownloadByChoose([FromBody]DownloadArg arg)
        {
            //httpClient.print(arg.Body);
            Task.Run(() =>
            {
                if (arg == null || arg.Body == null)
                {
                    log.AppendLine("地址数据为空.\n");
                    return;
                }

                var rows = arg.Body.Split('|');
                var count = 0;
                var rowIndex = 0;

                log.Clear();

                foreach (var row in rows)
                {
                    rowIndex++;

                    if (row == null) continue;
                    var message = row.Trim();
                    if (message.Length < 1) continue;

                    var arr = message.TrimEnd('|').Split('*');
                    var valid = arr[0].ToBool();
                    if (!valid)
                    {
                        log.AppendFormat("第{0}行数据有误.\n", rowIndex.ToString());
                        continue;
                    }

                    var index = arr[1].ToInt();
                    var img = Path.GetFileName(arr[2]);
                    var url = arr[3];

                    foreach (var item in lastModelMulti.data.vlist)
                    {
                        if (Path.GetFileName(item.pic).Equals(img, StringComparison.OrdinalIgnoreCase))
                        {
                            var name = string.Format("{0}.{1}", index.ToString(), GetTitle(item.title));

                            item.valid = valid;
                            item.url = url;

                            downloadItem = name;

                            httpClient.download(string.Format("https:{0}", item.pic), string.Format("{0}{1}.jpg", baseUrl, name));
                            httpClient.download(item.url, string.Format("{0}{1}.mp4", baseUrl, name));

                            log.AppendFormat("{0}.{1}\n", downloadItem, httpClient.ratio.ToString("0.00 %"));
                            downloadItem = string.Empty;

                            count++;
                        }
                    }
                }
            });
        }


        [HttpPost]
        public void DownloadByInput([FromBody]DownloadArg arg)
        {
            //httpClient.print(arg.Body);
            Task.Run(() =>
            {
                //var body = Helper.ReadFile(baseUrl + "urls.txt");
                //if (body == string.Empty)
                //{
                //    log.AppendFormat("警告！urls.txt内容有问题！");
                //    return;
                //}

                //GetList(arg);

                if (arg == null || arg.Body == null)
                {
                    log.AppendLine("地址数据为空.\n");
                    return;
                }

                if (lastModelMulti == null ||
                    lastModelMulti.data == null ||
                    lastModelMulti.data.vlist == null)
                {
                    log.AppendLine("lastModel数据为空.\n");
                    return;
                }

                var rows = arg.Body.Split('|');
                var count = 0;
                var rowIndex = 0;

                log.Clear();
                foreach (var row in rows)
                {
                    rowIndex++;

                    if (row == null) continue;
                    var message = row.Trim();
                    if (message.Length < 1) continue;

                    var arr = message.TrimEnd('|').Split('*');
                    var valid = arr[0].ToBool();
                    if (!valid)
                    {
                        log.AppendFormat("第{0}行数据有误.\n", rowIndex.ToString());
                        continue;
                    }

                    var index = arr[1].ToInt();
                    var img = Path.GetFileName(arr[2]);
                    var url = arr[3];

                    foreach (var item in lastModelMulti.data.vlist)
                    {
                        if (Path.GetFileName(item.pic).Equals(img, StringComparison.OrdinalIgnoreCase))
                        {
                            var name = string.Format("{0}.{1}", index.ToString(), GetTitle(item.title));

                            item.valid = valid;
                            item.url = url;

                            downloadItem = name;

                            httpClient.download(string.Format("https:{0}", item.pic), string.Format("{0}{1}.jpg", baseUrl, name));
                            httpClient.download(item.url, string.Format("{0}{1}.mp4", baseUrl, name));

                            log.AppendFormat("{0}.{1}\n", downloadItem, httpClient.ratio.ToString("0.00 %"));
                            downloadItem = string.Empty;

                            count++;
                        }
                    }
                }
            });
        }


        [HttpPost]
        public void DownloadBySingle([FromBody]DownloadArg arg)
        {
            //httpClient.print(arg.Body);
            Task.Run(() =>
            {
                if (arg == null || arg.Body == null)
                {
                    log.AppendLine("地址数据为空.\n");
                    return;
                }

                if (lastModelSingle == null ||
                    lastModelSingle.data == null)
                {
                    log.AppendLine("modelSingle数据为空.\n");
                    return;
                }

                var rows = arg.Body.Split('|');
                var count = 0;
                var rowIndex = 0;

                log.Clear();
                foreach (var row in rows)
                {
                    rowIndex++;

                    if (row == null) continue;
                    var message = row.Trim();
                    if (message.Length < 1) continue;

                    var arr = message.TrimEnd('|').Split('*');
                    var valid = arr[0].ToBool();
                    if (!valid)
                    {
                        log.AppendFormat("第{0}行数据有误.\n", rowIndex.ToString());
                        continue;
                    }

                    var index = arr[1].ToInt();
                    var img = Path.GetFileName(arr[2]);
                    var url = arr[3];

                    if (Path.GetFileName(lastModelSingle.data.pic).Equals(img, StringComparison.OrdinalIgnoreCase))
                    {
                        var name = GetTitle(lastModelSingle.data.title);

                        lastModelSingle.data.valid = valid;
                        lastModelSingle.data.url = url;

                        downloadItem = name;

                        httpClient.download(lastModelSingle.data.pic, string.Format("{0}{1}.jpg", baseUrl, name));
                        httpClient.download(lastModelSingle.data.url, string.Format("{0}{1}.mp4", baseUrl, name));

                        log.AppendFormat("{0}.{1}\n", downloadItem, httpClient.ratio.ToString("0.00 %"));
                        downloadItem = string.Empty;

                        count++;
                    }
                }
            });
        }


        [HttpGet]
        public string GetStatus()
        {
            if (downloadItem == string.Empty)
            {
                return log.ToString();
            }
            else
            {
                return string.Format("{0}{1}.{2}", log.ToString(), downloadItem, httpClient.ratio.ToString("0.00 %"));
            }
        }


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        public string Get(int id)
        {
            return "value";
        }

        public void Post([FromBody]string value)
        {

        }

        public void Put(int id, [FromBody]string value)
        {

        }

        public void Delete(int id)
        {

        }

        private string GetTitle(string title)
        {
            return title.Replace("\"", "")
                        .Replace("\\", "")
                        .Replace(">", "")
                        .Replace("<", "")
                        .Replace("|", "")
                        .Replace("*", "")
                        .Replace(":", "")
                        .Replace("?", "")
                        .Replace("/", "");
        }
    }

    public class ModelMulti
    {
        public bool status { get; set; }
        public ModelMultiData data { get; set; }
    }
    public class ModelMultiData
    {
        public List<ModelMultiDataList> vlist { get; set; }
        public int count { get; set; }
        public int pages { get; set; }
    }
    public class ModelMultiDataList
    {
        public string pic { get; set; }
        public string description { get; set; }
        public string title { get; set; }
        public int aid { get; set; }
        public string url { get; set; }
        public bool valid { get; set; }
    }

    public class ModelSingle
    {
        public ModelSingleData data { get; set; }
    }
    public class ModelSingleData
    {
        public string pic { get; set; }
        public string title { get; set; }

        public string url { get; set; }
        public bool valid { get; set; }
    }
}
