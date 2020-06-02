using System;
using System.IO;

using Microsoft.Build.Framework;

namespace Alissa.UniDic.CustomBuildTasks
{
    /// <summary>
    /// An MSBuild custom task to concatenate several files into a single file.
    /// </summary>
    public class ConcatnateFiles : ITask
    {
        public IBuildEngine BuildEngine { get; set; }

        public ITaskHost HostObject { get; set; }

        /// <summary>
        /// A set of source files to concatenate.
        /// </summary>
        /// <remarks>
        /// The source files are concatenated in the order of their file names (without directory/folder paths)
        /// in their alphabetical order,
        /// compared in the mannar of <see cref="StringComparison.InvariantCultureIgnoreCase"/>.
        /// </remarks>
        [Required]
        public ITaskItem[] Sources { get; set; }

        /// <summary>
        /// A target file to concatenate files into.
        /// </summary>
        [Required]
        public ITaskItem Target { get; set; }

        /// <summary>
        /// The main task action to concatenate files.
        /// </summary>
        /// <returns></returns>
        public bool Execute()
        {
            bool error = false;
            try
            {
                using (var output = File.OpenWrite(Target.ItemSpec))
                {
                    var sorted = new ITaskItem[Sources.Length];
                    Array.Copy(Sources, sorted, Sources.Length);
                    Array.Sort(sorted, (x, y) => string.Compare(
                        Path.GetFileName(x.ItemSpec),
                        Path.GetFileName(y.ItemSpec),
                        StringComparison.InvariantCultureIgnoreCase));
                    foreach (var source in sorted)
                    {
                        try
                        {
                            if (source.ItemSpec?.Length > 0)
                            {
                                using (var input = File.OpenRead(source.ItemSpec))
                                {
                                    input.CopyTo(output);
                                }
                            }
                        }
                        catch (IOException e)
                        {
                            // Log the error and keep going,
                            // so that any further issues are reported during this build.
                            LogException(e);
                            error = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogException(e);
                error = true;
            }
            if (error)
            {
                // Try not to leave a partial file, because it will confuse MSBuild.
                try
                {
                    File.Delete(Target.ItemSpec);
                }
                catch (Exception)
                {
                }
            }
            return error;
        }

        private void LogException(Exception exception)
        {
            var event_args = new BuildErrorEventArgs(
                subcategory: null,
                code: null,
                file: BuildEngine.ProjectFileOfTaskNode,
                lineNumber: BuildEngine.LineNumberOfTaskNode,
                columnNumber: BuildEngine.ColumnNumberOfTaskNode,
                endLineNumber: 0,
                endColumnNumber: 0,
                message: exception.Message,
                helpKeyword: null,
                senderName: nameof(ConcatnateFiles));
            BuildEngine.LogErrorEvent(event_args);
        }
    }
}
