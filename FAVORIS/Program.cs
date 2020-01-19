using System;
using System.IO;
using System.Text;

namespace FAVORIS
{

    class Program
    {

        static string Root;

        static void Main(string[] args)
        {

            using (FileStream FS = new FileStream("FAVORIS.HTM", FileMode.Create))
            {

                using (StreamWriter SW = new StreamWriter(FS, Encoding.GetEncoding(1252)))
                {

                    Root = Environment.GetFolderPath(Environment.SpecialFolder.Favorites);
                    
                    SW.Write("<html>");
                    SW.Write("<body style='background:#FFCC99;font-family:Arial,Verdana,Helvetica;text-align:justify'>");
                    SW.Write("<h1 style='background:#99CCFF;border:medium outset;color:#0000FF;text-align:center'>FAVORIS</h1>");

                    TDirectory(SW, new DirectoryInfo(Root));

                    SW.Write("<p align='right'>&copy;&nbsp;<a href='http://guimberteau.net/' target='_blank'>Alain Guimberteau</a></p>");
                    SW.Write("</body>");
                    SW.Write("</html>");

                }

            }

        }

        static void TDirectory(StreamWriter SW, DirectoryInfo DI)
        {

            string Name = DI.FullName.Substring(Root.Length);

            if (Name != "")
            {
                SW.Write("<h2 style='background:#99CCFF;border:thin outset;width:100%'>&nbsp;");
                SW.Write(Name.Substring(1).Replace(@"\", "&nbsp;-&nbsp;"));
                SW.Write("&nbsp;</h2>");
            }

            SW.Write("<ul>");

            foreach (var oFile in DI.GetFiles("*.URL"))
            {
                TFile(SW, oFile);
            }

            SW.Write("</ul>");

            foreach (var oDirectory in DI.GetDirectories())
            {
                TDirectory(SW, oDirectory);
            }

        }

        static void TFile(StreamWriter SW, FileInfo FI)
        {

            string Line;

            using (FileStream FS = new FileStream(FI.FullName, FileMode.Open))
            {

                using (StreamReader SR = new StreamReader(FS, Encoding.GetEncoding(1252)))
                {

                    Line = SR.ReadToEnd();

                    int I = Line.IndexOf("[InternetShortcut]");
                    if (I != -1)
                    {
                        int J = Line.IndexOf("URL=", I);
                        if (J != -1)
                        {
                            J += 4;
                            int K = Line.IndexOf("\r\n", J);
                            SW.Write("<li><a href='");
                            SW.Write(Line.Substring(J, K - J));
                            SW.Write("'>");
                            SW.Write(FI.Name.Substring(0,FI.Name.Length-4));
                            SW.Write("</a></li>");
                        }
                    }

                }

            }

        }

    }

}
