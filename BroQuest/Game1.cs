﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BroQuest
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D saluteTexture;
        SpriteFont font;

        Node testNode;
        Node testNode2;

        Node currentNode;

        bool isLeftMouseReady = true;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            IsMouseVisible = true;

            testNode = new Node(saluteTexture, "I am test node");
            testNode2 = new Node(saluteTexture, "I am the second node");

            currentNode = testNode2;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("arial_regular_14");
            saluteTexture = Content.Load<Texture2D>("salute");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            
            currentNode.Draw(spriteBatch, font);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    class Node
    {
        public Node(Texture2D texture, string text)
        {
            Texture = texture;
            Text = text;
        }

        Texture2D Texture { get; set; }
        string Text { get; set; }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.Draw(Texture, new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(font, Text, new Vector2(400, 200), Color.White);
        }
    }
}
