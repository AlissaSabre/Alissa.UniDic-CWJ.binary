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
        /// <summary>
        /// Implements <see cref="ITask.BuildEngine"/>
        /// </summary>
        public IBuildEngine BuildEngine { get; set; }

        /// <summary>
        /// Implements <see cref="ITask.HostObject"/>
        /// </summary>
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
        /// Implements <see cref="ITask.Execute"/> to concatenate files.
        /// </summary>
        /// <returns>True if successful.  False otherwise.</returns>
        public bool Execute()
        {
            bool success = true;
            try
            {
                using (var output = File.OpenWrite(Target.ItemSpec))
                {
                    foreach (var source in Helper.SortItems(Sources))
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
                            Helper.LogError(BuildEngine, e.Message);
                            success = false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Helper.LogError(BuildEngine, e.Message);
                success = false ;
            }
            if (!success)
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
            return success;
        }
    }

    /// <summary>
    /// An MSBuild task to split a single large file into mutltiple smaller parts.
    /// </summary>
    public class SplitFile : ITask
    {
        private const int BufferSize = 65536;

        /// <summary>
        /// Implements <see cref="ITask.BuildEngine"/>
        /// </summary>
        public IBuildEngine BuildEngine { get; set; }

        /// <summary>
        /// Implements <see cref="ITask.HostObject"/>
        /// </summary>
        public ITaskHost HostObject { get; set; }

        /// <summary>
        /// A file to split.
        /// </summary>
        [Required]
        public ITaskItem Source { get; set; }

        /// <summary>
        /// A set of files to split the file into.
        /// </summary>
        /// <remarks>
        /// At least one Target is required.
        /// The files will be almost same size after the split.
        /// 
        /// </remarks>
        [Required]
        public ITaskItem[] Targets { get; set; }

        /// <summary>
        /// Implements <see cref="ITask.Execute"/> to concatenate files.
        /// </summary>
        /// <returns>True if successful.  False otherwise.</returns>
        public bool Execute()
        {
            if (Targets.Length < 2)
            {
                Helper.LogError(BuildEngine, "At least two targets are needed.");
                return false;
            }

            bool success = true;
            try
            {
                using (var input = File.OpenRead(Source.ItemSpec))
                {
                    var targets = Helper.SortItems(Targets);
                    long part_length = input.Length / targets.Length;
                    int extras = (int)(input.Length % targets.Length);
                    var buffer = new byte[BufferSize];
                    for (int i = 0; i < targets.Length; i++)
                    {
                        long remaining = part_length + (i < extras ? 1 : 0);
                        long goal_position = input.Position + remaining;
                        try
                        {
                            using (var output = File.OpenWrite(targets[i].ItemSpec))
                            {
                                while (remaining > 0)
                                {
                                    int length = (int)Math.Min(remaining, buffer.Length);
                                    int n = input.Read(buffer, 0, length);
                                    output.Write(buffer, 0, n);
                                    remaining -= n;
                                }
                            }
                        }
                        catch (IOException e)
                        {
                            // Log the error and keep going,
                            // so that any further issues are reported during this build.
                            Helper.LogError(BuildEngine, e.Message);
                            success = false;

                            // Try not to leave an incomplete target file.
                            try
                            {
                                File.Delete(targets[i].ItemSpec);
                            }
                            catch (Exception)
                            {
                            }

                            // Try to make contents of other files as expected;
                            try
                            {
                                input.Position = goal_position;
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Helper.LogError(BuildEngine, e.Message);
                success = false;
            }
            return success;
        }
    }

    /// <summary>
    /// Provides helpful static methods.
    /// </summary>
    internal static class Helper
    {
        /// <summary>
        /// Returns an array of items ordered in <see cref="ITaskItem.ItemSpec"/>.
        /// </summary>
        /// <param name="items">An array of items to sort.</param>
        /// <returns>an array containing sorted items.</returns>
        public static ITaskItem[] SortItems(ITaskItem[] items)
        {
            var sorted = new ITaskItem[items.Length];
            Array.Copy(items, sorted, items.Length);
            Array.Sort(sorted, (x, y) => string.Compare(
                Path.GetFileName(x.ItemSpec),
                Path.GetFileName(y.ItemSpec),
                StringComparison.InvariantCultureIgnoreCase));
            return sorted;
        }

        /// <summary>
        /// Log an error event.
        /// </summary>
        /// <param name="engine">A build engine to log an error event.</param>
        /// <param name="message">An error message to log.</param>
        public static void LogError(IBuildEngine engine, string message)
        {
            var event_args = new BuildErrorEventArgs(
                subcategory: null,
                code: null,
                file: engine.ProjectFileOfTaskNode,
                lineNumber: engine.LineNumberOfTaskNode,
                columnNumber: engine.ColumnNumberOfTaskNode,
                endLineNumber: 0,
                endColumnNumber: 0,
                message: message,
                helpKeyword: null,
                senderName: nameof(ConcatnateFiles));
            engine.LogErrorEvent(event_args);
        }
    }
}
