using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.IO;
using PMDToolkit.Data;

namespace PMDToolkit.Graphics {
    public class TextureProgram : ShaderProgram {

        //Attribute locations
		int mVertexPos2DLocation;
		int mTexCoordLocation;

		//Coloring location
		int mTextureColorLocation;
		
		//Texture unit location
		int mTextureUnitLocation;

		//Projection matrix
		Matrix4 mProjectionMatrix;
		int mProjectionMatrixLocation;
		//Modelview matrix
		Matrix4 mModelViewMatrix;
		int mModelViewMatrixLocation;

        Stack<Matrix4> mStack;

        public TextureProgram() {
		    mVertexPos2DLocation = 0;
		    mTexCoordLocation = 0;
		    mProjectionMatrixLocation = 0;
		    mModelViewMatrixLocation = 0;
		    mTextureColorLocation = 0;
		    mTextureUnitLocation = 0;
            mStack = new Stack<Matrix4>();
        }

        public void LoadProgram() {
	        //Generate program
	        mProgramID = GL.CreateProgram();
	        //Load vertex shader
	        int vertexShader = LoadShaderFromFile( Paths.ShadersPath+"Texture2D.glvs", ShaderType.VertexShader );
	        //Attach vertex shader to program
	        GL.AttachShader( mProgramID, vertexShader );
	        //Create fragment shader
            int fragmentShader = LoadShaderFromFile(Paths.ShadersPath + "Texture2D.glfs", ShaderType.FragmentShader);
	        //Attach fragment shader to program
	        GL.AttachShader( mProgramID, fragmentShader );
	        //Link program
	        GL.LinkProgram( mProgramID );
	        //Check for errors
            int status_code;
            GL.GetProgram(mProgramID, ProgramParameter.LinkStatus, out status_code);
	        if( status_code != 1 ) {
		        string log = printProgramLog( mProgramID );
		        GL.DeleteProgram( mProgramID );
                mProgramID = 0;
                throw new Exception("Error linking program " + mProgramID + ": " + log);
	        }
	        //Get variable locations
	        mVertexPos2DLocation = GL.GetAttribLocation( mProgramID, "LVertexPos2D" );
	        if( mVertexPos2DLocation == -1 ) {
		        throw new Exception( "VertexPos2D is not a valid glsl program variable!" );
	        }
	        mTexCoordLocation = GL.GetAttribLocation( mProgramID, "LTexCoord" );
	        if( mTexCoordLocation == -1 ) {
		        throw new Exception( "TexCoord is not a valid glsl program variable!" );
	        }
	        mTextureColorLocation = GL.GetUniformLocation( mProgramID, "LTextureColor" );
	        if( mTextureColorLocation == -1 ) {
		        throw new Exception( "TextureColor is not a valid glsl program variable!" );
	        }
	        mTextureUnitLocation = GL.GetUniformLocation( mProgramID, "LTextureUnit" );
	        if( mTextureUnitLocation == -1 ) {
		        throw new Exception( "TextureUnit is not a valid glsl program variable!" );
	        }
	        mProjectionMatrixLocation = GL.GetUniformLocation( mProgramID, "LProjectionMatrix" );
	        if( mProjectionMatrixLocation == -1 ) {
		        throw new Exception( "ProjectionMatrix is not a valid glsl program variable!" );
	        }
	        mModelViewMatrixLocation = GL.GetUniformLocation( mProgramID, "LModelViewMatrix" );
	        if( mModelViewMatrixLocation == -1 ) {
		        throw new Exception( "ModelViewMatrix is not a valid glsl program variable!" );
	        }            
        }

        public void SetVertexPointer( int stride, int offset ) {
	        GL.VertexAttribPointer(mVertexPos2DLocation, 2, VertexAttribPointerType.Float, false, stride, offset);
        }
        public void SetTexCoordPointer( int stride, int offset ) {
            GL.VertexAttribPointer(mTexCoordLocation, 2, VertexAttribPointerType.Float, false, stride, offset);
        }

        public void EnableVertexPointer() {
	        GL.EnableVertexAttribArray( mVertexPos2DLocation );
        }
        public void DisableVertexPointer() {
	        GL.DisableVertexAttribArray( mVertexPos2DLocation );
        }
        public void EnableTexCoordPointer() {
	        GL.EnableVertexAttribArray( mTexCoordLocation );
        }
        public void DisableTexCoordPointer() {
	        GL.DisableVertexAttribArray( mTexCoordLocation );
        }

        public void SetProjection( Matrix4 matrix ) {
	        mProjectionMatrix = matrix;
        }
        public void SetModelView( Matrix4 matrix ) {
	        mModelViewMatrix = matrix;
        }
        public void LeftMultProjection(Matrix4 matrix) {
            mProjectionMatrix = Matrix4.Mult(matrix, mProjectionMatrix);
        }
        public void LeftMultModelView(Matrix4 matrix) {
            mModelViewMatrix = Matrix4.Mult(matrix, mModelViewMatrix);
        }
        public void PushModelView() {
            mStack.Push(mModelViewMatrix);
        }
        public void PopModelView() {
            mModelViewMatrix = mStack.Pop();
        }
        public void UpdateProjection() {
	        GL.UniformMatrix4( mProjectionMatrixLocation, false, ref mProjectionMatrix );
        }
        public void UpdateModelView() {
	        GL.UniformMatrix4( mModelViewMatrixLocation, false, ref mModelViewMatrix );
        }

        public void SetTextureColor( Color4 color ) {
            GL.Uniform4(mTextureColorLocation, color);
        }
        public void SetTextureUnit( int unit ) {
            GL.Uniform1(mTextureUnitLocation, unit);
        }
    }
}
