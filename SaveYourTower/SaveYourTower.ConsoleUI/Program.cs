﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SaveYourTower.GameEngine;
using SaveYourTower.GameEngine.DataContainers;
using SaveYourTower.GameEngine.GameObjects;
using SaveYourTower.GameEngine.GameObjects.Base;

namespace SaveYourTower.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Save Your Tower\n\n\n\n\tPress any key to start");
            Console.ReadKey();

            Game game = new Game(new Point(50, 30));
            game.Output += ReDraw;
            game.Input += Input;
            game.Start();

            if (game.GameStatus == Status.IsExit)
            {
                Console.Clear();
                Console.WriteLine("Thanks for playing. \n\n\n \t \t Your score: {0}", game.GetScore());
            }
        }

        public static void Input(Game game)
        {
            switch (ListenKey())
            {
                case ConsoleKey.Escape:
                    game.Stop();
                    break;
                case ConsoleKey.P:
                    if (game.GameStatus == Status.IsRuning)
                    {
                        game.Pause();
                    }
                    else if (game.GameStatus == Status.IsPaused)
                    {
                        game.Restore();
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    game.Rotate(10d / 180d * Math.PI);
                    break;
                case ConsoleKey.RightArrow:
                    game.Rotate(-10d / 180d * Math.PI);
                    break;
                case ConsoleKey.Spacebar:
                    game.Fire();
                    break;
                case ConsoleKey.D1:
                    game.BuyGameObject(new Turret(game.GameField, AskPosition(game.GameField), 1, 5, 10, cost: 1));
                    break;
                case ConsoleKey.D2:
                    game.BuyGameObject(new Mine(game.GameField, AskPosition(game.GameField), cost: 1));
                    break;
                case ConsoleKey.D:
                    Point objectPosition = AskPosition(game.GameField);
                    GameObject gameObject = game.GameField.GameObjects.Find(obj => { return (obj.Position.Equals(objectPosition)); });
                    game.SaleGameObject(gameObject);
                    break;
            }
        }

        public static void ReDraw(Field gameField)
        {
            int playerC = 0;
            Console.Clear();
            string output = "";

            for (var i = 0; i < gameField.Size.X; i++)
            {
                for (var j = 0; j < gameField.Size.Y; j++)
                {
                    foreach (var obj in gameField.GameObjects)
                    {
                        if ((int)obj.Position.X == i && (int)obj.Position.Y == j)
                        {
                            if (obj is Tower)
                            {
                                output += '0';
                            } 
                            else if (obj is Enemy)
                            {
                                output += '*';
                            }
                            else if (obj is CannonBall)
                            {
                                output += '.';
                            }
                            else if (obj is Turret)
                            {
                                output += 'o';
                            }
                            else if (obj is Mine)
                            {
                                output += '+';
                            }
                            playerC++;
                            break;
                        }
                    }

                    if (playerC <= 0)
                    {
                        if ((i == 0) || (j == 0) || (i == (int)gameField.Size.X - 1) || (j == (int)gameField.Size.Y - 1))
                            output += '▓';
                        else
                            output += " ";
                    }
                    playerC -= playerC <= 0 ? 0 : 1;
                }
                output += '\n';
            }

            var tower = gameField.GameObjects.Find(obj => { return (obj is Tower); });
            Console.WriteLine("Score : {0} \t\tAngle:{1:00}\t\t LifePoints: {2}", gameField.GameScore.Value, tower.Direction.Angle * 180 / 3.14, tower.LifePoints);

            output += "\n";
            output += "Press P to pause\n";
            output += "Press 1 to place turret\n";
            output += "Press 2 to place mine\n";

            Console.Write(output);
        }

        public static ConsoleKey ListenKey()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey().Key;
                ClearInputKeysBuffer();
                return key;
            }
            return 0;
        }

        public static void ClearInputKeysBuffer()
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }

        public static Point AskPosition(Field gameField)
        {
            Console.CursorSize = 100;
            Point position = new Point((gameField.Size.X / 2), (gameField.Size.Y / 2));

            while (true)
            {
                ConsoleKey key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        position.X -= ((position.X > 2) ? 1 : 0);
                        break;
                    case ConsoleKey.DownArrow:
                        position.X += ((position.X < (gameField.Size.X - 2)) ? 1 : 0);
                        break;
                    case ConsoleKey.LeftArrow:
                        position.Y -= ((position.Y > 1) ? 1 : 0);
                        break;
                    case ConsoleKey.RightArrow:
                        position.Y += ((position.Y < (gameField.Size.Y - 2)) ? 1 : 0);
                        break;
                    case ConsoleKey.Enter:
                        return position;
                    case ConsoleKey.Escape:
                        return null;
                }
                ReDraw(gameField);
                Console.SetCursorPosition((int)position.Y, (int)position.X + 1);
            }
        }
    }
}
