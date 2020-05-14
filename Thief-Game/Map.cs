﻿using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System;

namespace Thief_Game
{
    //Lev
    /// <summary>
    /// Класс инициализации игрового уровня
    /// </summary>
    class Map
    {
        // Enum as flags for CheckWallCollision method.
        private enum Dimension
        {
            mvUp,
            mvDown,
            mvRight,
            mvLeft
        }
        
        //Монстр с координатами { X = 1, Y = 2 } на форме находится в позиции
        //PositionMap[1, 2] = (70, 150)

        private List<Wall> Walls;
        private List<Monster> Monsters;
        private Pacman Pacman;
        private List<SmallPoint> Points;
        private List<Energizer> Energizers;

        //Я нигде не использую IMovable
        public Map()
        {
            var pattern = new LevelLoader().ParseFile();

            Walls = new List<Wall>();
            Monsters = new List<Monster>();
            Points = new List<SmallPoint>();
            Energizers = new List<Energizer>();

            InitWalls(pattern);
            InitMonsters(pattern);
            InitPlayer(pattern);
            InitSmallPoints(pattern);
            InitEnergizers(pattern);

            Application.Run(new Scene(Draw, MovePacmanUp, MovePacmanDown, MovePacmanRight, MovePacmanLeft, Redraw));
        }

        private void InitWalls(LevelPattern pattern)
        {
            //При инициализации уровня создаем стены
            foreach (var wall in pattern.Walls)
            {
                Walls.Add(wall);
            }
        }

        public void InitMonsters(LevelPattern pattern)
        {
            //При инициализации уровня создаем монстров
            foreach (var monster in pattern.MonsterSpawns)
            {
                Monsters.Add(monster);
            }
        }

        public void InitPlayer(LevelPattern pattern)
        {
            //При инициализации уровня создаем игрока
            Pacman = new Pacman(Pacman.StartX, Pacman.StartY, 10);
        }

        public void InitSmallPoints(LevelPattern pattern)
        {
            foreach(var point in pattern.SmallPoints)
            {
                Points.Add(point);
            }
        }

        public void InitEnergizers(LevelPattern pattern)
        {
            foreach(var energizer in pattern.Energizers)
            {
                Energizers.Add(energizer);
            }
        }

        public void MovePacmanDown()
        {
            if (CheckWallCollision(Pacman, Walls, Dimension.mvDown))
                Pacman.MoveDown();
        }
        public void MovePacmanUp()
        {
            //if (CheckWallCollision(Pacman, Walls, Dimension.mvUp))
            if (CheckWallCollision(Pacman, Walls, Dimension.mvUp))   
                Pacman.MoveUp();
        }
        public void MovePacmanRight()
        {
            if (CheckWallCollision(Pacman, Walls, Dimension.mvRight))
                Pacman.MoveRight();
        }
        public void MovePacmanLeft()
        {
            if (CheckWallCollision(Pacman, Walls, Dimension.mvLeft))
                Pacman.MoveLeft();
        }
        public void Redraw(Graphics graphics) => Pacman.Redraw(graphics);
        
        private bool CheckWallCollision(Pacman Pacman , List<Wall> Walls, Dimension DimFlag)
        {
            int pacmanX = Pacman.CurrentPositionX;
            int pacmanY = Pacman.CurrentPositionY;
            bool moveFlag = true;

            if (DimFlag == Dimension.mvUp)
                pacmanY -= Dimensions.StepY;
            else if (DimFlag == Dimension.mvDown)
                pacmanY += Dimensions.StepY;
            else if (DimFlag == Dimension.mvRight)
                pacmanX += Dimensions.StepX;
            else
                pacmanX -= Dimensions.StepX;

            foreach (Wall wall in Walls)
            {
                int wallX = wall.CurrentPositionX * Dimensions.SpriteHeightPixels;
                int wallY = wall.CurrentPositionY * Dimensions.SpriteHeightPixels;

                if ((pacmanY == wallY)
                    && (pacmanX == wallX))
                {
                    moveFlag = false;
                    break;
                }
            }

            return moveFlag;
        }
        //Произошло измнение - перерисовали карту
        public void Draw(Graphics graphics)
        {
            for (int i = 0; i < Walls.Count; i++)
            {
                var wall = Walls[i];
                var posX = (float)(wall.CurrentPositionX * Dimensions.SpriteWidthPixels);
                var posY = (float)(wall.CurrentPositionY * Dimensions.SpriteHeightPixels);

                graphics.DrawImage(wall.View, posX, posY, Dimensions.SpriteWidthPixels, Dimensions.SpriteHeightPixels);
            }

            for (int i = 0; i < Monsters.Count; i++)
            {
                var monster = Monsters[i];
                var posX = (float)(monster.CurrentPositionX * Dimensions.SpriteWidthPixels);
                var posY = (float)(monster.CurrentPositionY * Dimensions.SpriteHeightPixels);

                graphics.DrawImage(monster.View, posX, posY, Dimensions.SpriteWidthPixels, Dimensions.SpriteHeightPixels);
            }

            for (int i = 0; i < Energizers.Count; i++)
            {
                var energizer = Energizers[i];
                var posX = (float)(energizer.CurrentPositionX * Dimensions.SpriteWidthPixels);
                var posY = (float)(energizer.CurrentPositionY * Dimensions.SpriteHeightPixels);

                graphics.DrawImage(energizer.View, posX, posY, Dimensions.SpriteWidthPixels, Dimensions.SpriteHeightPixels);
            }

            for (int i = 0; i < Points.Count; i++)
            {
                var point = Points[i];
                var posX = (float)(point.CurrentPositionX * Dimensions.SpriteWidthPixels);
                var posY = (float)(point.CurrentPositionY * Dimensions.SpriteHeightPixels);

                graphics.DrawImage(point.View, posX, posY, Dimensions.SpriteWidthPixels, Dimensions.SpriteHeightPixels);
            }
        }
    }
}
