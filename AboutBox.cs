using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace assignment07_WinDEXT
{
    partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
            InitializeGUI();
            
            //this.Text = String.Format("About {0}", AssemblyTitle);
            //this.labelProductName.Text = AssemblyProduct;
            //this.labelCopyright.Text = AssemblyCopyright;
            //this.labelCompanyName.Text = AssemblyCompany;
            //this.textBoxDescription.Text = AssemblyDescription;
        }

        private void InitializeGUI()
        {
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;

            string longText = "Some years ago I got interested in keeping track on how much a " +
                "purchased item cost each day during its lifespan. That is, the daily cost from " +
                "the day it was purchased to the day it had run out or broken down. This was " +
                "done using pen and paper and an Excel spreadsheet. Win DExT is an attempt to try " +
                "to take this idea a step further using C#.";

            this.labelVersion.Text = String.Format("Version {0}", AssemblyVersion);

            this.Text = "About Win DExt";
            this.labelProductName.Text = "Win DExt - Daily Expense rate Tracker for Windows";
            this.labelCopyright.Text = "Happily coded by Fredrik Wallin, dec 2022 and jan 2023";
            this.textBoxDescription.Text = "This is the 7th assignment in the course Programming in C#, Malmö university, autumn 2022." + Environment.NewLine + Environment.NewLine + longText;

        }

        #region Assembly Attribute Accessors

        //public string AssemblyTitle
        //{
        //    get
        //    {
        //        object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
        //        if (attributes.Length > 0)
        //        {
        //            AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
        //            if (titleAttribute.Title != "")
        //            {
        //                return titleAttribute.Title;
        //            }
        //        }
        //        return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
        //    }
        //}

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        //public string AssemblyDescription
        //{
        //    get
        //    {
        //        object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
        //        if (attributes.Length == 0)
        //        {
        //            return "";
        //        }
        //        return ((AssemblyDescriptionAttribute)attributes[0]).Description;
        //    }
        //}

        //public string AssemblyProduct
        //{
        //    get
        //    {
        //        object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
        //        if (attributes.Length == 0)
        //        {
        //            return "";
        //        }
        //        return ((AssemblyProductAttribute)attributes[0]).Product;
        //    }
        //}

        //public string AssemblyCopyright
        //{
        //    get
        //    {
        //        object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
        //        if (attributes.Length == 0)
        //        {
        //            return "";
        //        }
        //        return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
        //    }
        //}

        //public string AssemblyCompany
        //{
        //    get
        //    {
        //        object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
        //        if (attributes.Length == 0)
        //        {
        //            return "";
        //        }
        //        return ((AssemblyCompanyAttribute)attributes[0]).Company;
        //    }
        //}
        #endregion

        /// <summary>
        /// Stänger AboutBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
