namespace Common.MultiThread
{
    public static class MultiThreadHelper
    {
        public static void MultiThread(this List<string>? data, int numberBatch, string pathSave, Func<string, string, Task> action)
        {
            if (data is null || data.Count() == 0) return;
            var currentTaskCount = data.Count / numberBatch + (data.Count % numberBatch > 0 ? 1 : 0);
            var batchNumber = currentTaskCount > RuntimeContext.MaxThraed ? (data.Count / RuntimeContext.MaxThraed + (data.Count % RuntimeContext.MaxThraed > 0 ? 1 : 0)) : numberBatch;
            var tasks = new List<Task>();
            foreach (var batch in data.Chunk(batchNumber))
            {
                var task = Task.Factory.StartNew(() =>
                {
                    foreach (var item in batch)
                    {
                        action(item, pathSave);
                    }
                });
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
        }

    }
}
