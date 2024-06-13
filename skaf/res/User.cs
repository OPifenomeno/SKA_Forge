using Azure.Core;
using Microsoft.Graph.Print.Printers.Item.Jobs.Item.Tasks.Item.Definition;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows;

namespace skaf
{
    public class User
    {
        public string Assinatura
        {
            get
            {
                StreamReader sr = new StreamReader("res/assinatura.html");
                string text = sr.ReadToEnd();
                sr.Close();
                text = text.Replace("@NOME", this.Name);

                if (Linkedin != null)
                {
                    text = text.Replace("@LINKEDIN", Linkedin);
                }
                else
                {
                    text = text.Replace("Conecte-se pelo:<a href=\"@LINKEDIN\">@LINKEDIN</a>", string.Empty);
                }

                if (Fone != null)
                {
                    text = text.Replace("@FONE", Fone);
                }
                else
                {
                    text = text.Replace("Fone:", string.Empty).Replace("@FONE", string.Empty);
                }

                if (Foto != null)
                {
                    string imagemBase64 = Convert.ToBase64String(Foto);
                    string imagemHtml = $"<img src='data:image/jpeg;base64,{imagemBase64}' width=\"136\" height=\"110\"/>";
                    text = text.Replace("@FOTO", imagemHtml);
                }
                else { 
                    text = text.Replace(@"<td style=""padding:0cm;width:102pt"">
                <p style=""line-height:105%;margin:0cm;font-family:Calibri,sans-serif;font-size:11pt"">
                    <span style=""font-family:&quot;Times New Roman&quot;,serif;font-size:10pt;line-height:105%"">@FOTO</span>
                </p>
            </td>", " ");
                }              
                

                return text;
            }
        }

        public string Name { get; set; }
        public string Token { get; set; }
        public string ?Fone { get; set; }
        public string ?Linkedin { get; set; }
        public string? fotoPath;
        public byte[] ?Foto { get;set;
        
        }

      

        // 
        public User(string nome, string token)
        {
            //dados de debug
            Fone = "(51) 3591-2900";
            Name = nome;
            Token = token;
        }
    }
}
