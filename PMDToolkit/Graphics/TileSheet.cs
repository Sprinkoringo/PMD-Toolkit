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
    public class TileSheet : Texture {
        //TODO: add single vertex use optimization
        //VBO data
        //one list of vertices
        int mVertexDataBuffer;
        //many lists of sprites
        int[] mIndexBuffers;

        public int MaxX { get; set; }
        public int MaxY { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        
        public TileSheet() {
	        //Initialize vertex buffer data
	        mVertexDataBuffer = 0;
	        mIndexBuffers = null;
        }

        public override void Dispose() {
	        //Clear sprite sheet data 
	        FreeSheet();
            base.Dispose();
        }

        public void FreeSheet()
        {
            //Console.WriteLine("F:"+TextureID);
            //Clear vertex buffer
            if (mVertexDataBuffer != 0)
            {
                GL.DeleteBuffers(1, ref mVertexDataBuffer);
                mVertexDataBuffer = 0;
            }
            //Clear index buffers
            if (mIndexBuffers != null)
            {
                GL.DeleteBuffers(MaxX * MaxY, mIndexBuffers);
                mIndexBuffers = null;
            }
            TileWidth = 0;
            TileHeight = 0;
            MaxX = 0;
            MaxY = 0;
        }

        public virtual void GenerateDataBuffer(int tileWidth, int tileHeight) {
	        //If there is a texture loaded and clips to make vertex data from
            int maxX = ImageWidth / tileWidth;
            int maxY = ImageHeight / tileHeight;
	        if( TextureID != 0 && maxX > 0 && maxY > 0) {
                MaxX = maxX;
                MaxY = maxY;
                TileWidth = tileWidth;
                TileHeight = tileHeight;
		        //Allocate vertex buffer data
                int totalSprites = MaxX * MaxY;
		        VertexData[] vertexData = new VertexData[ 4*totalSprites ];
		        mIndexBuffers = new int[ totalSprites ];

		        //Allocate vertex data buffer name
		        GL.GenBuffers( 1, out mVertexDataBuffer );
		        //Allocate index buffers names
		        GL.GenBuffers( totalSprites, mIndexBuffers );
		        //Go through clips
		        float tW = ImageWidth;
		        float tH = ImageHeight;
		        int[] spriteIndices = new int[ 4 ] { 0, 0, 0, 0 };

		        //Origin variables
		        float vTop = 0;
                float vBottom = TileHeight;
		        float vLeft = 0;
                float vRight = TileWidth;

                for (int y = 0; y < MaxY; y++) {
                    for (int x = 0; x < MaxX; x++) {
                        //Initialize indices
                        spriteIndices[0] = 4 * (y * MaxX + x);
                        spriteIndices[1] = 4 * (y * MaxX + x) + 1;
                        spriteIndices[2] = 4 * (y * MaxX + x) + 2;
                        spriteIndices[3] = 4 * (y * MaxX + x) + 3;

                        float clipTop = y * TileHeight;
                        float clipBottom = (y+1) * TileHeight;
                        float clipLeft = x * TileWidth;
                        float clipRight = (x+1) * TileWidth;

                        //Top left
                        vertexData[spriteIndices[0]] = new VertexData(vLeft, vTop, clipLeft / tW, clipTop / tH);
                        //Top right
                        vertexData[spriteIndices[1]] = new VertexData(vRight, vTop, clipRight / tW, clipTop / tH);
                        //Bottom right
                        vertexData[spriteIndices[2]] = new VertexData(vRight, vBottom, clipRight / tW, clipBottom / tH);
                        //Bottom left
                        vertexData[spriteIndices[3]] = new VertexData(vLeft, vBottom, clipLeft / tW, clipBottom / tH);

                        //Bind sprite index buffer data
                        GL.BindBuffer(BufferTarget.ElementArrayBuffer, mIndexBuffers[y*MaxX+x]);
                        GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(4 * sizeof(int)), spriteIndices, BufferUsageHint.StaticDraw);
                    }
                }

		        //Bind vertex data
		        GL.BindBuffer(BufferTarget.ArrayBuffer, mVertexDataBuffer );
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(totalSprites * 4 * VertexData.SizeInBytes), vertexData, BufferUsageHint.StaticDraw);
		        
	        } else {
		         if( TextureID == 0 ) {
			         throw new Exception( "No texture to render with!" );
		         }
                 if (!(maxX > 0 && maxY > 0)) {
			         throw new Exception( "No tile possible!" );
		         }
	        }
        }

        public void RenderTile( int x, int y ) {
            if (x >= MaxX || y >= MaxY)
            {
                TextureManager.ErrorTexture.RenderTile(0, 0);
                return;
            }
	        //Sprite sheet data exists
	        if( mVertexDataBuffer != 0 ) {
		        //Set texture
		        GL.BindTexture(TextureTarget.Texture2D, TextureID );
		        //Enable vertex and texture coordinate arrays
		        mTextureProgram.EnableVertexPointer();
		        mTextureProgram.EnableTexCoordPointer();
		        //Bind vertex data
		        GL.BindBuffer(BufferTarget.ArrayBuffer, mVertexDataBuffer );
		        //Set texture coordinate data
                mTextureProgram.SetTexCoordPointer(VertexData.SizeInBytes, VertexData.TexCoordOffset);
		        //Set vertex data
                mTextureProgram.SetVertexPointer(VertexData.SizeInBytes, VertexData.PositionOffset);
		        //Draw quad using vertex data and index data
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, mIndexBuffers[y * MaxX + x]);
		        GL.DrawElements(BeginMode.Quads, 4, DrawElementsType.UnsignedInt, 0 );
		        //Disable vertex and texture coordinate arrays
                mTextureProgram.DisableVertexPointer();
                mTextureProgram.DisableTexCoordPointer();
	        }
        }

    }
}
