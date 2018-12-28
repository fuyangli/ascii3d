using System;
using ASCII3D.Models;

namespace ASCII3D
{
    class Program
    {

        private static Player player;

        private static int mapHeight = 16;
        private static int mapWidth = 16;

        private static float fov = (float)Math.PI / 4f;
        private static float depth = 16f;

        static void Main(string[] args)
        {
            Console.SetBufferSize(120, 40);
            Console.SetWindowSize(120, 40);
            player = new Player()
            {
                Position = new System.Numerics.Vector2(8, 8)
            };
            var map =   "################" +
                        "#..............#" +
                        "#..............#" +
                        "#.#####........#" +
                        "#..............#" +
                        "#..............#" +
                        "#..............#" +
                        "#..............#" +
                        "#..............#" +
                        "#..............#" +
                        "#..............#" +
                        "#..............#" +
                        "#..............#" +
                        "#..............#" +
                        "#..............#" +
                        "################";

            var dt1 = DateTime.Now;
            var dt2 = DateTime.Now; 

            while(true)
            {

                dt2 = DateTime.Now;
                var elapsed = (float)(dt2 - dt1).TotalSeconds;
                dt1 = dt2;

                if(Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.A)
                        player.Angle -= (0.5f) * elapsed;
                    else if (key.Key == ConsoleKey.D)
                        player.Angle += (0.5f) * elapsed;

                    if(key.Key == ConsoleKey.W)
                    {
                        player.Position = new System.Numerics.Vector2(player.Position.X + (float)Math.Sin(player.Angle) * 5f * elapsed, player.Position.Y + (float)Math.Cos(player.Angle) * 5f * elapsed);
                    }
                    else if (key.Key == ConsoleKey.S)
                    {
                        player.Position = new System.Numerics.Vector2(player.Position.X - (float)Math.Sin(player.Angle) * 5f * elapsed, player.Position.Y - (float)Math.Cos(player.Angle) * 5f * elapsed);
                    }
                }


                for (int i = 0; i < Console.BufferWidth; i++)
                {
                    var rayAngle = (player.Angle - (fov / 2f)) + (i / Console.BufferWidth) * fov;

                    var distanceToWall = 0f;
                    var hitWall = false;

                    var eyeX = Math.Sin(rayAngle);
                    var eyeY = Math.Cos(rayAngle);

                    while (!hitWall && distanceToWall < depth)
                    {
                        distanceToWall += 0.1f;

                        var testX = (int)(player.Position.X + eyeX * distanceToWall);
                        var testY = (int)(player.Position.Y + eyeY * distanceToWall);

                        if (testX < 0 || testX >= mapWidth || testY < 0 || testY >= mapHeight)
                        {
                            hitWall = true;
                            distanceToWall = depth;
                        }
                        else
                        {
                            var c = map[testX * mapWidth + testY];
                            if (c == '#')
                            {
                                hitWall = true;
                            }
                        }

                    }

                    var ceiling = (Console.BufferHeight / 2) - Console.BufferHeight / distanceToWall;
                    var floor = Console.BufferHeight - ceiling;

                    char shade = ' ';
                    if (distanceToWall <= depth / 4f) shade =       (char)0x2588;
                    else if (distanceToWall <= depth / 3f) shade =  (char)0x2593;
                    else if (distanceToWall <= depth / 2f) shade =  (char)0x2592;
                    else if (distanceToWall <= depth) shade =       (char)0x2591;

                    for (int j = 0; j < Console.BufferHeight; j++)
                    {
                        if (j <= ceiling)
                        {
                            ScreenBuffer.Draw(" ", i, j);
                        }
                        else if (j > ceiling && j <= floor)
                        {
                            ScreenBuffer.Draw(shade.ToString(), i, j);
                        }
                        else
                        {
                            ScreenBuffer.Draw(".", i, j);
                        }
                    }
                }
                ScreenBuffer.DrawScreen();
            }

        }
    }
}
