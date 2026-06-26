Imports Xceed.Grid
Imports System.Drawing

Public Class IconViewer
    Implements ICellViewer

    ' Returns the required height for the viewer.
    ' In this case, we return the height of the image so that it can be displayed properly.
    Function GetFittedHeight(ByVal cell As Cell, ByVal mode As AutoHeightMode) As Integer Implements ICellViewer.GetFittedHeight
        Return DirectCast(cell.Value, Icon).Height
    End Function

    ' Returns the required width for the viewer.
    ' -1 means that we don't have any special requirements.
    Function GetFittedWidth(ByVal cell As Cell) As Integer Implements ICellViewer.GetFittedWidth
        Return -1
    End Function

    ' Paints the value of the cell. In our case, we will take the cell's 
    ' numeric value and draw the same number of images.
    Function PaintCellValue(ByVal e As GridPaintEventArgs, ByVal cell As Cell) As Boolean Implements ICellViewer.PaintCellValue
        Dim imageWidth As Integer = DirectCast(cell.Value, Icon).Width
        Dim horizontalPosition As Integer = 10
        e.Graphics.DrawImage(DirectCast(cell.Value, Icon).ToBitmap, horizontalPosition, e.DisplayRectangle.Y, imageWidth, e.DisplayRectangle.Height)
        Return True
    End Function
End Class