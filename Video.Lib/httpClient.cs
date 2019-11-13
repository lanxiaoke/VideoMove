//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ConsoleApp1
//{
//    class Class1
//    {
//    }
//}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using Newtonsoft.Json;

namespace Video.Lib
{
    /// <summary>
    /// 公用 Http 请求类
    /// </summary>
    public class httpClient
    {
        /// <summary>
        /// 下载文件保留字
        /// </summary>
        public static string PERSIST_EXP = ".cdel";

        public static string Serialize<T>(T obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static T Deserialize<T>(string json)
        {
            try
            {
                return (T)JsonConvert.DeserializeObject(json, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }

        public static string GetWebClient(string url)
        {
            string strHTML = "";
            WebClient myWebClient = new WebClient();
            Stream myStream = myWebClient.OpenRead(url);
            StreamReader sr = new StreamReader(myStream, System.Text.Encoding.GetEncoding("utf-8"));
            strHTML = sr.ReadToEnd();
            myStream.Close();
            return strHTML;
        }
        /// <summary>
        /// 获取基础WebRequest
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="lStartPos">请求的开始位置</param>
        /// <returns></returns>
        public static HttpWebRequest getWebRequest(string url, int lStartPos)
        {
            HttpWebRequest request = null;
            //try
            //{
            request = (System.Net.HttpWebRequest)HttpWebRequest.Create(url);
            request.AddRange(lStartPos); //设置Range值
            //}
            //catch (Exception ex)
            //{
            //    Program.print(ex.Message);
            //}

            return request;
        }


        public static void download(string url, string path)
        {
            ratio = 0;
            Console.WriteLine(url);

            if (File.Exists(path))
            {
                print("文件己存在！是否重新下载？");
                ratio = 1;
                return;
            }
            else
            {
                path = path + PERSIST_EXP;

                simpleDownload(url, path);//开始下载
            }
        }

        private static void arg(HttpWebRequest request)
        {
            //这个一定要加上，在某些网站没有会发生"远程服务器返回错误: (403) 已禁止。"错误
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; QQWubi 133; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; CIBA; InfoPath.2)";
            request.Method = "GET";
        }

        public static long currentLength = 0;
        public static long totalLength = 0;//总大小 
        public static double ratio = 0; 

        /// <summary>
        /// 下载网络资源(支持断点续传)
        /// </summary>
        /// <param name="url"></param>
        /// <param name="path"></param>
        public static void simpleDownload(string url, string path)
        {
            //print("1");
            HttpWebRequest request = httpClient.getWebRequest(url, 0);
            arg(request);

            ////url是传入的访问地址
            ////HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(url);
            ////这个一定要加上，在某些网站没有会发生"远程服务器返回错误: (403) 已禁止。"错误
            //request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; QQWubi 133; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; CIBA; InfoPath.2)";
            //request.Method = "GET";

            //print("2");
            WebResponse response = null;

            FileStream writer = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            long lStartPos = writer.Length; ;//当前文件大小
            currentLength = 0;
            totalLength = 0;//总大小 
                            //print("3");
                            //if (File.Exists(path))//断点续传
                            //{
                            //    response = request.GetResponse();
                            //    long sTotal = response.ContentLength;
                            //    print("3.1");
                            //    if (sTotal == lStartPos)
                            //    {
                            //        print("3.2");
                            //        close(writer);
                            //        File.Move(path, path.Replace(PERSIST_EXP, ""));
                            //        print("下载完成!");
                            //        return;

            //    }

            //    print("3.3");
            //    request = httpClient.getWebRequest(url, (int)lStartPos);//设置Range值
            //    arg(request);
            //    print("3.4");
            //    writer.Seek(lStartPos, SeekOrigin.Begin);//指针跳转
            //    response = request.GetResponse();
            //    print("3.5");
            //    totalLength = response.ContentLength + lStartPos; //总长度
            //    currentLength = lStartPos; //当前长度
            //    print("4");
            //}
            //else
            //{
            response = request.GetResponse();
            totalLength = response.ContentLength;
            //}
            //print("5");
            Stream reader = response.GetResponseStream();

            byte[] buff = new byte[1024];
            int c = 0; //实际读取的字节数
            //print("1!");
            while ((c = reader.Read(buff, 0, buff.Length)) > 0)
            {
                currentLength += c;
                writer.Write(buff, 0, c);
                progressBar(currentLength, totalLength);//进度条
                writer.Flush();
            }
            //print("2!");
            close(writer);
            if (currentLength == totalLength)
            {
                File.Move(path, path.Replace(PERSIST_EXP, ""));
                print("下载完成!");
            }

            if (reader != null)
            {
                reader.Close();
                reader.Dispose();
                response.Close();
            }
        }
        private static void close(FileStream writer)
        {
            if (writer != null)
            {
                writer.Close();
                writer.Dispose();
            }
        }
        /// <summary>
        /// 进度条
        /// </summary>
        /// <param name="currentLength">当前长度</param>
        /// <param name="totalLength">总长度</param>
        public static void progressBar(Object currentLength, Object totalLength)
        {
            double aaa = System.Convert.ToDouble(currentLength);
            double bbb = System.Convert.ToDouble(totalLength);
            ratio = (aaa / bbb);

            print(currentLength + "/" + totalLength + "__" + ratio.ToString("0.00 %"));
        }
        /// <summary>
        /// 系统输出
        /// </summary>
        /// <param name="obj"></param>
        public static void print(Object obj)
        {
            Console.WriteLine(obj);
        }
        public static void printStr(string[] str)
        {
            foreach (string d in str)
            {
                print(d);
            }
        }
        /// <summary>
        /// 文件写
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="content">要写入的内容</param>
        public static void fileWriter(string path, string content, Encoding encoding)
        {
            if (File.Exists(path))
            {
                StreamWriter sw = new StreamWriter(path, true, encoding);

                sw.WriteLine(content);

                sw.Flush();
                sw.Close();
            }
        }
        /// <summary>
        /// 读文件，返回内容
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="enCoding">默认编码格式</param>
        /// <returns></returns>
        public static string fileReader(string path, Encoding enCoding)
        {
            StringBuilder sb = new StringBuilder();
            if (enCoding == null)
            {
                enCoding = Encoding.Default;
            }
            //读取文件
            StreamReader sr = new StreamReader(path, enCoding);
            string s = "";
            while ((s = sr.ReadLine()) != null)
            {
                sb.AppendLine(s);
            }
            if (sr != null)
                sr.Close();

            return sb.ToString();
        }
        /// <summary>
        /// 获取文件编码格式
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="defaultEncoding">默认编码</param>
        /// <returns></returns>
        public static string getFileEncoding(string path, string defaultEncoding)
        {
            string ed = defaultEncoding;
            if (File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                ed = GetEncoding(fs, defaultEncoding);
                if (fs != null)
                    fs.Close();
            }
            return ed;
        }
        /// <summary>
        /// 取得一个文本文件流的编码方式。
        /// </summary>
        /// <param name="stream">文本文件流。</param>
        /// <param name="defaultEncoding">默认编码方式。当该方法无法从文件的头部取得有效的前导符时，将返回该编码方式。</param>
        /// <returns></returns>
        public static string GetEncoding(FileStream stream, string defaultEncoding)
        {
            string targetEncoding = defaultEncoding;
            if (stream != null && stream.Length >= 2)
            {
                //保存文件流的前4个字节
                byte byte1 = 0;
                byte byte2 = 0;
                byte byte3 = 0;
                byte byte4 = 0;
                //保存当前Seek位置
                long origPos = stream.Seek(0, SeekOrigin.Begin);
                stream.Seek(0, SeekOrigin.Begin);

                int nByte = stream.ReadByte();
                byte1 = Convert.ToByte(nByte);
                byte2 = Convert.ToByte(stream.ReadByte());
                if (stream.Length >= 3)
                {
                    byte3 = Convert.ToByte(stream.ReadByte());
                }
                if (stream.Length >= 4)
                {
                    byte4 = Convert.ToByte(stream.ReadByte());
                }
                //根据文件流的前4个字节判断Encoding
                //Unicode {0xFF, 0xFE};
                //BE-Unicode {0xFE, 0xFF};
                //UTF8 = {0xEF, 0xBB, 0xBF};
                if (byte1 == 0xFE && byte2 == 0xFF)//UnicodeBe
                {
                    targetEncoding = Encoding.BigEndianUnicode.BodyName;
                }
                if (byte1 == 0xFF && byte2 == 0xFE && byte3 != 0xFF)//Unicode
                {
                    targetEncoding = Encoding.Unicode.BodyName;
                }
                if (byte1 == 0xEF && byte2 == 0xBB && byte3 == 0xBF)//UTF8
                {
                    targetEncoding = Encoding.UTF8.BodyName;
                }
                //恢复Seek位置 
                stream.Seek(origPos, SeekOrigin.Begin);
            }
            return targetEncoding;
        }

    }
}
//该代码片段来自于: http://www.sharejs.com/codes/csharp/5646