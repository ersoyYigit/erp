using ArdaManager.Application.Extensions;
using ArdaManager.Application.Interfaces.Services;
using ArdaManager.Application.Requests;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using WinSCP;

namespace ArdaManager.Infrastructure.Services
{
    public class ArdaUploadService : IUploadService
    {


        private string ftpAddress;
        private string ftpUserName;
        private string ftpPassword;
        private string ftpSourcePath;
        private string rootPath;
        private int ftpPort;



        public ArdaUploadService(IConfiguration configuration)
        {
            ftpAddress = configuration["Storage:ArdaFtp:FtpUrl"];
            ftpUserName = configuration["Storage:ArdaFtp:FtpUser"];
            ftpPassword = configuration["Storage:ArdaFtp:FtpPassword"];
            ftpSourcePath = configuration["Storage:ArdaFtp:SourcePath"];
            rootPath = configuration["Storage:FileStream:Path"];


            if (!int.TryParse(configuration["Storage:ArdaFtp:Port"], out ftpPort))
                ftpPort = 13501;



        }


        public async Task<string> UploadAsync(UploadRequest request)
        {
            if(request.Data == null) return await Task.Run(() => string.Empty);
            var streamData = new MemoryStream(request.Data);
            var fileName = request.FileName.Trim('"');

            if (streamData.Length > 0)
            {

                var itemTypeFolder = request.UploadType.ToDescriptionStringVapp();
                var dbValue = Path.Combine(@"Files", itemTypeFolder, fileName);
                var finalPath = Path.Combine(ftpSourcePath, dbValue);

                try
                {
                    SessionOptions sessionOptions = new SessionOptions
                    {
                        Protocol = Protocol.Ftp,
                        HostName = ftpAddress,
                        UserName = ftpUserName,
                        Password = ftpPassword,
                        PortNumber = ftpPort,
                        FtpSecure = FtpSecure.Implicit,
                    };

                    using (Session session = new Session())
                    {
                        session.Open(sessionOptions);
                        session.PutFile(streamData, finalPath);
                    }
                    return dbValue;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: {0}", e);
                    return string.Empty;
                }
                
            }
            else
            {
                return string.Empty;
            }


        }
        public string Upload(UploadRequest request)
        {

            if (request.Data == null) return string.Empty;
            var streamData = new MemoryStream(request.Data);
            var fileName = request.FileName.Trim('"');

            if (streamData.Length > 0)
            {

                var itemTypeFolder = request.UploadType.ToDescriptionStringVapp();
                var finalPath = Path.Combine(@"", itemTypeFolder, fileName);
                var dbValue = Path.Combine(ftpSourcePath, finalPath);

                try
                {
                    SessionOptions sessionOptions = new SessionOptions
                    {
                        Protocol = Protocol.Ftp,
                        HostName = ftpAddress,
                        UserName = ftpUserName,
                        Password = ftpPassword,
                        PortNumber = ftpPort,
                        //FtpSecure = FtpSecure.None,
                    };

                    using (Session session = new Session())
                    {
                        session.Open(sessionOptions);
                        session.PutFile(streamData, finalPath);
                    }
                    return dbValue;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: {0}", e);
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }

        }

        private static string numberPattern = " ({0})";

        public static string NextAvailableFilename(string path)
        {
            // Short-cut if already available
            if (!File.Exists(path))
                return path;

            // If path has extension then insert the number pattern just before the extension and return next filename
            if (Path.HasExtension(path))
                return GetNextFilename(path.Insert(path.LastIndexOf(Path.GetExtension(path)), numberPattern));

            // Otherwise just append the pattern to the path and return next filename
            return GetNextFilename(path + numberPattern);
        }

        private static string GetNextFilename(string pattern)
        {
            string tmp = string.Format(pattern, 1);
            //if (tmp == pattern)
            //throw new ArgumentException("The pattern must include an index place-holder", "pattern");

            if (!File.Exists(tmp))
                return tmp; // short-circuit if no matches

            int min = 1, max = 2; // min is inclusive, max is exclusive/untested

            while (File.Exists(string.Format(pattern, max)))
            {
                min = max;
                max *= 2;
            }

            while (max != min + 1)
            {
                int pivot = (max + min) / 2;
                if (File.Exists(string.Format(pattern, pivot)))
                    min = pivot;
                else
                    max = pivot;
            }

            return string.Format(pattern, max);
        }
           


    }
}
