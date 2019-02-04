using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace BroQuest
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
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
            testNode = new Node(node1Text);
            testNode.LoadContent(Content);

            string node2Text = System.IO.File.ReadAllText("node2.txt");
            testNode2 = new Node(node2Text);
            testNode2.LoadContent(Content);

            currentNode = testNode2;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("arial_regular_14");
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

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            
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
        Vector2 textOrigin = new Vector2(210, 50);
        float wordStep = 5;
        int textBoxWidth = 380;
        int lineStep = 20;

        public Node(string text)
        {
            string[] wordList = text.Split();
            WordList = new List<string>(wordList);
        }

        Texture2D Texture { get; set; }
        Texture2D TextBackgroundTexture { get; set; }
        List<string> WordList { get; set; }

        public void LoadContent(ContentManager contentManager)
        {
            Texture = contentManager.Load<Texture2D>("background");
            TextBackgroundTexture = contentManager.Load<Texture2D>("text_background");
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.Draw(Texture, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(TextBackgroundTexture, new Vector2(200, 40), Color.White);

            float textLength = 0;
            float currentLine = 0;
            foreach (string word in WordList)
            {
                Vector2 wordLocation = textOrigin + new Vector2(textLength, currentLine * lineStep);
                textLength = wordStep + textLength + font.MeasureString(word).X;

                if (textLength < textBoxWidth)
                {
                    spriteBatch.DrawString(font, word, wordLocation, Color.White);
                }
                else
                {
                    textLength = 0;
                    currentLine = currentLine + 1;
                    wordLocation = textOrigin + new Vector2(textLength, currentLine * lineStep);
                    textLength = wordStep + textLength + font.MeasureString(word).X;
                    spriteBatch.DrawString(font, word, wordLocation, Color.White);
                }
            }
        }
    }
}
