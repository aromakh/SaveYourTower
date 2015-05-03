﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaveYourTower.DesktopUI.GamePages
{
    public partial class HelpPage : UserControl, ILoadable
    {
        public event Action<Type> PageEventHandler;

        public HelpPage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PageEventHandler(typeof(MainPage));
            this.Dispose();
        }
    }
}