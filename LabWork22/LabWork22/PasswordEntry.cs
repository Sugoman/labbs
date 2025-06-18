using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabWork22
{
    public class PasswordEntry
    {
        public string Site {  get; set; }
        public string Login { get; set; }
        public string EncryptedPassword { get; set; }
        public string DecryptedPassword { get; set; }
    }
}
