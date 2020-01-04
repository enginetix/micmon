using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace micmon
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                this.Text = string.Format("Your application name - v{0}",
                    ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(4));
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
