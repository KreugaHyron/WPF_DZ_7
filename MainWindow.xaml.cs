using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WPF_DZ_7
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string currentText = "";
        private int currentCharIndex = 0;
        private int errors = 0;
        private DispatcherTimer timer;
        private DateTime startTime;
        public MainWindow()
        {
            InitializeComponent();
            UpdateInstruction();
            LengthSlider.ValueChanged += LengthSlider_ValueChanged;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }
        private void LengthSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SliderValue.Text = ((int)e.NewValue).ToString();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            currentCharIndex = 0;
            errors = 0;
            currentText = GenerateRandomString((int)LengthSlider.Value, CaseCheckBox.IsChecked == true);
            UpdateInstruction();
            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;
            InstructionText.Text = $"Натисніть: {currentText[currentCharIndex]}";
            startTime = DateTime.Now;
            timer.Start();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;
            MessageBox.Show($"Тренування завершено! Помилки: {errors}");
        }

        private string GenerateRandomString(int length, bool caseSensitive)
        {
            Random random = new Random();
            char[] chars = caseSensitive ? "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray()
                                           : "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }
            return new string(result);
        }

        private void UpdateInstruction()
        {
            StatsText.Text = $"Помилки: {errors}";
        }

        private void Key_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null && clickedButton.Content.ToString() == currentText[currentCharIndex].ToString())
            {
                currentCharIndex++;
            }
            else
            {
                errors++;
            }

            if (currentCharIndex >= currentText.Length)
            {
                StopButton_Click(this, null); 
                return;
            }

            InstructionText.Text = $"Натисніть: {currentText[currentCharIndex]}";
            UpdateInstruction();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsed = DateTime.Now - startTime;
            double seconds = elapsed.TotalSeconds;

            double speed = (currentCharIndex + errors) / seconds * 60;
            SpeedText.Text = $"Швидкість: {speed:F2} символів/хв";
        }
    }
}
