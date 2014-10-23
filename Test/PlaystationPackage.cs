using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;

public class dirinfo
{
    public string target_name;
    List<dirinfo> dd;

    public void ParseDir(DirectoryInfo inDir)
    {
        foreach (var item in inDir.GetDirectories("*", SearchOption.TopDirectoryOnly))
        {
            var ff = new dirinfo(item.Name);
            ff.ParseDir(item);

            dd.Add(ff);
        }
    }

    public dirinfo()
    {

    }

    public dirinfo(string tn)
    {
        target_name = tn;

        dd = new List<dirinfo>();
    }

    public void WriteXml(XmlWriter writer)
    {
        foreach (var item in dd)
        {
            writer.WriteStartElement("dir");
            writer.WriteAttributeString("targ_name", item.target_name);
            item.WriteXml(writer);
            writer.WriteEndElement();
        }
    }
}


[Serializable]
public class rootdirinfo : IXmlSerializable
{
    List<dirinfo> dd;

    public rootdirinfo()
    {
        dd = new List<dirinfo>();
    }

    public System.Xml.Schema.XmlSchema GetSchema()
    {
        throw new NotImplementedException();
    }

    public void ReadXml(XmlReader reader)
    {
        //throw new NotImplementedException();
    }

    public void WriteXml(XmlWriter writer)
    {
        foreach (var item in dd)
        {
            writer.WriteStartElement("dir");
            writer.WriteAttributeString("targ_name", item.target_name);
            item.WriteXml(writer);
            writer.WriteEndElement();
        }
    }

    public void ParseDir(DirectoryInfo inDir)
    {
        foreach (var item in inDir.GetDirectories("*", SearchOption.TopDirectoryOnly))
        {
            var ff = new dirinfo(item.Name);
            ff.ParseDir(item);

            dd.Add(ff);
        }
    }
}

public class fileinfo
{
    //<file targ_path="Media/level0" orig_path="E:\Project\Candle\Build\WTF\Media\level0"/>
    public string targ_path;
    public string orig_path;

    public fileinfo()
    {

    }

    public fileinfo(string t, string o)
    {
        targ_path = t;
        orig_path = o;
    }
}

[Serializable]
public class filesinfo : IXmlSerializable
{
    [XmlAttribute("img_no")]
    public int img_no;

    public List<fileinfo> ff;

    public filesinfo()
    {
        ff = new List<fileinfo>();
    }

    public void AddFile(fileinfo newFile)
    {
        ff.Add(newFile);
    }

    public System.Xml.Schema.XmlSchema GetSchema()
    {
        throw new NotImplementedException();
    }

    public void ReadXml(XmlReader reader)
    {
        //throw new NotImplementedException();
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteAttributeString("img_no", img_no.ToString());

        foreach (var item in ff)
        {
            writer.WriteStartElement("file");
            writer.WriteAttributeString("targ_path", item.targ_path.ToString());
            writer.WriteAttributeString("orig_path", item.orig_path.ToString());
            writer.WriteEndElement();
        }

        //throw new NotImplementedException();
    }
}

public class chunkinfo
{
    public int id;
    public int layer_no;
    public string label;

    public chunkinfo()
    {

    }

    public chunkinfo(int i, int y, string l)
    {
        id = i;
        layer_no = y;
        label = l;
    }
}

[Serializable]
public class chunksinfo : IXmlSerializable
{
    [XmlAttribute("supported_languages")]
    public string supported_languages;

    [XmlArray("chunks")]
    [XmlArrayItem("chunkinfo")]
    public List<chunkinfo> cc { get; set; }

    public chunksinfo()
    {
        supported_languages = "";
        cc = new List<chunkinfo>();
        cc.Add(new chunkinfo(0, 0, "Chunk #0"));
    }

    public System.Xml.Schema.XmlSchema GetSchema()
    {
        throw new NotImplementedException();
    }

    public void ReadXml(XmlReader reader)
    {
        //throw new NotImplementedException();
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteAttributeString("supported_languages", supported_languages);

        foreach (var item in cc)
        {
            writer.WriteStartElement("chunk");
            writer.WriteAttributeString("id", item.id.ToString());
            writer.WriteAttributeString("layer_no", item.layer_no.ToString());
            writer.WriteAttributeString("label", item.label);
            writer.WriteEndElement();
        }
        //throw new NotImplementedException();
    }
}

public class scenarioinfo
{
    public int id;
    public string type;
    public int initial_chunk_count;
    public string label;

    public string value;

    public scenarioinfo()
    {

    }

    public scenarioinfo(int i, string t, int ic, string l, string v)
    {
        id = i;
        type = t;
        initial_chunk_count = ic;
        label = l;
        value = v;
    }
}

[Serializable]
public class scenariosinfo : IXmlSerializable
{
    [XmlAttribute("default_id")]
    public int default_id;

    public List<scenarioinfo> stuff;

    public scenariosinfo()
    {
        default_id = 0;
        stuff = new List<scenarioinfo>();

        stuff.Add(new scenarioinfo(0, "sp", 1, "Scenario #0", "0"));
    }

    public System.Xml.Schema.XmlSchema GetSchema()
    {
        throw new NotImplementedException();
    }

    public void ReadXml(XmlReader reader)
    {
        //throw new NotImplementedException();
    }

    public void WriteXml(XmlWriter writer)
    {
        //<scenario id="0" type="sp" initial_chunk_count="1" label="Scenario #0">0</scenario>
        writer.WriteAttributeString("default_id", default_id.ToString());

        foreach (var item in stuff)
        {
            writer.WriteStartElement("scenario");
            writer.WriteAttributeString("id", item.id.ToString());
            writer.WriteAttributeString("type", item.type);
            writer.WriteAttributeString("initial_chunk_count", item.initial_chunk_count.ToString());
            writer.WriteAttributeString("label", item.label);
            writer.WriteValue(item.value);
            writer.WriteEndElement();
        }
    }
}

[Serializable]
public class cInfo
{
    //<chunk_info chunk_count="1" scenario_count="1">
    //  <chunks supported_languages="">
    //    <chunk id="0" layer_no="0" label="Chunk #0"/>
    //  </chunks>
    //  <scenarios default_id="0">
    //    <scenario id="0" type="sp" initial_chunk_count="1" label="Scenario #0">0</scenario>
    //  </scenarios>
    //</chunk_info>

    [XmlAttribute("chunk_count")]
    public int chunk_count;

    [XmlAttribute("scenario_count")]
    public int scenario_count;

    public chunksinfo chunks;
    public scenariosinfo scenarios;

    public cInfo()
    {
        chunk_count = 1;
        scenario_count = 1;

        chunks = new chunksinfo();
        scenarios = new scenariosinfo();
    }
}

public class packageinfo
{
    // <package content_id="IV0002-CUSA00405_00-TETRIS3000000000" passcode="r0K_74YC95z0zF6hvyMkXJPg5BNRO56x" storage_type="bd50" app_type="full"/>

    [XmlAttribute("content_id")]
    public string content_id;

    [XmlAttribute("passcode")]
    public string passcode;

    [XmlAttribute("storage_type")]
    public string storage_type;

    [XmlAttribute("app_type")]
    public string app_type;

    public packageinfo()
    {
        content_id = "IV0002-CUSA00405_00-TETRIS3000000000";
        passcode = "r0K_74YC95z0zF6hvyMkXJPg5BNRO56x";
        storage_type = "bd50";
        app_type = "full";
    }
}

public class volumeinfo
{
    public string volume_type;
    public string volume_id;
    public string volume_ts;
    public packageinfo package;
    public cInfo chunk_info;

    public volumeinfo()
    {
        volume_type = "pkg_ps4_app";
        volume_id = "";
        volume_ts = System.DateTime.Now.ToString("yyyy'-'MM'-'dd HH':'mm':'ss");

        package = new packageinfo();
        chunk_info = new cInfo();
    }
}

[Serializable]
public class psproject
{
    [XmlAttribute("fmt")]
    public string fmt;

    [XmlAttribute("version")]
    public string version;


    public volumeinfo volume;
    public filesinfo files;
    public rootdirinfo rootdir;


    public psproject()
    {
        //fmt="gp4" version="1000"
        fmt = "gp4";
        version = "1000";


        volume = new volumeinfo();
        files = new filesinfo();
        rootdir = new rootdirinfo();

    }

    public static String ToAbsolutePath(string basePathb, string path)
    {
        if (String.IsNullOrEmpty(path))
            return null;
        if (String.IsNullOrEmpty(basePathb))
            return path;

        Uri path2 = new Uri(path, UriKind.RelativeOrAbsolute);
        if (path2.IsAbsoluteUri)
            return path;
        Uri basePath = new Uri(basePathb + "/", UriKind.Absolute);
        Uri absPath = new Uri(basePath, path);
        return absPath.LocalPath;
    }

    public static String ToRelativePath(string basePath, string path)
    {
        if (String.IsNullOrEmpty(path))
            return null;
        if (String.IsNullOrEmpty(basePath))
            return path;

        Uri uri1 = new Uri(path, UriKind.RelativeOrAbsolute);
        if (uri1.IsAbsoluteUri)
        {
            Uri uri2 = new Uri(basePath + "/", UriKind.Absolute);
            Uri relativeUri = uri2.MakeRelativeUri(uri1);
            return relativeUri.ToString();
        }
        // else it is already a relative path
        return path;
    }

    public void GatherFiles(string path)
    {
        DirectoryInfo diTop = new DirectoryInfo(path);

        // do a check for top level files
        foreach (var di in diTop.GetDirectories("*"))
        {
            try
            {
                foreach (var fi in di.GetFiles("*", SearchOption.AllDirectories))
                {
                    try
                    {
                        var xx = ToRelativePath(path, fi.FullName);                       
                        files.AddFile(new fileinfo(xx, fi.FullName));
                    }
                    catch (UnauthorizedAccessException UnAuthFile)
                    {
                        Console.WriteLine("UnAuthFile: {0}", UnAuthFile.Message);
                    }
                }
            }
            catch (UnauthorizedAccessException UnAuthSubDir)
            {
                Console.WriteLine("UnAuthSubDir: {0}", UnAuthSubDir.Message);
            }



        }

        // top level files in the base dir
        foreach (var fi in diTop.GetFiles())
        {
            try
            {
                files.AddFile(new fileinfo(fi.Name, fi.FullName));
            }
            catch (UnauthorizedAccessException UnAuthTop)
            {
                Console.WriteLine("{0}", UnAuthTop.Message);
            }
        }

        rootdir.ParseDir(diTop);

        Console.Write("");
    }
}



public class PlaystationPackage {

    public static void Serialize(Type type, object serializedObject, string path)
    {
        XmlSerializer serializer = new XmlSerializer(type);
        Console.WriteLine("PATH" + path);
        string directoryPath = Path.GetDirectoryName(path);
        Console.WriteLine("DIRECTORYPATH" + directoryPath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        using (TextWriter tw = new StreamWriter(path))
        {
            tw.NewLine = "\r\n";
            serializer.Serialize(tw, serializedObject);
        }
    }

    [MenuItem("SoMa/Build/Build PS4 Package")]
    static void BuildPS4Package()
    {
        var finalLocation = Application.dataPath + "/../../Build/PS4/tetris.gp4";
        var devLocation = Application.dataPath + "/../../Build/PS4Development/tetris.gp4";
        var ps4Location = Application.dataPath + "/../../Build/PS4/TetrisUltimate";
        var ps4devLocation = Application.dataPath + "/../../Build/PS4Development/TetrisUltimate";
        BuildPS4PackageInternal(ps4Location, finalLocation);
        BuildPS4PackageInternal(ps4devLocation, devLocation);
        EditorUserBuildSettings.SwitchActiveBuildTarget((BuildTarget)5);
    }

    private static string BuildPS4PackageInternal(string path, string output)
    {
        if (string.IsNullOrEmpty(path))
        {
            path = EditorUtility.OpenFolderPanel("Select PS4 build location", "", "");
        }
        psproject pp = new psproject();
        pp.GatherFiles(Path.GetFullPath(path));
        Serialize(typeof(psproject), pp, output);
        return path;
    }
}
