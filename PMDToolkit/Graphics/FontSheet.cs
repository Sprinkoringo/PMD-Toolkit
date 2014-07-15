using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace PMDToolkit.Graphics {
    public class FontSheet : AtlasSheet {

        int mCellW;
        int mCellH;

        int mRows;
        int mCols;

        int mCharOffset;
        
        public FontSheet() {
            mCellW = 0;
            mCellH = 0;
            mRows = 0;
            mCols = 0;
            
            mCharOffset = 0;
        }

        public void LoadFont(string path, int startChar, ref List<int> charList, ref int maxLineHeight, ref int space)
        {
            Color4 cellPixel = new Color4(0, 0, 0, 255);
            Color4 borderPixel = new Color4(255, 0, 0, 255);
	        //Get rid of the font if it exists
	        freeFont();
	        //Image pixels loaded
	        if( LoadPixelsFromFile32( path ) ) {
		        //Get cell dimensions
		        mCellW = 0;
                for (int x = 1; x < ImageWidth; x++)
                {
                    if (GetPixel(x, 1) == cellPixel)
                        break;
                    mCellW++;
                }
                mCellW += 2;
                mCols = ImageWidth / mCellW;

                mCellH = 0;
                for (int y = 1; y < ImageHeight; y++)
                {
                    if (GetPixel(y, 1) == cellPixel)
                        break;
                    mCellH++;
                }
                mCellH += 2;
                mRows = ImageHeight / mCellH;
                		        
		        //Begin parsing bitmap font
                mCharOffset = startChar;
		        int currentChar = startChar;
		
		        //Go through cell rows
                for (int rows = 0; rows < mRows; rows++)
                {
                    //Go through each cell column in the row
                    for (int cols = 0; cols < mCols; cols++)
                    {
                        //Begin cell parsing
                        //Set base offsets
                        int currentX = mCellW * cols;
                        int currentY = mCellH * rows;
                        
                        if (GetPixel(currentX + 1, currentY + 1) == borderPixel)
                        {
                            if (currentChar == 32)
                            {
                                int width = 0;
                                for (int x = 1; x < ImageWidth; x++)
                                {
                                    if (GetPixel(currentX + x, currentY + 1) != borderPixel)
                                        break;
                                    width++;
                                }
                                width -= 2;

                                if (width > space)
                                    space = width;
                            }
                            else
                            {
                                int width = 0;
                                for (int x = 1; x < ImageWidth; x++)
                                {
                                    if (GetPixel(currentX + x, currentY + 1) != borderPixel)
                                        break;
                                    width++;
                                }
                                width -= 2;

                                int height = 0;
                                for (int y = 1; y < ImageHeight; y++)
                                {
                                    if (GetPixel(currentX + 1, currentY + y) != borderPixel)
                                        break;
                                    height++;
                                }
                                height -= 2;

                                //Initialize clip
                                Rectangle nextClip = new Rectangle(mCellW * cols + 2, mCellH * rows + 2, width, height);

                                if (maxLineHeight < nextClip.Height)
                                    maxLineHeight = nextClip.Height;

                                charList.Add(currentChar);
                                mClips.Add(nextClip);
                            }
                        }

                        //Go to the next character
                        currentChar++;
                    }
                }

                LoadTextureFromPixels32();
                GenerateDataBuffer(SpriteVOrigin.Top, SpriteHOrigin.Left);
                
	        } else {
                throw new Exception("Could not load bitmap font image: " + path + "!");
	        }
        }


        private void freeFont() {
	        //Get rid of sprite sheet
	        FreeTexture();
        }


        public void BeginRender()
        {
            //Set texture
            GL.BindTexture(TextureTarget.Texture2D, TextureID);
            //Enable vertex and texture coordinate arrays
            Graphics.TextureManager.TextureProgram.EnableVertexPointer();
            Graphics.TextureManager.TextureProgram.EnableTexCoordPointer();
            //Bind vertex data
            GL.BindBuffer(BufferTarget.ArrayBuffer, mVertexDataBuffer);
            //Set texture coordinate data
            Graphics.TextureManager.TextureProgram.SetTexCoordPointer(VertexData.SizeInBytes, VertexData.TexCoordOffset);
            //Set vertex data
            Graphics.TextureManager.TextureProgram.SetVertexPointer(VertexData.SizeInBytes, VertexData.PositionOffset);
        }


        public void EndRender()
        {
            //Disable vertex and texture coordinate arrays
            Graphics.TextureManager.TextureProgram.DisableVertexPointer();
            Graphics.TextureManager.TextureProgram.DisableTexCoordPointer();
        }

        public void RenderFontSprite( int sprite ) {
	        //If there is a texture to render from

	        if( TextureID != 0 ) {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, mIndexBuffers[sprite]);
                GL.DrawElements(BeginMode.Quads, 4, DrawElementsType.UnsignedInt, 0);
	        }

        }

    }
}
