using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;

namespace BroQuest
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        Dictionary<string, Node> nodeDict = new Dictionary<string, Node>();

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
            Node testNode = new Node(node1Text, "node1");
            testNode.LoadContent(Content);

            string node2Text = System.IO.File.ReadAllText("node2.txt");
            Node testNode2 = new Node(node2Text, "node2");
            testNode2.LoadContent(Content);

            nodeDict.Add("node1", testNode);
            nodeDict.Add("node2", testNode2);

            currentNode = nodeDict["node1"];
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

                    if (currentNode == nodeDict["node1"])
                    {
                        currentNode = nodeDict["node2"];
                    }
                    else
                    {
                        currentNode = nodeDict["node1"];
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
            
            currentNode.Draw(spriteBatch, font, Mouse.GetState());

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

        public Node(string text, string key)
        {
            string[] lineList = text.Split(new[] { Environment.NewLine },StringSplitOptions.None);

            string[] wordList = lineList[0].Split();
            WordList = new List<string>(wordList);

            string[] linkList = lineList[1].Split();
            LinkString = linkList[1];
            LinkKey = linkList[0];

            Key = key;
        }

        Texture2D Texture { get; set; }
        Texture2D TextBackgroundTexture { get; set; }
        Texture2D NavigationBarTexture { get; set; }
        Texture2D ButtonTexture { get; set; }
        List<string> WordList { get; set; }
        string Key { get; }
        string LinkString { get; }
        string LinkKey { get; }

        public void LoadContent(ContentManager contentManager)
        {
            Texture = contentManager.Load<Texture2D>("background");
            TextBackgroundTexture = contentManager.Load<Texture2D>("text_background");
            NavigationBarTexture = contentManager.Load<Texture2D>("navigation_bar");
            ButtonTexture = contentManager.Load<Texture2D>("button");
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, MouseState mouseState)
        {
            spriteBatch.Draw(Texture, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(TextBackgroundTexture, new Vector2(200, 40), Color.White);
            spriteBatch.Draw(NavigationBarTexture, new Vector2(10, 10), Color.White);

            Color color = Color.White;
            Point mousePos = mouseState.Position;
            if ((mousePos.X > 15) && (mousePos.X < 185) && (mousePos.Y > 15) && (mousePos.Y < 45))
            {
                color = Color.CornflowerBlue;
            }

            spriteBatch.Draw(ButtonTexture, new Vector2(15, 15), color);
            spriteBatch.DrawString(font, LinkString, new Vector2(20, 20), Color.White);

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
