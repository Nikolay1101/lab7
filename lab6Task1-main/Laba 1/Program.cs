using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Laba_1
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime start = DateTime.Now;
            Load(0);
            bool isWorking = true;
            while(isWorking)
            {
                
                Console.WriteLine(".....................");
                Console.WriteLine("1 - Просмотр таблицы");
                Console.WriteLine("2 - Добавить запись");
                Console.WriteLine("3 - Удалить запись");
                Console.WriteLine("4 - Обновить запись");
                Console.WriteLine("5 - Поиск записей");
                Console.WriteLine("6 - Посмотреть лог");
                Console.WriteLine("7 - Выход");
                Console.WriteLine(".....................");

                string actionCode = Console.ReadLine();

                if(actionCode == "7")
                {
                    isWorking = false;
                    DoAction(actionCode);
                }
                else
                {
                    DoAction(actionCode);
                }

            }
            DateTime end = DateTime.Now;
            TimeSpan timeTaken = end - start;
            Console.WriteLine("Затрачено времени: {0}:{1}", timeTaken.Seconds, timeTaken.Milliseconds);
        }
        static string[] types = new string[] { "Д", "К", "М", "Б", "А" };

        static List<Data> data = new List<Data>();

        static int[] maxLength = new int[4];

        #region
        static public void Save(int type = 0)
        {
            switch (type)
            {
                default:
                case 0:
                    StringBuilder tmpText = new StringBuilder();

                    foreach (Data saveDat in data)
                        tmpText.Append(saveDat.toString());

                    File.WriteAllText(@"d:\lab_text.dat", tmpText.ToString());
                    break;

                case 1:
                    using (BinaryWriter writer = new BinaryWriter(File.Open(@"d:\lab_binary.dat", FileMode.OpenOrCreate)))
                    {
                        foreach (Data saveDat in data)
                            writer.Write(saveDat.toString());

                    }
                    break;
            }

        }

        static public void Load(int type = 0)
        {
            switch (type)
            {
                default:
                case 0:
                    String pathText = @"d:\lab_text.dat";
                    if (!File.Exists(pathText))
                        return;

                    String[] str = File.ReadAllLines(pathText);
                    data.Clear();
                    foreach (String row in str)
                    {
                        Data saveDat = new Data();
                        //Console.WriteLine($"test=>{row}");

                        data.Add(saveDat.fromString(row));
                    }

                    break;

                case 1:
                    String pathBinary = @"d:\lab_binary.dat";
                    if (!File.Exists(pathBinary))
                        return;

                    using (BinaryReader reader = new BinaryReader(File.Open(pathBinary, FileMode.Open)))
                    {
                        while (reader.PeekChar() > -1)
                        {
                            string row = reader.ReadString();

                            Data saveDat = new Data();
                            data.Add(saveDat.fromString(row));
                        }


                    }


                    break;
            }

        }
        #endregion

        #region Actions
        static void DoAction(string actionCode)
        {
            switch(actionCode)
            {
                case "1":
                    ViewTableSort();
                    break;
                case "2":
                    Add();
                    break;
                case "3":
                    Delete();
                    break;
                case "4":
                    Overwrite();
                    break;
                case "5":
                    Search();
                    break;
                case "6":
                    ViewLog();
                    break;
                case "7":
                    Save(0);
                    break;
            }
        }

        static List<Data> VstavkaSort(List<Data> mas)
        {
            //intArray - это массив целых чисел
            int indx; //переменная для хранения индекса минимального элемента массива
            for (int i = 0; i < mas.Count; i++) //проходим по массиву с начала и до конца
            {
                indx = i; //считаем, что минимальный элемент имеет текущий индекс 
                for (int j = i; j < mas.Count; j++) //ищем минимальный элемент в неотсортированной части
                {
                    if (mas[j].film.Length < mas[indx].film.Length)
                    {
                        indx = j; //нашли в массиве число меньше, чем intArray[indx] - запоминаем его индекс в массиве
                    }
                }
                if (mas[indx].film.Length == mas[i].film.Length) //если минимальный элемент равен текущему значению - ничего не меняем
                    continue;
                //меняем местами минимальный элемент и первый в неотсортированной части
                Data temp = mas[i]; //временная переменная, чтобы не потерять значение intArray[i]
                mas[i] = mas[indx];
                mas[indx] = temp;
            }

            return mas;
        }

        static void ViewTableSort()
        {
            List<Data> tmpList = new List<Data>();

            tmpList.Clear();

            tmpList = VstavkaSort(data);

            ViewTable(tmpList);
        }

        static void ViewTable(List<Data> dataTable)
        {
            toKnowMaxLength();
            Console.WriteLine("Кинопродукция");
            string output = "";
            Console.WriteLine("№ | " + 
                              "Фильм".PadRight(maxLength[0]) + " | " +
                              "Режиссер".PadRight(maxLength[1]) + " | " +
                              "Год выпуска".PadRight(maxLength[2]) + " | " +
                              "Тип".PadRight(maxLength[3]) + " | ");
            for(int j = 0; j < dataTable.Count; j++)
            {
                output += (j + 1).ToString();
                output += " | ";
                output += dataTable[j].film.PadRight(maxLength[0]);
                output += " | ";
                output += dataTable[j].producer.PadRight(maxLength[1]);
                output += " | ";
                output += dataTable[j].year.ToString().PadRight(maxLength[2]);
                output += " | ";
                output += types[((int)dataTable[j].type)].PadRight(maxLength[3]);
                output += " |";

                Console.WriteLine(output);
                output = "";
            }

            AddToLog("Была просмотрена таблица");
        }

        static void Add()
        {
            bool isAdd = true;
            Console.WriteLine("Введите в соотвествии: \"Фильм/Режиссер/Год выпуска/Тип\"");
            while(isAdd)
            {
                Data note = new Data();
                string input = Console.ReadLine();
                string[] fields = input.Split('/');
                note.film = fields[0];
                note.producer = fields[1];
                note.year = int.Parse(fields[2]);
                if(Array.IndexOf(types, fields[3].ToUpper()) != -1)
                {
                    if (fields[3].ToUpper().Equals("Д")){ note.type = Type.Dram; }
                    if (fields[3].ToUpper().Equals("К")){ note.type = Type.Comedy; }
                    if (fields[3].ToUpper().Equals("М")){ note.type = Type.Melodram; }
                    if (fields[3].ToUpper().Equals("Б")){ note.type = Type.Action; }
                    if (fields[3].ToUpper().Equals("А")){ note.type = Type.Cartoon; }
                }
                data.Add(note);

                Console.WriteLine("Добавить еще? Да | Нет");
                string answer = Console.ReadLine();
                if(answer != "Да")
                {
                    isAdd = false;
                }
                AddToLog($"Была добавлена новая запись: \"{note.film} | {note.producer} | {note.year} | {types[((int)note.type)]} |\"");
            }
        }

        static void Delete()
        {
            Console.WriteLine("Введите номер записи которую хотите удалить");
            int deleteIndex = int.Parse(Console.ReadLine()) - 1;
            if(deleteIndex >= data.Count)
            {
                Console.WriteLine("Такого номера нет");
            }
            else
            {
                AddToLog($"Была удалена запись: \"{data[deleteIndex].film} | {data[deleteIndex].producer} | {data[deleteIndex].year} | {types[((int)data[deleteIndex].type)]} |\"");
                data.RemoveAt(deleteIndex);
            }
        }

        static void Overwrite()
        {
            Console.WriteLine("Выберите номер записи, которую хотите отредактировать.");
            int rewriteIndex = int.Parse(Console.ReadLine()) - 1;
            Console.WriteLine("Введите отредактированную запись в виде: \"Фильмe/Режиссер/Год выпуска/Тип\"");
            string input = Console.ReadLine();

            string[] fields = input.Split('/');
            Data overwriteData = new Data();
            overwriteData.film = fields[0];
            overwriteData.producer = fields[1];
            overwriteData.year = int.Parse(fields[2]); ;
            if (Array.IndexOf(types, fields[3].ToUpper()) != -1)
            {
                if (fields[3].ToUpper().Equals("Д")){ overwriteData.type = Type.Dram; }
                if (fields[3].ToUpper().Equals("К")){ overwriteData.type = Type.Comedy; }
                if (fields[3].ToUpper().Equals("М")){ overwriteData.type = Type.Melodram; }
                if (fields[3].ToUpper().Equals("Б")){ overwriteData.type = Type.Action; }
                if (fields[3].ToUpper().Equals("А")){ overwriteData.type = Type.Cartoon; }
            }
            data[rewriteIndex] = overwriteData;

            AddToLog($"Запись № {rewriteIndex} отредактирована.");
        }

        static void Search()
        {
            Console.WriteLine("Введите год, с которого выводить фильмы");
            int minimalYear = int.Parse(Console.ReadLine());

            List<int> foundedIndex = new List<int>();

            for(int i = 0; i < data.Count; i++)
            {
                if (data[i].year >= minimalYear)
                {
                    foundedIndex.Add(i);
                }
            }

            SearchTable(foundedIndex);

            AddToLog("Был совершен поиск по фильтру");
        }

        

        static List<LogData> logData = new List<LogData>();

        static TimeSpan maxTimeSpan;
        static TimeSpan tmpMaxTimeSpan;

        static void AddToLog(string message)
        {
            LogData LogNote = new LogData();
            if (logData.Count == 50)
            {
                logData.RemoveAt(0);
                LogNote.timeLog = DateTime.Now;
                LogNote.messageLog = message;
                logData.Add(LogNote);
            }
            else
            {
                LogNote.timeLog = DateTime.Now;
                LogNote.messageLog = message;
                logData.Add(LogNote);
            }
            if (logData.Count == 2)
            {
                maxTimeSpan = logData[logData.Count - 1].timeLog - logData[logData.Count - 2].timeLog;
            }
            else if(logData.Count > 2)
            {
                tmpMaxTimeSpan = logData[logData.Count - 1].timeLog - logData[logData.Count - 2].timeLog;
                if(tmpMaxTimeSpan > maxTimeSpan)
                {
                    maxTimeSpan = tmpMaxTimeSpan;
                }
            }
        }

        static void ViewLog()
        {
            Console.WriteLine("......Log......");
            if(logData.Count == 0)
            {
                Console.WriteLine("Никаких действий не было совершено.");
            }
            else
            {
                for (int i = 0; i < logData.Count; i++)
                {
                    Console.WriteLine(logData[i].timeLog.ToLongTimeString() + " " + logData[i].messageLog);
                }
            }
            Console.WriteLine("Максимальное время простоя: " + maxTimeSpan);
        }
        #endregion

        static void toKnowMaxLength()
        {
            int tmpMaxLength = 5;
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].film.Length > tmpMaxLength)
                {
                    tmpMaxLength = data[i].film.Length;
                }
            }
            maxLength[0] = tmpMaxLength;

            tmpMaxLength = 8;
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].producer.Length > tmpMaxLength)
                {
                    tmpMaxLength = data[i].producer.Length;
                }
            }
            maxLength[1] = tmpMaxLength;

            tmpMaxLength = 11;
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].year.ToString().Length > tmpMaxLength)
                {
                    tmpMaxLength = data[i].year.ToString().Length;
                }
            }
            maxLength[2] = tmpMaxLength;
            maxLength[3] = 3;
        }

        static void SearchTable(List<int> foundedIndex)
        {
            toKnowMaxLength();
            Console.WriteLine("Кинопродукция");
            string output = "";
            Console.WriteLine("№ | " +
                              "Фильм".PadRight(maxLength[0]) + " | " +
                              "Режиссер".PadRight(maxLength[1]) + " | " +
                              "Год выпуска".PadRight(maxLength[2]) + " | " +
                              "Тип".PadRight(maxLength[3]) + " | ");
            for (int j = 0; j < foundedIndex.Count; j++)
            {
                output += (j + 1).ToString();
                output += " | ";
                output += data[foundedIndex[j]].film.PadRight(maxLength[0]);
                output += " | ";
                output += data[foundedIndex[j]].producer.PadRight(maxLength[1]);
                output += " | ";
                output += data[foundedIndex[j]].year.ToString().PadRight(maxLength[2]);
                output += " | ";
                output += types[(int)data[foundedIndex[j]].type].PadRight(maxLength[3]);
                output += " |";

                Console.WriteLine(output);
                output = "";
            }
        }

        struct Data
        {
            public string film;
            public string producer;
            public int year;
            public Type type;

            public Data fromString(String str)
            {
                String[] tmp = str.Split(';');
                Data hd = new Data();
                hd.film = tmp[0];
                hd.producer = tmp[1];
                hd.year = Int32.Parse(tmp[2]);
                hd.type = (Type)Double.Parse(tmp[3]);

                return hd;
            }
            public string toString()
            {
                return String.Format("{0};{1};{2};{3}\r\n",
                    this.film,
                    this.producer,
                    this.year,
                    (int)this.type
                    );
            }
        }

        struct LogData
        {
            public DateTime timeLog;
            public string messageLog;
        }

        enum Type
        {
            Dram = 0,
            Comedy = 1,
            Melodram = 2,
            Action = 3,
            Cartoon = 4,
        }
    }
}
