using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Configuration;
using PPRetail;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        String locationId;

        String accessToken;
        String PPENDPOINT_BASE_ADDRESS;

        String PPENDPOINT_TABS;
        String PPENDPOINT_TAB;

        String PPENDPOINT_LOCATIONS;
        String PPENDPOINT_LOCATION;

        String PPENDPOINT_INVOICES;
        String PPENDPOINT_INVOICE;


        RetailManager retailMgr = RetailManager.GetInstance();

        public Form1()
        {
            InitializeComponent();
            InitializeConfig();

        }
        public void InitializeConfig()
        {
            accessToken = ConfigurationManager.AppSettings["pp_access_token"];
            locationId = ConfigurationManager.AppSettings["pp_location"];
            PPENDPOINT_BASE_ADDRESS = ConfigurationManager.AppSettings["pp_base_address"];

            PPENDPOINT_TABS = ConfigurationManager.AppSettings["pp_uri_tabs"];
            PPENDPOINT_TAB = ConfigurationManager.AppSettings["pp_uri_tab"];

            PPENDPOINT_LOCATIONS = ConfigurationManager.AppSettings["pp_uri_locations"];
            PPENDPOINT_LOCATION = ConfigurationManager.AppSettings["pp_uri_location"];

            PPENDPOINT_INVOICES = ConfigurationManager.AppSettings["pp_uri_invoices"];
            PPENDPOINT_INVOICE = ConfigurationManager.AppSettings["pp_uri_invoice"];


        }
        public void InitializeView()
        {
            ColumnHeader colHeaderCustomerName = new ColumnHeader();
            colHeaderCustomerName.Text = "Customer";
            colHeaderCustomerName.TextAlign = HorizontalAlignment.Center;
            colHeaderCustomerName.Width = 150;


            ColumnHeader colHeaderCheckinTime = new ColumnHeader();
            colHeaderCheckinTime.Text = "CheckIn Time";
            colHeaderCheckinTime.Width = 100;


            ColumnHeader colHeaderCheckinExpiry = new ColumnHeader();
            colHeaderCheckinExpiry.Text = "Checkin Expiry";
            colHeaderCheckinExpiry.Width = 100;


            ColumnHeader colHeaderCheckinStatus = new ColumnHeader();
            colHeaderCheckinStatus.Text = "Status";
            colHeaderCheckinStatus.Width = 100;


            ColumnHeader colHeaderTabAssociated = new ColumnHeader();
            colHeaderTabAssociated.Text = "Tab";
            colHeaderTabAssociated.Width = 100;

            listView1.Columns.Add(colHeaderCustomerName);
            listView1.Columns.Add(colHeaderCheckinTime);
            listView1.Columns.Add(colHeaderCheckinExpiry);
            listView1.Columns.Add(colHeaderCheckinStatus);
            listView1.Columns.Add(colHeaderTabAssociated);


            listView1.View = View.Details;

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
            InitializeView();

            //Call PP to get the checkins
            try
            {
                GetPPCustomerTabs(locationId);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            int count = listView1.SelectedItems.Count;
            if (count == 1)
            {
                ListViewItem itemSelected = listView1.SelectedItems[0];
                PPCustomerTab ppTab = (PPCustomerTab)itemSelected.Tag;

                if (ppTab != null)
                {
                    // create invoice and associated with internal tab
                    String invoiceId = CreateInvoice(locationId,ppTab.id);
                    //if (invoiceId != null)
                    {
                        ppTab.invoiceId = invoiceId;

                        itemSelected.SubItems[4].Text = "YES";
                        itemSelected.BackColor = Color.AliceBlue;
                        itemSelected.Selected = true;
                        listView1.Select();
                    }
                }
            }
            
        }
        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("refreshing tabs...");
            GetPPCustomerTabs(locationId);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("charging...");

            ITabManager tabMgr = retailMgr.GetTabManager();
            int count = listView1.SelectedItems.Count;
            if (count == 1)
            {
                ListViewItem itemSelected = listView1.SelectedItems[0];
                PPCustomerTab ppTab = (PPCustomerTab)itemSelected.Tag;

                if (ppTab != null)
                {
                    //tabMgr.ChargeTab(ppTab);
                }
            }
        }
        public async void GetPPCustomerTabs(String locationId) 
        {
            List<PPCustomerTab> lstTabs = new List<PPCustomerTab>();
            try
            {

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(PPENDPOINT_BASE_ADDRESS);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",accessToken);

                String tabsUri = String.Format(PPENDPOINT_TABS, locationId);

                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, 
                    X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                HttpResponseMessage response = await httpClient.GetAsync(tabsUri);
                string responseBody = await response.Content.ReadAsStringAsync();
                
                System.Console.WriteLine(responseBody);

                if (response.IsSuccessStatusCode)
                {
                    PPCustomerTabs tabs = JsonConvert.DeserializeObject<PPCustomerTabs>(responseBody);

                    List<PPCustomerTab> tabs2 = tabs.tabs;
                    foreach (PPCustomerTab tab in tabs2)
                    {
                        System.Console.WriteLine("tabId={0}", tab.id);
                        System.Console.WriteLine("tabStatus={0}", tab.status);
                        System.Console.WriteLine("tabExpiry={0}", tab.expirationDate);
                        System.Console.WriteLine("customerName={0}", tab.customerName);

                        AddToList(tab);
                    }
                }
                else
                {
                    PPError error = JsonConvert.DeserializeObject<PPError>(responseBody);
                    MessageBox.Show(error.message);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }

        }
        void AddToList(PPCustomerTab tab)
        {
            ListViewItem item = new ListViewItem(tab.customerName);
            item.Tag = tab;
            item.SubItems.Add(tab.createDate);
            item.SubItems.Add(tab.expirationDate);
            item.SubItems.Add(tab.status);
            item.SubItems.Add("No");

            listView1.Items.Add(item);

        }
        String CreateInvoice(String locationId, String tabId) 
        {
            String invoiceId = null;

            return invoiceId;
        }

    }
}
