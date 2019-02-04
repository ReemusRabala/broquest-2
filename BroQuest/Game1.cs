using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BroQuest
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background;
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

            string node1Text = System.IO.File.ReadAllText("node1.txt");
            testNode = new Node(background, node1Text);

            string node2Text = System.IO.File.ReadAllText("node2.txt");
            testNode2 = new Node(background, node2Text);

            currentNode = testNode2;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("arial_regular_14");
            background = Content.Load<Texture2D>("background");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            if (isLeftMouseReady == true)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    isLeftMouseReady = false;

                    if (currentNode == testNode)
                    {
                        currentNode = testNode2;
                    }
                    else
                    {
                        currentNode = testNode;
                    }
                }
            }

            if (isLeftMouseReady == false)
            {
                if (mouseState.LeftButton == ButtonState.Released)
                {
                    isLeftMouseReady = true;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            
            currentNode.Draw(spriteBatch, font);

            string text;
            if (isLeftMouseReady == true)
            {
                text = "left mouse is ready";
            }
            else
            {
                text = "left mouse is not ready";
            }
            spriteBatch.DrawString(font, text, new Vector2(100, 400), Color.White);

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
