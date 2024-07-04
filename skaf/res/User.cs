﻿using Azure.Core;
using Microsoft.Graph.Print.Printers.Item.Jobs.Item.Tasks.Item.Definition;
using skaf.Properties;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows;

namespace skaf.res
{
    public class User
    {
        public string Assinatura
        {
            get
            {

                string text = Properties.Resource1.assinatura;
              
                text = text.Replace("@NOME", Name);

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

                if (Settings.Default.imagem != null)
                {

                    string imagemHtml = $"<img src='data:image/jpeg;base64,{Settings.Default.imagem}' width=\"136\" height=\"110\"/>";
                    text = text.Replace("@FOTO", imagemHtml);
                }
                else
                {
                    text = text.Replace(@"<td style=""padding:0cm;width:102pt"">
                <p style=""line-height:105%;margin:0cm;font-family:Calibri,sans-serif;font-size:11pt"">
                    <span style=""font-family:&quot;Times New Roman&quot;,serif;font-size:10pt;line-height:105%"">@FOTO</span>
                </p>
            </td>", " ");
                }


                return text;
            }
        }

        public string Name
        {
            get
            {
                if (!string.IsNullOrEmpty(Settings.Default.Nome))
                {
                    return Settings.Default.Nome;
                }

                else { return Name; }
            }
            set { }
        }
        public string Token { get; set; }
        public string? Fone { get; set; }
        public string? Linkedin { get; set; }
        public string? fotoPath;
        public byte[]? Foto
        {
            get; set;

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
