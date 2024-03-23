using System;
using System.Runtime.InteropServices;
using System.Threading;
class MouseIdleTimer
{
    [DllImport("user32.dll")]
    public static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
    [StructLayout(LayoutKind.Sequential)]
    public struct LASTINPUTINFO
    {
        public uint cbSize;
        public uint dwTime;
    }

    static uint lastInputTick = 0;
    static uint idleTick = 0;
    static int seconds = 0;
    static int minutes = 0;
    static int hours = 0;
    static void Main()
    {
        LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
        lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);

        Timer timer = new Timer(CheckMouseIdle, null, 0, 1000);

        Console.WriteLine("Нажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
    static void CheckMouseIdle(object state)
    {
        LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
        lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
        if (GetLastInputInfo(ref lastInputInfo))
        {
            uint tickCount = (uint)Environment.TickCount;

            if (lastInputInfo.dwTime != lastInputTick)
            {
                lastInputTick = lastInputInfo.dwTime;
                idleTick = tickCount;
                Console.WriteLine("Мышь двигается или активна клавиатура, таймер остановлен.");
            }
            else
            {
                if (tickCount - idleTick >3000) // 3 секунды бездействия
                {
                    seconds++;
                    if (seconds == 60)
                    {
                        seconds = 0;
                        minutes++;
                        if (minutes == 60)
                        {
                            minutes = 0;
                            hours++;
                        }
                    }
                    Console.WriteLine($"Прошло времени бездействия: {hours:D2}:{minutes:D2}:{seconds:D2}");
                }
            }
        }
    }
}

