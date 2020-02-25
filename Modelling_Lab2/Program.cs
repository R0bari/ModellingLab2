using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelling_Lab2
{
    class Program
    {
        static void Main()
        {
            try
            {
                bool repeat;
                do
                {
                    repeat = false;

                    Console.WriteLine("Введите массу шайбы: ");
                    if (!double.TryParse(Console.ReadLine().ToString(), out double mass))
                    {
                        Console.WriteLine("Некорректный ввод.");
                        return;
                    }
                    if (mass <= 0)
                    {
                        Console.WriteLine("Масса шайбы не может быть меньше или равна 0.");
                        return;
                    }

                    Console.WriteLine("Введите начальную скорость шайбы: ");
                    if (!double.TryParse(Console.ReadLine().ToString(), out double speed))
                    {
                        Console.WriteLine("Некорректный ввод.");
                        return;
                    }
                    if (speed <= 0)
                    {
                        Console.WriteLine("Начальная скорость не может быть меньше или равна 0.");
                        return;
                    }

                    Console.WriteLine("Введите начальный угол к горизонтали в градусах, под которым будет двигаться шайба [0;180]: ");
                    if (!double.TryParse(Console.ReadLine().ToString(), out double angle))
                    {
                        Console.WriteLine("Некорректный ввод.");
                        return;
                    }
                    if (angle < 0 || angle > 180)
                    {
                        Console.WriteLine("Начальный угол не может быть меньше 0 или больше 180.");
                        return;
                    }

                    Console.WriteLine("Введите толщину туннеля: ");
                    if (!double.TryParse(Console.ReadLine().ToString(), out double tunnelWidth))
                    {
                        Console.WriteLine("Некорректный ввод.");
                        return;
                    }
                    if (tunnelWidth <= 0)
                    {
                        Console.WriteLine("Толщина туннеля не может быть меньше или равна 0");
                        return;
                    }

                    Console.WriteLine("Введите начальное смещение шайбы от левого края по горизонтали: ");
                    if (!double.TryParse(Console.ReadLine().ToString(), out double startX))
                    {
                        Console.WriteLine("Некорректный ввод.");
                        return;
                    }
                    if (startX < 0)
                    {
                        Console.WriteLine("Начальное смещение не может быть меньше 0");
                        return;
                    }
                    if (startX > tunnelWidth)
                    {
                        Console.WriteLine("Начальное смещение не может быть больше ширины туннеля");
                        return;
                    }

                    Console.WriteLine("Введите расстояние до поворота: ");
                    if (!double.TryParse(Console.ReadLine().ToString(), out double tunnelHeight))
                    {
                        Console.WriteLine("Некорректный ввод.");
                        return;
                    }
                    if (tunnelHeight < 0)
                    {
                        Console.WriteLine("Расстояние до поворота в туннеле не может быть меньше 0");
                        return;
                    }

                    Console.WriteLine("Введите угол поворота к горизонтали в градусах [0; 90]: ");
                    if (!double.TryParse(Console.ReadLine().ToString(), out double tunnelAngle))
                    {
                        Console.WriteLine("Некорректный ввод.");
                        return;
                    }
                    if (tunnelAngle < 0 || tunnelAngle > 90)
                    {
                        Console.WriteLine("Расстояние до поворота в туннеле не может быть меньше 0 или больше 180.");
                        return;
                    }

                    Tunnel tunnel = new Tunnel(tunnelWidth, tunnelHeight, tunnelAngle);
                    HockeyPuck puck = new HockeyPuck(mass, speed, angle, startX);


                    //  Минимальный промежуток времени
                    double timeChange = 0.1, totalTime = 0;

                    //  Цикл движения: 
                    //  1)  Проверяем столкновение -> Изменяем направление движения и угол, если столкнулись со стеной ->
                    //  2)  Изменяем положение шайбы -> 
                    //  3)  Изменить скорость из-за силы трения
                    //  4)  Подсчитываем общее время
                    while (puck.IsMoving())
                    {
                        puck.CheckEncounter(tunnel);
                        puck.ChangePosition(timeChange);
                        puck.SlowDown(timeChange);

                        totalTime += timeChange;
                    }

                    Console.WriteLine($"Шайба остановилась через {totalTime}." +
                        $"Результирующие координаты: {puck.GetCoordinates()}.");

                    Console.WriteLine("Повторить? (Y/N)");
                    repeat = (Console.ReadLine().ToString() == "Y") ? true : false;

                } while (repeat);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(Environment.NewLine + ex.Message);
            }
        }
    }
}
