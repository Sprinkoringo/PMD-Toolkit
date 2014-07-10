/*The MIT License (MIT)

Copyright (c) 2014 Sprinkoringo

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace PMDToolkit.Graphics {
    public class ShaderProgram : IDisposable {

        protected int mProgramID;

        public int ProgramID { get { return mProgramID;} }

        public ShaderProgram() {
	        mProgramID = 0;
        }

        ~ShaderProgram()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (mProgramID != 0)
                GL.DeleteProgram(mProgramID);

            mProgramID = 0;
        }


        public int LoadShaderFromFile( string path, ShaderType shaderType ) {
	        //Open file
	        int shaderID = 0;
            //Source file loaded
            StreamReader reader = new StreamReader(path);
	        //Get shader source
            string shaderString = reader.ReadToEnd();
	        
	    
	        
	        
	        //Create shader ID
	        shaderID = GL.CreateShader(shaderType);
	        //Set shader source
	        GL.ShaderSource( shaderID, shaderString );
	        //Compile shader source
	        GL.CompileShader( shaderID );
	        //Check shader for errors
            string info;
            int status_code;
            GL.GetShaderInfoLog(shaderID, out info);
            GL.GetShader(shaderID, ShaderParameter.CompileStatus, out status_code);

            if (status_code != 1)
                throw new ApplicationException(info);

	        return shaderID;
        }

        public void Bind() {
	        //Use shader
	        GL.UseProgram( mProgramID );
	        //Check for error
            ErrorCode ec = GL.GetError();
	        if( ec != 0 ) {
                throw new System.Exception(ec.ToString());
            }
        }

        public void Unbind() {
            GL.UseProgram( 0 );
        }

        public void PrintProgramLog() {

        }

        protected string printProgramLog( int program ) {
	        //Make sure name is shader
	        if( GL.IsProgram( program ) ) {
		        //Get info log
		        string infoLog;
                GL.GetProgramInfoLog( program, out infoLog);
                return infoLog;
	        } else {
                throw new Exception("ID " + program + " is not a program.");
	        }
        }

    }

}


