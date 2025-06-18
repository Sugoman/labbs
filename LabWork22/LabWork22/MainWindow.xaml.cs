using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Windows;
using System.Security.Cryptography;


namespace LabWork22
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string FilePath = "C:\\Temp\\.ispp21\\LabWork22\\LabWork22\\Passwords.txt";
        private const string EncryptionKey = "GenshinImpactTop";
        public ObservableCollection<PasswordEntry> PasswordEntries {  get; set; }
        public MainWindow()
        {
            InitializeComponent();
            PasswordEntries = new ObservableCollection<PasswordEntry>();
            PasswordListView.ItemsSource = PasswordEntries;
            LoadPasswords();
        }

        private void LoadPasswords()
        {
            if (!File.Exists(FilePath)) return;
            PasswordEntries.Clear();

            foreach (var line in File.ReadAllLines(FilePath))
            {
                var parts = line.Split(';');
                if (parts.Length == 3)
                {
                    PasswordEntries.Add(new PasswordEntry
                    {
                        Site = parts[0],
                        Login = parts[1],
                        EncryptedPassword = parts[2],
                        DecryptedPassword = Decrypt(parts[2])
                    });
                }
            }
        }
        private string Encrypt(string plainText)
        {
            using(System.Security.Cryptography.Aes aes = System.Security.Cryptography.Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                aes.IV = new byte[16];

                using(var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
        }
        private string Decrypt(string encryptedText)
        {
            try
            {
                using(System.Security.Cryptography.Aes aes = System.Security.Cryptography.Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                    aes.IV = new byte[16];

                    using(var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    {
                        byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                        byte[] plainBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                        return Convert.ToBase64String(plainBytes);
                    }
                }
            }
            catch 
            {
                return "Ошибка дешифрования";
            }
        }
        private void AddPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            string site = SiteTextBox.Text;
            string login = LoginTextBox.Text;
            string password = PasswordTextBox.Text;

            if(string.IsNullOrEmpty(site)||string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            string encryptedPassword = Encrypt(password);

            File.AppendAllLines(FilePath, new List<string> { $"{site};{login};{encryptedPassword}\n" });

            PasswordEntries.Add(new PasswordEntry
            {
                Site = site,
                Login = login,
                EncryptedPassword = encryptedPassword,
                DecryptedPassword = password
            });

            SiteTextBox.Clear();
            LoginTextBox.Clear();
            PasswordTextBox.Clear();
        }

        private void GeneratePassword_Click(object sender, RoutedEventArgs e)
        {
            if(!int.TryParse(PasswordLengthTextBox.Text, out int length) || length < 6 || length > 20)
            {
                MessageBox.Show("Введите длину пароля от 6 до 20.");
                return;
            }

            PasswordTextBox.Text = asdGeneratePassword(length);
        }

        private string asdGeneratePassword(int length)
        {
            const string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
            var rng = new Random();
            var password = new char[length];

            for (int i = 0; i < length; i++)
            {
                password[i] = chars[rng.Next(chars.Length)];
            }
            return new string(password);
        }
    }
}