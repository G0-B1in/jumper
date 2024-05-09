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
using System.Windows.Shapes;

namespace попрыженок
{
    /// <summary>
    /// Логика взаимодействия для AditAgent.xaml
    /// </summary>
    public partial class AditAgent : Window
    {
        private Agent agent;
        public AditAgent(Agent agent)
        {
            InitializeComponent();
            this.agent = agent;
            DataContext = agent;
            cbType.ItemsSource = MainWindow.baza.AgentType.ToList();
            dgProdSale.ItemsSource = MainWindow.baza.ProductSale.Where(p => p.AgentID == agent.ID).ToList();
            cbProducts.ItemsSource = MainWindow.baza.Product.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (agent.ID == 0)
            {
                MainWindow.baza.Agent.Add(agent);
            }
            MainWindow.baza.SaveChanges();

            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var selectedprod = cbProducts.SelectedItem as Product;
            ProductSale ps = new ProductSale();
            ps.AgentID = agent.ID;
            ps.ProductID = selectedprod.ID;
            ps.ProductCount = 0;
            DateTime now = DateTime.Now;
            ps.SaleDate = now;
            if (ps.ID == 0)
            {
                MainWindow.baza.ProductSale.Add(ps);
            }
            MainWindow.baza.SaveChanges();
            dgProdSale.ItemsSource = null;
            dgProdSale.ItemsSource = MainWindow.baza.ProductSale.Where(p => p.AgentID == agent.ID).ToList();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var delButton = sender as Button;
            var deletedPS = delButton.DataContext as ProductSale;

            if (deletedPS != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    "Вы точно хотите удалить запись?",
                    "Внимание!",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Error);

                if (result == MessageBoxResult.Yes)
                {
                    MainWindow.baza.ProductSale.Remove(deletedPS);
                    MainWindow.baza.SaveChanges();
                    MessageBox.Show("Запись удалена!");
                    dgProdSale.ItemsSource = null;
                    dgProdSale.ItemsSource = MainWindow.baza.ProductSale.Where(p => p.AgentID == agent.ID).ToList();
                }

            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var ps = MainWindow.baza.ProductSale.Where(p => p.AgentID == agent.ID);
            if (agent.ProductSale.Count != 0)
            {
                MessageBox.Show("Этого агента нельзя удалять");
            }
            else
            {
                MessageBoxResult result = MessageBox.Show(
                    "Вы точно хотите удалить запись?",
                    "Внимание!",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Error);

                if (result == MessageBoxResult.Yes)
                {
                    MainWindow.baza.Agent.Remove(agent);
                    MainWindow.baza.SaveChanges();
                    MessageBox.Show("Агент удален");
                    MainWindow.baza.SaveChanges();
                    this.Close();
                }
            }
        }

        private void ButtonLogo_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Images (*.png)|*.png|All files (*.*)|*.*";
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                string filename = openFileDialog.SafeFileName;
                agent.Logo = "agents/" + filename;
                DataContext = null;
                DataContext = agent;
            }
        }
    }
}
