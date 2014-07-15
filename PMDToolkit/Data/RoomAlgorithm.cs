using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using PMDToolkit.Core;
using System.Reflection;
using PMDToolkit.Logs;

namespace PMDToolkit.Data {
    public class RoomAlgorithm {
        
        public string Name { get; set; }

        public List<Tuple<string, bool>> RoomSettings { get; set; }

        public Type Algorithm { get; set; }

        public RoomAlgorithm() {
            RoomSettings = new List<Tuple<string, bool>>();
        }

        //public void LoadAlgorithm(int algorithmNum) {
        //    string[] files = Directory.GetFiles("Data/RoomAlgorithm/Classes/" + algorithmNum);

        //    List<System.CodeDom.Compiler.CompilerError> errors = new List<System.CodeDom.Compiler.CompilerError>();

        //    Microsoft.CSharp.CSharpCodeProvider provider;
        //    Dictionary<string, string> param = new Dictionary<string, string>();
        //    param.Add("CompilerVersion", "v4.0");
        //    provider = new Microsoft.CSharp.CSharpCodeProvider(param);
        //    System.CodeDom.Compiler.CompilerParameters options = new System.CodeDom.Compiler.CompilerParameters();

        //    options.GenerateInMemory = true;
        //    options.TreatWarningsAsErrors = false;
        //    options.IncludeDebugInformation = true;

        //    List<string> refs = new List<string>();
        //    Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        //    for (int i = 0; i < assemblies.Length; i++) {
        //        if (!assemblies[i].FullName.Contains("System.") && !assemblies[i].FullName.Contains("Microsoft.")) {
        //            refs.Add(System.IO.Path.GetFileName(assemblies[i].Location));
        //        }
        //    }
        //    refs.Add("System.dll");
        //    refs.Add("System.Data.dll");
        //    refs.Add("System.Drawing.dll");
        //    refs.Add("System.Xml.dll");
        //    refs.Add("System.Windows.Forms.dll");
        //    refs.Add(System.Windows.Forms.Application.ExecutablePath);
        //    options.ReferencedAssemblies.AddRange(refs.ToArray());

        //    System.CodeDom.Compiler.ICodeCompiler compiler = provider.CreateCompiler();
        //    System.CodeDom.Compiler.CompilerResults results = compiler.CompileAssemblyFromFileBatch(options, files);

        //    foreach (System.CodeDom.Compiler.CompilerError err in results.Errors) {
        //        errors.Add(err);
        //    }

        //    if (results.Errors.Count == 0) {
        //        Algorithm = results.CompiledAssembly.GetType("Rooms." + Name);
        //    } else {
        //        Algorithm = null;
        //    }
        //}

        public void Load(int algorithmNum) {
            using (XmlReader reader = XmlReader.Create(Paths.DataPath + "RoomAlgorithm\\" + algorithmNum + ".xml")) {
                while (reader.Read()) {
                    if (reader.IsStartElement()) {
                        switch (reader.Name) {
                            case "Name": {
                                    Name = reader.ReadString();
                                    break;
                                }
                            case "RoomSetting": {
                                    if (reader.Read()) {
                                        string settingName = reader.ReadElementString("RoomInt");
                                        bool settingBool = reader.ReadElementString("RoomIntBool").ToBool();
                                        RoomSettings.Add(new Tuple<string, bool>(settingName, settingBool));
                                    }
                                }
                                break;
                        }
                    }
                }
            }
        }

        public void Save(int algorithmNum) {
            if (!Directory.Exists(Paths.DataPath + "RoomAlgorithm"))
                Directory.CreateDirectory(Paths.DataPath + "RoomAlgorithm");
            using (XmlWriter writer = XmlWriter.Create(Paths.DataPath + "RoomAlgorithm\\" + algorithmNum + ".xml", Logger.XmlWriterSettings)) {
                writer.WriteStartDocument();
                writer.WriteStartElement("AlgorithmEntry");

                #region Basic data
                writer.WriteStartElement("General");
                writer.WriteElementString("Name", Name);
                writer.WriteEndElement();
                #endregion
                #region Dungeon Settings
                writer.WriteStartElement("RoomSettings");
                for (int i = 0; i < RoomSettings.Count; i++) {
                    writer.WriteStartElement("RoomSetting");
                    writer.WriteElementString("RoomInt", RoomSettings[i].Item1);
                    writer.WriteElementString("RoomIntBool", RoomSettings[i].Item2.ToString());
                    writer.WriteEndElement();
                }
                #endregion

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        public void CreateRoom() {
            //pass in: map settings, map demands
            //pass out: room structure: tiles, items, npcs
        }
    }
}
