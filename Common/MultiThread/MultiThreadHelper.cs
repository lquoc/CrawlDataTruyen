using Repository.Model;

namespace Common.MultiThread
{
    public static class MultiThreadHelper
    {
        public static void MultiThread(this List<string>? data, int numberBatch, string pathSave, string pathSaveVoice, Func<string, string, string, Task> action)
        {
            if (data is null || data.Count() == 0) return;
            var currentTaskCount = data.Count / numberBatch + (data.Count % numberBatch > 0 ? 1 : 0);
            var batchNumber = currentTaskCount > RuntimeContext.MaxThread ? (data.Count / RuntimeContext.MaxThread + (data.Count % RuntimeContext.MaxThread > 0 ? 1 : 0)) : numberBatch;
            var tasks = new List<Task>();
            foreach (var batch in data.Chunk(batchNumber))
            {
                var task = Task.Factory.StartNew(() =>
                {
                    foreach (var item in batch)
                    {
                        if (RuntimeContext.IsStart)
                            action(item, pathSave, pathSaveVoice);
                    }
                });
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
        }

        public static void MultiThread(this List<string>? data, int numberBatch, Novel novel, Func<int, string, Novel, Task> action)
        {
            if (data is null || data.Count() == 0) return;
            var currentTaskCount = data.Count / numberBatch + (data.Count % numberBatch > 0 ? 1 : 0);
            var batchNumber = currentTaskCount > RuntimeContext.MaxThread ? (data.Count / RuntimeContext.MaxThread + (data.Count % RuntimeContext.MaxThread > 0 ? 1 : 0)) : numberBatch;
            var tasks = new List<Task>();
            int tamp = 1;
            foreach (var batch in data.Chunk(batchNumber))
            {
                var task = Task.Factory.StartNew(() =>
                {
                    foreach (var item in batch)
                    {
                        if (RuntimeContext.IsStart)
                            action(tamp++, item, novel); ;
                    }
                });
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
        }

        public static void MultiThreadParralle(this List<string>? listPathChapter, int numberBatch, Novel novel, Func<int, string, Novel, Task> action)
        {
            if (listPathChapter?.Count == 0) return;
            var concurrentTaskCount = listPathChapter.Count / numberBatch + (listPathChapter.Count % numberBatch > 0 ? 1 : 0);
            Task.Run(() =>
            {
                var option = new ParallelOptions()
                {
                    MaxDegreeOfParallelism = concurrentTaskCount,
                };
                int tamp = 1;
                Parallel.ForEach(listPathChapter, option, pathChapter =>
                {
                    if (RuntimeContext.IsStart)
                        action(tamp++, pathChapter, novel);
                });
            });
        }

        public static void SingleForEach(this List<string>? listPathChapter, Novel novel, Func<int, string, Novel, Task> action)
        {
            if (listPathChapter?.Count == 0) return;
            int tamp = 1;
            listPathChapter?.ForEach(pathChapter =>
            {
                if (RuntimeContext.IsStart)
                    action(tamp++, pathChapter, novel);
            });
        }

    }
}
