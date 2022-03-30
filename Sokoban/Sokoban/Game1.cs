using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace Sokoban
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        string levelPath = "level1.txt";
        string levelPath2 = "level2.txt";
        char[,] map;
        const int tileSize = 64;
        int width, height;
        Texture2D playerTexture, wallTexture, groundTexture, boxTexture, objectiveTexture;
        KeyboardManager km;

        //PLAYER
        Vector2 playerPos;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            km = new KeyboardManager();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            playerTexture = Content.Load<Texture2D>("Character4");
            wallTexture = Content.Load<Texture2D>("Wall_Black");
            groundTexture = Content.Load<Texture2D>("GroundGravel_Grass");
            boxTexture = Content.Load<Texture2D>("Crate_Beige");
            objectiveTexture = Content.Load<Texture2D>("EndPoint_Black");

            LoadLevel();
            

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            km.Update();

            Movement();
            Vector2 teste = playerPos;

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    char currentSymbol = map[x, y];
                    Console.Write(currentSymbol);


                    switch (currentSymbol)
                    {
                        case 'X':
                            _spriteBatch.Draw(wallTexture, new Vector2(x, y) * tileSize,Color.White);
                            break;

                        case ' ':
                            _spriteBatch.Draw(groundTexture, new Vector2(x, y) * tileSize, Color.White);
                            break;
                        case 'B':
                            _spriteBatch.Draw(boxTexture, new Vector2(x, y) * tileSize, Color.White);
                            break;
                        case '.':
                            _spriteBatch.Draw(objectiveTexture, new Vector2(x, y) * tileSize, Color.White);
                            break;

                        default:
                            _spriteBatch.Draw(groundTexture, new Vector2(x, y) * tileSize, Color.White);
                            break;
                    }
                }

            _spriteBatch.Draw(playerTexture, playerPos * tileSize, Color.White);

            // TODO: Add your drawing code here
            _spriteBatch.End();
                    base.Draw(gameTime);
        }

        void LoadLevel()
        {
            string[] lines = File.ReadAllLines("Content/" + levelPath);
            map = new char[lines[0].Length,lines.Length];

            for(int x = 0; x<lines[0].Length;x++ )
                for(int y = 0;y< lines.Length; y++)
                {
                    string currentLine = lines[y];
                    map[x, y] = currentLine[x];


                    if (currentLine[x] == 'i')
                        playerPos = new Vector2(x, y);

                }




            

            height = lines.Length;
            width = lines[0].Length;


            

            _graphics.PreferredBackBufferHeight = height * tileSize;
            _graphics.PreferredBackBufferWidth = width * tileSize;
            _graphics.ApplyChanges();


        }

        //void LoadLevel2()
        //{
        //    string[] lines = File.ReadAllLines("Content/" + levelPath2);
        //    map = new char[lines[0].Length, lines.Length];

        //    for (int x = 0;x<lines[0].Length;x++ )
        //        for (int y = 0; y<lines.Length;y++ )
        //        {
        //            string currentLine = lines[y];
        //            map[x, y] = currentLine[x];

        //            if (currentLine[x] == 'i')
        //                playerPos = new Vector2(x, y);
        //        }
        //    height = lines.Length;
        //    width= lines[0].Length;

        //    _graphics.PreferredBackBufferHeight = height * tileSize;
        //    _graphics.PreferredBackBufferWidth = width * tileSize;
        //    _graphics.ApplyChanges();
        //}


        void Movement()
        {

            Vector2 newPos = playerPos;
            Vector2 dir = Vector2.Zero;
            if (km.IsKeyPressed(Keys.W))
            {
                newPos -= Vector2.UnitY;
                dir = -Vector2.UnitY;
            }
            if (km.IsKeyPressed(Keys.A))
            {
                newPos -= Vector2.UnitX;
                dir = -Vector2.UnitX;


            }
            if (km.IsKeyPressed(Keys.S))
            {
                newPos += Vector2.UnitY;
                dir = Vector2.UnitY;


            }
            if (km.IsKeyPressed(Keys.D))
            {
                newPos += Vector2.UnitX;
                dir = Vector2.UnitX;


            }

            if (map[(int)newPos.X, (int)newPos.Y] == 'X')
                newPos = playerPos;

            else if (map[(int)newPos.X, (int)newPos.Y] == 'B')
            {
                if (map[(int)(newPos.X + dir.X), (int)(newPos.Y + dir.Y)] == ' ' ||
                    map[(int)(newPos.X + dir.X), (int)(newPos.Y + dir.Y)] == ' ')
                {
                    map[(int)(newPos.X + dir.X), (int)(newPos.Y + dir.Y)] = 'B';
                    map[(int)(newPos.X), (int)(newPos.Y)] = ' ';


                }
                else if(map[(int)(newPos.X + dir.X), (int)(newPos.Y + dir.Y)] == '.' ||
                    map[(int)(newPos.X + dir.X), (int)(newPos.Y + dir.Y)] == '.')
                {
                    map[(int)(newPos.X + dir.X), (int)(newPos.Y + dir.Y)] = 'B';
                    map[(int)(newPos.X), (int)(newPos.Y)] = ' ';
                    if (map[(int)(newPos.X + dir.X), (int)(newPos.Y + dir.Y)] == '.' ||
                    map[(int)(newPos.X - dir.X), (int)(newPos.Y - dir.Y)] == 'i')
                    {
                        map[(int)(newPos.X + dir.X), (int)(newPos.Y + dir.Y)] = 'B';
                        map[(int)(newPos.X), (int)(newPos.Y)] = '.';
                    }
                    

                }
                else newPos = playerPos;

            }
            if (map[(int)newPos.X, (int)newPos.Y] == '.')
                newPos = playerPos;




            map[(int)playerPos.X, (int)playerPos.Y] = ' ';
            playerPos = newPos;
            map[(int)playerPos.X, (int)playerPos.Y] = 'i';

            if (km.IsKeyPressed(Keys.R))
            {
                newPos = dir;
                LoadLevel();
            }






        }
    }
}
