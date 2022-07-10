using System.Windows.Forms;
using System.Drawing;
using CustomGroupBox;
using System.Collections.Generic;
using System;

namespace Samples
{
    public partial class SampleForm : Form
    {
        Dictionary<string, BindableItem<string>> kv = new Dictionary<string, BindableItem<string>>();

        public SampleForm()
        {
            InitializeComponent();

            // グループボックスのサイズが、各列で最長のセルに合わせて表示されることを確認
            var labelContains = new TableLayoutGroupBox(2)
            {
                Text = "LabelContains",
                Location = new Point(16)
            };
            labelContains.AddRow(new Label() { Text = "aaaaaa", AutoSize = true }, new Label() { Text = "bbbbb", AutoSize = true });
            labelContains.AddRow(new Label() { Text = "ccccccccccc", AutoSize = true }, new Label() { Text = "ddd", AutoSize = true });
            Controls.Add(labelContains);

            // グループボックスの中に"タイトル|テキストボックス|単位"みたいな形式の何かが入るか
            // テキストボックスの長いほうに表示がそろうことを確認
            // nullがあってもいいよね？
            var textContains = new TableLayoutGroupBox(3)
            {
                Text = "TextBoxContains",
                Location = new Point(16, labelContains.Height + DefaultMargin.Top)
            };
            textContains.AddRow(new Label() { Text = "年収" }, new TextBox() { Width = 30 }, new Label() { Text = "円" });
            textContains.AddRow(new Label() { Text = "年齢" }, new TextBox() { Width = 60 }, new Label() { Text = "歳" });
            textContains.AddRow(new Label() { Text = "住所" }, new TextBox() { Width = 60 }, null);
            Controls.Add(textContains);

            // 関係ないけどバインディングもできるよね？
            var binded = new TableLayoutGroupBox(2)
            {
                Text = "Binding",
                Location = new Point(16, textContains.Location.Y + textContains.Height + DefaultMargin.Top)
            };

            var label = new Label() { Text = "Data1" };
            var textbox = new TextBox();
            kv.Add(label.Text, new BindableItem<string>());
            textbox.DataBindings.Add(new Binding(nameof(textbox.Text), kv[label.Text], nameof(BindableItem<string>.Value)));
            binded.AddRow(label, textbox);
            Controls.Add(binded);

            var test = new TableLayoutWithAControlGroupBox(3)
            {
                Text = "コントロールが横にある",
                Location = new Point(16, binded.Location.Y + binded.Height + DefaultMargin.Top)
            };
            test.AddRow(new Label() { Text = "年収" }, new TextBox() { Width = 30 }, new Label() { Text = "円" });
            test.AddRow(new Label() { Text = "年齢" }, new TextBox() { Width = 60 }, new Label() { Text = "歳" });
            test.AddRow(new Label() { Text = "住所" }, new TextBox() { Width = 60 }, null);
            test.SetControlBesideTable(new Label() { BackColor = Color.Red, Width = 30, Height = 80 });
            Controls.Add(test);
        }

        private void SampleForm_Click(object sender, System.EventArgs e)
        {
            foreach (var key in kv.Keys)
            {
                Console.WriteLine($"kv[{key}] = {kv[key].Value}");
            }
        }
    }
}
