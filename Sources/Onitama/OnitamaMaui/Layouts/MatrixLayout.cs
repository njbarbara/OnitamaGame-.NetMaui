using Microsoft.Maui.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnitamaMaui.Layouts
{
    public class MatrixLayout : Layout
    {
        public static readonly BindableProperty NbColumnsProperty
            = BindableProperty.Create(nameof(NbColumns),
                                      typeof(int),
                                      typeof(MatrixLayout),
                                      10);

        public int NbColumns
        {
            get { return (int)GetValue(NbColumnsProperty); }
            set { SetValue(NbColumnsProperty, value); }
        }

        public static readonly BindableProperty NbRowsProperty
            = BindableProperty.Create(nameof(NbRows),
                                      typeof(int),
                                      typeof(MatrixLayout),
                                      10);

        public int NbRows
        {
            get { return (int)GetValue(NbRowsProperty); }
            set { SetValue(NbRowsProperty, value); }
        }

        public static readonly BindableProperty HorizontalSpacingProperty
            = BindableProperty.Create(nameof(HorizontalSpacing),
                                      typeof(double),
                                      typeof(MatrixLayout),
                                      0.0);

        public double HorizontalSpacing
        {
            get { return (double)GetValue(HorizontalSpacingProperty); }
            set { SetValue(HorizontalSpacingProperty, value); }
        }

        public static readonly BindableProperty VerticalSpacingProperty
            = BindableProperty.Create(nameof(VerticalSpacing),
                                      typeof(double),
                                      typeof(MatrixLayout),
                                      0.0);

        public double VerticalSpacing
        {
            get { return (double)GetValue(VerticalSpacingProperty); }
            set { SetValue(VerticalSpacingProperty, value); }
        }

        protected override ILayoutManager CreateLayoutManager()
        {
            return new MatrixLayoutManager(this);
        }
    }
 }
