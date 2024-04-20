namespace GraphSplit.Forms
{
    public partial class AuthorizationForm : Form
    {
        MainForm mainForm;
        Label titleLabel;
        Label loginLabel;
        TextBox loginTextBox;
        Label passwordLabel;
        TextBox passwordTextBox;
        Button executeSignInButton;
        Button changeModeSignInButton;

        public AuthorizationForm(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            InitializeForm();
        }

        private void InitializeForm()
        {
            // Установка размеров и позиции формы
            this.Size = new Size(280, 400);
            this.Text = "Authorization";

            // Надпись "Авторизация"
            titleLabel = new Label();
            titleLabel.Text = "Авторизация";
            titleLabel.Location = new Point(80, 30);
            titleLabel.AutoSize = true;
            this.Controls.Add(titleLabel);

            // Надпись "Логин"
            loginLabel = new Label();
            loginLabel.Text = "Логин";
            loginLabel.Location = new Point(30, 100);
            loginLabel.AutoSize = true;
            this.Controls.Add(loginLabel);

            // Поле ввода логина
            loginTextBox = new TextBox();
            loginTextBox.Location = new Point(30, 120);
            loginTextBox.Width = 200;
            this.Controls.Add(loginTextBox);

            // Надпись "Пароль"
            passwordLabel = new Label();
            passwordLabel.Text = "Пароль";
            passwordLabel.Location = new Point(30, 160);
            passwordLabel.AutoSize = true;
            this.Controls.Add(passwordLabel);

            // Поле ввода пароля
            passwordTextBox = new TextBox();
            passwordTextBox.Location = new Point(30, 180);
            passwordTextBox.Width = 200;
            passwordTextBox.UseSystemPasswordChar = true;
            this.Controls.Add(passwordTextBox);

            // Кнопка "Авторизоваться"
            executeSignInButton = new Button();
            executeSignInButton.Text = "Авторизоваться";
            executeSignInButton.Location = new Point(45, 220);
            executeSignInButton.Size = new Size(170, 40);
            executeSignInButton.Click += new EventHandler(LoginButton_Click);
            this.Controls.Add(executeSignInButton);

            // Кнопка "Регистрация"
            changeModeSignInButton = new Button();
            changeModeSignInButton.Text = "Регистрация";
            changeModeSignInButton.Location = new Point(45, 300);
            changeModeSignInButton.Size = new Size(170, 40);
            changeModeSignInButton.BackColor = Color.LightGray;
            changeModeSignInButton.Click += new EventHandler(ChangeModeOnRegistration_Click);
            this.Controls.Add(changeModeSignInButton);
        }



        private void ChangeModeOnRegistration_Click(object sender, EventArgs e)
        {
            changeModeSignInButton.Text = "Авторизация";
            changeModeSignInButton.Click += new EventHandler(ChangeModeOnLogin_Click);
            changeModeSignInButton.Click -= new EventHandler(ChangeModeOnRegistration_Click);

            titleLabel.Text = "Регистрация";
            executeSignInButton.Text = "Зарегистрироваться";
            executeSignInButton.Click -= new EventHandler(LoginButton_Click);
            executeSignInButton.Click += new EventHandler(RegisterButton_Click);
        }

        private void ChangeModeOnLogin_Click(object sender, EventArgs e)
        {
            changeModeSignInButton.Text = "Регистрация";
            changeModeSignInButton.Click -= new EventHandler(ChangeModeOnLogin_Click);
            changeModeSignInButton.Click += new EventHandler(ChangeModeOnRegistration_Click);

            titleLabel.Text = "Авторизация";
            executeSignInButton.Text = "Авторизоваться";
            executeSignInButton.Click += new EventHandler(LoginButton_Click);
            executeSignInButton.Click -= new EventHandler(RegisterButton_Click);
        }

        private void TryLogin(string login, string password)
        {
            if (Authorization.Authorization.Login(login, password))
            {
                SuccessSignIn(login, password);
            }
            else
            {
                MessageBox.Show("Ошибка авторизации, проверьте логин и пароль.");
            }
        }

        private void TryRegister(string login, string password)
        {
            if (login.Length <= 3 || password.Length <= 3)
            {
                MessageBox.Show("Логин и пароль должны быть длиннее 3 символов.");
                return;
            }

            if (Authorization.Authorization.Registration(login, password))
            {
                SuccessSignIn(login, password);
            }
            else
            {
                MessageBox.Show("Такой пользователь уже существует.");
            }
        }


        private void LoginButton_Click(object sender, EventArgs e)
        {
            TryLogin(loginTextBox.Text, passwordTextBox.Text);
        }
        private void RegisterButton_Click(object sender, EventArgs e)
        {
            TryRegister(loginTextBox.Text, passwordTextBox.Text);
        }

        private void SuccessSignIn(string login, string password) 
        {
            GraphSettings.ChangeLogin(login);
            GraphSettings.ChangePassword(Authorization.Authorization.ComputeSha256Hash(password));
            GraphSettings.SaveSettings();
            mainForm.InitializeForm();
            this.Close();
        }
    }
}
