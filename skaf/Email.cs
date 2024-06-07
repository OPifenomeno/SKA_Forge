using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Graph.Models;
namespace skaf
{
    internal class Email
    {
        public required string Body { get; set; }
        public required string Subject { get; set; }
        
        
        public Email() {
        
        }
    }

    internal class Anexo
{
       public required byte[] Arquivo {
        get; 
        
        set;
        }

        public Anexo(string arquivo) {
            byte[] arquivoFinal;            
            arquivoFinal = File.ReadAllBytes(arquivo);
            Arquivo = arquivoFinal;

        }

}
}
