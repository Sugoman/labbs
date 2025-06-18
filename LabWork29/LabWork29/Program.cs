using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Terminal.Gui;

public class Program
{
    private static List<User> users = new List<User>();
    private static string usersFilePath = "users.json";
    private const string AdminPassword = "admin123"; // Пароль администратора по умолчанию

    public static void Main(string[] args)
    {
        Application.Init();

        // Загрузка пользователей при старте
        LoadUsers();

        // Создаем главное окно
        var top = Application.Top;

        // Главное меню
        var menu = new MenuBar(new MenuBarItem[] {
            new MenuBarItem("_Файл", new MenuItem[] {
                new MenuItem("_Выход", "", () => Application.RequestStop())
            }),
            new MenuBarItem("_Справка", new MenuItem[] {
                new MenuItem("_О программе", "", () => MessageBox.Query("О программе", "Администратор пользователей v1.0\nTerminal.Gui", "OK"))
            })
        });

        top.Add(menu);

        // Главное окно с вводом пароля администратора
        var loginWindow = new Window("Авторизация администратора")
        {
            X = 0,
            Y = 1,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        var passwordLabel = new Label("Пароль администратора:")
        {
            X = Pos.Center(),
            Y = Pos.Center() - 2
        };

        var passwordField = new TextField("")
        {
            X = Pos.Center(),
            Y = Pos.Center(),
            Width = 20,
            Secret = true
        };

        var loginButton = new Button("Войти")
        {
            X = Pos.Center(),
            Y = Pos.Center() + 2
        };

        loginButton.Clicked += () =>
        {
            if (passwordField.Text.ToString() == AdminPassword)
            {
                top.Remove(loginWindow);
                ShowAdminPanel();
            }
            else
            {
                MessageBox.ErrorQuery("Ошибка", "Неверный пароль администратора", "OK");
            }
        };

        loginWindow.Add(passwordLabel, passwordField, loginButton);
        top.Add(loginWindow);

        Application.Run();
    }

    private static void ShowAdminPanel()
    {
        var top = Application.Top;

        // Основное окно администратора
        var adminWindow = new Window("Панель администратора")
        {
            X = 0,
            Y = 1,
            Width = Dim.Fill(),
            Height = Dim.Fill(),
            ColorScheme = new ColorScheme
            {
                Normal = Terminal.Gui.Attribute.Make(Color.White, Color.Blue),
                HotNormal = Terminal.Gui.Attribute.Make(Color.White, Color.Blue),
                Focus = Terminal.Gui.Attribute.Make(Color.Black, Color.Gray),
                HotFocus = Terminal.Gui.Attribute.Make(Color.Black, Color.Gray)
            }
        };

        // Список пользователей
        var usersListView = new ListView(users.Select(u => u.Username).ToList())
        {
            X = 1,
            Y = 1,
            Width = Dim.Percent(40),
            Height = Dim.Fill() - 3,
            AllowsMarking = true,
            AllowsMultipleSelection = false
        };

        // Информация о выбранном пользователе
        var selectedUserLabel = new Label("Выбранный пользователь: ")
        {
            X = Pos.Right(usersListView) + 2,
            Y = 1
        };

        var selectedUsernameLabel = new Label("")
        {
            X = Pos.Right(selectedUserLabel),
            Y = 1,
            Width = 20
        };

        // Поля для добавления нового пользователя
        var addUserLabel = new Label("Добавить нового пользователя:")
        {
            X = Pos.Right(usersListView) + 2,
            Y = 3
        };

        var usernameLabel = new Label("Логин:")
        {
            X = Pos.Right(usersListView) + 2,
            Y = 5
        };

        var usernameField = new TextField("")
        {
            X = Pos.Right(usersListView) + 10,
            Y = 5,
            Width = 20
        };

        var passwordLabel = new Label("Пароль:")
        {
            X = Pos.Right(usersListView) + 2,
            Y = 7
        };

        var passwordField = new TextField("")
        {
            X = Pos.Right(usersListView) + 10,
            Y = 7,
            Width = 20,
            Secret = true
        };

        var addButton = new Button("Добавить")
        {
            X = Pos.Right(usersListView) + 2,
            Y = 9
        };

        // Кнопки управления
        var deleteButton = new Button("Удалить")
        {
            X = 1,
            Y = Pos.Bottom(usersListView) + 1,
            ColorScheme = new ColorScheme
            {
                Normal = Terminal.Gui.Attribute.Make(Color.Red, Color.Black),
                HotNormal = Terminal.Gui.Attribute.Make(Color.BrightRed, Color.Black)
            }
        };

        var changePasswordButton = new Button("Изменить пароль")
        {
            X = Pos.Right(deleteButton) + 2,
            Y = Pos.Bottom(usersListView) + 1
        };

        var refreshButton = new Button("Обновить")
        {
            X = Pos.Right(changePasswordButton) + 2,
            Y = Pos.Bottom(usersListView) + 1
        };

        // Обработчики событий
        usersListView.SelectedItemChanged += (args) =>
        {
            if (args.Item >= 0 && args.Item < users.Count)
            {
                selectedUsernameLabel.Text = users[args.Item].Username;
            }
        };

        addButton.Clicked += () =>
        {
            var username = usernameField.Text.ToString();
            var password = passwordField.Text.ToString();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.ErrorQuery("Ошибка", "Логин и пароль не могут быть пустыми", "OK");
                return;
            }

            if (users.Any(u => u.Username == username))
            {
                MessageBox.ErrorQuery("Ошибка", "Пользователь с таким логином уже существует", "OK");
                return;
            }

            var newUser = new User
            {
                Username = username,
                PasswordHash = HashPassword(password)
            };

            users.Add(newUser);
            SaveUsers();

            usersListView.SetSource(users.Select(u => u.Username).ToList());
            usernameField.Text = "";
            passwordField.Text = "";

            MessageBox.Query("Успех", "Пользователь успешно добавлен", "OK");
        };

        deleteButton.Clicked += () =>
        {
            if (usersListView.SelectedItem == -1)
                return;

            var result = MessageBox.Query("Подтверждение",
                $"Вы уверены, что хотите удалить пользователя '{users[usersListView.SelectedItem].Username}'?",
                "Да", "Нет");

            if (result == 0)
            {
                users.RemoveAt(usersListView.SelectedItem);
                SaveUsers();
                usersListView.SetSource(users.Select(u => u.Username).ToList());
                selectedUsernameLabel.Text = "";
            }
        };

        changePasswordButton.Clicked += () =>
        {
            if (usersListView.SelectedItem == -1)
                return;

            var dialog = new Dialog("Изменение пароля", 50, 10);

            var passwordLabel = new Label("Новый пароль:")
            {
                X = 1,
                Y = 1
            };

            var passwordField = new TextField("")
            {
                X = 1,
                Y = 2,
                Width = Dim.Fill() - 2,
                Secret = true
            };

            var okButton = new Button("OK")
            {
                X = Pos.Center() - 10,
                Y = 4
            };

            var cancelButton = new Button("Отмена")
            {
                X = Pos.Center() + 2,
                Y = 4
            };

            okButton.Clicked += () =>
            {
                if (string.IsNullOrWhiteSpace(passwordField.Text.ToString()))
                {
                    MessageBox.ErrorQuery("Ошибка", "Пароль не может быть пустым", "OK");
                    return;
                }

                users[usersListView.SelectedItem].PasswordHash = HashPassword(passwordField.Text.ToString());
                SaveUsers();
                Application.RequestStop();
            };

            cancelButton.Clicked += () => Application.RequestStop();

            dialog.Add(passwordLabel, passwordField, okButton, cancelButton);
            Application.Run(dialog);
        };

        refreshButton.Clicked += () =>
        {
            LoadUsers();
            usersListView.SetSource(users.Select(u => u.Username).ToList());
        };

        // Подсказки
        var addButtonHint = new Label("Добавить нового пользователя в систему")
        {
            X = Pos.Right(usersListView) + 2,
            Y = 11,
            Width = Dim.Fill() - 2
        };

        var deleteButtonHint = new Label("Удалить выбранного пользователя")
        {
            X = 1,
            Y = Pos.Bottom(deleteButton) + 1,
            Width = 30,
            ColorScheme = new ColorScheme { Normal = Terminal.Gui.Attribute.Make(Color.BrightRed, Color.Black) }
        };

        var changePasswordHint = new Label("Изменить пароль выбранного пользователя")
        {
            X = Pos.Right(deleteButtonHint) + 2,
            Y = Pos.Bottom(changePasswordButton) + 1,
            Width = 40
        };

        var refreshButtonHint = new Label("Обновить список пользователей")
        {
            X = Pos.Right(changePasswordHint) + 2,
            Y = Pos.Bottom(refreshButton) + 1,
            Width = 30
        };

        // Добавляем элементы на форму
        adminWindow.Add(
            usersListView,
            selectedUserLabel, selectedUsernameLabel,
            addUserLabel, usernameLabel, usernameField, passwordLabel, passwordField, addButton,
            deleteButton, changePasswordButton, refreshButton,
            addButtonHint,
            deleteButtonHint,
            changePasswordHint,
            refreshButtonHint
        );

        top.Add(adminWindow);
        Application.Refresh();
    }

    private static void LoadUsers()
    {
        if (File.Exists(usersFilePath))
        {
            var json = File.ReadAllText(usersFilePath);
            users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }
        else
        {
            users = new List<User>();
        }
    }

    private static void SaveUsers()
    {
        var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(usersFilePath, json);
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}

public class User
{
    public string Username { get; set; }
    public string PasswordHash { get; set; }
}