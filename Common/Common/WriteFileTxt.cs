﻿using Aspose.Cells;
using Migration.Common;
using Repository.Model;
using System.Collections;

namespace Common
{
    public class WriteFileTxt
    {
        private static MyLogger logger = MyLogger.GetInstance();

        public static void WriteFileXLSX(string path, string nameFile, Dictionary<string, Novel> dicResult)
        {
            logger.Info("Start write report xlsx");
            Workbook workbook = new Workbook();
            var workSheet = workbook.Worksheets[0];
            workSheet.Name = "ResultCrawl";
            AddHeaderWorkSheet(workSheet);
            FillData(workSheet, dicResult);
            var pathCombine = Path.Combine(path, nameFile);
            workbook.Save(pathCombine);
            logger.Error("End write report xlsx");
        }
        public static void AddHeaderWorkSheet(Worksheet workSheet)
        {
            ArrayList header = new ArrayList();
            header.Add("Name Novel");
            header.Add("Genre");
            header.Add("NumberChapter");
            header.Add("Author");
            header.Add("Description");
            header.Add("Path");
            workSheet.Cells.ImportArrayList(header, 0, 0, false);
        }

        public static void FillData(Worksheet workSheet, Dictionary<string, Novel> data)
        {
            int i = 1;
            foreach (var (key, novel) in data)
            {
                try
                {
                    ArrayList arrayData = new();
                    arrayData.Add(novel.Name);
                    arrayData.Add(novel.Genre);
                    arrayData.Add(novel.NumberChapter);
                    arrayData.Add(novel.Author);
                    arrayData.Add(novel.Description);
                    arrayData.Add(novel.Path);
                    workSheet.Cells.ImportArrayList(arrayData, i, 0, false);
                    i++;
                }
                catch (Exception ex)
                {
                    logger.Error($"Error while create report, index: {i}, novel: {novel.Name}, msg: {ex}");
                }
            }
        }


        public static void WriteFile(string path, string nameFile, string? content)
        {
            try
            {
                var newFolderName = nameFile.RemoveInvalidPathChars();
                var pathCombine = Path.Combine(path, newFolderName);
                using (StreamWriter writer = new StreamWriter($"{pathCombine}.txt"))
                {
                    writer.WriteLine(content);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Error while write content to file {nameFile}.txt, msg {ex}");
            }
        }
        public static string CreateFolder(string path, string nameFolder)
        {
            var newFolderName = nameFolder.RemoveInvalidPathChars();
            var pathCombine = Path.Combine(path, newFolderName);
            if (!Directory.Exists(pathCombine))
            {
                Directory.CreateDirectory(pathCombine);
            }
            return pathCombine;
        }

    }
}
