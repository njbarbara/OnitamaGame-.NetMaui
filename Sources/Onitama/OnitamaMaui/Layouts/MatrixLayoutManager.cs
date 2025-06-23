using Microsoft.Maui.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnitamaMaui.Layouts
{
    public class MatrixLayoutManager : ILayoutManager
    {
        MatrixLayout? MatrixLayout { get; set; }
        double LayoutWidth { get; set; }
        double LayoutHeight { get; set; }
        double MaxCellWidth { get; set; }
        double MaxCellHeight { get; set; }

        public MatrixLayoutManager(MatrixLayout matrixLayout)
        {
            MatrixLayout = matrixLayout;
        }

        public Size Measure(double widthConstraint, double heightConstraint)
        {
            if (MatrixLayout == null)
                return new Size(widthConstraint, heightConstraint);

            int nbRows = MatrixLayout.NbRows;
            int nbColumns = MatrixLayout.NbColumns;

            var padding = MatrixLayout.Padding;
            var horizontalSpacing = MatrixLayout.HorizontalSpacing;
            var verticalSpacing = MatrixLayout.VerticalSpacing;

            MaxCellWidth = (widthConstraint - padding.HorizontalThickness - (nbColumns - 1) * horizontalSpacing) / nbColumns;
            MaxCellHeight = (heightConstraint - padding.VerticalThickness - (nbRows - 1) * verticalSpacing) / nbRows;

            double minDim = Math.Min(MaxCellWidth, MaxCellHeight);
            MaxCellWidth = minDim;
            MaxCellHeight = minDim;

            LayoutWidth = MaxCellWidth * nbColumns + (nbColumns - 1) * horizontalSpacing + padding.HorizontalThickness;
            LayoutHeight = MaxCellHeight * nbRows + (nbRows - 1) * verticalSpacing + padding.VerticalThickness;
            return new Size(LayoutWidth, LayoutHeight);
        }

        public Size ArrangeChildren(Rect bounds)
        {
            if (MatrixLayout == null)
                return new Size(LayoutWidth, LayoutHeight);

            var padding = MatrixLayout.Padding;
            var horizontalSpacing = MatrixLayout.HorizontalSpacing;
            var verticalSpacing = MatrixLayout.VerticalSpacing;
            int nbColumns = MatrixLayout.NbColumns;
            double top = padding.Top + bounds.Top;
            double left = padding.Left + bounds.Left;

            for (int cellId = 0; cellId < MatrixLayout.Count; cellId++)
            {
                var cell = MatrixLayout[cellId];
                int numRow = cellId / nbColumns;
                int numColumn = cellId - numRow * nbColumns;

                double leftSide = left + numColumn * (MaxCellWidth + horizontalSpacing);
                double topSide = top + numRow * (MaxCellHeight + verticalSpacing);

                var destination = new Rect(leftSide, topSide, MaxCellWidth, MaxCellHeight);
                cell.Arrange(destination);
            }

            return new Size(LayoutWidth, LayoutHeight);
        }
    }
}
