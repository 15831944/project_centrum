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
    public partial class Form1 : Form
    {
        public static Form1 _form;
        __DrawingData input;
        __DrawingData output;

        public Form1()
        {
            InitializeComponent();
            _form = this;
        }

        private void copy(Func<__DrawingData> getter)
        {
            add_text("Copy... ");

            input = getter();

            add_text("Done \n");
            add_text(input.countObjects());
        }

        private void paste(Func<__DrawingData> getter)
        {
            add_text("Paste... ");

            output = getter();
            add_text("Done \n");
            add_text(output.countObjects());

            UserProperties.set(cb_view.Checked, cb_mark.Checked, cb_txt.Checked,
                                                cb_mark_attr.Checked,
                               cb_section.Checked, cb_detail.Checked, cb_dim.Checked, cb_line.Checked,
                               cb_red.Checked);

            add_text("***********\n");
            add_text("Redraw... ");
            __CopyDrawingHandler.main(input, output);
            add_text("Done \n");

        }

        private void btn_input_all_Click(object sender, EventArgs e)
        {
            if (rb_copy_all.Checked)
            {
                copy_paste_handler(copy, TeklaGetter.getAllData, "Error copying - 1");
            }
            else if (rb_copy_fast.Checked)
            {
                copy_paste_handler(copy, TeklaGetter.getSelectedData, "Error copying - 2");
            }
        }

        private void btn_output_Click(object sender, EventArgs e)
        {
            if (rb_paste_all.Checked)
            {
                copy_paste_handler(paste, TeklaGetter.getAllData, "Error pasting - 3");
            }
            else if (rb_paste_fast.Checked)
            {
                copy_paste_handler(paste, TeklaGetter.getSelectedData, "Error pasting - 4");
            }
        }
        
        private void copy_paste_handler(Action<Func<__DrawingData>> function, Func<__DrawingData> getter, string error)
        {
            toggle_all_controls(false);

            try
            {
                DateTime start = DateTime.Now;
                function(getter);
                DateTime end = DateTime.Now;
                timerReport(start, end);
            }
            catch (DivideByZeroException )
            {
                add_text("FAILED\n");
                add_text("Drawing not opened");
            }
            catch
            {
                add_text("FAILED\n");
                add_text(error);
            }

            toggle_all_controls(true);
        }

        private void timerReport(DateTime start, DateTime end)
        {
            add_text("Time: " + end.Subtract(start).TotalSeconds.ToString("F0") + " seconds \n");
            add_text("***********\n");
        }

        public void add_text(string message)
        {
            txt_status.AppendText(message);
        }

        private void cb_onoff_CheckedChanged(object sender, EventArgs e)
        {
            on_off(cb_onoff.Checked);
        }

        private void toggle_all_controls(bool status)
        {
            cb_view.Enabled = status;
            cb_mark.Enabled = status;
            cb_txt.Enabled = status;

            cb_mark_attr.Enabled = status;

            cb_section.Enabled = status;
            cb_detail.Enabled = status;
            cb_dim.Enabled = status;
            cb_line.Enabled = status;

            cb_red.Enabled = status;

            cb_onoff.Enabled = status;

            btn_input.Enabled = status;
            btn_output.Enabled = status;
        }

        private void on_off(bool status)
        {
            cb_view.Checked = status;
            cb_mark.Checked = status;
            cb_txt.Checked = status;

            cb_mark_attr.Checked = status;

            cb_section.Checked = status;
            cb_detail.Checked = status;
            cb_dim.Checked = status;
            cb_line.Checked = status;

            cb_red.Checked = status;
        }
    }
}
