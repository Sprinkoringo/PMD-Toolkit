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
    public class AtlasSheet : Texture {

        //Sprite drawing origin
        public enum SpriteVOrigin {
            Top,
            Center,
            Bottom
        };
        public enum SpriteHOrigin {
            Left,
            Center,
            Right
        };

        //Sprite clips
        protected List<Rectangle> mClips;
        //VBO data
        //one list of vertices
        protected int mVertexDataBuffer;
        //many lists of sprites
        protected int[] mIndexBuffers;


        public AtlasSheet() {
	        //Initialize vertex buffer data
	        mVertexDataBuffer = 0;
	        mIndexBuffers = null;
            mClips = new List<Rectangle>();
        }

        public override void Dispose() {
	        //Clear sprite sheet data 
            FreeSheet();
            base.Dispose();
        }

        public void FreeSheet()
        {
            //Clear vertex buffer
            if (mVertexDataBuffer != 0)
            {
                GL.DeleteBuffers(1, ref mVertexDataBuffer);
                mVertexDataBuffer = 0;
            }
            //Clear index buffers
            if (mIndexBuffers != null)
            {
                GL.DeleteBuffers(mClips.Count, mIndexBuffers);
                mIndexBuffers = null;
            }
            //Clear clips
            mClips.Clear();
        }

        public int AddClipSprite( Rectangle newClip ) {
	        //Add clip and return index
	        mClips.Add(newClip);
	        return mClips.Count - 1;
        }

        public Rectangle GetClip( int index ) {
	        return mClips[ index ];
        }

        public void GenerateDataBuffer(SpriteVOrigin vOrigin, SpriteHOrigin hOrigin) {
	        //If there is a texture loaded and clips to make vertex data from
	        if( TextureID != 0 && mClips.Count > 0 ) {
		        //Allocate vertex buffer data
		        int totalSprites = mClips.Count;
		        VertexData[] vertexData = new VertexData[ totalSprites * 4 ];
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
		        float vBottom = 0;
		        float vLeft = 0;
		        float vRight = 0;

		        for( int i = 0; i < totalSprites; ++i ) {
			        //Initialize indices
			        spriteIndices[ 0 ] = i * 4 + 0;
			        spriteIndices[ 1 ] = i * 4 + 1;
			        spriteIndices[ 2 ] = i * 4 + 2;
			        spriteIndices[ 3 ] = i * 4 + 3;

			        //Set origin (vertical)
                    switch (vOrigin) {
                        case SpriteVOrigin.Top: {
                            vTop = 0;
					        vBottom = mClips[ i ].Height;
                        }
                            break;
                        case SpriteVOrigin.Bottom: {
                            vTop = -mClips[ i ].Height;
					        vBottom = 0;
                        }
                            break;
                        default: {
                            vTop = (float)-mClips[ i ].Height / 2;
					        vBottom = (float)mClips[ i ].Height / 2;
                        }
                            break;
                    }
                    //Set origin (horizontal)
                    switch (hOrigin) {
                        case SpriteHOrigin.Left: {
                            vLeft = 0;
					        vRight = mClips[ i ].Width;
                        }
                            break;
                        case SpriteHOrigin.Right: {
                            vLeft = -mClips[ i ].Width;
					        vRight = 0;
                        }
                            break;
                        default: {
					        vLeft = (float)-mClips[ i ].Width / 2;
					        vRight = (float)mClips[ i ].Width / 2;
                        }
                            break;
                    }

			        //Top left
                    vertexData[spriteIndices[0]] = new VertexData(vLeft, vTop, ((float)mClips[i].Left) / tW, ((float)mClips[i].Top) / tH);
			        //Top right
                    vertexData[spriteIndices[1]] = new VertexData(vRight, vTop, ((float)mClips[i].Right) / tW, ((float)mClips[i].Top) / tH);
			        //Bottom right
                    vertexData[spriteIndices[2]] = new VertexData(vRight, vBottom, ((float)mClips[i].Right) / tW, ((float)mClips[i].Bottom) / tH);
			        //Bottom left
                    vertexData[spriteIndices[3]] = new VertexData(vLeft, vBottom, ((float)mClips[i].Left) / tW, ((float)mClips[i].Bottom) / tH);

			        //Bind sprite index buffer data
			        GL.BindBuffer(BufferTarget.ElementArrayBuffer, mIndexBuffers[ i ] );
                    GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(4 * sizeof(int)), spriteIndices, BufferUsageHint.StaticDraw);

		        }
		        //Bind vertex data
		        GL.BindBuffer(BufferTarget.ArrayBuffer, mVertexDataBuffer );
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(totalSprites * 4 * VertexData.SizeInBytes), vertexData, BufferUsageHint.StaticDraw);
		        
	        } else {
		         if( TextureID == 0 ) {
			         throw new Exception( "No texture to render with!" );
		         }
		         if( mClips.Count <= 0 ) {
			         throw new Exception( "No clips to generate vertex data from!" );
		         }
	        }
        }

        public void RenderSprite( int index ) {
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
		        GL.BindBuffer(BufferTarget.ElementArrayBuffer, mIndexBuffers[ index ] );
		        GL.DrawElements(BeginMode.Quads, 4, DrawElementsType.UnsignedInt, 0 );
		        //Disable vertex and texture coordinate arrays
                mTextureProgram.DisableVertexPointer();
                mTextureProgram.DisableTexCoordPointer();
	        }
        }

    }
}
