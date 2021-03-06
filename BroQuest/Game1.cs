﻿using Microsoft.Xna.Framework;
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

            string[] filePathList = System.IO.Directory.GetFiles("world");
            
            foreach (string filePath in filePathList)
            {
                string nodeText = System.IO.File.ReadAllText(filePath);
                string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
                Node node = new Node(nodeText, fileName);
                node.LoadContent(Content);
                nodeDict.Add(fileName, node);
            }

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
                    string clickResult = currentNode.Input(mouseState);
                    
                    if (clickResult != null)
                    {
                        currentNode = nodeDict[clickResult];
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

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    class Node
    {
        Vector2 textOrigin = new Vector2(205, 55);
        float wordStep = 5;
        int textBoxWidth = 585;
        int lineStep = 20;

        Vector2 buttonOrigin = new Vector2(15, 15);
        int buttonStep = 35;

        public Node(string text, string key)
        {
            string[] lineArray = text.Split(new[] { Environment.NewLine },StringSplitOptions.None);
            List<string> lineList = new List<string>(lineArray);

            TitleString = lineList[0];
            lineList.RemoveAt(0);

            string[] wordList = lineList[0].Split();
            WordList = new List<string>(wordList);

            List<string> optionList = new List<string>(lineList);
            optionList.RemoveAt(0);

            LinkDict = new Dictionary<string, string>();
            foreach (string option in optionList)
            {
                if (option != null)
                {
                    string[] linkList = option.Split();
                    string linkString = linkList[1];
                    string linkKey = linkList[0];
                    LinkDict.Add(linkString, linkKey);
                }
            }
            
            Key = key;
        }

        Texture2D Texture { get; set; }
        Texture2D TextBackgroundTexture { get; set; }
        Texture2D NavigationBarTexture { get; set; }
        Texture2D TitleBarTexture { get; set; }
        Texture2D ButtonTexture { get; set; }
        string TitleString { get; set; }
        List<string> WordList { get; set; }
        string Key { get; }
        Dictionary<string, string> LinkDict { get; set; }

        public void LoadContent(ContentManager contentManager)
        {
            Texture = contentManager.Load<Texture2D>("background");
            TextBackgroundTexture = contentManager.Load<Texture2D>("text_background");
            NavigationBarTexture = contentManager.Load<Texture2D>("navigation_bar");
            TitleBarTexture = contentManager.Load<Texture2D>("title_bar");
            ButtonTexture = contentManager.Load<Texture2D>("button");
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, MouseState mouseState)
        {
            spriteBatch.Draw(Texture, new Vector2(0, 0), Color.White);

            spriteBatch.Draw(TitleBarTexture, new Vector2(200, 10), Color.White);
            spriteBatch.DrawString(font, TitleString, new Vector2(205, 15), Color.White);

            DrawNavigationBar(spriteBatch, font, mouseState);

            DrawTextBox(spriteBatch, font);
        }

        private void DrawNavigationBar(SpriteBatch spriteBatch, SpriteFont font, MouseState mouseState)
        {
            spriteBatch.Draw(NavigationBarTexture, new Vector2(10, 10), Color.White);
            int buttonY = 0;
            foreach (string linkString in LinkDict.Keys)
            {
                Color color = Color.White;
                Point mousePos = mouseState.Position;

                Vector2 buttonPos = buttonOrigin + new Vector2(0, buttonY);
                if ((mousePos.X > 15) && (mousePos.X < 185) && (mousePos.Y > buttonPos.Y) && (mousePos.Y < buttonPos.Y + 30))
                {
                    color = Color.CornflowerBlue;
                }

                spriteBatch.Draw(ButtonTexture, buttonPos, color);

                Vector2 textPos = buttonPos + new Vector2(5, 5);
                spriteBatch.DrawString(font, linkString, textPos, Color.White);
                buttonY = buttonY + buttonStep;
            }
        }

        private void DrawTextBox(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.Draw(TextBackgroundTexture, new Vector2(200, 50), Color.White);
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

        public string Input(MouseState mouseState)
        {
            string result = null;
            Point mousePos = mouseState.Position;

            float buttonY = 0;
            float Xmin = buttonOrigin.X;
            float Xmax = 185;
            foreach (string link in LinkDict.Values)
            {
                float Ymin = buttonOrigin.Y + (buttonY * buttonStep);
                float Ymax = Ymin + 30;

                if ((mousePos.X > Xmin) && (mousePos.X < Xmax) && (mousePos.Y < Ymax) && (mousePos.Y > Ymin))
                {
                    result = link;
                }

                buttonY = buttonY + 1;
            }

            return result;
        }
    }
}
