using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace FileSystemLogger.Model
{
    [DataContract]
    public class WatchLog
    {
        /// <summary>監視ディレクトリパス</summary>
        [DataMember] public string Path { get; set; }
        /// <summary>監視ディレクトリ名</summary>
        [DataMember] public string Name { get; set; }
        /// <summary>観測イベント名</summary>
        [DataMember] public string EventName { get; set; }
        /// <summary>観測イベント対象ファイルパス</summary>
        [DataMember] public string FilePath { get; set; }
        /// <summary>観測イベント対象ファイル名</summary>
        [DataMember] public string FileName { get; set; }
        /// <summary>リネーム前ファイルパス</summary>
        [DataMember] public string BeforeFilePath { get; set; }
        /// <summary>リネーム前ファイル名</summary>
        [DataMember] public string BeforeFileName { get; set; }
        /// <summary>イベント発生時刻</summary>
        [DataMember] public DateTime Date { get; set; }

        public static WatchLog Create(string path, FileSystemEventArgs e)
        {
            var log = new WatchLog();
            log.Path = path;
            log.Name = System.IO.Path.GetFileName(path);
            log.EventName = e.ChangeType.ToString();
            log.FilePath = e.FullPath;
            log.FileName = e.Name;
            log.Date = DateTime.Now;

            if (e is RenamedEventArgs renamed)
            {
                log.BeforeFilePath = renamed.OldFullPath;
                log.BeforeFileName = renamed.OldName;
            }
            else
            {
                log.BeforeFilePath = string.Empty;
                log.BeforeFileName = string.Empty;
            }

            return log;
        }

        public static string Save(IEnumerable<WatchLog> logs)
        {
            return JSON.Serialize(logs);
        }

        public static IEnumerable<WatchLog> Load(string logs)
        {
            return JSON.Deserialize<IEnumerable<WatchLog>>(logs);
        }
    }
}
