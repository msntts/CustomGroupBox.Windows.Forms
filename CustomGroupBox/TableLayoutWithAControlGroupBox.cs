using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CustomGroupBox
{
    public class TableLayoutWithAControlGroupBox : TableLayoutGroupBox
    {
        private Control _control;

        public TableLayoutWithAControlGroupBox(int column) : base(column)
        {
        }

        public void SetControlBesideTable(Control control)
        {
            if (_control != null)
            {
                Controls.Remove(_control);
            }

            _control = control;
            
            if(control != null)
            {
                Controls.Add(_control);
            }

            UpdateGroupBoxSize();
        }

        protected override void UpdateGroupBoxSize()
        {
            // まずテーブルサイズをもとにグループボックスのサイズを調整
            base.UpdateGroupBoxSize();

            if (_control != null)
            {
                // コントロールの配置位置を決める
                int x = _table.Location.X + _table.Width + DefaultMargin.Right;
                int y = _table.Location.Y;

                // テーブルサイズよりコントロールが小さい場合は
                // 上下中央にコントロールを配置
                if (_table.Height > _control.Height)
                {
                    y = (Height - _control.Height) / 2;
                }
                else
                {
                    // テーブルサイズよりコントロールが大きい場合は
                    // グループボックスの高さを更新
                    Height += (_control.Height - _table.Height);
                }

                // 横幅も更新
                Width += _control.Width + DefaultMargin.Right;
                _control.Location = new Point(x, y);

                Update();
            }
        }
    }
}
