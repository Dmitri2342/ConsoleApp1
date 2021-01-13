using System;
using System.IO;
using System.Runtime.InteropServices;
namespace ConsoleApp1
{
    class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
        [Flags]
        private enum ConsoleInputModes : uint
        {
            ENABLE_PROCESSED_INPUT = 0x0001,
            ENABLE_LINE_INPUT = 0x0002,
            ENABLE_ECHO_INPUT = 0x0004,
            ENABLE_WINDOW_INPUT = 0x0008,
            ENABLE_MOUSE_INPUT = 0x0010,
            ENABLE_INSERT_MODE = 0x0020,
            ENABLE_QUICK_EDIT_MODE = 0x0040,
            ENABLE_EXTENDED_FLAGS = 0x0080,
            ENABLE_AUTO_POSITION = 0x0100
        }
        [Flags]
        private enum ConsoleOutputModes : uint
        {
            ENABLE_PROCESSED_OUTPUT = 0x0001,
            ENABLE_WRAP_AT_EOL_OUTPUT = 0x0002,
            ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004,
            DISABLE_NEWLINE_AUTO_RETURN = 0x0008,
            ENABLE_LVB_GRID_WORLDWIDE = 0x0010
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);
        static int STD_INPUT_HANDLE = -10;
        
        static void Main(string[] args)
        {
            long razmer=0;//создание целочисленной переменной
            string text=null; //создание пустой текстовой переменной
            Console.Write("Enter text line and press Enter for new line. Press Ctrl + S to Save file" + Environment.NewLine);//вывод сообщения + переход на новую строку
            ConsoleKeyInfo input;
            while (!Console.KeyAvailable)
            {
            SetConsoleMode(GetStdHandle(STD_INPUT_HANDLE), ~(uint)(ConsoleInputModes.ENABLE_EXTENDED_FLAGS));//отключение режима консоли
            text = text + Console.ReadLine() + Environment.NewLine;//заполнение + переход на новую строку
            SetConsoleMode(GetStdHandle(STD_INPUT_HANDLE), (uint)(//включение возможности нажимать комбинацию ctrl+S в консоли
            ConsoleInputModes.ENABLE_WINDOW_INPUT |
            ConsoleInputModes.ENABLE_MOUSE_INPUT |
            ConsoleInputModes.ENABLE_EXTENDED_FLAGS));
            input = Console.ReadKey(true);//скрытое нажатие
                if (input.Modifiers == ConsoleModifiers.Control)//проверка нажатия ctrl
                {
                  if (input.Key == ConsoleKey.S) { save();break;}//проверка нажатия S, вызов метода и выход из цикла
                }
            }
            void save()
            {   
                string time = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");//получение системного времени
                File.WriteAllText($"{time}.txt", text);//сохранение
                razmer = new FileInfo(@$"{time}.txt").Length;//запись размера файла в переменную
                Console.WriteLine($"File successfully saved. {razmer} bytes");//вывод сообщения
                SetConsoleMode(GetStdHandle(STD_INPUT_HANDLE), ~(uint)(ConsoleInputModes.ENABLE_EXTENDED_FLAGS));
            }
            Console.ReadKey();
        }
    }
}

