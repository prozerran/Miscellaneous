using System;
using System.Globalization;
using System.Windows;

// https://docs.microsoft.com/en-us/bingmaps/rest-services/common-parameters-and-types/supported-culture-codes
// https://azuliadesigns.com/list-net-culture-country-codes/

namespace ChangeUICulture
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DateTime _displayDate = DateTime.Now;

        public MainWindow()
        {
            InitializeComponent();
            UpdateUIElements();
        }

        private void UpdateUIElements()
        {
            CurrentCultureTextBlock.Text = string.Format(
                Properties.Resources.CurrentCulture,
                CultureInfo.CurrentCulture.ToString());

            DateTextBlock.Text = _displayDate.ToString("d");
            LongDateTextBlock.Text = _displayDate.ToString("D");
        }

        private void USButton_Click(object sender, RoutedEventArgs e)
        {
            //Properties.Resources.Culture = new CultureInfo("en-US");
            App.ChangeCulture(new CultureInfo("en-US"));
        }

        private void CNButton_Click(object sender, RoutedEventArgs e)
        {
            //Properties.Resources.Culture = new CultureInfo("zh-CN");
            App.ChangeCulture(new CultureInfo("zh-CN"));
        }

        private void HantButton_Click(object sender, RoutedEventArgs e)
        {
            //Properties.Resources.Culture = new CultureInfo("zh-Hant");
            App.ChangeCulture(new CultureInfo("zh-Hant"));
        }

        private void HansButton_Click(object sender, RoutedEventArgs e)
        {
            //Properties.Resources.Culture = new CultureInfo("zh-Hans");
            App.ChangeCulture(new CultureInfo("zh-Hans"));
        }

        private void JAButton_Click(object sender, RoutedEventArgs e)
        {
            //Properties.Resources.Culture = new CultureInfo("ja-JP");
            App.ChangeCulture(new CultureInfo("ja-JP"));
        }
    }
}
