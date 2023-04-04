using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sort_Laba
{
    internal class Program
    {
        static void SaveToFile(string fileName, int[] array)
        {
            using(StreamWriter sw = new StreamWriter(fileName))
            {
                array.ToList().ForEach(item =>
                {
                    sw.WriteLine(item);
                });
            }
        }

        static void Main(string[] args)
        {
            int[] rndArray = GenerateRandom();
            int[] sel = SelectionSort(rndArray);
            //Print(sel);
            int[] vst = Vstavka(rndArray);
            //Print(vst);
            int[] bub = Buble(rndArray);
            //Print(bub);
            int[] shake = Shake(rndArray);
            //Print(shake);
            int[] shall = Shall(rndArray);
            //Print(shall);

            SaveToFile("sel-sorted.dat", sel);
            SaveToFile("vst-sorted.dat", vst);
            SaveToFile("bub-sorted.dat", bub);
            SaveToFile("shake-sorted.dat", shake);
            SaveToFile("shall-sorted.dat", shall);

            Console.WriteLine($"Выбор: {count.Selection / 1000}с \\ {count.Selection} мс");
            Console.WriteLine($"Вставка: {count.Vstavka / 1000}с \\ {count.Vstavka} мс");
            Console.WriteLine($"Пузырь: {count.Buble / 1000}с \\ {count.Buble} мс");
            Console.WriteLine($"Шейк: {count.Shake / 1000}с \\ {count.Shake} мс");
            Console.WriteLine($"Шелл: {count.Shall / 1000}с \\ {count.Shall} мс");
        }

        static void Print(int[] sortedArr)
        {

            for (int i = 0; i < sortedArr.Length; i++)
            {
                Console.WriteLine(sortedArr[i]);
            }
        }

        static int[] GenerateRandom()
        {
            Random rnd = new Random();
            int[] array = new int[100000];
            for (int i = 0; i < 100000; i++)
            {
                array[i] = rnd.Next(0, 100000);
            }
            return array;
        }

        static int[] SelectionSort(int[] array)
        {
            long tmpTime1 = DateTime.Now.Ticks;
            int n = array.Length;
            for (int i = 0; i < n - 1; i++)
            {
                int maxIndex = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (array[j] > array[maxIndex])
                    {
                        maxIndex = j;
                    }
                }
                if (maxIndex != i)
                {
                    int temp = array[i];
                    array[i] = array[maxIndex];
                    array[maxIndex] = temp;
                }
            }
            long tmpTime2 = DateTime.Now.Ticks;
            count.Selection = tmpTime2 - tmpTime1;
            return array;
        }

        static int[] Vstavka(int[] arr)
        {
            long tmpTime1 = DateTime.Now.Ticks;
            int n = arr.Length;
            for (int i = 1; i < n; ++i)
            {
                int key = arr[i];
                int j = i - 1;

                /* Перемещаем элементы arr[0..i-1], которые больше чем key, на одну позицию вправо */
                while (j >= 0 && arr[j] < key)
                {
                    arr[j + 1] = arr[j];
                    j = j - 1;
                }
                arr[j + 1] = key;
            }
            long tmpTime2 = DateTime.Now.Ticks;
            count.Vstavka = tmpTime2 - tmpTime1;
            return arr;
        }

        static int[] Buble(int[] arr)
        {
            long tmpTime1 = DateTime.Now.Ticks;
            int n = arr.Length;
            for (int i = 0; i < n - 1; ++i)
            {
                for (int j = 0; j < n - i - 1; ++j)
                {
                    /* Если текущий элемент меньше следующего элемента, то они меняются местами */
                    if (arr[j] < arr[j + 1])
                    {
                        int temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                    }
                }
            }
            long tmpTime2 = DateTime.Now.Ticks;
            count.Buble = tmpTime2 - tmpTime1;
            return arr;
        }

        static int[] Shake(int[] arr)
        {
            long tmpTime1 = DateTime.Now.Ticks;
            int n = arr.Length;
            bool swapped = true;
            int start = 0;
            int end = n - 1;

            while (swapped)
            {
                swapped = false;

                /* Перебираем массив слева направо */
                for (int i = start; i < end; ++i)
                {
                    /* Если текущий элемент меньше следующего элемента, то они меняются местами */
                    if (arr[i] < arr[i + 1])
                    {
                        int temp = arr[i];
                        arr[i] = arr[i + 1];
                        arr[i + 1] = temp;
                        swapped = true;
                    }
                }

                if (!swapped)
                    break;

                swapped = false;
                end--;

                /* Перебираем массив справа налево */
                for (int i = end - 1; i >= start; --i)
                {
                    /* Если текущий элемент меньше следующего элемента, то они меняются местами */
                    if (arr[i] < arr[i + 1])
                    {
                        int temp = arr[i];
                        arr[i] = arr[i + 1];
                        arr[i + 1] = temp;
                        swapped = true;
                    }
                }
                start++;
            }
            long tmpTime2 = DateTime.Now.Ticks;
            count.Shake = tmpTime2 - tmpTime1;
            return arr;
        }

        static int[] Shall(int[] arr)
        {
            long tmpTime1 = DateTime.Now.Ticks;
            int n = arr.Length;
            int gap = n / 2;

            while (gap > 0)
            {
                for (int i = gap; i < n; i++)
                {
                    int temp = arr[i];
                    int j = i;
                    while (j >= gap && arr[j - gap] < temp)
                    {
                        arr[j] = arr[j - gap];
                        j -= gap;
                    }
                    arr[j] = temp;
                }
                gap /= 2;
            }
            long tmpTime2 = DateTime.Now.Ticks;
            count.Shall = tmpTime2 - tmpTime1;
            return arr;
        }
        static public SortTypeTimeCount count;
        public struct SortTypeTimeCount
        {
            public long Selection = 0;
            public long Vstavka = 0;
            public long Buble = 0;
            public long Shake = 0;
            public long Shall = 0;

            public SortTypeTimeCount()
            {
            
            }
        }
    }
}
