using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelling_Lab2
{
    public class Tunnel
    {
        /// <summary>
        /// Ширина туннеля
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// Расстояние от начала туннеля до поворота
        /// </summary>
        public double Height { get; set; }
        /// <summary>
        /// Угол поворота
        /// </summary>
        public double Angle { get; set; }
        /// <summary>
        /// Конструктор объекта класса Tunnel
        /// </summary>
        /// <param name="L">Толщина туннеля</param>
        /// <param name="H">Расстояние от начала пути до поворота</param>
        /// <param name="angle">Угол поворота</param>
        public Tunnel(double L, double H, double angle)
        {
            Width = L;
            Height = H;
            Angle = angle;
        }
    }
}
