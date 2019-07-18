using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SimvasisXML
{

    public class XmlGenerator
    {
        public string CompanyVat { get; set; }
        public List<Contract> Contracts { get; set; }


        public int ErrorsNo
        {
            get
            {
                int errorsNo = 0;

                if (this.Contracts != null)
                {
                    foreach (Contract c in this.Contracts)
                    {
                        if (c.Errors != null)
                            errorsNo += c.Errors.Count;
                    }
                }
                return errorsNo;
            }
        }

        public List<string> TopErrors
        {
            get
            {
                int counter = 0;
                List<string> errs = new List<string>();
                if (this.Contracts != null)
                {
                    foreach (Contract c in this.Contracts)
                    {
                        if (c.Errors != null)
                        {
                            foreach (Contract.ContractError e in c.Errors)
                            {
                                errs.Add("AA " + c.AA + ": " + e.Description);
                                counter++;
                                if (counter >= 10)
                                    break;
                            }
                        }
                        if (counter >= 10)
                            break;
                    }
                }

                return errs;
            }
        }

        //public void AddContract(string no, string contractDate, string vat, string name, string address, string type, string start, string end, string amount, string comments)
        //{
        //    if (contracts == null)
        //        contracts = new List<Contract>();

        //    Contract c = new Contract(no: no, contractDate: contractDate, vat: vat, name: name, address: address, type: type, start: start, end: end, amount: amount, comments: comments);

        //    contracts.Add(c);
        //}

        public int Year
        {
            get
            {
                int y = DateTime.Now.AddDays(-30).Year;

                if (this.Contracts != null)
                {
                    y = this.Contracts[0].ContractDate.GetValueOrDefault().Year;
                }

                return y;
            }
        }
        public int Trimester
        {
            get
            {
                int t = DateTime.Now.AddDays(-30).Month;


                if (this.Contracts != null)
                {
                    t = this.Contracts[0].ContractDate.GetValueOrDefault().Month;
                }

                switch (t)
                {
                    case 1:
                    case 2:
                    case 3:
                        t = 1;
                        break;
                    case 4:
                    case 5:
                    case 6:
                        t = 2;
                        break;
                    case 7:
                    case 8:
                    case 9:
                        t = 3;
                        break;
                    case 10:
                    case 11:
                    case 12:
                        t = 4;
                        break;
                }
                return t;
            }
        }
        public int ContractsCount
        {
            get
            {
                int c = 0;


                if (this.Contracts != null)
                {
                    c = this.Contracts.Count;
                }
                return c;
            }
        }

        public StringBuilder GenerateXml()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>");
            sb.AppendLine(@"<deductionAgreementVer2007Batch xmlns=""http://www.taxisnet.gr/deduction"">");
            sb.AppendLine("\t<deductionAgreementVer2007Document>");
            sb.AppendLine("\t\t<Al2c1>" + this.Year.ToString() + "</Al2c1>");
            sb.AppendLine("\t\t<Al3c1>" + this.Trimester.ToString() + "</Al3c1>");

            sb.AppendLine("\t\t<BlockA>");
            sb.AppendLine("\t\t\t<Al5c1>" + this.CompanyVat + "</Al5c1>");
            sb.AppendLine("\t\t</BlockA>");

            GenerateContractsXml(ref sb);

            sb.AppendLine("\t</deductionAgreementVer2007Document>");
            sb.AppendLine("</deductionAgreementVer2007Batch>");
            return sb;
        }

        private void GenerateContractsXml(ref StringBuilder sb)
        {
            int pages = 1;
            int rows = 1;
            int maxRows = 16;

            int total = this.Contracts.Count;
            int idx = 1;

            if (this.Contracts != null)
            {
                while (idx <= total)
                {
                    //write table header
                    if (rows == 1)
                    {
                        //sb.AppendLine("\t\t<TableA CurrentPage=\"" + pages.ToString() + "\">");
                        sb.AppendLine("\t\t<TableA>");
                    }

                    //write contents
                    GenerateContractXml(ref sb, this.Contracts[idx - 1]);

                    //write table footer
                    if (idx == total || rows == maxRows)
                    {
                        sb.AppendLine("\t\t</TableA>");


                    }


                    if (rows == maxRows)
                    {
                        rows = 1;
                        pages++;
                    }
                    else
                        rows++;

                    //stop looping when the last one has been processed
                    if (idx > total)
                        break;
                    idx++;
                }
            }
        }
        private void GenerateContractXml(ref StringBuilder sb, Contract c)
        {
            string customerName = c.CustomerName.Replace("&", "&amp;");
            string address = c.Address.Replace("&", "&amp;");

            sb.AppendLine("\t\t\t<Rows toBeDeleted=\"false\">");

            sb.AppendLine("\t\t\t\t<Bl1c2>" + c.ContractNo + "</Bl1c2>");
            sb.AppendLine("\t\t\t\t<Bl1c3>" + c.ContractDate.GetValueOrDefault().ToString("yyyy-MM-dd") + "</Bl1c3>");


            if (String.IsNullOrWhiteSpace(c.VatCountry))
            {
                sb.AppendLine("\t\t\t\t<Bl1c4>" + c.Vat + "</Bl1c4>");
                sb.AppendLine("\t\t\t\t<Bl1c5>" + c.CustomerName + "</Bl1c5>");
                sb.AppendLine("\t\t\t\t<Bl1c6>" + c.Address + "</Bl1c6>");
            }
            else {
                sb.AppendLine("\t\t\t\t<Bl1c7>" + c.Vat + "</Bl1c7>");
                sb.AppendLine("\t\t\t\t<Bl1c8>" + c.CustomerName + "</Bl1c8>");
                sb.AppendLine("\t\t\t\t<Bl1c9>" + c.VatCountry + "</Bl1c9>");
            }
            sb.AppendLine("\t\t\t\t<Bl1c10>" + c.ContractType + "</Bl1c10>");
            sb.AppendLine("\t\t\t\t<Bl1c11>" + c.StartDate.GetValueOrDefault().ToString("yyyy-MM-dd") + "</Bl1c11>");
            sb.AppendLine("\t\t\t\t<Bl1c12>" + c.EndDate.GetValueOrDefault().ToString("yyyy-MM-dd") + "</Bl1c12>");
            sb.AppendLine("\t\t\t\t<Bl1c13>" + c.Amount.ToString() + "</Bl1c13>");
            //if (!String.IsNullOrWhiteSpace(c.Comments))
                sb.AppendLine("\t\t\t\t<Bl1c14>" + (String.IsNullOrWhiteSpace(c.Comments) ? "ΔΕΝ ΥΠΑΡΧΟΥΝ ΠΑΡΑΤΗΡΗΣΕΙΣ" : c.Comments) + "</Bl1c14>");

            sb.AppendLine("\t\t\t</Rows>");
        }
    }
}
