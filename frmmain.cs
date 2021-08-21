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

namespace TestItEditor
{
    public partial class frmmain : Form
    {
        public frmmain()
        {
            InitializeComponent();
        }


        void InsertHtmlList(string ListType)
        {
            //Split the selected text.
            string[] Lines = txtHtml.SelectedText.Split('\n');
            string Buffer = string.Empty;

            //Append list start
            Buffer = "<" + ListType + ">\n";

            //Build the list
            for (int i = 0; i < Lines.Length; i++)
            {
                //Append list item
                Buffer = Buffer + "    <li>" + Lines[i] + "</li>\n";
            }

            //Append end of list
            Buffer = Buffer + "</" + ListType + ">";

            //Update html editor.
            txtHtml.SelectedText = Buffer;

            //Clear up string
            Buffer = string.Empty;
            //Clear array.
            Array.Clear(Lines, 0, Lines.Length);
            txtHtml.Focus();
        }

        void insertBreakingSpace(int size)
        {
            int i = 1;
            string buffer = string.Empty;
            //Repeat the string
            for (i = 1; i <= size; i++)
            {
                buffer = buffer + "&nbsp;";
            }
            txtHtml.SelectedText = buffer;
            txtHtml.Focus();
        }

        string GetHtmlColor()
        {
            ColorDialog cd = new ColorDialog();

            cd.AllowFullOpen = true;
            if (cd.ShowDialog() == DialogResult.OK)
            {
                //Return html color
                return "#" + cd.Color.R.ToString("X2") +
                    cd.Color.G.ToString("X2") + cd.Color.B.ToString("X2");
            }
            cd.Dispose();
            return "";
        }

        int NoneWhiteSpacePos(string src)
        {
            int x = 0;
            int idx = -1;

            while (x < src.Length)
            {
                if(src[x] !=32)
                {
                    idx = x;
                    break;
                }
                x++;
            }

            if ((x > 0) &&(idx==-1))
            {
                idx = src.Length;
            }
            
            if (idx == -1)
            {
                idx = 0;
            }

            return idx;
        }

        void OutIndent(RichTextBox rtb)
        {
            int s_start = rtb.SelectionStart;
            int s_end = 0;

            if (rtb.SelectionLength > 0)
            {

                //Get selected lines.
                string[] lines = rtb.SelectedText.Split('\n');

                int i = 0;
                while (i < lines.Length)
                {
                    if (lines[i][0]== 32){
                        lines[i] = lines[i].Remove(0, 1);
                    }
                    //INC counter
                    i++;
                }

                string s = string.Join("\n", lines);
                s_end = s.Length;

                rtb.SelectedText = s;
                rtb.SelectionStart = s_start;
                rtb.SelectionLength = s_end;
            }
            rtb.Focus();
        }

        void Indent(RichTextBox rtb)
        {
            int s_start = rtb.SelectionStart;
            int s_end = 0;

            if (rtb.SelectionLength > 0)
            {

                //Get selected lines.
                string[] lines = rtb.SelectedText.Split('\n');

                int i = 0;
                while (i < lines.Length)
                {
                    lines[i] = " " + lines[i];
                    //INC counter
                    i++;
                }

                string s = string.Join("\n", lines);
                s_end = s.Length;

                rtb.SelectedText = s;
                rtb.SelectionStart = s_start;
                rtb.SelectionLength = s_end;
            }
            rtb.Focus();
        }

        void TabsToSpaces(RichTextBox rtb)
        {
            //Replace all tab with 4 spaces.
            rtb.Text = rtb.Text.Replace("\t", "    ");
            rtb.Focus();
        }

        void SpacesToTabs(RichTextBox rtb)
        {
            //Replace all space with 1 tab.
            rtb.Text = rtb.Text.Replace("    ", "\t");
            rtb.Focus();
        }

        void BuildProject(StringBuilder sb)
        {
            int i = 0;
            //HTML
            sb.AppendLine("BEGIN");
            i = 0;
            while (i < txtHtml.Lines.Length)
            {
                sb.AppendLine(txtHtml.Lines[i]);
                i++;
            }
            sb.AppendLine("END.");
            //CSS
            sb.AppendLine("BEGIN");
            i = 0;
            while (i < txtCss.Lines.Length)
            {
                sb.AppendLine(txtCss.Lines[i]);
                i++;
            }
            sb.AppendLine("END.");
            //JS
            sb.AppendLine("BEGIN");
            i = 0;
            while (i < txtJava.Lines.Length)
            {
                sb.AppendLine(txtJava.Lines[i]);
                i++;
            }
            sb.AppendLine("END.");
        }

        void LoadProject(List<string>proj_data)
        {
            int x = 0;
            string StrBuff = string.Empty;
            bool Found = false;
            List<string> Files = new List<string>();
            StringBuilder sb = new StringBuilder();

            while (x < proj_data.Count())
            {
                //Get line
                StrBuff = proj_data[x];

                if (StrBuff.ToUpper() == "END.")
                {
                    Found = false;
                    //Remove last crlf
                    string s = sb.ToString();
                    //Check string length
                    if (s.Length > 2)
                    {
                        //Strip off crlf
                        s = s.Remove(s.Length - 2, 2);
                    }
                    Files.Add(s);
                    //Clear buffer.
                    s = string.Empty;
                    sb.Clear();
                }

                if (StrBuff.ToUpper() == "BEGIN")
                {
                    //Read in the next line
                    Found = true;
                }

                if (Found)
                {
                    if (StrBuff.ToUpper() != "BEGIN")
                    {
                        sb.AppendLine(StrBuff);
                    }
                }
                //INC x
                x++;
            }
            //Update editor fields.
            txtHtml.Text = Files[0];
            txtCss.Text = Files[1];
            txtJava.Text = Files[2];
            //Clear files.
            Files.Clear();
            //Clear buffer.
            sb.Clear();
        }

        void InsertHtml_1(int index)
        {
            switch (index)
            {
                case 0:
                    //Bold
                    txtHtml.SelectedText = "<strong>" + txtHtml.SelectedText + "</strong>";
                    break;
                case 1:
                    //Italic
                    txtHtml.SelectedText = "<i>" + txtHtml.SelectedText + "</i>";
                    break;
                case 2:
                    //Underline
                    txtHtml.SelectedText = "<u>" + txtHtml.SelectedText + "</u>";
                    break;
                case 3:
                    //<P>
                    txtHtml.SelectedText = "<p>" + txtHtml.SelectedText + "</p>";
                    break;
                case 4:
                    //Pre
                    txtHtml.SelectedText = "<pre>" + txtHtml.SelectedText + "</pre>";
                    break;
                case 5:
                    //Code
                    txtHtml.SelectedText = "<code>" + txtHtml.SelectedText + "</code>";
                    break;
                case 6:
                    //Align left
                    txtHtml.SelectedText = "<p align=\"left\">" + txtHtml.SelectedText + "</p>";
                    break;
                case 7:
                    //Align center
                    txtHtml.SelectedText = "<p align=\"center\">" + txtHtml.SelectedText + "</p>";
                    break;
                case 8:
                    //Align right
                    txtHtml.SelectedText = "<p align=\"right\">" + txtHtml.SelectedText + "</p>";
                    break;
                case 9:
                    //Align justify
                    txtHtml.SelectedText = "<p align=\"justify\">" + txtHtml.SelectedText + "</p>";
                    break;
                case 10:
                    //Comment html
                    txtHtml.SelectedText = "<!-- " + txtHtml.SelectedText + " -->";
                    break;
                case 11:
                    //Insert <font></font>
                    txtHtml.SelectedText = "<font></font>";
                    break;
                case 12:
                    //Mone-breaking space
                    txtHtml.SelectedText = "&nbsp;";
                    break;
                case 13:
                    //Inset image tag
                    txtHtml.SelectedText = @"<img src="""" alt="""" />";
                    break;
                case 14:
                    //Anchor
                    txtHtml.SelectedText = @"<a href="""">" + txtHtml.SelectedText + "</a>";
                    break;
                case 15:
                    //Div
                    txtHtml.SelectedText = "<div>" + txtHtml.SelectedText + "</div>";
                    break;
                case 16:
                    //Span
                    txtHtml.SelectedText = "<span>" + txtHtml.SelectedText + "</span>";
                    break;
                case 17:
                    //blockquote
                    txtHtml.SelectedText = "<blockquote>" + txtHtml.SelectedText + "</blockquote>";
                    break;
                case 18:
                    //Definition List
                    txtHtml.Text = "<dl>\n    <dt>" + txtHtml.SelectedText + "</dt>\n    <dd></dd>\n</dl>";
                    break;
            }
            txtHtml.Focus();
        }

        void DoHtmlHeadings(int index)
        {
            //Set html headings.
            txtHtml.SelectedText = "<H" + index.ToString() + ">" +
                txtHtml.SelectedText + "</H" + index.ToString() + ">";
            //Set focus on editor.
            txtHtml.Focus();
        }

        void DoCustom(RichTextBox tb, object sender, KeyPressEventArgs e)
        {
            string curline = string.Empty;
            int pos = tb.SelectionStart;
            int LineIdx = tb.GetLineFromCharIndex(pos) - 1;

            //Indent new line on enter to last space position.
            try
            {
                if (e.KeyChar == '\r')
                {
                    //Get current line.
                    curline = tb.Lines[LineIdx];
                    //Create a stream of blank spaces.
                    string sp = new string(' ', NoneWhiteSpacePos(curline));
                    //Append a space to the new line
                    tb.SelectedText = sp;

                }

                if (e.KeyChar == '{')
                {
                    e.Handled = true;
                    tb.SelectedText = "{}";
                    tb.SelectionStart--;
                }

                //Auto complete for " and '
                if ((e.KeyChar == 34) || (e.KeyChar == 39))
                {
                    tb.SelectedText = e.KeyChar.ToString() +
                        e.KeyChar.ToString();
                    tb.SelectionStart--;
                    e.Handled = true;
                }

                //Replace and indent with 4 spaces.
                if (e.KeyChar == 9)
                {
                    tb.SelectedText = "    ";
                    e.Handled = true;
                }
            }
            catch { }
        }

        private void RunCode(string Html, string Css, string JavaS)
        {

            StringBuilder htmldata = new StringBuilder();

            htmldata.AppendLine("<!DOCTYPE html>");
            htmldata.AppendLine("<html>");
            htmldata.AppendLine("<head>");
            htmldata.AppendLine(@"<meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" /");
           
            htmldata.AppendLine("<!--STYLE DATA-->");
            htmldata.AppendLine("<style>");
            htmldata.AppendLine(Css);
            htmldata.AppendLine("</style>");

            htmldata.AppendLine("</head>");
            htmldata.AppendLine("<body>");

            htmldata.AppendLine("<!--HTML DATA-->");
            htmldata.AppendLine(Html);

            htmldata.AppendLine("<!--JAVA SCRIPT DATA-->");
            htmldata.AppendLine("<script>");
            htmldata.AppendLine(JavaS);
            htmldata.AppendLine("</script>");

            htmldata.AppendLine("</body>");
            htmldata.AppendLine("</html>");

            HtmlDocument Doc = WebView.Document;
          
            Doc.OpenNew(true);
            Doc.Write(htmldata.ToString());

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Nav to blank page.
            WebView.Navigate("about:blank");
        }

        private void cmdRun_Click(object sender, EventArgs e)
        {
            RunCode(txtHtml.Text, txtCss.Text, txtJava.Text);
        }

        private void splitContainer3_SplitterMoved(object sender, SplitterEventArgs e)
        {
           // splitContainer1.SplitterDistance = splitContainer3.SplitterDistance;
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
         //   splitContainer3.SplitterDistance = splitContainer1.SplitterDistance;
        }

        private void txtHtml_KeyPress(object sender, KeyPressEventArgs e)
        {
            DoCustom(txtHtml, sender, e);
        }

        private void txtCss_KeyPress(object sender, KeyPressEventArgs e)
        {
            DoCustom(txtCss, sender, e);
        }

        private void txtJava_KeyPress(object sender, KeyPressEventArgs e)
        {
            DoCustom(txtJava, sender, e);
        }
        
        private void cmdSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sd = new SaveFileDialog();
            StringBuilder sb = new StringBuilder();

            sd.Title = "Save";
            sd.Filter = "Test It Project(*.tip)|*.tip";

            if (sd.ShowDialog() == DialogResult.OK)
            {
                BuildProject(sb);
                //Save project.
                try
                {
                    using (StreamWriter sw = new StreamWriter(sd.FileName))
                    {
                        //Write to file.
                        sw.Write(sb.ToString());
                        //Close
                        sw.Close();
                        //Clear string builder.
                        sb.Clear();
                    }
                }
                catch (FieldAccessException ex)
                {
                    MessageBox.Show(ex.Message, "FileIO Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            //Clear up
            sd.Dispose();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void mnuHtmlCut_Click(object sender, EventArgs e)
        {
            txtHtml.Cut();
        }

        private void mnuHtmlCopy_Click(object sender, EventArgs e)
        {
            txtHtml.Copy();
        }

        private void mnuHtmlPaste_Click(object sender, EventArgs e)
        {
            txtHtml.Paste();
        }

        private void mnuHtmlSelect_Click(object sender, EventArgs e)
        {
            txtHtml.SelectAll();
        }

        private void mnuCssCut_Click(object sender, EventArgs e)
        {
            txtCss.Cut();
        }

        private void mnuCssCopy_Click(object sender, EventArgs e)
        {
            txtCss.Copy();
        }

        private void mnuCssPaste_Click(object sender, EventArgs e)
        {
            txtCss.Paste();
        }

        private void mnucCssSelect_Click(object sender, EventArgs e)
        {
            txtCss.SelectAll();
        }

        private void mnuJsCut_Click(object sender, EventArgs e)
        {
            txtJava.Cut();
        }

        private void mnuJsCopy_Click(object sender, EventArgs e)
        {
            txtJava.Copy();
        }

        private void mnuJsPaste_Click(object sender, EventArgs e)
        {
            txtJava.Paste();
        }

        private void mnuJsSelect_Click(object sender, EventArgs e)
        {
            txtJava.SelectAll();
        }

        private void mnuHtmlUndo_Click(object sender, EventArgs e)
        {
            txtHtml.Undo();
        }

        private void mnuCssUndo_Click(object sender, EventArgs e)
        {
            txtCss.Undo();
        }

        private void mnuJsUndo_Click(object sender, EventArgs e)
        {
            txtJava.Undo();
        }

        private void cmdNew_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to create a new project.",
                "New", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //Clear edit fields.
                txtHtml.Clear();
                txtCss.Clear();
                txtJava.Clear();
                //Refresh the viewer.
                RunCode(txtHtml.Text, txtCss.Text, txtJava.Text);
                //Set focus on html field.
                txtHtml.Focus();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void mnuHead1_Click(object sender, EventArgs e)
        {
            DoHtmlHeadings(1);
        }

        private void mnuHead2_Click(object sender, EventArgs e)
        {
            DoHtmlHeadings(2);
        }

        private void mnuHead3_Click(object sender, EventArgs e)
        {
            DoHtmlHeadings(3);
        }

        private void mnuHead4_Click(object sender, EventArgs e)
        {
            DoHtmlHeadings(4);
        }

        private void mnuBreak_Click(object sender, EventArgs e)
        {
            txtHtml.SelectedText = "<br />";
        }

        private void mnuHtmlBold_Click(object sender, EventArgs e)
        {
            InsertHtml_1(0); // Bold 
        }

        private void mnuHtmlItalic_Click(object sender, EventArgs e)
        {
            InsertHtml_1(1); // Italic 
        }

        private void mnuHtmlUnderline_Click(object sender, EventArgs e)
        {
            InsertHtml_1(2); // Underline 
        }

        private void mnuHtmlPar_Click(object sender, EventArgs e)
        {
            InsertHtml_1(3); // Paragraph 
        }

        private void mnuHtmlPre_Click(object sender, EventArgs e)
        {
            InsertHtml_1(4); // Preformatted. 
        }

        private void mnuHtmlCode_Click(object sender, EventArgs e)
        {
            InsertHtml_1(5); // Code 
        }

        private void mnuHtmlHeading_Click(object sender, EventArgs e)
        {

        }

        private void mnuHtmlLeft_Click(object sender, EventArgs e)
        {
            InsertHtml_1(6); // Align left
        }

        private void mnuHtmlCenter_Click(object sender, EventArgs e)
        {
            InsertHtml_1(7); // Align center
        }

        private void mnuHtmlRight_Click(object sender, EventArgs e)
        {
            InsertHtml_1(8); // Align right
        }

        private void mnuHtmlJustify_Click(object sender, EventArgs e)
        {
            InsertHtml_1(9); // Align justify
        }

        private void cmdLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog od = new OpenFileDialog();
            List<string> data = new List<string>();
            string sLine = string.Empty;

            od.Title = "Open";
            od.Filter = "Test It Project(*.tip)|*.tip";

            if (od.ShowDialog() == DialogResult.OK)
            {
                //Load project.
                try
                {
                    using (StreamReader sr = new StreamReader(od.FileName))
                    {
                        //Load each line
                        while (!sr.EndOfStream)
                        {
                            sLine = sr.ReadLine();
                            //Load in to data list
                            data.Add(sLine);
                        }
                        //Close file
                        sr.Close();
                    }
                    //Load the project.
                    LoadProject(data);
                    //Clear data
                    data.Clear();
                    //Refresh the viewer.
                    RunCode(txtHtml.Text, txtCss.Text, txtJava.Text);
                    //Set focus on editor
                    txtHtml.Focus();
                }
                catch (FieldAccessException ex)
                {
                    MessageBox.Show(ex.Message, "FileIO Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            //Clear up
            od.Dispose();
        }

        private void mnuHtmlTabs_Click(object sender, EventArgs e)
        {
            //Replace tabs with spaces.
            TabsToSpaces(txtHtml);
        }

        private void mnuHtmlSpcTabs_Click(object sender, EventArgs e)
        {
            //Space to tabs.
            SpacesToTabs(txtHtml);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            OutIndent(txtHtml);
            //Indent(txtHtml);
        }

        private void mnuHtmlIndet1_Click(object sender, EventArgs e)
        {
            //Indent lines.
            Indent(txtHtml);
        }

        private void mnuHtmlOutdent_Click(object sender, EventArgs e)
        {
            //Remove indent.
            OutIndent(txtHtml);
        }

        private void mnuHtmlComment_Click(object sender, EventArgs e)
        {

        }

        private void mnuHtmlComment_Click_1(object sender, EventArgs e)
        {
            //Comment line
            InsertHtml_1(10);
        }

        private void mnuCssTabs_Click(object sender, EventArgs e)
        {
            //Tabs to spaces
            TabsToSpaces(txtCss);
        }

        private void mnuCssSpaces_Click(object sender, EventArgs e)
        {
            //Spaces to tabs
            SpacesToTabs(txtCss);
        }

        private void mnuCssIndent1_Click(object sender, EventArgs e)
        {
            //Indent text
            Indent(txtCss);
        }

        private void mnuCssOutdent1_Click(object sender, EventArgs e)
        {
            //Out indent
            OutIndent(txtCss);
        }

        private void mnuHtmlFont_Click(object sender, EventArgs e)
        {
            //Insert font tag
            InsertHtml_1(11);
        }

        private void mnuHtmlPickColor_Click(object sender, EventArgs e)
        {
            //Inset html color into html editor.
            txtHtml.SelectedText = GetHtmlColor();
            txtHtml.Focus();
        }

        private void mnuHtmlSpace_Click(object sender, EventArgs e)
        {

        }

        private void mnuHtmlSpc1_Click(object sender, EventArgs e)
        {
            //Insert 1 space
            insertBreakingSpace(1);
        }

        private void mnuHtmlSpc2_Click(object sender, EventArgs e)
        {
            //Insert 2 spaces
            insertBreakingSpace(2);
        }

        private void mnuHtmlSpc3_Click(object sender, EventArgs e)
        {
            //Insert 3 spaces
            insertBreakingSpace(3);
        }

        private void mnuHtmlSpc4_Click(object sender, EventArgs e)
        {
            //Insert 4 spaces
            insertBreakingSpace(4);
        }

        private void mnuHtmlImage_Click(object sender, EventArgs e)
        {
            //Inset image tag
            InsertHtml_1(13);
        }

        private void mnuHtmlAnchor_Click(object sender, EventArgs e)
        {
            //Anchor
            InsertHtml_1(14);
        }

        private void mnuHtmlDiv_Click(object sender, EventArgs e)
        {
            //Insert div
            InsertHtml_1(15);
        }

        private void mnuHtmlSpan_Click(object sender, EventArgs e)
        {
            //Insert span
            InsertHtml_1(16);
        }

        private void mnuBlockQuote_Click(object sender, EventArgs e)
        {
            //Insert blockquote
            InsertHtml_1(17);
        }

        private void mnuHtmlList1_Click(object sender, EventArgs e)
        {
            //Insert unordered list
            InsertHtmlList("ul");
        }

        private void mnuHtmlList2_Click(object sender, EventArgs e)
        {
            //Insert ordered list
            InsertHtmlList("ol");
        }

        private void mnuHtmlDefList_Click(object sender, EventArgs e)
        {
            //Insert definition list
            InsertHtml_1(18);
        }
    }
}
