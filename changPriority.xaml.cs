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
    /// Логика взаимодействия для changPriority.xaml
    /// </summary>
    public partial class changPriority : Window
    {
        List<Agent> listpriority;

        public changPriority(List<Agent> listpriority)
        {
            InitializeComponent();

            this.listpriority = listpriority;
            int a = 0;
            for(int i = 0; i < listpriority.Count; i++)
            {
                if (a < listpriority[i].Priority) a = listpriority[i].Priority;
            }

            PriorityCount.Text = a.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < listpriority.Count; i++)
            {
                listpriority[i].Priority = int.Parse(PriorityCount.Text);
            }
            MainWindow.baza.SaveChanges();
            this.Close();
        }
    }
}
