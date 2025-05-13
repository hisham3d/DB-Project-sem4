using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
//using Microsoft.Data.SqlClient;

namespace Project_FrontEnd
{
    public partial class MainPage : Form
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void MainPageTravelerButton_Click(object sender, EventArgs e)
        {   
            TravelerLogIn travelerLogin = new TravelerLogIn();
            travelerLogin.Show();
            this.Hide();
        }

        private void MainPageOperatorButton_Click(object sender, EventArgs e)
        {
            OperatorLogin operatorLogin = new OperatorLogin();
            operatorLogin.Show();
            this.Hide();
        }

        private void MainPageAdminButton_Click(object sender, EventArgs e)
        {
            AdminLogin adminLogin = new AdminLogin();
            adminLogin.Show();
            this.Hide();
        }

        private void MainPageServiceProviderButton_Click(object sender, EventArgs e)
        {
            ServiceProviderLogin providerLogin = new ServiceProviderLogin();
            providerLogin.Show();
            this.Hide();
        }
    }
}