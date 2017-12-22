using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace project_centrum
{
    public partial class MainForm : Form
    {

        public static MainForm _form;
        __DrawingData input;
        __DrawingData output;
        double offsetRotation = 0.0;


        public MainForm()
        {
            InitializeComponent();
            _form = this;
            txt_deg.Text = offsetRotation.ToString("f1");
        }


        private void tekla_interface(Func<__DrawingData> getter, ref __DrawingData container)
        {
            UserProperties.set(cb_view.Checked, cb_mark.Checked,                                 
                                cb_section.Checked, cb_detail.Checked, cb_line.Checked, cb_dim.Checked,
                                cb_txt.Checked, cb_dwg.Checked,
                                cb_red.Checked, 
                                offsetRotation);

            add_text("Getting data... ");

            container = getter();

            add_text("[DONE]");
            add_text(container.countObjects() );
        }


        private void btn_input_all_Click(object sender, EventArgs e)
        {
            txt_status.Text = "";
            add_text("[COPY]");
            if (rb_copy_all.Checked)
            {
                copy_paste_handler(TeklaGetter.getAllData, ref input, "[ERROR] copying (e1)");
            }
            else if (rb_copy_selected.Checked)
            {
                copy_paste_handler(TeklaGetter.getSelectedData, ref input, "[ERROR] copying (e2)");
            }
        }


        private void btn_output_Click(object sender, EventArgs e)
        {
            add_text("");
            add_text("[PASTE]");
            if (rb_paste_all.Checked)
            {
                copy_paste_handler(TeklaGetter.getAllData, ref output, "[ERROR] pasting (e3)");
            }
            else if (rb_paste_selected.Checked)
            {
                copy_paste_handler(TeklaGetter.getSelectedData, ref output, "[ERROR] pasting (e4)");
            }

            add_text("");
            add_text("[REDRAW]");
            redraw_handler();            
        }

        
        private void copy_paste_handler(Func<__DrawingData> getter, ref __DrawingData container, string error)
        {
            toggle_all_controls(false);

            try
            {
                DateTime start = DateTime.Now;
                tekla_interface(getter, ref container);
                DateTime end = DateTime.Now;
                timeReport(start, end);
            }
            catch
            {
                add_text(error);
            }

            toggle_all_controls(true);
        }


        private void redraw_handler()
        {
            toggle_all_controls(false);

            if (input == null || output == null)
            {
                add_text("[ERROR] redrawing (e5)");
            }

            try
            {
                __CopyDrawingHandler.main(input, output);
                add_text("[DONE]");
            }
            catch
            {
                add_text("[ERROR] redrawing (e6)");
            }

            toggle_all_controls(true);
        }  



        private void timeReport(DateTime start, DateTime end)
        {
            add_text("Time: " + end.Subtract(start).TotalSeconds.ToString("F0") + " seconds");
            add_text("--------------------------------------------------------");
            add_text("");
        }


        public void add_text(string message)
        {
            txt_status.AppendText(message + Environment.NewLine);
        }


        public void replace_text(string message)
        {
            try
            {
                txt_status.Text = txt_status.Text.Remove(txt_status.Text.LastIndexOf(Environment.NewLine));
                txt_status.AppendText(Environment.NewLine + message);
            }
            catch
            {
                add_text(message);
            }
        }
        

        private void cb_onoff_CheckedChanged(object sender, EventArgs e)
        {
            on_off(cb_onoff.Checked);
        }


        private void toggle_all_controls(bool status)
        {
            cb_view.Enabled = status;
            cb_mark.Enabled = status;
                        
            cb_section.Enabled = status;
            cb_detail.Enabled = status;
            cb_line.Enabled = status;
            cb_dim.Enabled = status;

            cb_txt.Enabled = status;
            cb_dwg.Enabled = status;

            cb_red.Enabled = status;

            txt_deg.Enabled = status;

            cb_onoff.Enabled = status;
            btn_input.Enabled = status;
            btn_output.Enabled = status;            
        }


        private void on_off(bool status)
        {
            cb_view.Checked = status;
            cb_mark.Checked = status;
            
            cb_section.Checked = status;
            cb_detail.Checked = status;
            cb_line.Checked = status;
            cb_dim.Checked = status;

            cb_txt.Checked = status;
            cb_dwg.Checked = status;

            cb_red.Checked = status;            
        }


        private void txt_deg_TextChanged(object sender, EventArgs e)
        {
            if (txt_deg.Text != "")
            {
                try
                {
                    string value = txt_deg.Text;
                    value = value.Replace('.', ',');
                    offsetRotation = Double.Parse(value);
                }
                catch
                {
                    txt_deg.Text = "0.0";
                    offsetRotation = 0.0;
                }
            }
            else
            {
                offsetRotation = 0.0;
            }
        }

    }
}
