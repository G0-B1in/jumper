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

namespace попрыженок
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Agent> startvalue;
        List<Agent> tbsort;
        List<Agent> cb1sort;
        List<Agent> cb2sort;
        List<string> listforcbtype = new List<string>();
        List<string> listforcbf = new List<string>();

        List<Agent> listpriority = new List<Agent>();

        public static AgentsEntities baza;

        public MainWindow()
        {
            InitializeComponent();
            baza = new AgentsEntities();
            startvalue = baza.Agent.ToList();
            tbsort = baza.Agent.ToList();
            cb1sort = baza.Agent.ToList();
            cb2sort = baza.Agent.ToList();

            lwAgents.ItemsSource = baza.Agent.ToList();

            listforcbtype.Add("Все типы");
            cbType.SelectedItem = listforcbtype[0];
            var listType = baza.AgentType.ToList();
            foreach (var a in listType)
            {
                listforcbtype.Add(a.Title);
            }
            cbType.ItemsSource = listforcbtype;

            listforcbf.Add("Без сортировки");
            listforcbf.Add("Наименование ↑");
            listforcbf.Add("Наименование ↓");
            listforcbf.Add("Скидка ↑");
            listforcbf.Add("Скидка ↓");
            listforcbf.Add("Приоритет ↑");
            listforcbf.Add("Приоритет ↓");

            cbFilters.SelectedItem = listforcbf[0];
            cbFilters.ItemsSource = listforcbf;
        }

        private int currentPage = 1;
        private int countPosition = 10;
        private int maxPages;

        private void Refresh()
        {
            var listPizza = startvalue;
            maxPages = (int)Math.Ceiling(listPizza.Count * 1.0 / countPosition);

            var listPizzaPage = listPizza.Skip((currentPage - 1) * countPosition).Take(countPosition).ToList();

            TxtCurrentPage.Text = currentPage.ToString();
            LblTotalPages.Content = "of " + maxPages;

            lwAgents.ItemsSource = listPizzaPage;
        }

        private void GoToFirstPage(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
            Refresh();

        }

        private void GoToPreviousPage(object sender, RoutedEventArgs e)
        {
            if (currentPage <= 1) currentPage = 1;
            else
                currentPage--;
            Refresh();
        }

        private void GoToNextPage(object sender, RoutedEventArgs e)
        {
            if (currentPage >= maxPages) currentPage = maxPages;
            else
                currentPage++;
            Refresh();
        }

        private void GoToLastPage(object sender, RoutedEventArgs e)
        {
            currentPage = maxPages;
            Refresh();
        }


        public void TextSort()
        {
            if (tbSearch.Text != "")
                tbsort = startvalue.Where(p => p.Title.Contains(tbSearch.Text) | p.Email.Contains(tbSearch.Text) | p.Phone.Contains(tbSearch.Text)).ToList();
            else
                tbsort = startvalue;
            TypeSort();
        }

        public void TypeSort()
        {
            var selectedType = cbType.SelectedItem as string;
            if (selectedType != listforcbtype[0])
            {
                cb1sort = tbsort.Where(p => p.AgentType.Title == selectedType).ToList();
            }
            else
            {
                cb1sort = tbsort;
            }

            SORT();

        }

        public void SORT()
        {
            var selectedFilter = cbFilters.SelectedItem as string;
            if (selectedFilter == "Без сортировки")
            {
                cb2sort = cb1sort;
            }
            else if (selectedFilter == "Наименование ↑")
            {
                cb2sort = cb1sort.OrderBy(p => p.Title).ToList();
            }
            else if (selectedFilter == "Наименование ↓")
            {
                cb2sort = cb1sort.OrderByDescending(p => p.Title).ToList();
            }
            else if (selectedFilter == "Скидка ↑")
            {
                cb2sort = cb1sort.OrderBy(p => p.Discount).ToList();
            }
            else if (selectedFilter == "Скидка ↓")
            {
                cb2sort = cb1sort.OrderByDescending(p => p.Discount).ToList();
            }
            else if (selectedFilter == "Приоритет ↑")
            {
                cb2sort = cb1sort.OrderBy(p => p.Priority).ToList();
            }
            else
            {
                cb2sort = cb1sort.OrderByDescending(p => p.Priority).ToList();
            }

            lwAgents.ItemsSource = cb2sort.ToList();
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextSort();
        }

        private void cbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TypeSort();
        }

        private void cbFilters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SORT();
        }

        private void lwAgents_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedagent = lwAgents.SelectedItem as Agent;
            listpriority.Add(selectedagent);
            if (listpriority.Count > 0)
                btnPriority.IsEnabled = true;    
        }

        private void btnPriority_Click(object sender, RoutedEventArgs e)
        {
            changPriority changPriority = new changPriority(listpriority);
            changPriority.ShowDialog();
            lwAgents.ItemsSource = null;
            lwAgents.ItemsSource = cb2sort.ToList();
            listpriority = null;
            btnPriority.IsEnabled = false;
        }

        private void Adit_Click(object sender, RoutedEventArgs e)
        {
            listpriority = null;
            btnPriority.IsEnabled = false;

            var editButton = sender as Button;
            var selectedAgent = editButton.DataContext as Agent;

            AditAgent aditAgent = new AditAgent(selectedAgent);
            aditAgent.ShowDialog();
            lwAgents.ItemsSource = null;
            lwAgents.ItemsSource = baza.Agent.ToList();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Agent agent = new Agent();
            AditAgent aditAgent = new AditAgent(agent);
            aditAgent.delbtn.Visibility = Visibility.Hidden;
            aditAgent.ShowDialog();
            lwAgents.ItemsSource = null;
            lwAgents.ItemsSource = baza.Agent.ToList();
        }
    }
}
