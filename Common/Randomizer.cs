using MobileNetworkFramework.Common.Geometry;

namespace MobileNetworkFramework.Common
{
    public class Randomizer
    {
        private static readonly Random RandomGenerator = new Random();


        #region Random Point

        /// <summary>
        /// Возвращает случайную точку в заданных для каждой координаты диапазонах
        /// </summary>
        /// <param name="minValueX">Минимальное значение для X</param>
        /// <param name="maxValueX">Максимальное значение для X</param>
        /// <param name="minValueY">Минимальное значение для Y</param>
        /// <param name="maxValueY">Максимальное значение для Y</param>
        /// <returns></returns>
        public static Point RandomPoint(double minValueX, double maxValueX, double minValueY, double maxValueY)
        {
            double lengthX = maxValueX - minValueX, lengthY = maxValueY - minValueY;
            var randomX = minValueX + RandomDouble(0, lengthX);
            var randomY = minValueY + RandomDouble(0, lengthY);
            return new Point(randomX, randomY);
        }



        #region RandomPointInQuad

        /// <summary>
        /// Возвращает случайную точку внутри заданного прямоугольника
        /// </summary>
        /// <param name="space">Прямоугольник, внутри которого нужна случайная точка</param>
        /// <returns></returns>
        public static Point RandomPointInQuad(Quad space)
        {
            double minValueX = space.MinX, maxValueX = space.MaxX, minValueY = space.MinY, maxValueY = space.MaxY;
            return new Point(RandomDouble(minValueX, maxValueX), RandomDouble(minValueY, maxValueY));
        }

        /// <summary>
        /// Возвращает случайную точку внутри заданного прямоугольника, при этом не создает новый объект класса Point,
        /// а перезаписывает старый
        /// </summary>
        /// <param name="space">Прямоугольник, внутри которого нужна случайная точка</param>
        /// <param name="oldPoint">Старая точка, данные в которой будут перезаписаны новыми значениями</param>
        /// <returns></returns>
        public static void RandomPointInQuad(Quad space, ref Point oldPoint)
        {
            double minValueX = space.MinX, maxValueX = space.MaxX, minValueY = space.MinY, maxValueY = space.MaxY;
            oldPoint.X = RandomDouble(minValueX, maxValueX);
            oldPoint.Y = RandomDouble(minValueY, maxValueY);
        }

        #endregion


        /// <summary>
        /// Возвращает случаную точку на границе заданного прямоугольника
        /// </summary>
        /// <param name="space">Прямоугольник, на границе которого нужна случайная точка</param>
        /// <returns></returns>
        public static Point RandomPointOnQuad(Quad space)
        {
            double randomX, randomY;
            if (RandomDouble(0, 1) < 0.5)
            {
                randomX = RandomDouble(space.MinX, space.MaxX);
                randomY = RandomChoice(space.MinY, space.MaxY);
            }
            else
            {
                randomY = RandomDouble(space.MinY, space.MaxY);
                randomX = RandomChoice(space.MinX, space.MaxX);
            }

            return new Point(randomX, randomY);
        }

        public static void RandomPointOnQuad(Quad space, Point oldPoint)
        {
            if (RandomDouble(0, 1) < 0.5)
            {
                oldPoint.X = RandomDouble(space.MinX, space.MaxX);
                oldPoint.Y = RandomChoice(space.MinY, space.MaxY);
            }
            else
            {
                oldPoint.Y = RandomDouble(space.MinY, space.MaxY);
                oldPoint.X = RandomChoice(space.MinX, space.MaxX);
            }
        }

        /// <summary>
        /// Generates two points on different sides of Quad
        /// </summary>
        /// <param name="space">Quad, where are point should be generated</param>
        /// <param name="first">First point of segment (would be rewritten)</param>
        /// <param name="second">Second point of segment (would be rewritten)</param>
        public static void RandomTwoPointsOnQuad(Quad space, ref Point first, ref Point second)
        {
            if (RandomChoice(0, 1) == 0)
            {
                first.X = space.MinX;
                first.Y = RandomDouble(space.MinY, space.MaxY);
            }
            else
            {
                first.X = RandomDouble(space.MinX, space.MaxX);
                first.Y = space.MaxY;
            }
            if (RandomChoice(0, 1) == 0)
            {
                second.X = space.MaxX;
                second.Y = RandomDouble(space.MinY, space.MaxY);
            }
            else
            {
                second.X = RandomDouble(space.MinX, space.MaxX);
                second.Y = space.MinY;
            }
        }


        #region RandomPointInCircle

        /// <summary>
        /// Возвращает случайную точку внутри окружности
        /// </summary>
        /// <param name="x">х-координата центра окружности</param>
        /// <param name="y">у-координата центра окружности</param>
        /// <param name="radius">Радиус окружности</param>
        /// <returns></returns>
        public static Point RandomPointInCircle(double x, double y, double radius)
        {
            var r = radius * Math.Sqrt(RandomGenerator.NextDouble());
            var theta = RandomGenerator.NextDouble() * 2 * Math.PI;
            var pointX = x + r * Math.Cos(theta);
            var pointY = y + r * Math.Sin(theta);
            return new Point(pointX, pointY);
        }
        public static Point RandomPointInCircle(Point center, double radius)
        {
            return RandomPointInCircle(center.X, center.Y, radius);
        }

        #endregion


        #region RandomPointOnCircle

        /// <summary>
        /// Возвращает случайную точку на окружности
        /// </summary>
        /// <param name="x">х-координата центра окружности</param>
        /// <param name="y">у-координата центра окружности</param>
        /// <param name="radius">Радиус окружности</param>
        /// <returns></returns>
        public static Point RandomPointOnCircle(double x, double y, double radius)
        {
            var r = radius;
            var theta = RandomGenerator.NextDouble() * 2 * Math.PI;
            var pointX = x + r * Math.Cos(theta);
            var pointY = y + r * Math.Sin(theta);
            return new Point(pointX, pointY);
        }
        public static Point RandomPointOnCircle(Point center, double radius)
        {
            return RandomPointOnCircle(center.X, center.Y, radius);
        }

        public static void RandomPointOnCircle(ref Point oldPoint, Point center, double radius)
        {
            var theta = RandomGenerator.NextDouble() * 2 * Math.PI;
            oldPoint.X = center.X + radius * Math.Cos(theta);
            oldPoint.Y = center.Y + radius * Math.Sin(theta);
        }

        #endregion

        #endregion



        #region Random Numeric Value

        /// <summary>
        /// Возвращает случайное целочисленное значение в заданном диапазоне [minValue; maxValue).
        /// </summary>
        public static int RandomInt(int minValue, int maxValue)
        {
            return RandomGenerator.Next(minValue, maxValue);
        }

        /// <summary>
        /// Returns random integer value in [minValue; maxValue) that is not equals to "except" value 
        /// </summary>
        /// <param name="minValue">Minimum random value</param>
        /// <param name="maxValue">Maximum random value</param>
        /// <param name="except">Value That should not be returned</param>
        /// <param name="maximumAttempts">Maximum number of attempts (return -1 if not created correct value)</param>
        /// <returns></returns>
        public static int RandomIntExcept(int minValue, int maxValue, int except, int maximumAttempts)
        {
            var attempts = 0;
            while (true)
            {
                attempts++;
                if (attempts >= maximumAttempts) return -1;
                var result = RandomGenerator.Next(minValue, maxValue);
                if (result != except) return result;
            }
        }

        /// <summary>
        /// Возвращает случайное целочисленное значение в заданном диапазоне такое, что оно
        /// точно не равно значениям в exceptionValues
        /// </summary>
        public static int RandomIntExceptValues(int minValue, int maxValue, List<int> exceptionValues)
        {
            var result = 0;
            while (true)
            {
                result = RandomGenerator.Next(minValue, maxValue);
                if (exceptionValues.All(t => result != t)) return result;
            }
        }


        /// <summary>
        /// Возвращает случайное число, с плавающей запятой
        /// </summary>
        public static double RandomDouble(double minValue, double maxValue)
        {
            return RandomGenerator.NextDouble() * (maxValue - minValue) + minValue;
        }

        /// <summary>
        /// Возвращает случайное число, с плавающей запятой, согласно экспоненциальному распределению
        /// </summary>
        public static double RandomExponentialDouble(double averageValue)
        {
            return -averageValue * Math.Log(RandomGenerator.NextDouble());
        }

        /// <summary>
        /// Возвращает случайное число с плавающей запятой
        /// </summary>
        /// <param name="minValue">Минимальное значение</param>
        /// <param name="maxValue">Максимальное значение</param>
        /// <returns></returns>
        public static float RandomFloat(float minValue, float maxValue)
        {
            return (float)RandomGenerator.NextDouble() * (maxValue - minValue) + minValue;
        }

        #endregion



        #region Random Values

        /// <summary>
        /// Случайно возвращает один из двух параметров
        /// </summary>
        public static double RandomChoice(double a, double b)
        {
            return RandomDouble(0, 1) < 0.5 ? a : b;
        }


        /// <summary>
        /// Случайно возвращает один из двух параметров
        /// </summary>
        public static int RandomChoice(int a, int b)
        {
            return RandomDouble(0, 1) < 0.5 ? a : b;
        }

        /// <summary>
        /// Возвращает случайный знак
        /// </summary>
        public static int RandomSign()
        {
            if (RandomDouble(0, 1) < 0.5) return -1;
            else return 1;
        }

        #endregion
    }
}
