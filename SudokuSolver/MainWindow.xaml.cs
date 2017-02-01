using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

using SudokuSolver.Utility;
using SudokuSolver.Sudoku;
using SudokuSolver.Settings;

namespace SudokuSolver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static LanguageReader lr;
        
        private TextBox[,] boardText;
        private int[,] board;
        private int[,] boardSet;
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event, when the form is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSettings();

            //Loads the JSON and creates a LanguageReader object  with the loaded JSON data.
            byte[] jsonBuffer = Properties.Resources.languageData;
            string jsonData = Encoding.UTF8.GetString(jsonBuffer, 0, jsonBuffer.Length);
            lr = new LanguageReader(jsonData);

            SetText();
            CreateSudoku();
        }

        /// <summary>
        /// Function that loads the settings.
        /// </summary>
        private void LoadSettings()
        {
            GameSettings.colSize = Properties.Settings.Default.colSize;
            GameSettings.rowSize = Properties.Settings.Default.rowSize;
            GameSettings.gridSize = Properties.Settings.Default.gridSize;
            GameSettings.minValue = Properties.Settings.Default.minValue;
            GameSettings.maxValue = Properties.Settings.Default.maxValue;

            foreach(Language lang in Enum.GetValues(typeof(Language)))
            {
                if(Properties.Settings.Default.language == lang.ToString())
                {
                    InterfaceSettings.selectedLanguage = lang;
                    break;
                }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Sets the text of all the controls, based on the selected language.
        /// </summary>
        public void SetText()
        {
            mainWindowTitle.Title = LanguageReader.GetText("mainWindowTitle", InterfaceSettings.selectedLanguage);
            menuAbout.Header = LanguageReader.GetText("menuAbout", InterfaceSettings.selectedLanguage);
            menuSettings.Header = LanguageReader.GetText("menuSettings", InterfaceSettings.selectedLanguage);
            menuSettingsGame.Header = LanguageReader.GetText("menuSettingsGame", InterfaceSettings.selectedLanguage);
            menuSettingsGUI.Header = LanguageReader.GetText("menuSettingsGUI", InterfaceSettings.selectedLanguage);

            btnSolve.Content = LanguageReader.GetText("btnGo", InterfaceSettings.selectedLanguage);
            btnClear.Content = LanguageReader.GetText("btnClear", InterfaceSettings.selectedLanguage);
        }

        /// <summary>
        /// Removes and Disposes TextBox objects, if the Sudoku board is recreated.
        /// </summary>
        /// <param name="boardText"></param>
        private void DisposeObjects(TextBox[,] boardText)
        {
            for(int col = 0; col < boardText.GetLength(0); col++)
            {
                for(int row = 0; row < boardText.GetLength(1); row++)
                {
                    TextBox t = boardText[col, row];
                    t.Clear();
                    this.test.Children.Remove(t);
                }
            }
        }

        /// <summary>
        /// Draws and creates textboxes for the user
        /// </summary>
        public void CreateSudoku()
        {
            if(boardText != null)
                DisposeObjects(boardText);
            boardText = new TextBox[GameSettings.colSize, GameSettings.rowSize];

            progressBar.Maximum = (GameSettings.colSize - 1) * 10 + GameSettings.rowSize - 1;
            progressBar.Foreground = Brushes.LightBlue;

            for (int x = 0; x < GameSettings.colSize; x++)
            {
                for (int y = 0; y < GameSettings.rowSize; y++)
                {
                    int widthMargin = (y + 1) % GameSettings.gridSize == 0 ? InterfaceSettings.marginSpace : 0;
                    int heightMargin = (x + 1) % GameSettings.gridSize == 0 ? InterfaceSettings.marginSpace : 0;
                    
                    TextBox text = new TextBox();
                    text.TextChanged += text_TextChanged;   
                    text.Height = InterfaceSettings.textBoxSize;
                    text.Width = InterfaceSettings.textBoxSize;
                    text.FontSize = InterfaceSettings.fontSize;
                    text.MaxLength = GameSettings.maxValue.ToString().Length;
                    text.MaxLines = 1;
                    text.TextAlignment = TextAlignment.Center;
                    text.Margin = new Thickness(0, 0, widthMargin, heightMargin);
                    boardText[x, y] = text;
                    Grid.SetRow(text, x);
                    Grid.SetColumn(text, y);
                    this.test.Children.Add(text);
                }
            }
        }


        /// <summary>
        /// Event when text is changed in one of the sudoku textboxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void text_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            int num;
            int.TryParse(textBox.Text, out num);
            Console.WriteLine(num + "|" + num.ToString().Length);
            if ((num > GameSettings.maxValue || num.ToString().Length > GameSettings.maxValue.ToString().Length) || num == 0)
            {
                textBox.Text = String.Empty;
                textBox.Background = Brushes.White;
            }
            else
            {
                textBox.Foreground = Brushes.Blue;
                textBox.FontWeight = FontWeights.Bold;
                textBox.Background = Brushes.White;
            }
        }

        /// <summary>
        /// Button: "Solve"
        /// Solves the sudoku puzzle. If it's not possible, it will mark the "mistakes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSolve_Click(object sender, RoutedEventArgs e)
        {
            board = new int[GameSettings.colSize, GameSettings.rowSize];
            boardSet = new int[GameSettings.colSize, GameSettings.rowSize];

            for (int col = 0; col < GameSettings.colSize; col++)
            {
                for (int row = 0; row < GameSettings.rowSize; row++)
                {
                    boardText[col, row].IsReadOnly = true;
                    boardText[col, row].Background = Brushes.White;
                    if (validNumber(boardText[col, row].Text))
                    {
                        int num;
                        int.TryParse(boardText[col, row].Text, out num);
                        boardSet[col, row] = num;
                        board[col, row] = num;
                    }
                }
            }

            Solver solver = new Solver(this, board, boardSet);
            solver.StartSolver();
        }

        /// <summary>
        /// Sets the color of a textbox
        /// </summary>
        /// <param name="value"></param>
        /// <param name="col"></param>
        /// <param name="row"></param>
        public void TextColor(int value, int col, int row)
        {
            this.boardText[col, row].Background = getColor(value);
        }

        /// <summary>
        /// Gets a color based on the value of the tile
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private Brush getColor(int value)
        {
            return InterfaceSettings.colors[value % InterfaceSettings.colors.Length];
        }

        /// <summary>
        /// This sets all textboxes to ReadOnly
        /// </summary>
        /// <param name="readOnly"></param>
        public void TextReadOnly(bool readOnly)
        {
            for (int col = 0; col < GameSettings.colSize; col++)
            {
                for (int row = 0; row < GameSettings.rowSize; row++)
                {
                    boardText[col, row].IsReadOnly = readOnly;
                }
            }
        }

        /// <summary>
        /// Button "Clear"
        /// When this button is pressed, the board is reset back to starting position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            GameSettings.isRunning = false;

            progressBar.Value = progressBar.Minimum;
            for (int col = 0; col < GameSettings.colSize; col++)
            {
                for (int row = 0; row < GameSettings.rowSize; row++)
                {
                    boardText[col, row].IsReadOnly = false;
                    if (validNumber(boardText[col, row].Text))
                    {
                        boardSet[col, row] = 0;
                        board[col, row] = 0;
                        boardText[col, row].Text = "";
                    }
                }
            }
        }

        /// <summary>
        /// Draws the finished sudoku board for the user
        /// </summary>
        /// <param name="board"></param>
        /// <param name="boardSet"></param>
        public void Draw(int[,] board, int[,] boardSet)
        {
            for (int col = 0; col < GameSettings.colSize; col++)
            {
                for (int row = 0; row < GameSettings.rowSize; row++)
                {
                    boardText[col, row].Text = board[col, row].ToString();
                    if (boardSet[col, row] == 0)
                    {
                        TextBox t = boardText[col, row];
                        t.Foreground = Brushes.Black;
                        t.FontWeight = FontWeights.Medium;
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the string parameter, can be turned into a number within: GameSettings.minValue && GameSettings.maxValue
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private bool validNumber(string num)
        {
            try
            {
                int newNum;
                int.TryParse(num, out newNum);
                if (newNum >= GameSettings.minValue && newNum <= GameSettings.maxValue)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// This is the event that is opening up a new WPF window, for the user able to change GameSettings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSettingsGame_Click(object sender, RoutedEventArgs e)
        {
            if (GameSettings.isRunning)
            {
                string body = LanguageReader.GetText("gameInstanceRunningBody", InterfaceSettings.selectedLanguage);
                string title = LanguageReader.GetText("gameInstanceRunningTitle", InterfaceSettings.selectedLanguage);
                MessageBox.Show(body, title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            GameSettingsWindow gsw = new GameSettingsWindow();
            gsw.Owner = this;
            gsw.ShowDialog();
        }

        /// <summary>
        /// This is the event for when the button menuGUI is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSettingsGUI_Click(object sender, RoutedEventArgs e)
        {
            InterfaceSettingsWindow isw = new InterfaceSettingsWindow();
            isw.Owner = this;
            isw.ShowDialog();
        }

        /// <summary>
        /// This is the event for when the button menuAbout is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuAbout_Click(object sender, RoutedEventArgs e)
        {
            string about = LanguageReader.GetText("aboutDescription", InterfaceSettings.selectedLanguage);
            string title = LanguageReader.GetText("mainWindowTitle", InterfaceSettings.selectedLanguage);

            MessageBox.Show(about, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}