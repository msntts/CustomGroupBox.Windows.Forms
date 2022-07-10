using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CustomGroupBox
{
    public class TableLayoutGroupBox : GroupBox
    {
        protected TableLayoutPanel _table = new TableLayoutPanel();

        public Point TableLocation
        {
            set
            {
                if (value != null)
                {
                    _table.Location = value;
                }
            }

            get => _table.Location;
        }

        // Paddingを変更した際、内包するテーブルサイズをもとにサイズを更新するため
        // ControlのPaddingを上書きする。
        public new Padding Padding
        {
            set
            {
                if (!Padding.Equals(value))
                {
                    base.Padding = value;

                    UpdateGroupBoxSize();
                }
            }

            get => base.Padding;
        }

        // サイズはモジュールの中で計算しているので常にfalse
        public new bool AutoSize
        {
            set => base.AutoSize = false;

            get => false;
        }

        public TableLayoutGroupBox(int columnCount)
        {
            base.Padding = new Padding(8, 16, 8, 8);
            base.AutoSize = false;

            _table.Location = new Point(Padding.Left, Padding.Top);
            _table.ColumnCount = columnCount;

            base.Controls.Add(_table);

            UpdateGroupBoxSize();
        }

        public void AddRow(params Control[] controls)
        {
            /* 
             * このグループボックスにコントロールを追加した場合は以下の仕様で配置される
             *   - 各セルの上下中央左寄せ
             *   - 各列の最も幅が広いものに列幅がそろう
             *     - テキストの表示領域は指定がない場合、自動調整
            */
            foreach (var (control, index) in controls.Select((control, index) => (control, index)))
            {
                if (control != null)
                {
                    control.Dock = DockStyle.Fill;

                    // コントロールによってプロパティがないのでdynamic型に変換
                    // 例外が発生したときはそのプロパティを持っていないだけなので無視
                    try
                    {
                        dynamic textAlignable = control;
                        textAlignable.TextAlign = ContentAlignment.MiddleLeft;
                    }
                    catch { }

                    try
                    {
                        dynamic autoAdjustable = control;
                        autoAdjustable.AutoSize = true;
                    }
                    catch { }

                    _table.Controls.Add(control, index, _table.RowCount);
                }
                else
                {
                    // コントロールがnullの時は、害がない空コントロールを追加しておく
                    _table.Controls.Add(new Label() { AutoSize = true }, index, _table.RowCount);
                }
            }

            _table.RowCount++;
            UpdateGroupBoxSize();
        }

        virtual protected void UpdateGroupBoxSize()
        {
            int[] maxWidthEachColumn = new int[_table.ColumnCount];
            int tableHeight = 0;

            // Table内コントロールの高さ、幅からグループボックスのサイズを決める
            // Width: 列ごとの最長セルの総和
            // Height: 各行の高さの総和
            for (int row = 0; row < _table.RowCount; ++row)
            {

                int maxHeight = 0;

                for (int column = 0; column < _table.ColumnCount; ++column)
                {
                    var control = _table.Controls[column + (row * _table.ColumnCount)];

                    if (control != null)
                    {
                        maxWidthEachColumn[column] = Math.Max(maxWidthEachColumn[column],
                            control.Width
                            + control.Margin.Left + control.Margin.Right
                            + _table.Padding.Left + _table.Padding.Right);

                        maxHeight = Math.Max(maxHeight,
                            control.Height
                            + control.Margin.Top + control.Margin.Bottom
                            + _table.Padding.Top + _table.Padding.Bottom);
                    }
                }

                tableHeight += maxHeight;
            }

            _table.Width = maxWidthEachColumn.Sum();
            _table.Height = tableHeight;

            // パディングを足し合わせてサイズを更新
            Width = _table.Width + Padding.Left + Padding.Right;
            Height = _table.Height + Padding.Top + Padding.Bottom;
            // サイズを変えるとロケーションが変わってしまうので元に戻す
            TableLocation = new Point(Padding.Left, Padding.Top);

            Update();
        }
    }
}
