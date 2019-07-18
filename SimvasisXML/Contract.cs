using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SimvasisXML
{
    public class Contract
    {
        public string AA { get; set; }
        public string ContractNo { get; set; }
        public DateTime? ContractDate { get; set; }
        public string Vat { get; set; }
        public string VatCountry { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string ContractType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Decimal? Amount { get; set; }
        public string Comments { get; set; }

        public List<ContractError> Errors { get; set; }

        private static Regex regxVatCountry = new Regex("^[A-Za-z]{2}");

        public Contract()
        {

        }

        public static string XmlEncode(string input)
        {
            string output = input.Replace("&", "&amp;");
            output = output.Replace("<", "&lt;");
            output = output.Replace(">", "&gt;");
            output = output.Replace("\"", "&quot;");
            output = output.Replace("'", "&apos;");

            return output;
        }

        public Contract(string aa, string no, string contractDate, string vat, string name, string address, string type, string start, string end, string amount, string comments)
        {
            this.AA = aa;
            this.ContractNo = no.Trim();
            this.ContractDate = ProperContractDate(contractDate);
            this.Vat = vat.Trim();

            if (!regxVatCountry.IsMatch(this.Vat))
            {
                this.VatCountry = string.Empty;
            }
            else
            {
                this.Vat = this.Vat.Substring(2);
                this.VatCountry = this.Vat.Substring(0, 2);
            }

            this.CustomerName = XmlEncode(name.Trim());
            this.Address = XmlEncode(ProperAddress(address));
            this.ContractType = XmlEncode(type);
            this.Comments = XmlEncode(comments);

            

            if (!String.IsNullOrWhiteSpace(this.Comments) && this.Comments.Length > 50)
                this.Comments = this.Comments.Substring(0, 50);

            if (String.IsNullOrWhiteSpace(this.AA))
                throw new Exception("Κενό Α/Α, δε θα έπρεπε να καταγραφεί αυτή η γραμμή σαν Σύμβαση");

            if (String.IsNullOrWhiteSpace(this.ContractNo))
                AddContractError("Αριθμ.Συμφων", "O Αριθμ.Συμφων είναι κενός");
            else if (this.ContractNo.Length > 15)
            {
                this.ContractNo = this.ContractNo.Substring(0, 15);
                AddContractError("Αριθμ.Συμφων", "O Αριθμ.Συμφων είναι πάνω από 15 χαρακτήρες και θα σταλεί τμήμα του");
            }

            if (String.IsNullOrWhiteSpace(contractDate))
                AddContractError("Ημερομην..Σύνταξης", "Η Ημερομην..Σύνταξης είναι κενή");
            else
            {
                this.ContractDate = ProperRangeDate(contractDate);
                if (!this.ContractDate.HasValue)
                    AddContractError("Ημερομην..Σύνταξης", "Η Ημερομην..Σύνταξης δεν έχει σωστή μορφή (η/μ/εεεε)");
            }

            if (String.IsNullOrWhiteSpace(this.Vat))
                AddContractError("Α.Φ.Μ", "O Α.Φ.Μ είναι κενός");
            else if (this.Vat.Length > 12)
            {
                this.Vat = this.Vat.Substring(0, 12);
                AddContractError("Α.Φ.Μ", "O Α.Φ.Μ είναι πάνω από 12 χαρακτήρες και θα σταλεί τμήμα του");
            }

            if (String.IsNullOrWhiteSpace(this.CustomerName))
                AddContractError("ΕΠΩΝΥΜΙΑ", "Η ΕΠΩΝΥΜΙΑ είναι κενή");
            else if (this.CustomerName.Length > 50)
            {
                this.CustomerName = this.CustomerName.Substring(0, 50);
            }

            if (String.IsNullOrWhiteSpace(this.Address))
                AddContractError("Διεύθυνση Επαγγέλματος η Κατοικίας", "Η Διεύθυνση Επαγγέλματος η Κατοικίας είναι κενή");
            else if (this.Address.Length > 50)
            {
                this.Address = this.Address.Substring(0, 50);
            }

            if (String.IsNullOrWhiteSpace(this.ContractType))
                AddContractError("Αντικείμενο Συμφωνητικου", "Το Αντικείμενο Συμφωνητικου είναι κενό");

            if (String.IsNullOrWhiteSpace(amount))
                AddContractError("Ποσό Συμφ-κου", "Το Ποσό Συμφ-κου είναι κενό");
            else
            {
                Decimal am;
                amount = amount.Replace(",", ".");
                if (Decimal.TryParse(amount, out am))
                    this.Amount = am;
                else
                    AddContractError("Ποσό Συμφ-κου", "Το Ποσό Συμφ-κου δεν έχει σωστή μορφή (μόνο αριθμοί, τελεία για δεκαδικά κλπ)");
            }

            if (String.IsNullOrWhiteSpace(start))
                AddContractError("ΔΙΑΡΚΕΙΑ ΑΠΟ", "Η ημερομηνία ΔΙΑΡΚΕΙΑ ΑΠΟ είναι κενή");
            else
            {
                this.StartDate = ProperRangeDate(start);
                if (!this.StartDate.HasValue)
                    AddContractError("ΔΙΑΡΚΕΙΑ ΑΠΟ", "Η ημερομηνία ΔΙΑΡΚΕΙΑ ΑΠΟ δεν έχει σωστή μορφή (η/μ/εεεε)");
            }

            if (String.IsNullOrWhiteSpace(end))
                AddContractError("ΔΙΑΡΚΕΙΑ ΕΩΣ", "Η ημερομηνία ΔΙΑΡΚΕΙΑ ΕΩΣ είναι κενή");
            else
            {
                this.EndDate = ProperRangeDate(end);
                if (!this.EndDate.HasValue)
                    AddContractError("ΔΙΑΡΚΕΙΑ ΕΩΣ", "Η ημερομηνία ΔΙΑΡΚΕΙΑ ΕΩΣ δεν έχει σωστή μορφή (η/μ/εεεε)");
            }
        }

        public string ProperAddress(string input)
        {
            string proper = input;

            if (!String.IsNullOrWhiteSpace(proper))
                proper = proper.Trim().Replace(Environment.NewLine, " ");

            return proper;
        }

        public DateTime? ProperContractDate(string input)
        {
            DateTime? val = null;

            string proper = input;

            if (!String.IsNullOrWhiteSpace(proper))
            {
                //remove unexpected ,
                proper = proper.Replace(",", "");

                //remove leading commas and spaces
                while (true)
                {
                    if (proper.Substring(0, 1) == " ")
                        proper = proper.Substring(1);
                    else
                        break;
                }

                //remove spaces
                proper = proper.Trim();

                //remove prefix ,
                proper = proper.Replace(",", "");

                //replace . with /
                proper = proper.Replace(".", "/");

                DateTime dt;
                if (!DateTime.TryParseExact(proper, "d/M/yyyy", null, DateTimeStyles.None, out dt))
                    val = null;
                else
                    val = dt;
            }

            return val;
        }

        public DateTime? ProperRangeDate(string input)
        {
            DateTime? val = null;

            string proper = input;

            if (!String.IsNullOrWhiteSpace(proper))
            {
                //remove spaces
                proper = proper.TrimStart();

                //remove ,
                proper = proper.Replace(",", "");

                //remove spaces
                proper = proper.TrimStart();

                //remove time part if present
                int sep = proper.IndexOf(" ");
                if (sep >= 0)
                    proper = proper.Substring(0, sep);

                //replace . with /
                proper = proper.Replace(".", "/");

                DateTime dt;
                if (!DateTime.TryParseExact(proper, "d/M/yyyy", null, DateTimeStyles.None, out dt))
                {
                    if (!DateTime.TryParseExact(proper, "d/M/yy", null, DateTimeStyles.None, out dt))
                    {
                        val = null;
                    }
                    else
                        val = dt;
                }
                else
                    val = dt;
            }

            return val;
        }

        private void AddContractError(string field, string description)
        {
            if (this.Errors == null)
                this.Errors = new List<ContractError>();

            this.Errors.Add(new ContractError(field, description));
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("No {2}{0}{1}Ημνια {3}{0}{1}ΑΦΜ {4}{0}{1}Ονομα {5}{0}{1}Διεθ {6}{0}{1}Αντικειμενο {7}{0}{1}Απο {8}{0}{1}Εως {9}{0}{1}Ποσο {10}{0}{1}Σχολια {11}",
                            Environment.NewLine, "\t",
                            this.ContractNo, this.ContractDate, this.Vat, this.CustomerName, this.Address, this.ContractType, this.StartDate, this.EndDate, this.Amount, this.Comments
                            );
            return sb.ToString();
        }


        public static List<Contract> ProcessExcelFile(string filename, Action<string> showMessage)
        {
            //string connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=yes'", filename);

            List<Contract> contracts = new List<Contract>();
            Contract c;

            string con = Properties.Settings.Default.connectionString;
            try
            {
                con = string.Format(con, filename);
            }
            catch
            {
                throw new Exception($"To connectionString στο .config αρχείο δε είναι σωστό{Environment.NewLine}{con}");
            }
            //string con = $"Provider=Microsoft.ACE.OLEDB.16.0;Data Source={filename};" + @"Extended Properties='Excel 8.0;HDR=Yes;'";
            using (OleDbConnection connection = new OleDbConnection(con))
            {
                connection.Open();

                var tables = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (tables == null || tables.Rows.Count == 0)
                    throw new Exception("Το επιλεγμένο αρχείο δε φαίνεται να περιέχει sheets!");
                var sheetName = tables.Rows[0]["TABLE_NAME"].ToString();

                OleDbCommand command = new OleDbCommand($"select * from [{sheetName}]", connection);
                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    string aa, contractNo, contractDate, vat, customerName, address, contractType, startDate, endDate, amount, comments;
                    int idx = 0;
                    while (dr.Read())
                    {
                        aa = dr[0].ToString();
                        contractNo = dr[1].ToString();
                        contractDate = dr[2].ToString();
                        vat = dr[3].ToString();
                        customerName = dr[4].ToString();
                        address = dr[5].ToString();
                        contractType = dr[6].ToString();
                        startDate = dr[7].ToString();
                        endDate = dr[8].ToString();
                        amount = dr[9].ToString();
                        comments = dr[10].ToString();

                        idx++;
                        showMessage(String.Format("Ανάγνωση γραμμής {0} με αρ. σύμβασης {1}...", idx.ToString(), contractNo));

                        if (!String.IsNullOrWhiteSpace(contractNo) &&
                            !String.IsNullOrWhiteSpace(contractDate) && 
                            !String.IsNullOrWhiteSpace(vat) &&
                            !String.IsNullOrWhiteSpace(customerName) &&
                            !String.IsNullOrWhiteSpace(address) &&
                            !String.IsNullOrWhiteSpace(contractType) &&
                            !String.IsNullOrWhiteSpace(startDate) &&
                            !String.IsNullOrWhiteSpace(endDate) &&
                            !String.IsNullOrWhiteSpace(amount))
                        {
                            c = new Contract(aa: aa, no: contractNo, contractDate: contractDate, vat: vat, name: customerName, address: address, type: contractType, start: startDate, end: endDate, amount: amount, comments: comments);
                            contracts.Add(c);
                            //Console.WriteLine(c.ToString());
                        }
                        else
                        {
                            Console.WriteLine("Incorrect data found");
                        }
                    }
                }
            }

            if (contracts!= null && contracts.Count > 20 * 16)
            {
                string msg = String.Format("Το αρχείο περιέχει {0} συμβάσεις, αλλά ενδέχεται να επιτρέπεται η ταυτόχρονη αποστολή μέχρι {1} συμβάσεων, οπότε το αρχείο σας να μη γίνει δεκτό.{2}Σε αυτήν την περίπτωση, παρακαλώ δημιουργήστε διαφορετικά αρχεία Excel ώστε να μην ξεπερνάτε το όριο του gsis.gr", contracts.Count.ToString(), (20*16).ToString(), Environment.NewLine);
                MessageBox.Show(msg, "Προσοχή", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            //verify contract dates within same trimester
            if (contracts != null && contracts.Count > 0)
            {
                DateTime refDate = contracts.FirstOrDefault().ContractDate.GetValueOrDefault();
                int year = 0, trimester = 0, year1 = 0, trimester1 = 0;
                GetDateTrimester(refDate, ref year, ref trimester);
                foreach (Contract contract in contracts)
                {
                    GetDateTrimester(contract.ContractDate.GetValueOrDefault(), ref year1, ref trimester1);
                    if (year != year1 || trimester != trimester1)
                    {
                        contract.AddContractError("Ημερομην..Σύνταξης", String.Format("Η ημερομηνία σύνταξης [{1}] στο συμβόλαιο με Α/Α {0} δεν είναι εντός ίδιου τριμήνου με την 1η σύμβαση στο αρχείο!", contract.AA, contract.ContractDate.GetValueOrDefault().ToString()));
                    }
                }
            }

            return contracts;
        }

        private static void GetDateTrimester(DateTime contractDate, ref int year, ref int trimester)
        {
            year = contractDate.Year;
            int month = contractDate.Month;

            switch (month)
            {
                case 1:
                case 2:
                case 3:
                    trimester = 1;
                    break;
                case 4:
                case 5:
                case 6:
                    trimester = 2;
                    break;
                case 7:
                case 8:
                case 9:
                    trimester = 3;
                    break;
                case 10:
                case 11:
                case 12:
                    trimester = 4;
                    break;
                default:
                    throw new Exception("Date month trimester error");
            }
        }

        public class ContractError
        {
            public string Field { get; set; }
            public string Description { get; set; }

            public ContractError(string field, string description)
            {
                this.Field = field;
                this.Description = description;
            }
        }
    }
}
