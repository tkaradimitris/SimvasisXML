using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

///TODO: Save file on success, ask for target filename/location, show file in folder on success

namespace SimvasisXML
{
    public partial class frmMain : Form
    {
        private const string appTitle = "Δημιουργία Αρχείου Συμβάσεων σε xml";
        public frmMain()
        {
            InitializeComponent();
            this.Text = appTitle;
        }

        private void pbGenerate_Click(object sender, EventArgs e)
        {
            GenerateXmlFile();
        }

        private void GenerateXmlFile()
        {
            string filenameExcel = txtVat.Text;

            DialogResult res = SelectInputFile(ref filenameExcel);

            if (res != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            else if (String.IsNullOrWhiteSpace(filenameExcel))
            {
                MessageBox.Show("Πρέπει να επιλέξετε το excel αρχείο των συμβάσεων που θα αποθηκευτεί σε μορφή xml", appTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (!File.Exists(filenameExcel))
            {
                MessageBox.Show("Το αρχείο " + filenameExcel + " δε βρέθηκε στο σύστημά σας", appTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (String.IsNullOrWhiteSpace(txtVat.Text) || txtVat.Text.Length != 9)
            {
                MessageBox.Show("Πρέπει να συμπληρώσετε το ΑΦΜ της εταιρείας σας σωστά (9ψήφιο)", appTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DisableForm();

            XmlGenerator generator = new XmlGenerator();
            generator.CompanyVat = txtVat.Text;
            try
            {
                generator.Contracts = Contract.ProcessExcelFile(filenameExcel, ShowMessage);
                
                ShowMessage(String.Format("{0} {1}o τρίμηνο. Αριθμός Συμβάσεων {2}",generator.Year.ToString(), generator.Trimester.ToString(), generator.ContractsCount.ToString()));
            }
            catch(Exception ex)
            {
                MessageBox.Show("Λάθος κατά την ανάγνωση του αρχείου των συμβάσεων.\nΜήπως δεν έχετε διαγράψει την 1η γραμμή του αρχείου που παράγει το λογιστικό σας πρόγραμμα;\n\n" + ex.Message, appTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            DialogResult resErrors = System.Windows.Forms.DialogResult.OK;

            if (generator.ErrorsNo > 0)
            {
                string msg = String.Format("Εντοπίστηκ{2} {3} λάθ{4} στα δεδομένα του αρχείου.{0}Θέλετε να δημιουργήσετε το xml αρχείο παρόλαυτα;",
                    Environment.NewLine, 
                    "\t", 
                    (generator.ErrorsNo > 1) ? "αν" : "ε" ,
                    generator.ErrorsNo.ToString(), 
                    (generator.ErrorsNo > 1) ? "η" : "ος" 
                    );
                List<string> errors = generator.TopErrors;
                foreach (string s in errors)
                {
                    msg += Environment.NewLine + s;
                }
                resErrors = MessageBox.Show(msg, appTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }

            if (resErrors == System.Windows.Forms.DialogResult.OK)
            {
                StringBuilder sb = generator.GenerateXml();
                FileInfo fi1 = new FileInfo(filenameExcel);
                string filename = filenameExcel;
                filename = filename.Substring(0, filename.Length - fi1.Extension.Length) + ".xml";
                if (SelectOutputFile(ref filename) == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(filename, sb.ToString());
                        string argument = @"/select, """ + filename + @"""";

                        System.Diagnostics.Process.Start("explorer.exe", argument);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Λάθος κατά την αποθήκευση των xml δεδομένων στο επιλεγμένο αρχείο:\n" + ex.Message, appTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            EnableForm();
        }

        private void ShowMessage(string txt)
        {
            lblMessage.Text = txt;
        }

        private void DisableForm()
        {
            Cursor.Current = Cursors.WaitCursor;
            txtVat.Enabled = false;
            pbGenerate.Enabled = false;
        }
        private void EnableForm()
        {
            Cursor.Current = Cursors.Default;
            txtVat.Enabled = true;
            pbGenerate.Enabled = true;
        }


        private DialogResult SelectInputFile(ref string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            
            filename = string.Empty;

            //openFileDlg.Filter = "Excel xslx files|*.xlsx|Excel xls files|*.xls|All files|*.*";
            openFileDlg.Filter = "Excel xlsx files|*.xlsx|Excel xls files|*.xls|All files|*.*";
            openFileDlg.InitialDirectory = path;
            openFileDlg.Title = "Επιλογή αρχείο συμβάσεων σε μορφή excel";
            DialogResult res =  openFileDlg.ShowDialog();
            if (res == DialogResult.OK)
            {
                filename = openFileDlg.FileName;
            }

            return res;
        }
        private DialogResult SelectOutputFile(ref string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            //filename = string.Empty;
            saveFileDlg.FileName = filename;
            saveFileDlg.InitialDirectory = path;
            saveFileDlg.Title = "Επιλογή αρχείο συμβάσεων σε μορφή xml";
            saveFileDlg.Filter = "XML Files|*.xml|All files|*.*";
            DialogResult res = saveFileDlg.ShowDialog();
            if (res == DialogResult.OK)
            {
                filename = saveFileDlg.FileName;
            }

            return res;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            txtVat.Text = Properties.Settings.Default.afm;
        }


    }
}
