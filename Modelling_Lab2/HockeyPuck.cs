using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelling_Lab2
{
    public class HockeyPuck
    {
        /// <summary>
        /// Ускорение свободного падения
        /// </summary>
        private static readonly double g = 9.8;
        /// <summary>
        /// Направление движения по оси X. -1 - влево, 1 - вправо, 0 - если координата X не изменяется.
        /// </summary>
        private int _directionX;
        /// <summary>
        /// Направление движения по оси Y. -1 - вниз, 1 - вверх, 0 - если координата Y не изменяется.
        /// </summary>
        private int _directionY;
        /// <summary>
        /// Масса шайбы
        /// </summary>
        public double Mass { get; set; }
        /// <summary>
        /// Текущая координата шайбы по оси X
        /// </summary>
        public double PositionX { get; set; }
        /// <summary>
        /// Текущая координата шайбы по оси Y
        /// </summary>
        public double PositionY { get; set; }
        /// <summary>
        /// Текущая линейная скорость шайбы
        /// </summary>
        public double CurrentSpeed { get; set; }
        /// <summary>
        /// Значение проекции линейной скорости шайбы на ось X
        /// </summary>
        public double CurrentSpeedX { get; set; }
        /// <summary>
        /// Значение проекции линейной скорости шайбы на ось Y
        /// </summary>
        public double CurrentSpeedY { get; set; }
        /// <summary>
        /// Текущий угол движения шайбы
        /// </summary>
        public double CurrentAngle { get; set; }
        /// <summary>
        /// Конструктор объекта класса HockeyPuck
        /// </summary>
        /// <param name="mass">Масса шайбы</param>
        /// <param name="speed">Начальная скорость шайбы</param>
        /// <param name="startAngle">Начальный угол движения</param>
        /// <param name="startPositionX">Начальное положение шайбы в трубе по оси X</param>
        public HockeyPuck(double mass, double speed, double startAngle, double startPositionX)
        {
            Mass = mass;
            CurrentSpeed = speed;
            CurrentSpeedX = CurrentSpeed * Math.Cos(startAngle * (Math.PI / 180));
            CurrentSpeedY = CurrentSpeed * Math.Sin(startAngle * (Math.PI / 180));
            CurrentAngle = startAngle;
            PositionX = startPositionX;
            PositionY = 0;
            _directionX = (CurrentSpeedX > 0) ? 1 : -1;
            _directionY = 1;
        }
        /// <summary>
        /// Уменьшает скорость из-за силы трения
        /// </summary>
        /// <param name="frictionForce">Сила трения</param>
        public void SlowDown(double time)
        {
            // Fтр = мю * N = мю*m*g
            double frictionForce = 0.035 * Mass * g;
            CurrentSpeed -= frictionForce * time;
            if (CurrentSpeed < 0)
            {
                CurrentSpeed = 0;
            }

            CurrentSpeedX = CurrentSpeed * Math.Cos(CurrentAngle * (Math.PI / 180));
            CurrentSpeedY = CurrentSpeed * Math.Sin(CurrentAngle * (Math.PI / 180));
        }
        /// <summary>
        /// Передвигает шайбу в соответствии с ее текущей скоростью
        /// </summary>
        /// <param name="time">Промежуток времени</param>
        public void ChangePosition(double time) =>
            (PositionX, PositionY) = (PositionX + CurrentSpeedX * time, PositionY + CurrentSpeedY * time);
        /// <summary>
        /// Устанавливает направления движения по значениям проекций скоростей
        /// </summary>
        private void CheckDirection()
        {
            if (CurrentSpeedX < 0)
            {
                _directionX = -1;
            }
            else if (CurrentSpeedX > 0)
            {
                _directionX = 1;
            }
            else if (CurrentSpeedX == 0)
            {
                _directionX = 0;
            }

            if (CurrentSpeedY < 0)
            {
                _directionY = -1;
            }
            else if (CurrentSpeedY > 0)
            {
                _directionY = 1;
            }
            else if (CurrentSpeedY == 0)
            {
                _directionY = 0;
            }
        }
        /// <summary>
        /// Устанавливает проекции скоростей по значению линейной скорости
        /// </summary>
        private void CheckSpeed() => 
            (CurrentSpeedX, CurrentSpeedY) = 
            (CurrentSpeed * Math.Cos(CurrentAngle * (Math.PI / 180)), CurrentSpeed * Math.Sin(CurrentAngle * (Math.PI / 180)));
        /// <summary>
        /// Проверяет наличие столкновения шайбы с одной из стенок туннеля. Возвращает
        /// true, если шайба коснулась стенки, и false, если шайба не коснулась стенки.
        /// </summary>
        /// <param name="tunnel">Туннель, в котором движется данная шайба</param>
        private bool IsEncountered(Tunnel tunnel)
        {
            //  Если еще не дошли до поворота
            if (PositionY < tunnel.Height)
            {
                return (PositionY != 0 && (PositionX >= tunnel.Width || PositionX <= 0));
            }
            //  Если уже вошли в поворот
            else
            {
                double tan = Math.Tan(tunnel.Angle * Math.PI / 180);
                return ((PositionY >= tunnel.Height + PositionX * tan) ||                    //  Если бьемся о верхнюю стенку
                        (PositionY <= tunnel.Height + (PositionX - tunnel.Width) * tan));    //  Если бьемся о нижнюю стенку
            }

        }
        public void CheckEncounter(Tunnel tunnel)
        {
            if (IsEncountered(tunnel))
            {
                //  1. Движемся снизу вверх слева направо
                if (_directionX == 1 && _directionY == 1)
                {
                    if (PositionY >= tunnel.Height)
                    {
                        //  1.1.    Бьемся о верхнюю стенку
                        if (CurrentAngle > tunnel.Angle)
                        {
                            //  Высчитываем новый угол движения
                            CurrentAngle = 2 * tunnel.Angle - CurrentAngle;
                        }
                        //  1.2     Бьемся о нижнюю стенку
                        else
                        {
                            //  Высчитываем новый угол движения
                            CurrentAngle = 2 * tunnel.Angle - CurrentAngle;
                        }
                    } 
                    //  1.3.    Бьемся о правую стенку
                    else
                    {
                        //  Вычисляем новый угол движения
                        CurrentAngle = 180 - CurrentAngle;
                    }
                }
                //  2. Движемся снизу вверх справа налево
                else if (_directionX == -1 && _directionY == 1)
                {
                    //  Бьемся о верхнюю стенку
                    if (PositionY >= tunnel.Height)
                    {
                        //  2.1.    Отскакиваем вниз левее
                        if (CurrentAngle + tunnel.Angle > 180)
                        {
                            //  Высчитываем новый угол движения
                            CurrentAngle = 2 * tunnel.Angle - CurrentAngle + 90;
                        }
                        //  2.2.    Отскакиваем вниз правее
                        if (CurrentAngle + tunnel.Angle < 180)
                        {
                            //  Высчитываем новый угол движения
                            CurrentAngle = 270 - 2 * tunnel.Angle - CurrentAngle;
                        }
                        //  2.3.    Бьемся под прямым углом и движемся в противоположном направлении
                        if (CurrentAngle + tunnel.Angle == 180)
                        {
                            //  Высчитываем новый угол движения
                            CurrentAngle += 180;
                        }
                    }
                    //  2.4.    Бьемся о левую стенку
                    else
                    {
                        //  Вычисляем новый угол движения
                        CurrentAngle -= 90;
                    }
                }
                //  3. Движемся сверху вниз слева направо
                else if (_directionX == 1 && _directionY == -1)
                {
                    if (PositionY > tunnel.Height)
                    {
                        //  3.1.    Движемся вниз вправо
                        if (360 - CurrentAngle + tunnel.Angle > 90)
                        {
                            CurrentAngle = CurrentAngle - 2 * tunnel.Angle - 180;
                        }
                        //  3.2.   Движемся вверх влево 
                        if (360 - CurrentAngle + tunnel.Angle < 90)
                        {
                            CurrentAngle = 360 + 2 * tunnel.Angle - CurrentAngle;
                        }
                        //  3.3.    Движемся горизонтально влево
                        if (360 - CurrentAngle + tunnel.Angle == 90)
                        {
                            CurrentAngle = 180;
                        }
                    }
                    //  3.4.    Бьемся о правую стенку
                    else
                    {
                        CurrentAngle = 360 - CurrentAngle;
                    }
                }
                //  4. Движемся сверху вниз справа налево
                else if (_directionX == -1 && _directionY == -1)
                {
                    if (PositionY > tunnel.Height)
                    {
                        //  4.1.    Движемся вниз вправо 
                        if (CurrentAngle + tunnel.Angle < 90)
                        {
                            CurrentAngle = 2 * tunnel.Angle - CurrentAngle;
                        }
                        //  4.2.    Движемся вниз влево
                        if (CurrentAngle + tunnel.Angle > 90)
                        {
                            CurrentAngle = 2 * tunnel.Angle - CurrentAngle;
                        }
                        //  4.3.    Движемся вертикально вниз
                        if (CurrentAngle + tunnel.Angle == 90)
                        {
                            CurrentAngle = 270;
                        }
                    }
                    //  4.4.    Бьемся о левую стенку
                    else
                    {
                        CurrentAngle = 180 - CurrentAngle;
                    }
                }
                //  5. Движемся вертикально 
                else if (_directionX == 0 && _directionY == 1)
                {
                    CurrentAngle = 2 * tunnel.Angle - 90;
                }
                //  6. Движемся вертикально вниз
                else if (_directionX == 0 && _directionY == -1)
                {
                    CurrentAngle = 2 * tunnel.Angle - 90;
                }
                //  7. Движемся горизонтально вправо
                else if (_directionX == 1 && _directionY == 0)
                {
                    CurrentAngle = 2 * tunnel.Angle;
                }
                //  8. Движемся горизонтально влево
                else if (_directionX == -1 && _directionY == 0)
                {
                    CurrentAngle = 2 * tunnel.Angle;
                }

                //  Если координаты шайбы вышли за стенки туннеля, изменяем их
                SetCurrentCoordinate(tunnel);
                //  Заново высчитываем скорость по осям
                CheckSpeed();
                //  Проверяем, как изменилось направление движения
                CheckDirection();
            }
        }
        /// <summary>
        /// Устанавливает корректные координаты после столкновения
        /// </summary>
        private void SetCurrentCoordinate(Tunnel tunnel)
        {
            if (PositionY < tunnel.Height)
            {
                //  Если бьемся о левую стенку
                if (PositionX >= tunnel.Width) {
                    PositionX = tunnel.Width;
                }
                //  Если бьемся о правую стенку
                else if (PositionX <= 0)
                {
                    PositionX = 0;
                }
            }
            else
            {
                double tan = Math.Tan(tunnel.Angle * Math.PI / 180);
                //  Если бьемся о верхнюю стенку
                if (PositionY >= tunnel.Height + PositionX * tan)
                {
                    PositionY = tunnel.Height + PositionX * tan;
                }
                //  Если бьемся о нижнюю стенку
                else if (PositionY <= tunnel.Height + (PositionX - tunnel.Width) * tan)
                {
                    PositionY = tunnel.Height + (PositionX - tunnel.Width) * tan;
                }
            }
        }
        /// <summary>
        /// Проверяет, движется шайба или нет. Возвращает true, если скорость не равна нулю, 
        /// и false, если скорость равна нулю.
        /// </summary>
        public bool IsMoving() => ((CurrentSpeed > 0) ? true : false);
        /// <summary>
        /// Возвращает текущие координаты и линейную скорость шайбы в формате "X; Y; V"
        /// </summary>
        /// <returns></returns>
        public override string ToString() => (PositionX.ToString() + "; " + PositionY.ToString() + "; " + CurrentSpeed.ToString());
    }
}
